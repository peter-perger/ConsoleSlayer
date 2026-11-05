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

            Player player1 = new Player(1, 1);
            Game myGame = new Game(player1);

            myGame.Run();
        }
    }
}
