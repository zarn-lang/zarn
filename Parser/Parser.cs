using ZARN.Lexer;
using ZARN.AST;

namespace ZARN.Parser
{
    public class Parser
    {
        private readonly List<Token> _tokens;
        private int _current = 0;

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
        }

        public List<Stmt> Parse()
        {
            List<Stmt> statements = new();
            
            while (!IsAtEnd())
            {
                // Skip newlines at the top level
                if (Check(TokenType.NEWLINE))
                {
                    Advance();
                    continue;
                }
                
                var stmt = Declaration();
                if (stmt != null)
                    statements.Add(stmt);
            }

            return statements;
        }

        private Stmt? Declaration()
        {
            try
            {
                if (Match(TokenType.FUN)) return FunctionDeclaration();
                return Statement();
            }
            catch (ParseException error)
            {
                Synchronize();
                throw error;
            }
        }

        private Stmt FunctionDeclaration()
        {
            Token name = Consume(TokenType.IDENTIFIER, "Expected function name.");

            Consume(TokenType.LEFT_PAREN, "Expected '(' after function name.");
            List<Token> parameters = new();
            
            if (!Check(TokenType.RIGHT_PAREN))
            {
                do
                {
                    parameters.Add(Consume(TokenType.IDENTIFIER, "Expected parameter name."));
                } while (Match(TokenType.COMMA));
            }
            
            Consume(TokenType.RIGHT_PAREN, "Expected ')' after parameters.");
            Consume(TokenType.LEFT_BRACE, "Expected '{' before function body.");
            
            List<Stmt> body = Block();
            
            return new Function(name, parameters, body);
        }

        private Stmt Statement()
        {
            if (Match(TokenType.IF)) return IfStatement();
            if (Match(TokenType.WHILE)) return WhileStatement();
            if (Match(TokenType.GIVEBACK)) return ReturnStatement();
            if (Match(TokenType.LEFT_BRACE)) return new Block(Block());

            return ExpressionStatement();
        }

        private Stmt IfStatement()
        {
            Expr condition = Expression();
            Consume(TokenType.LEFT_BRACE, "Expected '{' after if condition.");
            Stmt thenBranch = new Block(Block());

            Stmt? elseBranch = null;
            if (Match(TokenType.ELSE))
            {
                if (Check(TokenType.IF))
                {
                    elseBranch = IfStatement();
                }
                else
                {
                    Consume(TokenType.LEFT_BRACE, "Expected '{' after else.");
                    elseBranch = new Block(Block());
                }
            }

            return new If(condition, thenBranch, elseBranch);
        }

        private Stmt WhileStatement()
        {
            Expr condition = Expression();
            Consume(TokenType.LEFT_BRACE, "Expected '{' after while condition.");
            Stmt body = new Block(Block());

            return new While(condition, body);
        }

        private Stmt ReturnStatement()
        {
            Token keyword = Previous();
            Expr? value = null;
            
            if (!Check(TokenType.SEMICOLON) && !Check(TokenType.NEWLINE))
            {
                value = Expression();
            }

            ConsumeStatementEnd("Expected ';' or newline after return value.");
            return new Return(keyword, value);
        }

        private List<Stmt> Block()
        {
            List<Stmt> statements = new();

            while (!Check(TokenType.RIGHT_BRACE) && !IsAtEnd())
            {
                // Skip newlines in blocks
                if (Check(TokenType.NEWLINE))
                {
                    Advance();
                    continue;
                }
                
                var stmt = Declaration();
                if (stmt != null)
                    statements.Add(stmt);
            }

            Consume(TokenType.RIGHT_BRACE, "Expected '}' after block.");
            return statements;
        }

        private Stmt ExpressionStatement()
        {
            Expr expr = Expression();
            ConsumeStatementEnd("Expected ';' or newline after expression.");
            return new Expression(expr);
        }

        private Expr Expression()
        {
            return Assignment();
        }

        private Expr Assignment()
        {
            Expr expr = Or();

            if (Match(TokenType.ASSIGN))
            {
                Token equals = Previous();
                Expr value = Assignment();

                if (expr is Variable variable)
                {
                    Token name = variable.Name;
                    return new Assign(name, value);
                }

                throw new ParseException(equals, "Invalid assignment target.");
            }

            return expr;
        }

        private Expr Or()
        {
            Expr expr = And();

            while (Match(TokenType.OR))
            {
                Token @operator = Previous();
                Expr right = And();
                expr = new Logical(expr, @operator, right);
            }

            return expr;
        }

        private Expr And()
        {
            Expr expr = Equality();

            while (Match(TokenType.AND))
            {
                Token @operator = Previous();
                Expr right = Equality();
                expr = new Logical(expr, @operator, right);
            }

            return expr;
        }

        private Expr Equality()
        {
            Expr expr = Comparison();

            while (Match(TokenType.NOT_EQUAL, TokenType.EQUAL))
            {
                Token @operator = Previous();
                Expr right = Comparison();
                expr = new Binary(expr, @operator, right);
            }

            return expr;
        }

        private Expr Comparison()
        {
            Expr expr = Term();

            while (Match(TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL))
            {
                Token @operator = Previous();
                Expr right = Term();
                expr = new Binary(expr, @operator, right);
            }

            return expr;
        }

        private Expr Term()
        {
            Expr expr = Factor();

            while (Match(TokenType.MINUS, TokenType.PLUS))
            {
                Token @operator = Previous();
                Expr right = Factor();
                expr = new Binary(expr, @operator, right);
            }

            return expr;
        }

        private Expr Factor()
        {
            Expr expr = Unary();

            while (Match(TokenType.DIVIDE, TokenType.MULTIPLY))
            {
                Token @operator = Previous();
                Expr right = Unary();
                expr = new Binary(expr, @operator, right);
            }

            return expr;
        }

        private Expr Unary()
        {
            if (Match(TokenType.NOT, TokenType.MINUS))
            {
                Token @operator = Previous();
                Expr right = Unary();
                return new Unary(@operator, right);
            }

            return Call();
        }

        private Expr Call()
        {
            Expr expr = Primary();

            while (true)
            {
                if (Match(TokenType.LEFT_PAREN))
                {
                    expr = FinishCall(expr);
                }
                else if (Match(TokenType.LEFT_BRACKET))
                {
                    Expr index = Expression();
                    Consume(TokenType.RIGHT_BRACKET, "Expected ']' after index.");
                    expr = new ZARN.AST.Index(expr, index);
                }
                else
                {
                    break;
                }
            }

            return expr;
        }

        private Expr FinishCall(Expr callee)
        {
            List<Expr> arguments = new();
            
            if (!Check(TokenType.RIGHT_PAREN))
            {
                do
                {
                    arguments.Add(Expression());
                } while (Match(TokenType.COMMA));
            }

            Token paren = Consume(TokenType.RIGHT_PAREN, "Expected ')' after arguments.");
            return new Call(callee, paren, arguments);
        }

        private Expr Primary()
        {
            if (Match(TokenType.NOTHING)) return new Literal(null);

            if (Match(TokenType.NUMBER, TokenType.STRING))
            {
                return new Literal(Previous().Literal);
            }

            if (Match(TokenType.IDENTIFIER))
            {
                return new Variable(Previous());
            }

            if (Match(TokenType.LEFT_PAREN))
            {
                Expr expr = Expression();
                Consume(TokenType.RIGHT_PAREN, "Expected ')' after expression.");
                return new Grouping(expr);
            }

            if (Match(TokenType.LEFT_BRACKET))
            {
                List<Expr> elements = new();
                
                if (!Check(TokenType.RIGHT_BRACKET))
                {
                    do
                    {
                        elements.Add(Expression());
                    } while (Match(TokenType.COMMA));
                }
                
                Consume(TokenType.RIGHT_BRACKET, "Expected ']' after list elements.");
                return new ListExpression(elements);
            }

            throw new ParseException(Peek(), "Expected expression.");
        }

        private bool Match(params TokenType[] types)
        {
            foreach (TokenType type in types)
            {
                if (Check(type))
                {
                    Advance();
                    return true;
                }
            }

            return false;
        }

        private Token Consume(TokenType type, string message)
        {
            if (Check(type)) return Advance();
            throw new ParseException(Peek(), message);
        }

        private void ConsumeStatementEnd(string message)
        {
            if (Match(TokenType.SEMICOLON, TokenType.NEWLINE)) return;
            if (IsAtEnd()) return;
            throw new ParseException(Peek(), message);
        }

        private bool Check(TokenType type)
        {
            if (IsAtEnd()) return false;
            return Peek().Type == type;
        }

        private Token Advance()
        {
            if (!IsAtEnd()) _current++;
            return Previous();
        }

        private bool IsAtEnd()
        {
            return Peek().Type == TokenType.EOF;
        }

        private Token Peek()
        {
            return _tokens[_current];
        }

        private Token Previous()
        {
            return _tokens[_current - 1];
        }

        private void Synchronize()
        {
            Advance();

            while (!IsAtEnd())
            {
                if (Previous().Type == TokenType.SEMICOLON) return;

                switch (Peek().Type)
                {
                    case TokenType.CLASS:
                    case TokenType.FUN:
                    case TokenType.IF:
                    case TokenType.WHILE:
                    case TokenType.GIVEBACK:
                        return;
                }

                Advance();
            }
        }
    }

    public class ParseException : Exception
    {
        public Token Token { get; }

        public ParseException(Token token, string message) : base(message)
        {
            Token = token;
        }
    }
}

