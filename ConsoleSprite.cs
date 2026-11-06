using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSlayer
{
    internal class ConsoleSprite
    {
        public ConsoleColor BackgroundColor { get; set; }
        public ConsoleColor ForeGroundColor { get; set; }
        public char Glyph { get; set; }

        public ConsoleSprite(ConsoleColor backgroundColor, ConsoleColor foreGroundColor, char glyph)
        {
            this.BackgroundColor = backgroundColor;
            this.ForeGroundColor = foreGroundColor;
            this.Glyph = glyph;
        }
    }
}
