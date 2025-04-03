using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BISAYA__
{
    internal class Scanner
    {
        private static readonly Dictionary<string, TokenType> keywords;
        private string source { get; }
        private List<Token> tokens = new List<Token>();
        private int start = 0;
        private int current = 0;
        private int line = 1;

        // Static constructor (runs once when class is first accessed)
        static Scanner()
        {
            keywords = new Dictionary<string, TokenType> 
            {
                { "UG", TokenType.UG },
                { "KLASE", TokenType.KLASE },
                { "KUNG WALA", TokenType.KUNG_WALA },
                { "DILI", TokenType.DILI },
                { "ALANG SA", TokenType.ALANG_SA },
                { "KUNG", TokenType.KUNG },
                { "WALA", TokenType.WALA },
                { "O", TokenType.O },
                { "IPAKITA", TokenType.IPAKITA },
                { "OO", TokenType.OO }
            };
        }

        public Scanner(string source)
        {
            this.source = source;

        }

        public List<Token> scanTokens()
        {
            while(!isAtEnd())
            {
                start = current;
                scanToken();
            }

            tokens.Add(new Token(TokenType.EOF, "", null, line));
            return tokens;
        }

        private bool isAtEnd()
        {
            return current >= source.Length;
        }

        private char advance()
        {
            return source[current++];
        }

        private void addToken(TokenType type)
        {
            addToken(type, null);
        }

        private void addToken(TokenType type, object literal)
        {
            string text = source.Substring(start, current - start);
            tokens.Add(new Token(type, text, literal, line));
        }

        private void scanToken()
        {
            char c = advance();
            switch(c)
            {
                case '(': addToken(TokenType.WALA_NA_PAREN); break;
                case ')': addToken(TokenType.TUO_NA_PAREN); break;
                case '{': addToken(TokenType.WALA_NA_BRACE); break;
                case '}': addToken(TokenType.TUO_NA_BRACE); break;
                case '[': addToken(TokenType.WALA_NA_BRACKET); break;
                case ']': addToken(TokenType.TUO_NA_BRACKET); break;
                case ',': addToken(TokenType.COMMA); break;
                case '.': addToken(TokenType.DOT); break;
                case '-': 
                    if (match('-'))
                    {
                        // A comment goes until the end of the line.
                        while (peek() != '\n' && !isAtEnd()) advance();
                    }
                    else
                    {
                        addToken(TokenType.MINUS);
                    }
                    break;
                case '+': addToken(TokenType.PLUS); break;
                case ';': addToken(TokenType.SEMICOLON); break;
                case '*': addToken(TokenType.STAR); break;
                case '!':
                    addToken(match('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
                    break;
                case '=':
                    addToken(match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
                    break;
                case '<':
                    addToken(match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
                    break;
                case '>':
                    addToken(match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
                    break;
                case '/':
                    addToken(TokenType.SLASH);
                    break;
                case ' ':
                //case '\r':
                //case '\t':
                    // Ignore whitespace.
                    break;
                case '$':
                    line++;
                    break;
                case '"': String(); break;
                default:
                    if (isDigit(c))
                    {
                        number();
                    }
                    else if (isAlpha(c))
                    {
                        identifier();
                    }
                    else
                    {
                        BISAYA.Error(line, "Unexpected character.");
                    }
                    //BISAYA.Error(line, "Unexpected character.");
                    break;
            }
        }

        private void identifier()
        {
            while (isAlphaNumeric(peek())) advance();
            string text = source.Substring(start, current-start);
            if (!keywords.TryGetValue(text, out TokenType type))
            {
                type = TokenType.IDENTIFIER;
            }
            addToken(type);
        }

        private void number()
        {
            while (isDigit(peek())) advance();
            // Look for a fractional part.
            if (peek() == '.' && isDigit(peekNext()))
            {
                // Consume the "."
                advance();
                while (isDigit(peek())) advance();
            }
            addToken(TokenType.NUMERO,
            double.Parse(source.Substring(start, current-start)));
        }

        private void String() {
            while (peek() != '"' && !isAtEnd()) {
                if (peek() == '\n') line++;
                advance();
            }
            if (isAtEnd())
            {
                BISAYA.Error(line, "Unterminated string.");
                return;
            }

            // The closing ".
            advance();
            // Trim the surrounding quotes.
            string value = source.Substring(start + 1, current - 1);
            addToken(TokenType.STRING, value);
        }

        private bool match(char expected)
        {
            if (isAtEnd()) return false;
            if (source[current] != expected) return false;
            current++;
            return true;
        }

        private char peek()
        {
            if (isAtEnd()) return '\0';
            return source[current];
        }

        private char peekNext()
        {
            if (current + 1 >= source.Length) return '\0';
            return source[current + 1];
        }

        private bool isAlpha(char c)
        {
            return (c >= 'a' && c <= 'z') ||
            (c >= 'A' && c <= 'Z') ||
            c == '_';
        }
        private bool isAlphaNumeric(char c)
        {
            return isAlpha(c) || isDigit(c);
        }

        private bool isDigit(char c)
        {
            return c >= '0' && c <= '9';
        }
    }
}
