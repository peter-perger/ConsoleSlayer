using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSlayer
{
    internal class GameLogic
    {
        Game game;

        public GameLogic (Game Game)
        {
            this.game = Game;
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
        
        private void CleanUpGameItems()
        {
            List<GameItem> notAvailableItems = new List<GameItem>();

            foreach (GameItem item in game.Items)
            {
                if (!item.Available)
                {
                    notAvailableItems.Add(item);
                }
            }

            foreach (GameItem item in notAvailableItems)
            {
                game.Items.Remove(item);
            }
        }

        public void PlayerAttackLogic()
        {
            if (game.Player.Ammo > 0)
            {
                game.Player.Shoot();
                List<Demon> demonsInDistance = GetDemonsWithinDistance(game.Player.Position, game.Player.SightRange);

                foreach (Demon demon in demonsInDistance)
                {
                    Random rng = new Random();

                    int u = rng.Next(35, 105);

                    int randomDamage = 2 * u;
                    double damageDivider = 1 + Position.Distance(game.Player.Position, demon.Position);

                    double finalDamage = randomDamage / damageDivider;

                    demon.TakeDamage(finalDamage);

                    if (!demon.Alive)
                    {
                        switch (demon.Type)
                        {
                            case DemonType.Zombieman:
                                game.Player.AddCombatPoints(1);
                                break;

                            case DemonType.Imp:
                                game.Player.AddCombatPoints(3);
                                break;

                            case DemonType.Mancubus:
                                game.Player.AddCombatPoints(10);
                                break;

                        }
                    }
                }
            }
        }

        public void Move(Player player, Position position)
        {
            double posFillingRatio = GetTotalFillingRatio(position);
            double userAndItemFillingRatio = posFillingRatio + game.Player.FillingRatio;

            if (userAndItemFillingRatio <= 1.0)
            {
                player.Position = position;
            }
        }

        public void Move(Demon demon, Position position)
        {
            double posFillingRatio = GetTotalFillingRatio(position);
            double userAndItemFillingRatio = posFillingRatio + demon.FillingRatio;

            if (userAndItemFillingRatio <= 1.0)
            {
                demon.Position = position;
            }
        }

        private void CleanUpDemons()
        {
            List<Demon> notAliveDemons = new List<Demon>();

            foreach (Demon d in game.Demons)
            {
                if (!d.Alive)
                {
                    notAliveDemons.Add(d);
                }
            }

            foreach (Demon d in notAliveDemons)
            {
                game.Demons.Remove(d);
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

        public void DemonAttackLogic(Demon demon)
        {
            double distanceFormPlayer = Position.Distance(game.Player.Position, demon.Position);

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

            game.Player.TakeDamage(finalDamage);
        }

        private List<GameItem> GetGameItemsWithinDistance(Position position, double distance)
        {
            List<GameItem> itemsInRange = new List<GameItem>();

            foreach (GameItem item in game.Items)
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

            foreach (Demon d in game.Demons)
            {
                double objectDistance = Position.Distance(d.Position, position);

                if (objectDistance <= distance)
                {
                    demonsInRange.Add(d);
                }
            }

            return demonsInRange;
        }
        
        private void UpdateDemons()
        {
            foreach (Demon d in game.Demons)
            {
                if (d is not null)
                {
                    d.UpdateState(game.Player);

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
    
        public void UpdateGameState()
        {
            UpdateDemons();
            CleanUpGameItems();
            CleanUpDemons();
        }
    }
}
