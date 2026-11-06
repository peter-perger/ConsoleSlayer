using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSlayer
{
    internal class Player
    {
        public Position Position { get; set; }
        public ConsoleSprite Sprite { get; set; }
        public double FillingRatio { get; set; }

        public Player (int x, int y)
        {
            this.Position = new Position(x, y);
            this.Sprite = new ConsoleSprite(ConsoleColor.Black, ConsoleColor.Green, 'O');
            this.FillingRatio = 0.4;
        }
    }
}
