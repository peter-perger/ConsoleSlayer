using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ConsoleSlayer
{
    enum DemonType { Zombieman, Imp, Mancubus }
    enum DemonStateType { Idle, Move, Attack}
    internal class Demon
    {
        int health;
        public Position Position { get; set; }
        public ConsoleSprite Sprite { get; private set; }
        public DemonType Type { get; private set; }
        public double FillingRatio { get; set; }
        public int Health { 
            get;

            set
            {
                if (value >= 0)
                {
                    health = value;
                }
            }
        }
        public bool Alive { get; set; } = true;
        public int SightRange { get; set; }
        public int AttackRange { get; set; }

        public Demon (int x, int y, DemonType type)
        {
            Position = new Position(x, y);
            Type = type;
            State = DemonStateType.Idle;
            SetInitialProperties();
        }

        public DemonStateType State { get; set; }

        private void SetInitialProperties ()
        {
            switch (Type)
            {
                case DemonType.Zombieman:
                    FillingRatio = 0.4;
                    Health = 10;
                    SightRange = 3;
                    AttackRange = 1;
                    Sprite = new ConsoleSprite(ConsoleColor.Black, ConsoleColor.White, 'o');

                    break;

                case DemonType.Imp:
                    FillingRatio = 0.4;
                    Health = 12;
                    SightRange = 6;
                    AttackRange = 3;
                    Sprite = new ConsoleSprite(ConsoleColor.Black, ConsoleColor.Red, 'o');

                    break;

                case DemonType.Mancubus:
                    FillingRatio = 0.9;
                    Health = 15;
                    SightRange = 9;
                    AttackRange = 6;
                    Sprite = new ConsoleSprite(ConsoleColor.Black, ConsoleColor.Magenta, 'O');

                    break;
            }
        }

        public void UpdateState (Player player)
        {
            double distanceBetween = Position.Distance(Position, player.Position);

            if(distanceBetween < AttackRange)
            {
                State = DemonStateType.Attack;
            }
            else if (distanceBetween < SightRange)
            {
                State = DemonStateType.Move;
            }
            else
            {
                State = DemonStateType.Idle;
            }
        }

        public void TakeDamage(double value)
        {
            Health -= (int)value;

            if (Health <= 0)
            {
                Alive = false;
            }
        }
    }
}
