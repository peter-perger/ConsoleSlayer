using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSlayer
{
    enum ItemType { Ammo, BFGCell, Door, LevelExit, Medikit, ToxicWaste, Wall }

    internal class GameItem
    {
        Position position;
        ConsoleSprite sprite;
        ItemType type;
        double fillingRatio;

        public GameItem(int x, int y, ItemType type)
        {
            Type = type;
            Position = new Position(x, y);
            SetInitialProperties();
        }

        public Position Position { get; private set; }
        public ConsoleSprite Sprite { get; private set; }
        public ItemType Type { get; private set; }
        public double FillingRatio { get; set; }
        public bool Available { get; private set; } = true;

        private void SetInitialProperties()
        {
            switch (Type)
            {
                case ItemType.Ammo:
                    FillingRatio = 0.0;
                    Sprite = new ConsoleSprite(ConsoleColor.Red, ConsoleColor.Yellow, 'A');
                    
                    break;

                case ItemType.BFGCell:
                    FillingRatio = 0.0;
                    Sprite = new ConsoleSprite(ConsoleColor.Green, ConsoleColor.White, 'B');

                    break;

                case ItemType.Door:
                    FillingRatio = 1.0;
                    Sprite = new ConsoleSprite(ConsoleColor.Gray, ConsoleColor.Yellow, '/');

                    break;

                case ItemType.LevelExit:
                    FillingRatio = 1.0;
                    Sprite = new ConsoleSprite(ConsoleColor.Blue, ConsoleColor.Black, 'E');

                    break;

                case ItemType.Medikit:
                    FillingRatio = 0.0;
                    Sprite = new ConsoleSprite(ConsoleColor.Gray, ConsoleColor.Red, '+');

                    break;

                case ItemType.ToxicWaste:
                    FillingRatio = 0.0;
                    Sprite = new ConsoleSprite(ConsoleColor.DarkGreen, ConsoleColor.Green, ':');

                    break;

                case ItemType.Wall:
                    FillingRatio = 1.0;
                    Sprite = new ConsoleSprite(ConsoleColor.Gray, ConsoleColor.Gray, ' ');

                    break;

            }
        }
        public void Interact()
        {
            if (Type == ItemType.Ammo ||
                Type == ItemType.BFGCell ||
                Type == ItemType.Medikit)
            {
                Available = false;
            } 
            else if (Type == ItemType.Door)
            {
                //Change FillingRatio
                FillingRatio = (FillingRatio == 0)
                    ?
                    FillingRatio = 1.0
                    :
                    FillingRatio = 0.0;

                //Change Srpite
                if (FillingRatio == 0.0)
                {
                    Sprite = new ConsoleSprite(ConsoleColor.Gray, ConsoleColor.Blue, '/');
                }
                else if (FillingRatio == 1.0)
                {
                    Sprite = new ConsoleSprite(ConsoleColor.Gray, ConsoleColor.Yellow, '/');
                }
            }
            
        }

    }
}
