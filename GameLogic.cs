using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
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

        private bool IsInsideMap(Position position)
        {
            return position.X >= 0
                && position.X < game.MapWidth
                && position.Y >= 0
                && position.Y <= game.MapHeight;
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
            if (Position.Distance(game.Player.Position, position) == 0)
            {
                totalFillingRatio += game.Player.FillingRatio;
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
            if(!IsInsideMap(position))
            {
                return;
            }

            if(Position.Distance(player.Position, position) == 0)
            {
                return;
            }

            double totalFillingRatio = GetTotalFillingRatio(position) + player.FillingRatio;


            if (totalFillingRatio <= 1.0)
            {
                player.Position = position;
            }
        }

        public void Move(Demon demon, Position position)
        {
            if(!IsInsideMap(position))
            {
                return;
            }

            if(Position.Distance(demon.Position, position) == 0)
            {
                return;
            }

            double totalFillingRatio = GetTotalFillingRatio(position) + demon.FillingRatio;

            if (totalFillingRatio <= 1.0)
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
                if(!item.Available)
                {
                    continue;
                }

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

            foreach (Demon demon in game.Demons)
            {
                if (!demon.Alive)
                {
                    continue;
                }

                double objectDistance = Position.Distance(demon.Position, position);

                if (objectDistance <= distance)
                {
                    demonsInRange.Add(demon);
                }
            }

            return demonsInRange;
        }
        
        private void UpdateDemons()
        {
            foreach (Demon demon in game.Demons)
            {
                if (demon is null || !demon.Alive)
                {
                    continue;
                }

                demon.UpdateState(game.Player);

                if (demon.State == DemonStateType.Move)
                {
                    DemonMoveLogic(demon);
                }
                else if (demon.State == DemonStateType.Attack)
                {
                    DemonAttackLogic(demon);
                }

                DemonIndirectInteractionLogic(demon);
            }
        }
    
        public void UpdateGameState()
        {
            UpdateDemons();
            CleanUpGameItems();
            CleanUpDemons();
        }

        public void PlayerDirectInteractionLogic()
        {
            List<GameItem> items = GetGameItemsWithinDistance(this.game.Player.Position, 1);

            foreach(GameItem item in items)
            {
                switch (item.Type)
                {
                    case ItemType.Door:
                        item.Interact();
                        break;

                    case ItemType.LevelExit:
                        item.Interact();
                        game.Exited = true;
                        break;
                }
            }
        }

        public void PlayerIndirectInteractionLogic()
        {
            List<GameItem> items = GetGameItemsWithinDistance(this.game.Player.Position, 0);

            foreach(GameItem item in items)
            {
                switch (item.Type)
                {
                    case ItemType.Ammo:
                        if (game.Player.Ammo < game.Player.MaxAmmo)
                        {
                            game.Player.PickupAmmo(5);
                            item.Interact();
                        }

                        break;

                    case ItemType.Medikit:
                        if(game.Player.Health < game.Player.MaxHealth)
                        {
                            game.Player.PickupHealth(25);
                            item.Interact();
                        }

                        break;

                    case ItemType.ToxicWaste:
                        item.Interact();
                        game.Player.TakeDamage(5);
                        break;
                }
            }
        }

        private void DemonIndirectInteractionLogic(Demon d)
        {
            List<GameItem> items = GetGameItemsWithinDistance(d.Position, 0);

            foreach (GameItem item in items)
            {
                if (item.Type == ItemType.ToxicWaste)
                {
                    d.TakeDamage(5);
                }
            }
        }
    }
}
