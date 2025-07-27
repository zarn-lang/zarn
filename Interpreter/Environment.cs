using ZARN.Lexer;

namespace ZARN.Interpreter
{
    public class Environment
    {
        private readonly Environment? _enclosing;
        private readonly Dictionary<string, object?> _values = new();

        public Environment()
        {
            _enclosing = null;
        }

        public Environment(Environment enclosing)
        {
            _enclosing = enclosing;
        }

        public void Define(string name, object? value)
        {
            _values[name] = value;
        }

        public object? Get(Token name)
        {
            if (_values.TryGetValue(name.Lexeme, out object? value))
            {
                return value;
            }

            if (_enclosing != null)
            {
                return _enclosing.Get(name);
            }

            throw new RuntimeException(name, $"Undefined variable '{name.Lexeme}'.");
        }

        public void Assign(Token name, object? value)
        {
            if (_values.ContainsKey(name.Lexeme))
            {
                _values[name.Lexeme] = value;
                return;
            }

            if (_enclosing != null)
            {
                try
                {
                    _enclosing.Assign(name, value);
                    return;
                }
                catch (RuntimeException)
                {
                    // If variable doesn't exist in enclosing scope, create it in current scope
                }
            }

            // Auto-create variable if it doesn't exist (Python-like behavior)
            _values[name.Lexeme] = value;
        }
    }

    public class RuntimeException : Exception
    {
        public Token Token { get; }

        public RuntimeException(Token token, string message) : base(message)
        {
            Token = token;
        }
    }
}

