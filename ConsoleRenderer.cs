using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSlayer
{
    internal class ConsoleRenderer
    {
        private readonly Game game;

        public ConsoleRenderer (Game Game)
        {
            game = Game;
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

        public void RenderGame()
        {
            Console.Clear();
            Console.CursorVisible = false;
            Console.ResetColor();

            foreach (GameItem item in game.Items)
            {
                if (item is not null)
                {
                    RenderSingleSprite(item.Position, item.Sprite);
                }
            }

            foreach (Demon demon in game.Demons)
            {
                if (demon is not null)
                {
                    RenderSingleSprite(demon.Position, demon.Sprite);
                }
            }

            RenderSingleSprite(game.Player.Position, game.Player.Sprite);
        }

    }
}
