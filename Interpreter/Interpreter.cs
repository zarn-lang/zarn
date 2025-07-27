using ZARN.AST;
using ZARN.Lexer;
using ZARN.BuiltIns;

namespace ZARN.Interpreter
{
    public class Interpreter : IVisitor<object?>
    {
        public Environment Globals { get; } = new Environment();
        private Environment _environment;

        public Interpreter()
        {
            _environment = Globals;
            
            // Register built-in functions
            Globals.Define("say", new SayFunction());
            Globals.Define("input", new InputFunction());
            Globals.Define("append", new AppendFunction());
            Globals.Define("get", new GetFunction());
            Globals.Define("set", new SetFunction());
            Globals.Define("len", new LenFunction());
        }

        public void Interpret(List<Stmt> statements)
        {
            try
            {
                foreach (Stmt statement in statements)
                {
                    Execute(statement);
                }
            }
            catch (RuntimeException error)
            {
                throw new Exception($"Runtime error at line {error.Token.Line}: {error.Message}");
            }
        }

        private void Execute(Stmt stmt)
        {
            stmt.Accept(this);
        }

        public void ExecuteBlock(List<Stmt> statements, Environment environment)
        {
            Environment previous = _environment;
            try
            {
                _environment = environment;

                foreach (Stmt statement in statements)
                {
                    Execute(statement);
                }
            }
            finally
            {
                _environment = previous;
            }
        }

        private object? Evaluate(Expr expr)
        {
            return expr.Accept(this);
        }

        // Expression visitors
        public object? VisitAssignExpr(Assign expr)
        {
            object? value = Evaluate(expr.Value);
            _environment.Assign(expr.Name, value);
            return value;
        }

        public object? VisitBinaryExpr(Binary expr)
        {
            object? left = Evaluate(expr.Left);
            object? right = Evaluate(expr.Right);

            switch (expr.Operator.Type)
            {
                case TokenType.GREATER:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left! > (double)right!;
                case TokenType.GREATER_EQUAL:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left! >= (double)right!;
                case TokenType.LESS:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left! < (double)right!;
                case TokenType.LESS_EQUAL:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left! <= (double)right!;
                case TokenType.NOT_EQUAL:
                    return !IsEqual(left, right);
                case TokenType.EQUAL:
                    return IsEqual(left, right);
                case TokenType.MINUS:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left! - (double)right!;
                case TokenType.PLUS:
                    if (left is double && right is double)
                    {
                        return (double)left + (double)right;
                    }
                    if (left is string || right is string)
                    {
                        return Stringify(left) + Stringify(right);
                    }
                    throw new RuntimeException(expr.Operator, "Operands must be two numbers or at least one string.");
                case TokenType.DIVIDE:
                    CheckNumberOperands(expr.Operator, left, right);
                    if ((double)right! == 0)
                    {
                        throw new RuntimeException(expr.Operator, "Division by zero.");
                    }
                    return (double)left! / (double)right!;
                case TokenType.MULTIPLY:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left! * (double)right!;
            }

            return null;
        }

        public object? VisitCallExpr(Call expr)
        {
            object? callee = Evaluate(expr.Callee);

            List<object?> arguments = new();
            foreach (Expr argument in expr.Arguments)
            {
                arguments.Add(Evaluate(argument));
            }

            if (callee is not IZarnCallable function)
            {
                throw new RuntimeException(expr.Paren, "Can only call functions and classes.");
            }

            if (arguments.Count != function.Arity)
            {
                throw new RuntimeException(expr.Paren, $"Expected {function.Arity} arguments but got {arguments.Count}.");
            }

            return function.Call(this, arguments);
        }

        public object? VisitGroupingExpr(Grouping expr)
        {
            return Evaluate(expr.Expression);
        }

        public object? VisitLiteralExpr(Literal expr)
        {
            return expr.Value;
        }

        public object? VisitLogicalExpr(Logical expr)
        {
            object? left = Evaluate(expr.Left);

            if (expr.Operator.Type == TokenType.OR)
            {
                if (IsTruthy(left)) return left;
            }
            else
            {
                if (!IsTruthy(left)) return left;
            }

            return Evaluate(expr.Right);
        }

        public object? VisitUnaryExpr(Unary expr)
        {
            object? right = Evaluate(expr.Right);

            switch (expr.Operator.Type)
            {
                case TokenType.NOT:
                    return !IsTruthy(right);
                case TokenType.MINUS:
                    CheckNumberOperand(expr.Operator, right);
                    return -(double)right!;
            }

            return null;
        }

        public object? VisitVariableExpr(Variable expr)
        {
            return _environment.Get(expr.Name);
        }

        public object? VisitListExpr(ListExpression expr)
        {
            List<object?> elements = new();
            foreach (Expr element in expr.Elements)
            {
                elements.Add(Evaluate(element));
            }
            return elements;
        }

        public object? VisitIndexExpr(ZARN.AST.Index expr)
        {
            object? obj = Evaluate(expr.Object);
            object? index = Evaluate(expr.IndexExpr);

            if (obj is List<object?> list)
            {
                if (index is not double indexNum)
                {
                    throw new RuntimeException(new Token(TokenType.LEFT_BRACKET, "[", null, 0, 0), "List index must be a number.");
                }

                int i = (int)indexNum;
                if (i < 0 || i >= list.Count)
                {
                    throw new RuntimeException(new Token(TokenType.LEFT_BRACKET, "[", null, 0, 0), "List index out of bounds.");
                }

                return list[i];
            }

            throw new RuntimeException(new Token(TokenType.LEFT_BRACKET, "[", null, 0, 0), "Only lists can be indexed.");
        }

        // Statement visitors
        public object? VisitBlockStmt(Block stmt)
        {
            ExecuteBlock(stmt.Statements, new Environment(_environment));
            return null;
        }

        public object? VisitExpressionStmt(Expression stmt)
        {
            Evaluate(stmt.Expr);
            return null;
        }

        public object? VisitFunctionStmt(Function stmt)
        {
            ZarnFunction function = new ZarnFunction(stmt, _environment);
            _environment.Define(stmt.Name.Lexeme, function);
            return null;
        }

        public object? VisitIfStmt(If stmt)
        {
            if (IsTruthy(Evaluate(stmt.Condition)))
            {
                Execute(stmt.ThenBranch);
            }
            else if (stmt.ElseBranch != null)
            {
                Execute(stmt.ElseBranch);
            }
            return null;
        }

        public object? VisitReturnStmt(Return stmt)
        {
            object? value = null;
            if (stmt.Value != null) value = Evaluate(stmt.Value);

            throw new ReturnException(value);
        }

        public object? VisitVarStmt(Var stmt)
        {
            object? value = null;
            if (stmt.Initializer != null)
            {
                value = Evaluate(stmt.Initializer);
            }

            _environment.Define(stmt.Name.Lexeme, value);
            return null;
        }

        public object? VisitWhileStmt(While stmt)
        {
            while (IsTruthy(Evaluate(stmt.Condition)))
            {
                Execute(stmt.Body);
            }
            return null;
        }

        // Helper methods
        private bool IsTruthy(object? obj)
        {
            if (obj == null) return false;
            if (obj is bool b) return b;
            return true;
        }

        private bool IsEqual(object? a, object? b)
        {
            if (a == null && b == null) return true;
            if (a == null) return false;
            return a.Equals(b);
        }

        private void CheckNumberOperand(Token @operator, object? operand)
        {
            if (operand is double) return;
            throw new RuntimeException(@operator, "Operand must be a number.");
        }

        private void CheckNumberOperands(Token @operator, object? left, object? right)
        {
            if (left is double && right is double) return;
            throw new RuntimeException(@operator, "Operands must be numbers.");
        }

        private string Stringify(object? obj)
        {
            if (obj == null) return "nothing";

            if (obj is double d)
            {
                string text = d.ToString();
                if (text.EndsWith(".0"))
                {
                    text = text.Substring(0, text.Length - 2);
                }
                return text;
            }

            if (obj is List<object?> list)
            {
                return "[" + string.Join(", ", list.Select(Stringify)) + "]";
            }

            return obj.ToString() ?? "nothing";
        }
    }
}

