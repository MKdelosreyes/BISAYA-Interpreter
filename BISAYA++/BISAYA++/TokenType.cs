using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BISAYA__
{
    enum TokenType
    {
        // Single-character tokens.
        WALA_NA_PAREN, TUO_NA_PAREN, WALA_NA_BRACE, TUO_NA_BRACE,
        WALA_NA_BRACKET, TUO_NA_BRACKET,
        COMMA, DOT, MINUS, PLUS, SEMICOLON, SLASH, STAR, CONCAT, 
        // One or two character tokens.
        BANG, BANG_EQUAL,
        EQUAL, EQUAL_EQUAL,
        GREATER, GREATER_EQUAL,
        LESS, LESS_EQUAL,
        // Literals.
        //IDENTIFIER, STRING, NUMBER,
        IDENTIFIER, NUMERO, LETRA, TINUOD, TIPIK, WALA, STRING,


        // Keywords.
        UG, KLASE, KUNG_WALA, DILI, ALANG_SA, KUNG, NIL, O,
        IPAKITA, OO, SUGOD, KATAPUSAN, MUGNA, EOF
        // RETURN,SUPER, THIS, VAR, WHILE,EOF
    }

}
