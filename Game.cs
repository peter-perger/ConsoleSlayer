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
        public int MapWidth { get; private set; }
        public int MapHeight { get; private set; }

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
                        gameLogic.PlayerDirectInteractionLogic();
                        break;

                    case ConsoleKey.A:
                        gameLogic.PlayerAttackLogic();
                        break;

                    case ConsoleKey.Escape:
                        Exited = true;
                        break;
                }

                gameLogic.PlayerIndirectInteractionLogic();
            }
        }

        public void LoadMapFromPlainText(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);

            if (lines.Length == 0)
            {
                throw new FormatException("The map file is empty.");
            }

            string[] dimension = lines[0].Split(',');

            if (dimension.Length != 2
                || !int.TryParse(dimension[0], out int height)
                || !int.TryParse(dimension[1], out int width)
                || height <= 0
                || width <= 0)
            {
                throw new FormatException
                    (
                        "The first line must contain positive numbers. (row,columns)"
                    );
            }

            if (lines.Length - 1 != height)
            {
                throw new FormatException
                    (
                        "The number of map rows does not match the header"
                    );
            }

            int playerCount = 0;
            string allowedCharacters = "ABDEMTWzimp_";


            for (int y = 0; y < height; y++)
            {
                string row = lines[y + 1];

                if (row.Length != width)
                {
                    throw new FormatException
                        (
                            $"Map row {y + 1} must conatin {width} charachters"
                        );
                }

                foreach (char tile in row)
                {
                    if (!allowedCharacters.Contains(tile))
                    {
                        throw new FormatException
                            (
                                $"Unknown map charachter: '{tile}'."
                            );
                    }

                    if (tile == 'p')
                    {
                        playerCount++;
                    }
                }
            }

            if (playerCount != 1)
            {
                throw new FormatException
                    (
                        "The map must contain exactly one player: p."
                    );
            }

            Console.SetWindowSize(width, height);

            MapWidth = width;
            MapHeight = height;

            Items.Clear();
            Demons.Clear();

            for (int y = 0; y < height; y++)
            {
                string row = lines[y + 1];

                for (int x = 0; x < width; x++)
                {
                    char tile = row[x];

                    switch (tile)
                    {
                        case 'A':
                            Items.Add(new GameItem(x, y, ItemType.Ammo));
                            break;

                        case 'B':
                            Items.Add(new GameItem(x, y, ItemType.BFGCell));
                            break;

                        case 'D':
                            Items.Add(new GameItem(x, y, ItemType.Door));
                            break;

                        case 'E':
                            Items.Add(new GameItem(x, y, ItemType.LevelExit));
                            break;

                        case 'M':
                            Items.Add(new GameItem(x, y, ItemType.Medikit));
                            break;

                        case 'T':
                            Items.Add(new GameItem(x, y, ItemType.ToxicWaste));
                            break;

                        case 'W':
                            Items.Add(new GameItem(x, y, ItemType.Wall));
                            break;

                        case 'z':
                            Demons.Add(new Demon(x, y, DemonType.Zombieman));
                            break;

                        case 'i':
                            Demons.Add(new Demon(x, y, DemonType.Imp));
                            break;

                        case 'm':
                            Demons.Add(new Demon(x, y, DemonType.Mancubus));
                            break;

                        case 'p':
                            Player.Position = new Position(x, y);
                            break;

                        case '_':
                            break;
                    }
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
                    stpWtchRenderer.Restart();
                }

                UserAction();

                Thread.Sleep(25);
            }
        }

    }
}

     