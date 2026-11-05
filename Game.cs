using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSlayer
{
    internal class Game
    {
        Player player;

        bool isRunning;

        public Player Player { get; private set; }

        public bool Exited { get => isRunning; set => isRunning = value; }

        public Game (Player player)
        {
            Player = player;
        }

        private void RenderSingleSprite (Position position, ConsoleSprite sprite)
        {

            if (position.X <= Console.WindowWidth && position.X <= Console.WindowHeight)
            
            {
                Console.SetCursorPosition (position.X, position.Y);
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
            RenderSingleSprite(this.Player.Position, this.Player.Sprite);
        }
    
        private void UserAction()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo pressed = Console.ReadKey(true);

                switch (pressed.Key)
                {
                    case ConsoleKey.LeftArrow:
                        if (Player.Position.X > 0) {
                             Player.Position.X -= 1;

                        }
                        break;

                    case ConsoleKey.RightArrow:
                        if (Player.Position.X < Console.WindowWidth - 1)
                        {
                            Player.Position.X += 1;
                        }
                        break;

                    case ConsoleKey.UpArrow:
                        if (Player.Position.Y > 0)
                        {
                            Player.Position.Y -= 1;
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        if (Player.Position.Y < Console.WindowHeight - 1)
                        {
                            Player.Position.Y += 1;
                        }
                        break;

                    case ConsoleKey.Escape:
                        Exited = true;
                        break;
                }
            }
        }

        public void Run()
        {
            while (!Exited)
            {
                RenderGame();
                UserAction();
                Thread.Sleep(25);
            }
        }
    }
}
