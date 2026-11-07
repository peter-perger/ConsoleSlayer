using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSlayer
{
    internal class Player
    {
        int health;
        int ammo;

        public Position Position { get; set; }
        public ConsoleSprite Sprite { get; set; }
        public double FillingRatio { get; set; }
        public int Health
        {
            get { return health; }
            
            set
            {
                // If damage pushes health below 0, stop exactly at 0 (Dead)
                if (value < 0)
                {
                    health = 0;
                }
                // If a medikit overheals, stop at MaxHealth
                else if (value > MaxHealth)
                {
                    health = MaxHealth;
                }
                // Normal health update
                else
                {
                    health = value;
                }
            }
        }
        public int Ammo {
            get;

            set
            {
                if (value <= MaxAmmo && value >= 0)
                {
                    ammo = value;
                }
            }
        }
        public bool Alive { get; set; }
        public int CombatPoints { get; set; }
        public int MaxHealth => 100 +  (CombatPoints / 10);
        public int MaxAmmo => 10 + (CombatPoints / 50);
        public int SightRange { get; set; }

        public Player (int x, int y)
        {
            this.Position = new Position(x, y);
            this.Sprite = new ConsoleSprite(ConsoleColor.Black, ConsoleColor.Green, 'O');
            this.FillingRatio = 0.4;
            this.Health = 100;
            this.SightRange = 8;
            this.Alive = true;
            this.Ammo = 150;
        }

        public void Shoot()
        {
            Ammo -= 1;
        }

        public void AddCombatPoints(int value)
        {
            CombatPoints += value;
        }

        public void TakeDamage (double value)
        {
            Health -= (int)value;

            if (Health == 0)
            {
                Alive = false;
            }
        }
    }
}
