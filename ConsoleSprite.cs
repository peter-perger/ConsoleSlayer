using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSlayer
{
    internal class ConsoleSprite
    {
        ConsoleColor backgroundColor;
        ConsoleColor foreGroundColor;
        char glyph;

        public ConsoleSprite(ConsoleColor backgroundColor, ConsoleColor foreGroundColor, char Glyph)
        {
            this.BackgroundColor = backgroundColor;
            this.ForeGroundColor = foreGroundColor;
            this.glyph = Glyph;
        }

        public ConsoleColor BackgroundColor { get => backgroundColor; set => backgroundColor = value; }
        public ConsoleColor ForeGroundColor { get => foreGroundColor; set => foreGroundColor = value; }
        public char Glyph { get => glyph; set => glyph = value; }
    }
}
