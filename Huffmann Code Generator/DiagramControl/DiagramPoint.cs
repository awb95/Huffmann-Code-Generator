using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Huffmann_Code_Generator.DiagramControl
{
    /// <summary>
    /// Repräsentiert einen Punkt im Diagramm 
    /// -> sollte eigentlich Point erben, da Point aber ein Struct ist musste ide komplette Klasse neu erstellt werden.
    /// </summary>
    public class DiagramPoint
    {
        #region "Konstruktors"
        public DiagramPoint()
        {
        }

        public DiagramPoint(double x, double y, string caption)
        {
            this.X = x;
            this.Y = y;
            Caption = caption;
        }

        public DiagramPoint(Point point, string caption)
        {
            this.X = point.X;
            this.Y = point.Y;
            Caption = caption;
        }

        #endregion

        #region "Properties"

        /// <summary>
        /// X-Position des Punkts
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Y-Position des Punkts
        /// </summary>
        public double Y { get; set; }        

        /// <summary>
        /// Beschriftung/Name des Punkts
        /// </summary>
        public string Caption { get; set; }
        #endregion

        #region "Methods"
        /// <summary>
        /// Konvertiert einen DiagramPoint in ein Point Struct
        /// </summary>
        /// <returns></returns>
        public Point GetPoint() => new Point(this.X, this.Y);
        #endregion
    }
}
