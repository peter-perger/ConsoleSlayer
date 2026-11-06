using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices.Swift;
using System.Text;

namespace ConsoleSlayer
{
    internal class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Position Add (Position p1, Position p2)
        {
            if (p1 is null || p2 is null)
            {
                throw new ArgumentException("The positions value can't be null!");
            }

            int newXCoordinate = p1.X + p2.X;
            int newYCooridante = p1.Y + p2.Y;

            return new Position(newXCoordinate, newYCooridante);
        }

        public static double Distance (Position p1, Position p2)
        {
            if (p1 is null || p2 is null)
            {
                throw new ArgumentException("The positions value can't be null!");
            }

            int totalX = p1.X - p2.X;
            int totalY = p1.Y - p2.Y;
            
            double distance = Math.Sqrt(Math.Pow(totalX, 2) + Math.Pow(totalY, 2));

            return distance;
        }
    }
}
