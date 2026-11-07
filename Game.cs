using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace ConsoleSlayer
{
    internal class Game
    {
        public Player Player { get; private set; }
        public bool Exited { get; set; }
        public List<GameItem> Items { get; set; }
        public List<Demon> Demons { get; set; }

        public Game(Player player)
        {
            Player = player;
            Items = new List<GameItem>();
            Demons = new List<Demon>();
        }

        private void RenderSingleSprite(Position position, ConsoleSprite sprite)
        {

            if (position.X >= 0 && position.X < Console.WindowWidth
                && position.Y >= 0 && position.Y < Console.WindowHeight)

            {
                Console.SetCursorPosition(position.X, position.Y);
                Console.BackgroundColor = sprite.BackgroundColor;
                Console.ForegroundColor = sprite.ForeGroundColor;

                Console.Write(sprite.Glyph);
                Console.ResetColor();

            }

        }

        private void RenderGame()
        {
            Console.Clear();
            Console.CursorVisible = false;
            Console.ResetColor();

            foreach (GameItem item in Items)
            {
                if (item is not null)
                {
                    RenderSingleSprite(item.Position, item.Sprite);
                }
            }

            foreach (Demon demon in Demons)
            {
                if (demon is not null)
                {
                    RenderSingleSprite(demon.Position, demon.Sprite);
                }
            }

            RenderSingleSprite(this.Player.Position, this.Player.Sprite);
        }

        private void CleanUpGameItems()
        {
            List<GameItem> notAvailableItems = new List<GameItem>();

            foreach (GameItem item in Items)
            {
                if (!item.Available)
                {
                    notAvailableItems.Add(item);
                }
            }

            foreach (GameItem item in notAvailableItems)
            {
                Items.Remove(item);
            }
        }

        private void CleanUpDemons()
        {
            List<Demon> notAliveDemons = new List<Demon>();

            foreach (Demon d in Demons)
            {
                if (!d.Alive)
                {
                    notAliveDemons.Add(d);
                }
            }

            foreach (Demon d in notAliveDemons)
            {
                Demons.Remove(d);
            }
        }

        private List<GameItem> GetGameItemsWithinDistance(Position position, double distance)
        {
            List<GameItem> itemsInRange = new List<GameItem>();

            foreach (GameItem item in Items)
            {
                double objectDistance = Position.Distance(item.Position, position);

                if (objectDistance <= distance)
                {
                    itemsInRange.Add(item);
                }
            }

            return itemsInRange;
        }

        private List<Demon> GetDemonsWithinDistance(Position position, double distance)
        {
            List<Demon> demonsInRange = new List<Demon>();

            foreach (Demon d in Demons)
            {
                double objectDistance = Position.Distance(d.Position, position);

                if (objectDistance <= distance)
                {
                    demonsInRange.Add(d);
                }
            }

            return demonsInRange;
        }

        private double GetTotalFillingRatio(Position position)
        {
            List<GameItem> itemsWithinDistance = GetGameItemsWithinDistance(position, 0);
            List<Demon> demonsWithinDistance = GetDemonsWithinDistance(position, 0);

            double totalFillingRatio = 0.0;

            foreach (GameItem item in itemsWithinDistance)
            {
                totalFillingRatio += item.FillingRatio;
            }

            foreach (Demon demon in demonsWithinDistance)
            {
                totalFillingRatio += demon.FillingRatio;
            }

            return totalFillingRatio;
        }

        private void Move(Player player, Position position)
        {
            double posFillingRatio = GetTotalFillingRatio(position);
            double userAndItemFillingRatio = posFillingRatio + Player.FillingRatio;

            if (userAndItemFillingRatio <= 1.0)
            {
                player.Position = position;
            }
        }

        private void Move(Demon demon, Position position)
        {
            double posFillingRatio = GetTotalFillingRatio(position);
            double userAndItemFillingRatio = posFillingRatio + demon.FillingRatio;
            
            if (userAndItemFillingRatio <= 1.0)
            {
                demon.Position = position;
            }
        }

        private void DemonMoveLogic(Demon demon)
        {
            Random rng = new Random();
            int x = demon.Position.X + rng.Next(-1, 2);
            int y = demon.Position.Y + rng.Next(-1, 2);

            Position targetPosition = new Position(x, y);
            Move(demon, targetPosition);
        }
        
        private void UpdateDemons()
        {
            foreach (Demon d in Demons)
            {
                if (d is not null)
                {
                    d.UpdateState(Player);

                    DemonStateType currentState = d.State;

                    if (currentState == DemonStateType.Move)
                    {
                        DemonMoveLogic(d);
                    }

                    else if (currentState == DemonStateType.Attack)
                    {
                        DemonAttackLogic(d);
                    }
                }
            }
        }

        private void UserAction()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo pressed = Console.ReadKey(true);

                switch (pressed.Key)
                {
                    case ConsoleKey.LeftArrow:
                        if (Player.Position.X > 0)
                        {
                            Position step = new Position(-1, 0);
                            Position targetPosition = Position.Add(Player.Position, step);

                            Move(Player, targetPosition);

                        }
                        break;

                    case ConsoleKey.RightArrow:
                        if (Player.Position.X < Console.WindowWidth - 1)
                        {
                            Position step = new Position(1, 0);
                            Position targetPosition = Position.Add(Player.Position, step);

                            Move(Player, targetPosition);
                        }
                        break;

                    case ConsoleKey.UpArrow:
                        if (Player.Position.Y > 0)
                        {
                            Position step = new Position(0, -1);
                            Position targetPosition = Position.Add(Player.Position, step);

                            Move(Player, targetPosition);
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        if (Player.Position.Y < Console.WindowHeight - 1)
                        {
                            Position step = new Position(0, 1);
                            Position targetPosition = Position.Add(Player.Position, step);

                            Move(Player, targetPosition);
                        }
                        break;

                    case ConsoleKey.D:
                        foreach (GameItem item in Items)
                        {
                            item.Interact();
                        }
                        break;

                    case ConsoleKey.A:
                        PlayerAttackLogic();
                        break;

                    case ConsoleKey.Escape:
                        Exited = true;
                        break;
                }
            }
        }

        public void PlayerAttackLogic()
        {
            if (Player.Ammo > 0)
            {
                Player.Shoot();
                List<Demon> demonsInDistance = GetDemonsWithinDistance(Player.Position, Player.SightRange);

                foreach (Demon demon in demonsInDistance)
                {
                    Random rng = new Random();
                    
                    int u = rng.Next(35, 105);

                    int randomDamage = 2 * u;
                    double damageDivider = 1 + Position.Distance(Player.Position, demon.Position);

                    double finalDamage = randomDamage / damageDivider;

                    demon.TakeDamage(finalDamage);

                    if (!demon.Alive)
                    {
                        switch (demon.Type)
                        {
                            case DemonType.Zombieman:
                                Player.AddCombatPoints(1);
                                break;

                            case DemonType.Imp:
                                Player.AddCombatPoints(3);
                                break;

                            case DemonType.Mancubus:
                                Player.AddCombatPoints(10);
                                break;

                        }
                    }
                }
            }
        }

        public void DemonAttackLogic(Demon demon)
        {
            double distanceFormPlayer = Position.Distance(Player.Position, demon.Position);

            Random rng = new Random();
            int u = 0;

            switch (demon.Type)
            {
                case DemonType.Imp:
                    u = rng.Next(3, 24);
                    break;

                case DemonType.Zombieman:
                    u = rng.Next(3, 15);
                    break;

                case DemonType.Mancubus:
                    u = rng.Next(8, 64);
                    break;
            }
            
            int randomDamage = 2 * u;
            double damageDivider = 1 + distanceFormPlayer;

            double finalDamage = randomDamage / damageDivider;

            Player.TakeDamage(finalDamage);
        }
       
        public void Run()
        {
            while (!Exited && Player.Alive)
            {
                RenderGame();
                UserAction();

                UpdateDemons();
                CleanUpDemons();

                CleanUpGameItems();
                Thread.Sleep(20);
            }

            if (!Player.Alive)
            {
                Console.Clear();
                Console.WriteLine("YOU ARE DEAD");
            }
        }
    }
}