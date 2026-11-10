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
            get => health;
            
            set
            {
                if (value + health >= MaxHealth)
                {
                    health = MaxHealth;
                }
                else
                {
                    health += value;
                }
            }
        }
        public int Ammo 
        {
            get => ammo;

            set
            {
                if (value < 0)
                {
                    ammo = 0;
                }
                else if (value > MaxAmmo)
                {
                    ammo = MaxAmmo;
                }
                else
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
            this.Ammo = MaxAmmo;
            this.Alive = true;
        }

        public void Shoot()
        {
            Ammo -= 1;
        }

        public void AddCombatPoints(int value)
        {
            CombatPoints += value;
        }

        public void TakeDamage(double value)
        {
            Health -= (int)value;

            if (Health == 0)
            {
                Alive = false;
            }
        }
    
        public void PickupAmmo(int amount)
        {
            if (amount > 0)
            {
                Ammo += amount;
            }
        }

        public void PickupHealth(int amount)
        {
            if (amount > 0)
            {
                Health += amount;
            }
        }
    }
}
