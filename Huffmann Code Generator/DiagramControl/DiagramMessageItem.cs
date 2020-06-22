using Huffmann_Code_Generator.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Huffmann_Code_Generator.DiagramControl
{
    /// <summary>
    /// Repräsentiert ein Zeichen aus der zu übermittlenden Nachricht wurde erweitert durch Infos die zum Zeichen des Diagramms benötigt werden
    /// </summary>
    public class DiagramMessageItem : MessageItem
    {
        #region "Konstruktors"
        public DiagramMessageItem() {
            DiagramPoint = new DiagramPoint();
            Point = new Point();
        }

        public DiagramMessageItem(MessageItem messageItem) : this(messageItem, new Point(), true) { }


        public DiagramMessageItem(MessageItem messageItem, Point point, bool isOrigin)
        {
            this.Character = messageItem.Character;
            this.CharacterCountInMessage = messageItem.CharacterCountInMessage;
            this.Probability = messageItem.Probability;
            this.Optimalcode = messageItem.Optimalcode;
            this.Point = point;
            this.DiagramPoint = new DiagramPoint(point, Character); ;
            this.IsOriginPoint = isOrigin;
        }
        #endregion

        #region "Properties"

        /// <summary>
        /// Repräsentiert den Punkt eines MessageItems
        /// </summary>
        private Point _Point;
        public Point Point
        {
            get => _Point;
            set
            {
                _Point = value;
                _DiagramPoint = new DiagramPoint(value, Character);
            }
        }

        /// <summary>
        /// Repräsentiert den Diagram Punkt eines MessageItems
        /// </summary>
        private DiagramPoint _DiagramPoint;
        public DiagramPoint DiagramPoint
        {
            get => _DiagramPoint;
            set
            {
                _DiagramPoint = value;
                _Point = _DiagramPoint.GetPoint();
            }
        }

        /// <summary>
        /// Gibt an ob es sich bei dm Punkt um einen Berechneten Punkt handelt oder einen Punkt eines Zechens der Nachricht
        /// </summary>
        public bool IsOriginPoint { get; set; }

        #endregion

        #region "Operators"

        /// <summary>
        /// Addition zweier DiagrammMessageItems = grafische Addition der werte im Diagramm
        /// </summary>
        /// <param name="Item1"></param>
        /// <param name="Item2"></param>
        /// <returns></returns>
        public static DiagramMessageItem operator + (DiagramMessageItem Item1, DiagramMessageItem Item2)
        {
            var newItem = new DiagramMessageItem();
            newItem.Character = Item1.Character + "+" + Item2.Character;
            newItem.CharacterCountInMessage = Item1.CharacterCountInMessage + Item2.CharacterCountInMessage;
            newItem.Probability = Item1.Probability + Item2.Probability;
            // X-werte werden addiert, Y Wert soll miteelpunkt sein -> addieren / 2
            newItem.DiagramPoint = new DiagramPoint(Item1.DiagramPoint.X + Item2.DiagramPoint.X, (Item1.DiagramPoint.Y + Item2.DiagramPoint.Y) /2, newItem.Character);

            return newItem;
        }

        #endregion
    }

}
