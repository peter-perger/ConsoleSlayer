using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices.Swift;
using System.Text;

namespace ConsoleSlayer
{
    internal class Position
    {
        int x;
        int y;

        public int X { 
            get => x;
            
            set
            {
                if (value > 0)
                {
                    x = value;
                }
            }
        }
        public int Y
        {
            get => y;
            set
            {
                if (value > 0)
                {
                    y = value;
                }
            }
        }

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Position Add (Position p1, Position p2)
        {
            if (p1 is null || p2 is null)
            {
                throw new ArgumentException("The positions value can't be null!");
            }

            int newXCoordinate = p1.x + p2.x;
            int newYCooridante = p1.y + p2.y;

            return new Position(newXCoordinate, newYCooridante);
        }

        public static double Distance (Position p1, Position p2)
        {
            if (p1 is null || p2 is null)
            {
                throw new ArgumentException("The positions value can't be null!");
            }

            int totalX = p1.x - p2.x;
            int totalY = p1.y - p2.y;
            
            double distance = Math.Sqrt(Math.Pow(totalX, 2) + Math.Pow(totalY, 2));

            return distance;
        }
    }
}
