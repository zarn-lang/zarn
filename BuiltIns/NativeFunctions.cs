using ZARN.Interpreter;
using ZARN.Lexer;

namespace ZARN.BuiltIns
{
    public class SayFunction : IZarnCallable
    {
        public int Arity => 1;

        public object? Call(Interpreter.Interpreter interpreter, List<object?> arguments)
        {
            Console.WriteLine(Stringify(arguments[0]));
            return null;
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

        public override string ToString()
        {
            return "<native fn say>";
        }
    }

    public class InputFunction : IZarnCallable
    {
        public int Arity => 1;

        public object? Call(Interpreter.Interpreter interpreter, List<object?> arguments)
        {
            string prompt = Stringify(arguments[0]);
            Console.Write(prompt);
            string? input = Console.ReadLine();
            return input ?? "";
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
            return obj.ToString() ?? "nothing";
        }

        public override string ToString()
        {
            return "<native fn input>";
        }
    }

    public class AppendFunction : IZarnCallable
    {
        public int Arity => 2;

        public object? Call(Interpreter.Interpreter interpreter, List<object?> arguments)
        {
            if (arguments[0] is not List<object?> list)
            {
                throw new RuntimeException(new Token(TokenType.IDENTIFIER, "append", null, 0, 0), 
                    "First argument to append() must be a list.");
            }

            list.Add(arguments[1]);
            return null;
        }

        public override string ToString()
        {
            return "<native fn append>";
        }
    }

    public class GetFunction : IZarnCallable
    {
        public int Arity => 2;

        public object? Call(Interpreter.Interpreter interpreter, List<object?> arguments)
        {
            if (arguments[0] is not List<object?> list)
            {
                throw new RuntimeException(new Token(TokenType.IDENTIFIER, "get", null, 0, 0), 
                    "First argument to get() must be a list.");
            }

            if (arguments[1] is not double indexNum)
            {
                throw new RuntimeException(new Token(TokenType.IDENTIFIER, "get", null, 0, 0), 
                    "Second argument to get() must be a number.");
            }

            int index = (int)indexNum;
            if (index < 0 || index >= list.Count)
            {
                throw new RuntimeException(new Token(TokenType.IDENTIFIER, "get", null, 0, 0), 
                    "List index out of bounds.");
            }

            return list[index];
        }

        public override string ToString()
        {
            return "<native fn get>";
        }
    }

    public class SetFunction : IZarnCallable
    {
        public int Arity => 3;

        public object? Call(Interpreter.Interpreter interpreter, List<object?> arguments)
        {
            if (arguments[0] is not List<object?> list)
            {
                throw new RuntimeException(new Token(TokenType.IDENTIFIER, "set", null, 0, 0), 
                    "First argument to set() must be a list.");
            }

            if (arguments[1] is not double indexNum)
            {
                throw new RuntimeException(new Token(TokenType.IDENTIFIER, "set", null, 0, 0), 
                    "Second argument to set() must be a number.");
            }

            int index = (int)indexNum;
            if (index < 0 || index >= list.Count)
            {
                throw new RuntimeException(new Token(TokenType.IDENTIFIER, "set", null, 0, 0), 
                    "List index out of bounds.");
            }

            list[index] = arguments[2];
            return null;
        }

        public override string ToString()
        {
            return "<native fn set>";
        }
    }

    public class LenFunction : IZarnCallable
    {
        public int Arity => 1;

        public object? Call(Interpreter.Interpreter interpreter, List<object?> arguments)
        {
            if (arguments[0] is List<object?> list)
            {
                return (double)list.Count;
            }

            if (arguments[0] is string str)
            {
                return (double)str.Length;
            }

            throw new RuntimeException(new Token(TokenType.IDENTIFIER, "len", null, 0, 0), 
                "Argument to len() must be a list or string.");
        }

        public override string ToString()
        {
            return "<native fn len>";
        }
    }
}

