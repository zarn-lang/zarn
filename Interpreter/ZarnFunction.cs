using ZARN.AST;
using ZARN.Lexer;

namespace ZARN.Interpreter
{
    public interface IZarnCallable
    {
        int Arity { get; }
        object? Call(Interpreter interpreter, List<object?> arguments);
    }

    public class ZarnFunction : IZarnCallable
    {
        private readonly Function _declaration;
        private readonly Environment _closure;

        public ZarnFunction(Function declaration, Environment closure)
        {
            _declaration = declaration;
            _closure = closure;
        }

        public int Arity => _declaration.Params.Count;

        public object? Call(Interpreter interpreter, List<object?> arguments)
        {
            Environment environment = new Environment(_closure);
            
            for (int i = 0; i < _declaration.Params.Count; i++)
            {
                environment.Define(_declaration.Params[i].Lexeme, arguments[i]);
            }

            try
            {
                interpreter.ExecuteBlock(_declaration.Body, environment);
            }
            catch (ReturnException returnValue)
            {
                return returnValue.Value;
            }

            return null;
        }

        public override string ToString()
        {
            return $"<fn {_declaration.Name.Lexeme}>";
        }
    }

    public class ReturnException : Exception
    {
        public object? Value { get; }

        public ReturnException(object? value)
        {
            Value = value;
        }
    }
}

