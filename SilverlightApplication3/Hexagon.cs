using System;
using System.Net;
using System.Windows;
using System.Windows.Media;
using System.Linq;

namespace SilverlightApplication3
{
    public class Hexagon
    {
        public string Name;
        public int XCord;
        public int YCord;
        public Point Location;
        public PointCollection VertexPoints;

        /// <summary>
        /// Constructor assumes base hex (0,0)
        /// </summary>
        /// <param name="length">Length of a hexside</param>
        public Hexagon(double length)
        {
            CreateBaseHex(length);
        }

        /// <summary>
        /// Class constructor for base (1st) hex on grid (0, 0)
        /// </summary>
        /// <param name="length">Length of a hexside</param>
        /// <param name="column">Hex grid column</param>
        /// <param name="row">Hex grid row</param>
        public Hexagon(double length, int column, int row)
        {
            if ((column == 0) && (row == 0))
            {
                CreateBaseHex(length);
            }
        }

        /// <summary>
        /// Class constructor for subsequent hexes on grid
        /// </summary>
        /// <param name="hex">Hex to be used as basis for new hex.</param>
        /// <param name="length">Length of a hexside</param>
        /// <param name="column">Hex grid column</param>
        /// <param name="row">Hex grid row</param>
        public Hexagon(Hexagon hex, double length, int column, int row)
        {
            if ((column == 0) && (row == 0))
            {
                CreateBaseHex(length);
            }
            else
            {
                CreateHex(hex, length, column, row);
            }
        }

        private void CreateHex(Hexagon previousHex, double length, int column, int row)
        {
            double A = (length / 2);
            double BSine = (Math.Sin(DegreeToRadian(60)));
            double B = (BSine * length);
            double H = 2 * B;
            double C = A + length;
            double D = 2 * length;

            PointCollection prevPoints = previousHex.VertexPoints;
            VertexPoints = new PointCollection();

            for (int point = 0; point <= 5; point++)
            {
                Point prevPoint = prevPoints.ElementAt(point);

                if ((column % 2) == 0) //EVEN COLUMN
                {
                    if ((row == 0) & (column > 0))  //first hex in column
                    {
                        //compute off previous column
                        VertexPoints.Add(new Point(prevPoint.X + C, prevPoint.Y - B));
                        XCord = previousHex.XCord + 2;
                        YCord = previousHex.YCord - 2;
                    }
                    else // row or column is greater than 0 (not the base hex or first hex in column)
                    {
                        //compute off previous row
                        VertexPoints.Add(new Point(prevPoint.X, prevPoint.Y + H));
                        XCord = previousHex.XCord;
                        YCord = previousHex.YCord + 4;
                    }
                }
                else //ODD COLUMN
                {
                    if (row == 0) //first hex in column
                    {
                        //compute off previous column
                        VertexPoints.Add(new Point(prevPoint.X + C, prevPoint.Y + B));
                        XCord = previousHex.XCord + 2;
                        YCord = previousHex.YCord + 2;
                    }
                    else
                    {
                        //compute off previous row
                        VertexPoints.Add(new Point(prevPoint.X, prevPoint.Y + H));
                        XCord = previousHex.XCord;
                        YCord = previousHex.YCord + 4;
                    }
                }
            }

            Name = column.ToString() + row.ToString();
            Location = new Point(XCord, YCord);
        }

        private void CreateBaseHex(double length)
        {
            double A = (length / 2);
            double BSine = (Math.Sin(DegreeToRadian(60)));
            double B = (BSine * length);
            double H = 2 * B;
            double C = A + length;
            double D = 2 * length;
            //BASE HEX 0,0
            VertexPoints = new PointCollection();
            VertexPoints.Add(new Point(0, B));
            VertexPoints.Add(new Point(A, 0));
            VertexPoints.Add(new Point(C, 0));
            VertexPoints.Add(new Point(D, B));
            VertexPoints.Add(new Point(C, H));
            VertexPoints.Add(new Point(A, H));
            Name = "00";
            XCord = 4;
            YCord = 8;
            Location = new Point(XCord, YCord);
        }

        public static double ComputeRange(Point destination, Point location)
        {
            double diffX = Math.Abs(location.X - destination.X);
            double diffY = Math.Abs(location.Y - destination.Y);
            double range;
            //double range = (diffX > diffY) ? (diffX / 2) : ((diffX + diffY) / 4);
            if (diffX > diffY)
            {
                range = diffX / 2;
            }
            else
            {
                range = (diffX + diffY) / 4;
            }

            return range;
            //ΔX = abs ( x2 - x1 ) and ΔY = abs( y2 - y1 )
            //If ΔX > ΔY Then range = ΔX / 2
            //Else range = (ΔX + ΔY) / 4
        }

        private double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

    }
}