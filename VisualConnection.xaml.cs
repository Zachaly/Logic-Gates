using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;



namespace Symulator_układów_logicznych
{
    // Class responsible to show connection between two gates in workspace
    public partial class VisualConnection : UserControl
    {
        IWorkspaceItem Start; // Gate that start the connection
        IWorkspaceItem Target;
        Point StartPoint;// Graphical point where connection start
        Point EndPoint;

        // returns place on the list of connections
        int ConnectionNum
        {
            get
            {
                return Target.Connections.FindIndex(a => a == this);
            }
        }

        public VisualConnection()
        {
            InitializeComponent();
        }

        public VisualConnection(UserControl start, UserControl target)
        {
            InitializeComponent();
            Start = start as IWorkspaceItem;
            Target = target as IWorkspaceItem;
            Update();
        }

        // Updates how the line looks like
        public void Update()
        {
            // Start on the Right side of the first gate, in the middle vertically
            StartPoint = Start.GetStartPoint();

            // Ends on the left side of control, verticall alignment depends on number of other inputs
            EndPoint = Target.GetEndPoint(ConnectionNum);

            List<Point> breaks = new List<Point>();

            breaks.Add(StartPoint);

            // Adds another breaks if end point is behind start one
            if (StartPoint.X > EndPoint.X)
            {
                double distanceFromElement = 10;

                breaks.Add(new Point(StartPoint.X + distanceFromElement, StartPoint.Y));


                if (StartPoint.Y < EndPoint.Y)
                    breaks.Add(new Point(StartPoint.X + distanceFromElement,
                        StartPoint.Y + (Start as UserControl).Height / 2 + distanceFromElement));
                else
                    breaks.Add(new Point(StartPoint.X + distanceFromElement,
                        StartPoint.Y - (Start as UserControl).Height / 2 - distanceFromElement));


                breaks.Add(new Point(EndPoint.X - distanceFromElement,
                    breaks[breaks.Count - 1].Y));

                breaks.Add(new Point(EndPoint.X - distanceFromElement,
                    EndPoint.Y));

            }
            else
            {
                breaks.Add(new Point((EndPoint.X + StartPoint.X) / 2, StartPoint.Y));
                breaks.Add(new Point((EndPoint.X + StartPoint.X) / 2, EndPoint.Y));
            }

            breaks.Add(EndPoint);

            GeometryGroup group = new GeometryGroup();

            for (int i = 0; i < breaks.Count - 1; i++)
                group.Children.Add(new LineGeometry(breaks[i], breaks[i + 1]));

            Line.Data = group;

        }

        // Removes this connection
        public void Delete()
        {
            try
            {
                Start.Connections.Remove(this);
                Target.Connections.Remove(this);
                Start.DeleteConnection(Target);
                (App.Current.MainWindow as MainWindow).DeleteConnection(this);
            }
            catch (System.NullReferenceException e) { }
        }

        private void Click(object sender, RoutedEventArgs e)
        {
            Delete();
        }
    }
}
