namespace ZARN.Lexer
{
    public enum TokenType
    {
        // Literals
        IDENTIFIER,
        STRING,
        NUMBER,
        
        // Keywords
        FUN,
        IF,
        ELSE,
        WHILE,
        GIVEBACK,
        NOTHING,
        THIS,
        CLASS,
        USE,
        TRY,
        CATCH,
        
        // Operators
        PLUS,
        MINUS,
        MULTIPLY,
        DIVIDE,
        ASSIGN,
        EQUAL,
        NOT_EQUAL,
        GREATER,
        GREATER_EQUAL,
        LESS,
        LESS_EQUAL,
        AND,
        OR,
        NOT,
        
        // Delimiters
        LEFT_PAREN,
        RIGHT_PAREN,
        LEFT_BRACE,
        RIGHT_BRACE,
        LEFT_BRACKET,
        RIGHT_BRACKET,
        COMMA,
        SEMICOLON,
        DOT,
        
        // Special
        EOF,
        NEWLINE
    }

    public class Token
    {
        public TokenType Type { get; }
        public string Lexeme { get; }
        public object? Literal { get; }
        public int Line { get; }
        public int Column { get; }

        public Token(TokenType type, string lexeme, object? literal, int line, int column)
        {
            Type = type;
            Lexeme = lexeme;
            Literal = literal;
            Line = line;
            Column = column;
        }

        public override string ToString()
        {
            return $"{Type} {Lexeme} {Literal}";
        }
    }
}

