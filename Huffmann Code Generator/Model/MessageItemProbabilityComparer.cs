using System.Collections;

namespace Huffmann_Code_Generator.Model
{
    /// <summary>
    /// Klasse die zum Sortierern der MessageItems verwendet wird
    /// </summary>
    internal class MessageItemProbabilityComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            // Objekte konvertieren
            MessageItem MsgItem1 = x as MessageItem;
            MessageItem MsgItem2 = y as MessageItem;

            // Objecte prüfen
            if (MsgItem1 == null && MsgItem2 == null)
                return 0;
            else if (MsgItem1 == null)
                return -1;
            else if (MsgItem2 == null)
                return 1;

            // Wahrscheinlichkeit vergleichen
            if (MsgItem1.Probability > MsgItem2.Probability)
                return 1;
            else if (MsgItem1.Probability < MsgItem2.Probability)
                return -1;

            // Wahrscheinlichkeit gleich -> Zeichen alphabetisch sortieren
            return string.Compare(MsgItem2.Character, MsgItem1.Character);

        }
    }
}