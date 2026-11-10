using System.Diagnostics.Metrics;

namespace ConsoleSlayer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region EarlierTetst

            //#region Position
            //Position p1 = new Position(3, 4);
            //Position p2 = new Position(1, 2);

            ////Console.WriteLine($"Initial positions: p1: {p1}, p2: {p2}");

            //Position sum = Position.Add(p1, p2);
            ////Console.WriteLine($"Add (p1 + p2) = ({sum.X}, {sum.Y})");

            //double distance = Position.Distance(p1, p2);
            ////Console.WriteLine($"Distance (p1, p2) = {distance}");

            //Console.WriteLine('\n');
            //#endregion


            //#region ConsolSprite
            //ConsoleSprite csp = new ConsoleSprite(ConsoleColor.DarkCyan, ConsoleColor.Black, 'D');
            //Console.BackgroundColor = csp.BackgroundColor;
            //Console.ForegroundColor = csp.ForeGroundColor;

            ////Console.WriteLine(csp.BackgroundColor);
            ////Console.WriteLine(csp.ForeGroundColor);
            ////Console.WriteLine(csp.Glyph);

            //Console.ResetColor();

            ////Console.WriteLine('\n');

            //#endregion


            //#region Player
            //Player player1 = new Player(10, 1);

            //Console.BackgroundColor = player1.Sprite.BackgroundColor;
            //Console.ForegroundColor = player1.Sprite.ForeGroundColor;

            //Console.SetCursorPosition(player1.Position.X, player1.Position.Y);

            ////Console.WriteLine(player1.Sprite.Glyph);

            //Console.ResetColor();
            //Console.SetCursorPosition(0, 0);

            //#endregion

            #endregion

            #region ToDo list
            //Demon & Player health - int or double mismatch
            #endregion

            #region Testing map load
            //Player player1 = new Player(1, 1);
            //Game myGame = new Game(player1);

            //myGame.LoadMapFromPlainText(@"C:\Users\Peter\Documents\program\C#\ConsoleSlayer\ConsoleSlayer\Maps\demo_map.txt");
            #endregion

            #region Demo items and demons
            //myGame.Items.Add(new GameItem(5, 2, ItemType.Door));
            //myGame.Items.Add(new GameItem(8, 2, ItemType.Medikit));
            //myGame.Items.Add(new GameItem(1, 14, ItemType.Ammo));
            //myGame.Items.Add(new GameItem(4, 22, ItemType.Wall));
            //myGame.Items.Add(new GameItem(0, 19, ItemType.ToxicWaste));
            //myGame.Items.Add(new GameItem(5, 19, ItemType.ToxicWaste));
            //myGame.Items.Add(new GameItem(10, 19, ItemType.ToxicWaste));

            //myGame.Demons.Add(new Demon(0, 20, DemonType.Imp));
            //myGame.Demons.Add(new Demon(5, 20, DemonType.Zombieman));
            //myGame.Demons.Add(new Demon(10, 20, DemonType.Mancubus));
            #endregion

            #region Run the Game
            Player player1 = new Player(1, 1);
            Game myGame = new Game(player1);

            string mapPath = Path.Combine(
                AppContext.BaseDirectory,
                "Maps",
                "pmp_arena.txt"
                );

            myGame.LoadMapFromPlainText(mapPath);            
            myGame.Run();
            #endregion
        }
    }
}
