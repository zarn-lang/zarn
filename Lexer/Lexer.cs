using System.Text;

namespace ZARN.Lexer
{
    public class Lexer
    {
        private readonly string _source;
        private readonly List<Token> _tokens = new();
        private int _start = 0;
        private int _current = 0;
        private int _line = 1;
        private int _column = 1;

        private static readonly Dictionary<string, TokenType> _keywords = new()
        {
            { "fun", TokenType.FUN },
            { "if", TokenType.IF },
            { "else", TokenType.ELSE },
            { "while", TokenType.WHILE },
            { "giveback", TokenType.GIVEBACK },
            { "nothing", TokenType.NOTHING },
            { "this", TokenType.THIS },
            { "class", TokenType.CLASS },
            { "use", TokenType.USE },
            { "try", TokenType.TRY },
            { "catch", TokenType.CATCH }
        };

        public Lexer(string source)
        {
            _source = source;
        }

        public List<Token> ScanTokens()
        {
            while (!IsAtEnd())
            {
                _start = _current;
                ScanToken();
            }

            _tokens.Add(new Token(TokenType.EOF, "", null, _line, _column));
            return _tokens;
        }

        private void ScanToken()
        {
            char c = Advance();

            switch (c)
            {
                case ' ':
                case '\r':
                case '\t':
                    // Ignore whitespace
                    break;
                case '\n':
                    AddToken(TokenType.NEWLINE);
                    _line++;
                    _column = 1;
                    break;
                case '(':
                    AddToken(TokenType.LEFT_PAREN);
                    break;
                case ')':
                    AddToken(TokenType.RIGHT_PAREN);
                    break;
                case '{':
                    AddToken(TokenType.LEFT_BRACE);
                    break;
                case '}':
                    AddToken(TokenType.RIGHT_BRACE);
                    break;
                case '[':
                    AddToken(TokenType.LEFT_BRACKET);
                    break;
                case ']':
                    AddToken(TokenType.RIGHT_BRACKET);
                    break;
                case ',':
                    AddToken(TokenType.COMMA);
                    break;
                case '.':
                    AddToken(TokenType.DOT);
                    break;
                case '-':
                    AddToken(TokenType.MINUS);
                    break;
                case '+':
                    AddToken(TokenType.PLUS);
                    break;
                case ';':
                    AddToken(TokenType.SEMICOLON);
                    break;
                case '*':
                    AddToken(TokenType.MULTIPLY);
                    break;
                case '/':
                    if (Match('/'))
                    {
                        // Comment goes until end of line
                        while (Peek() != '\n' && !IsAtEnd()) Advance();
                    }
                    else
                    {
                        AddToken(TokenType.DIVIDE);
                    }
                    break;
                case '!':
                    AddToken(Match('=') ? TokenType.NOT_EQUAL : TokenType.NOT);
                    break;
                case '=':
                    AddToken(Match('=') ? TokenType.EQUAL : TokenType.ASSIGN);
                    break;
                case '<':
                    AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
                    break;
                case '>':
                    AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
                    break;
                case '&':
                    if (Match('&'))
                    {
                        AddToken(TokenType.AND);
                    }
                    else
                    {
                        throw new Exception($"Unexpected character '&' at line {_line}, column {_column}");
                    }
                    break;
                case '|':
                    if (Match('|'))
                    {
                        AddToken(TokenType.OR);
                    }
                    else
                    {
                        throw new Exception($"Unexpected character '|' at line {_line}, column {_column}");
                    }
                    break;
                case '"':
                    ScanString();
                    break;
                default:
                    if (IsDigit(c))
                    {
                        ScanNumber();
                    }
                    else if (IsAlpha(c))
                    {
                        ScanIdentifier();
                    }
                    else
                    {
                        throw new Exception($"Unexpected character '{c}' at line {_line}, column {_column}");
                    }
                    break;
            }
        }

        private void ScanString()
        {
            while (Peek() != '"' && !IsAtEnd())
            {
                if (Peek() == '\n')
                {
                    _line++;
                    _column = 1;
                }
                Advance();
            }

            if (IsAtEnd())
            {
                throw new Exception($"Unterminated string at line {_line}");
            }

            // The closing "
            Advance();

            // Trim the surrounding quotes
            string value = _source.Substring(_start + 1, _current - _start - 2);
            AddToken(TokenType.STRING, value);
        }

        private void ScanNumber()
        {
            while (IsDigit(Peek())) Advance();

            // Look for a fractional part
            if (Peek() == '.' && IsDigit(PeekNext()))
            {
                // Consume the "."
                Advance();

                while (IsDigit(Peek())) Advance();
            }

            string text = _source.Substring(_start, _current - _start);
            AddToken(TokenType.NUMBER, double.Parse(text));
        }

        private void ScanIdentifier()
        {
            while (IsAlphaNumeric(Peek())) Advance();

            string text = _source.Substring(_start, _current - _start);
            TokenType type = _keywords.GetValueOrDefault(text, TokenType.IDENTIFIER);
            AddToken(type);
        }

        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private bool IsAlpha(char c)
        {
            return (c >= 'a' && c <= 'z') ||
                   (c >= 'A' && c <= 'Z') ||
                   c == '_';
        }

        private bool IsAlphaNumeric(char c)
        {
            return IsAlpha(c) || IsDigit(c);
        }

        private bool Match(char expected)
        {
            if (IsAtEnd()) return false;
            if (_source[_current] != expected) return false;

            _current++;
            _column++;
            return true;
        }

        private char Peek()
        {
            if (IsAtEnd()) return '\0';
            return _source[_current];
        }

        private char PeekNext()
        {
            if (_current + 1 >= _source.Length) return '\0';
            return _source[_current + 1];
        }

        private char Advance()
        {
            _column++;
            return _source[_current++];
        }

        private void AddToken(TokenType type)
        {
            AddToken(type, null);
        }

        private void AddToken(TokenType type, object? literal)
        {
            string text = _source.Substring(_start, _current - _start);
            _tokens.Add(new Token(type, text, literal, _line, _column - text.Length));
        }

        private bool IsAtEnd()
        {
            return _current >= _source.Length;
        }
    }
}

