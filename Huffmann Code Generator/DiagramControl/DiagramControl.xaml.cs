using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Huffmann_Code_Generator.DiagramControl;
using Huffmann_Code_Generator.Model;

namespace Huffmann_Code_Generator.DiagramControl
{
    /// <summary>
    /// Interaktionslogik für DiagramControl.xaml
    /// </summary>
    public partial class DiagramControl : UserControl
    {
        #region "Konstuktor"
        public DiagramControl()
        {
            InitializeComponent();

            Points = new List<DiagramPoint>();            
            Lines = new List<DiagramLine>();
            AdditionalPoints = new List<DiagramPoint>();
            base.SizeChanged += DiagramControl_SizeChanged;

        }

        // Neu Zeichnen wenn sich die Größe des Steuerelemnts ändert
        private void DiagramControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CalculateDiagram();
        }
        #endregion

        #region "DependencyProperty"
        /// <summary>
        /// Dependency Property deklarieren und registrieren
        /// </summary>
        public static readonly DependencyProperty MessageItemsProperty =
            DependencyProperty.Register("MessageItems", typeof(ObservableCollection<MessageItem>), typeof(DiagramControl),
                new FrameworkPropertyMetadata(OnMessageItemsPropertyChanged));

        /// <summary>
        /// Callbackmethode die Aufgerufen wird, wenn ein DependencyProperty geändert werden soll
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnMessageItemsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DiagramControl)d).MessageItems = (ObservableCollection<MessageItem>)e.NewValue;
        }

        /// <summary>
        /// PropertyWrapper damit DependencyProperty auch wie ei normales Property verwendet werden kann
        /// </summary>
        public ObservableCollection<MessageItem> MessageItems
        {
            get { return (ObservableCollection<MessageItem>)GetValue(MessageItemsProperty); }
            set { 
                SetCurrentValue(MessageItemsProperty, value);   // Mit SetValue wird die Datenbindung überchrieben und funktioniert deshalb nur 1x
                CalculateDiagram();
            }
        }

        #endregion

        #region "Properties"

        private List<DiagramPoint> Points { get; set; }

        private List<DiagramLine> Lines { get; set; }

        private List<DiagramPoint> AdditionalPoints { get; set; }

        private double IntervallX;
        private double IntervallY;

        private double _MaxValueX;
        public double MaxValueX
        {
            get { return _MaxValueX; }
            set
            {
                _MaxValueX = value;
                this.IntervallX = (this.ActualWidth-10) / _MaxValueX;
            }
        }

        private double _ValueCountY;
        public double ValueCountY
        {
            get { return _ValueCountY; }
            set
            {
                _ValueCountY = value;
                this.IntervallY = (this.ActualHeight-10) / _ValueCountY;
            }
        }

        #endregion

        #region "Drawing"
        /// <summary>
        /// in drawing Area wird die refernz auf ein Objekt gespeicher in dem die Zeichnung jederzeit berabeitet werden kann
        /// </summary>
        DrawingGroup drawingArea = new DrawingGroup();

        /// <summary>
        /// Fügt die drawing Area zur Renderliste des Fenserts hinzu
        /// </summary>
        /// <param name="drawingContext"></param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);            
            drawingContext.DrawDrawing(drawingArea);
            Render();
        }

        // I can call this anytime, and it'll update my visual drawing
        // without ever triggering layout or OnRender()
        private void Render()
        {
            var drawingContext = drawingArea.Open();
            
            Draw(drawingContext);
            drawingContext.Close();
        }

        private void Draw(DrawingContext drawingContext)
        {
            var PenBlack = new Pen(Brushes.Black, 1);
            var PenBlue = new Pen(Brushes.Blue, 1);
            var PenGrey = new Pen(Brushes.LightGray, 1);

            // X-Achse
            drawingContext.DrawLine(PenBlack, FixPoint(new Point(0, 0)), FixPoint(new Point(this.ActualWidth, 0)));
            // Y-Achse
            drawingContext.DrawLine(PenBlack, FixPoint(new Point(0, 0)), FixPoint(new Point(0, this.ActualHeight)));

            // Raster zeichen Links nach Rechts
            for (var i = 1; i < this.ValueCountY+1; i++)
            {
                drawingContext.DrawLine(PenGrey, FixPoint(new Point(0, i* IntervallY)), FixPoint(new Point(this.ActualWidth, i*IntervallY)));
                // Zeichen
                drawingContext.DrawText(
                    GetFormattedText(MessageItems.ElementAt(i-1).Character, Brushes.Black, TextAlignment.Right),
                    FixPoint(new Point(-5, i * IntervallY))
                    );
            }
            // Raster zechnen Unten nach Oben
            for (var i=0.1; i<this.MaxValueX; i += 0.1)
            {
                drawingContext.DrawLine(PenGrey, FixPoint(new Point(i*IntervallX, 0)), FixPoint(new Point(i*IntervallX, this.ActualHeight)));
                // Wahrscheinlichkeit
                drawingContext.DrawText(
                    GetFormattedText(i.ToString(), Brushes.Black),
                    FixPoint(new Point(i * IntervallX, 0))
                    );
            }

            // Punkte zeichnen
            foreach (var point in Points)
            {
                drawingContext.DrawEllipse(Brushes.Black, PenBlack, FixPoint(point), 1, 1);

                drawingContext.DrawText(
                    GetFormattedText(point.Caption, Brushes.Black),
                    FixPoint(point.GetPoint())
                    );
            }

            // Punkt-Verbindungs-Linien zeichnen
            foreach (DiagramLine line in Lines)
            {
                drawingContext.DrawLine(PenBlue, FixPoint(line.Start), FixPoint(line.End));
                 
                drawingContext.DrawText(
                    GetFormattedText(line.GetCaption(), Brushes.Red),
                    FixPoint(new Point((line.Start.X+line.End.X)/2, (line.Start.Y+line.End.Y)/2))
                    );
             }

            // ZusatzPunkte zeichnen
            foreach (var point in AdditionalPoints)
            {
                drawingContext.DrawEllipse(Brushes.Blue, PenBlue, FixPoint(point), 1, 1);
                // Beschriftung Zusatzpunkte
                //drawingContext.DrawText(
                //    GetFormattedText(point.Caption, Brushes.Blue),
                //    FixPoint(point.GetPoint())
                //    );

            }
        }

        private FormattedText GetFormattedText(string text, Brush fontColor, TextAlignment textAlignment = TextAlignment.Center)
        {
#pragma warning disable CS0618 // Typ oder Element ist veraltet
            var formText = new FormattedText(
                text,
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(
                    this.FontFamily,
                    this.FontStyle,
                    this.FontWeight,
                    this.FontStretch),
                this.FontSize,
               fontColor);
#pragma warning restore CS0618 // Typ oder Element ist veraltet
            formText.TextAlignment = textAlignment;
            return formText;
        }

        /// <summary>
        /// Hilfsfunktion zum Anpassen des koordinatensystems damit 0 = links unten und nicht links oben
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private Point FixPoint(Point point)
        {
            return new Point(point.X, this.ActualHeight - point.Y);
        }

        private Point FixPoint(DiagramPoint point)
        {
            return new Point(point.GetPoint().X, this.ActualHeight - point.GetPoint().Y);
        }

        #endregion



        /// <summary>
        /// Punkte zum zeichnen berechnen
        /// </summary>
        private void CalculateDiagram()
        {
            if (MessageItems == null) return;

            this.MaxValueX = 1;
            this.ValueCountY = MessageItems.Count;

            //Speicher leeren
            Points.Clear();
            Lines.Clear();
            AdditionalPoints.Clear();
            foreach(var item in MessageItems)
            {
                item.Optimalcode = "";
            }

            // Temporäre Liste für alle Punkte die noch nicht verarbeitet wurden
            List<DiagramMessageItem> DiagItems = new List<DiagramMessageItem>();
            foreach(var item in MessageItems)
            {
                DiagItems.Add(new DiagramMessageItem(item));
            }

            // Ausgangspunkte berechnen            
            foreach (var item in DiagItems)
            {
                item.DiagramPoint = new DiagramPoint(item.Probability * IntervallX, (DiagItems.IndexOf(item) + 1) * IntervallY, item.Character);
                Points.Add(item.DiagramPoint);
            }


            // Alle Punkte von links nach rechts durchgehen zusammenrechnen und verbindungslinien generieren
            while (DiagItems.Count >= 2)
            {
                var exp = DiagItems.OrderBy(i => i.Probability);

                // Die kleinsten 2 Items holen
                var Item1 = exp.ElementAt(0);
                var Item2 = exp.ElementAt(1);

                // Neuen Punkt aus den 2 kleinsten bilden
                var NewItem = Item1 + Item2;

                // Neue Punkte und Linen aus den neuen Infos generieren
                AdditionalPoints.Add(NewItem.DiagramPoint);

                var line1 = new DiagramLine(Item1.DiagramPoint.GetPoint(), NewItem.DiagramPoint.GetPoint());
                AddOptimalcode(Item1.Character, line1.GetCaption());
                Lines.Add(line1);

                var line2 = new DiagramLine(Item2.DiagramPoint.GetPoint(), NewItem.DiagramPoint.GetPoint());
                AddOptimalcode(Item2.Character, line2.GetCaption());
                Lines.Add(line2);

                // Die kleinsten 2 Punkte löschen und den neu berechneten hinzufügen
                DiagItems.Remove(Item1);
                DiagItems.Remove(Item2);
                DiagItems.Add(NewItem);                
            }

                this.Render();
            
        }

        /// <summary>
        /// speichert den übergebenen Wert in den Optimalcode der im String übergebenen MessagItems(Zeichen)
        /// </summary>
        /// <param name="messageItemsString"></param>
        /// <param name="value"></param>
        void AddOptimalcode(string messageItemsString, string value)
        {
            if(messageItemsString.Length == 1)
            {
                var exp = MessageItems.Where(x => x.Character == messageItemsString).FirstOrDefault();
                exp.Optimalcode = value + exp.Optimalcode;  // WErt links anfügen -> dann muss am Ende dei Reihenfolge nicht gedreht werden
                return;
            }

            var items = messageItemsString.Split('+');
            foreach (var item in items) {
                var exp = MessageItems.Where(x => x.Character == item).FirstOrDefault();
                exp.Optimalcode = value + exp.Optimalcode;
            }
        }
    }
}
