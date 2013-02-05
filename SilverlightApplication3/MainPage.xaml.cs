using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;

namespace SilverlightApplication3
{
    public partial class MainPage : UserControl
    {
        
        public Collection<Planet> planetList;
        private Collection<Hexagon> hexGrid;
        Random random = new Random();
        public Ship farTrader = new Ship(100, 10);
        Image shipImage = new Image();
        public String selectedHex = "00";
        public String destination = "00";
        public Catalog catalog = new Catalog();
     
        public MainPage()
        {
            InitializeComponent();

            planetList = new Collection<Planet>();

            CreateHexGrid(8, 10, 30);

            dgPlanetList.ItemsSource = planetList;

            startingPosition();

        }

        private void startingPosition()
        {
            Planet start = planetList.ElementAt(random.Next(0, planetList.Count()));
            while (start.UPC.Substring(0,1) == "X")
            {
                start = planetList.ElementAt(random.Next(0, planetList.Count()));
            }
            
            //Find canvas X,Y
            Ellipse sys = new Ellipse();
            sys = (Ellipse)cnvsHexGrid.FindName("Planet" + start.Location);
            shipImage.Name = "ship";
            shipImage.Source = new BitmapImage(new Uri("Images/ship.png", UriKind.Relative));

            shipImage.SetValue(Canvas.LeftProperty, (Canvas.GetLeft(sys) - 4));
            shipImage.SetValue(Canvas.TopProperty, (Canvas.GetTop(sys) - 4));

            cnvsHexGrid.Children.Add(shipImage);
           
            farTrader.Location = start.Location;
            txtGridPosition.Text = HexLocation(farTrader.Location);
        }

        private string HexLocation(string location)
        {
            foreach (Planet item in planetList)
            {
                if (item.Location == location)
                {
                    return item.Name + " (" + location.Insert(1, ", ") + ") ";
                }              
            }
            return "(" + location.Insert(1, ", ") + ")";            
        }

        private void CreateHexGrid(int columns, int rows, double length)
        {
            hexGrid = new Collection<Hexagon>();
            Hexagon newHex = null;

            //compute points for each hexagon
            for (int column = 0; column < columns; column++)
            {
                for (int row = 0; row < rows; row++)
                {
                    if ((column == 0) & (row == 0))
                    {
                        //BASE HEX 0,0
                        newHex = new Hexagon(length, column, row);                        
                    }
                    else
                    {
                        if ((column % 2) == 0) //EVEN COLUMN
                        {
                            if ((row == 0) & (column > 0))  //first hex in column
                            {
                                //compute off previous column
                                newHex = new Hexagon(hexGrid.First(h => h.Name == ((column - 1).ToString() + row.ToString())), length, column, row);
                            }
                            else // row or column is greater than 0 (not the base hex or first hex in column)
                            {
                                //compute off previous row
                                newHex = new Hexagon(hexGrid.First(h => h.Name == (column.ToString() + (row - 1).ToString())), length, column, row);
                            }
                        }
                        else //ODD COLUMN
                        {
                            if (row == 0) //first hex in column
                            {
                                //compute off previous column
                                newHex = new Hexagon(hexGrid.First(h => h.Name == ((column - 1).ToString() + row.ToString())), length, column, row);
                            }
                            else
                            {
                                //compute off previous row
                                newHex = new Hexagon(hexGrid.First(h => h.Name == (column.ToString() + (row - 1).ToString())), length, column, row);
                            }
                        }
                    }

                    hexGrid.Add(newHex);
                    
                    //Paint hex
                    SolidColorBrush blackBrush = new SolidColorBrush(Colors.Black);
                    SolidColorBrush clearBrush = new SolidColorBrush(Colors.Transparent);
                    SolidColorBrush greenBrush = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
                    Polygon hex = new Polygon();
                    hex.Stroke = greenBrush;
                    hex.StrokeThickness = 1;
                    hex.Fill = clearBrush;
                    hex.Name = column.ToString() + row.ToString();

                    hex.Points = hexGrid.Last().VertexPoints;
                    hex.IsHitTestVisible = true;
                    hex.MouseLeftButtonDown += new MouseButtonEventHandler(this.hex_MouseLeftButtonDown);
                    hex.MouseEnter += new MouseEventHandler(hex_MouseEnter);
                    hex.MouseLeave += new MouseEventHandler(hex_MouseLeave);
                 
                    cnvsHexGrid.Children.Add(hex);
                    PlanetCheck(newHex, column, row, length);
                    
                }//end row
            }//end column

        }

        private void PlanetCheck(Hexagon nHex, int col, int row, double length)
        {           
            if (random.Next(1, 7) > 5)
            {
                SolidColorBrush blackBrush = new SolidColorBrush(Colors.Black);
                SolidColorBrush redBrush = new SolidColorBrush(Colors.Red);
                SolidColorBrush clearBrush = new SolidColorBrush(Colors.Transparent);
                SolidColorBrush amberBrush = new SolidColorBrush(Colors.Yellow);
                SolidColorBrush greenBrush = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));

                double xValue = nHex.VertexPoints.ElementAt(0).X + length - 4;
                double yValue = nHex.VertexPoints.ElementAt(0).Y - 4;
                Ellipse star = new Ellipse();
                star.Stroke = greenBrush;
                star.StrokeThickness = 1;
                star.Fill = greenBrush;
                star.Width = 8;
                star.Height = 8;
                star.Name = "Planet" + col.ToString() + row.ToString();
                star.SetValue(Canvas.LeftProperty, xValue);
                star.SetValue(Canvas.TopProperty, yValue);
                star.IsHitTestVisible = false;
                //ToolTipService.SetToolTip(star, star.Name);
                cnvsHexGrid.Children.Add(star);
                Planet newPlanet = new Planet(col.ToString() + row.ToString());
                planetList.Add(newPlanet);
                if ((newPlanet.UPC.Substring(15, 1) == "R") || (newPlanet.UPC.Substring(15, 1) == "A"))
                {
                    //Draw Red Circle
                    Ellipse tz = new Ellipse();
                    if (newPlanet.UPC.Substring(15, 1) == "R")
                    {
                        tz.Stroke = redBrush;
                    }
                    else
                    {
                        tz.Stroke = amberBrush;
                    }
                    tz.Fill = clearBrush;
                    tz.SetValue(Canvas.LeftProperty, xValue - 10);
                    tz.SetValue(Canvas.TopProperty, yValue - 10);
                    tz.Width = 28;
                    tz.Height = 28;
                    tz.IsHitTestVisible = false;
                    cnvsHexGrid.Children.Add(tz);
                }

                TextBlock tbStarport = new TextBlock();
                tbStarport.Name = "Starport" + star.Name;
                tbStarport.SetValue(Canvas.LeftProperty, xValue);
                tbStarport.SetValue(Canvas.TopProperty, yValue - 17);
                tbStarport.Width = 50;
                tbStarport.Foreground = greenBrush;
                tbStarport.Height = 10;
                tbStarport.FontWeight = FontWeights.Bold;
                tbStarport.Text = newPlanet.UPC.Substring(0, 1);
                cnvsHexGrid.Children.Add(tbStarport);

                if (newPlanet.UPC.Substring(11, 1) == "N")
                {
                    Image navalBase = new Image();
                    navalBase.Source = new BitmapImage(new Uri("Images/anchor.png", UriKind.Relative));

                    navalBase.SetValue(Canvas.LeftProperty, Canvas.GetLeft(star) - 14);
                    navalBase.SetValue(Canvas.TopProperty, Canvas.GetTop(star) - 12);

                    navalBase.IsHitTestVisible = false;
                    cnvsHexGrid.Children.Add(navalBase);
                }
                //TODO: Add Scout Base symbol
                //TODO: Add Gas Giants symbol
            }
        }

        void hex_MouseEnter(object sender, MouseEventArgs e)
        {
            Polygon hex = sender as Polygon;
            SolidColorBrush blackFill = new SolidColorBrush(Colors.Black);
            hex.Fill = blackFill;
        }

        void hex_MouseLeave(object sender, MouseEventArgs e)
        {
            Polygon hex = sender as Polygon;
            SolidColorBrush clearFill = new SolidColorBrush(Colors.Transparent);
            hex.Fill = clearFill;
        }

        void hex_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Polygon hex = sender as Polygon;
            selectedHex = hex.Name.ToString();
            destination = selectedHex;

            bool empty = true;
            
            Hexagon destinationHex = hexGrid.First(h => h.Name == selectedHex);
            Hexagon locationHex = hexGrid.First(h => h.Name == farTrader.Location);

            foreach (Planet item in planetList)
            {
                if (item.Location == selectedHex)
                {
                    selectedHex = item.Name + " (" + selectedHex.Insert(1, ", ") + ") ";
                    empty = false;
                    txtStatus.Text = Hexagon.ComputeRange(locationHex.Location, destinationHex.Location).ToString();
                }
            }

            if (empty)
            {
                selectedHex = selectedHex.Insert(1, ", ");
                txtStatus.Text = Hexagon.ComputeRange(locationHex.Location, destinationHex.Location).ToString();
                txtStatus.Text = "Only the cold emptiness of space awaits you there!";
            }

            txtGridDestination.Text = selectedHex;
        }

        private void Load_Main(object sender, RoutedEventArgs e)
        {
            UIElement target = sender as UIElement;
            target.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(hex_MouseLeftButtonDown), true);
        }

        private void dgHandle_Click(object sender, RoutedEventArgs e)
        {
            if (dgPlanetList.Height == 450)
            {
                dgCollapse.Begin();
                dgHandleMoveUp.Begin();
                dgHandleTransform.CenterX = (dgHandle.ActualWidth / 2);
                dgHandleTransform.Angle = 180;
            }
            else
            {
                dgExpand.Begin();
                dgHandleMoveDown.Begin();
                dgHandleTransform.CenterX = (dgHandle.ActualWidth / 2);
                dgHandleTransform.Angle = 0;
            }

            //TODO: Add products grid and expose as planet list slides up. hide as planet list slides down?
        }

        private void btnMove_Click(object sender, RoutedEventArgs e)
        {
            mySB.Pause();

            Double x = Canvas.GetLeft(shipImage);
            Double y = Canvas.GetTop(shipImage);

            mySB.Stop();
            mySB.Children.Clear();

            Ellipse sys = new Ellipse();
            sys = (Ellipse)cnvsHexGrid.FindName("Planet" + destination);
            

            Double x1 = new Double();
            Double y1 = new Double();

            if (sys != null)
            {
                x1 = (Double)sys.GetValue(Canvas.LeftProperty) - 4;
                y1 = (Double)sys.GetValue(Canvas.TopProperty) - 4;
                farTrader.Location = destination;
                txtGridPosition.Text = HexLocation(farTrader.Location);
                txtGridDestination.Text = "";
                txtStatus.Text = "";
            }
            else
            {
                x1 = x;
                y1 = y;
                txtGridPosition.Text = HexLocation(farTrader.Location);
                txtStatus.Text = "Only the cold emptiness of space awaits you there!";
                txtGridDestination.Text = "";
            }

            DoubleAnimation animationX = new DoubleAnimation();
            DoubleAnimation animationY = new DoubleAnimation();

            animationX.From = x;
            animationY.From = y;

            animationX.To = x1;
            animationY.To = y1;

            Storyboard.SetTargetProperty(animationX, new PropertyPath(Canvas.LeftProperty));
            Storyboard.SetTargetProperty(animationY, new PropertyPath(Canvas.TopProperty));

            Storyboard.SetTarget(animationX, shipImage);
            Storyboard.SetTarget(animationY, shipImage);

            mySB.Children.Add(animationX);
            mySB.Children.Add(animationY);

            mySB.Begin();

            
            
        }

    }
}