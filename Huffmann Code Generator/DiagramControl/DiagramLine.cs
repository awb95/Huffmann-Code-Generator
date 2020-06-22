using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Huffmann_Code_Generator.DiagramControl
{
    /// <summary>
    /// Repräsentiert iene Linie im Diagramm
    /// </summary>
    public class DiagramLine
    {
        public DiagramLine(Point start, Point end)
        {
            this.Start = start;
            this.End = end;
        }

        /// <summary>
        /// Startpunkt der Linie
        /// </summary>
        public Point Start { get; set; }

        /// <summary>
        /// Endpunkt der Linie
        /// </summary>
        public Point End { get; set; }

        /// <summary>
        /// Beschriftung der Linie je nach dem in welche Richtung sie geht
        /// </summary>
        /// <returns></returns>
        public string GetCaption()
        {
            if (this.Start.Y > this.End.Y)
                return "0";
            else
                return "1";
        }
    }
}
