using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace ConsoleSlayer
{
    internal class Game
    {
        ConsoleRenderer consoleRenderer;
        GameLogic gameLogic;

        Stopwatch stpWtchLogic = new Stopwatch();
        Stopwatch stpWtchRenderer = new Stopwatch();

        public Player Player { get; private set; }
        public bool Exited { get; set; }
        public List<GameItem> Items { get; set; }
        public List<Demon> Demons { get; set; }

        public Game(Player player)
        {
            consoleRenderer = new ConsoleRenderer(this);
            gameLogic = new GameLogic(this);
            Player = player;
            Items = new List<GameItem>();
            Demons = new List<Demon>();
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

                            gameLogic.Move(Player, targetPosition);

                        }
                        break;

                    case ConsoleKey.RightArrow:
                        if (Player.Position.X < Console.WindowWidth - 1)
                        {
                            Position step = new Position(1, 0);
                            Position targetPosition = Position.Add(Player.Position, step);

                            gameLogic.Move(Player, targetPosition);
                        }
                        break;

                    case ConsoleKey.UpArrow:
                        if (Player.Position.Y > 0)
                        {
                            Position step = new Position(0, -1);
                            Position targetPosition = Position.Add(Player.Position, step);

                            gameLogic.Move(Player, targetPosition);
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        if (Player.Position.Y < Console.WindowHeight - 1)
                        {
                            Position step = new Position(0, 1);
                            Position targetPosition = Position.Add(Player.Position, step);

                            gameLogic.Move(Player, targetPosition);
                        }
                        break;

                    case ConsoleKey.D:
                        foreach (GameItem item in Items)
                        {
                            item.Interact();
                        }
                        break;

                    case ConsoleKey.A:
                        gameLogic.PlayerAttackLogic();
                        break;
                        
                    case ConsoleKey.Escape:
                        Exited = true;
                        break;
                }
            }
        }
       
        public void Run()
        {
            stpWtchLogic.Start();
            stpWtchRenderer.Start();

            while (!Exited && Player.Alive)
            {
                if (stpWtchLogic.ElapsedMilliseconds > 500)
                {
                    gameLogic.UpdateGameState();
                    stpWtchLogic.Restart();
                } 

                if (stpWtchRenderer.ElapsedMilliseconds > 25)
                {
                    consoleRenderer.RenderGame();
                }

                UserAction();

                Thread.Sleep(20);
            }
        }
    }
}