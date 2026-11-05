using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSlayer
{
    internal class Player
    {
        Position position;
        ConsoleSprite sprite;

        public Position Position { get => position; set => position = value; }
        public ConsoleSprite Sprite { get => sprite; set => sprite = value; }

        public Player (int x, int y)
        {
            this.Position = new Position(x, y);
            this.Sprite = new ConsoleSprite(ConsoleColor.Black, ConsoleColor.Green, 'O');
        }
    }
}
