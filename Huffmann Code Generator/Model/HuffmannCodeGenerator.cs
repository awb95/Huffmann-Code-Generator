using Huffmann_Code_Generator.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Huffmann_Code_Generator.Model
{
    class HuffmannCodeGenerator : ViewModelBase
    {
        #region "Konstruktur"
        public HuffmannCodeGenerator()
        {
            MessageItems = new List<MessageItem>();
        }
        #endregion

        #region "Properties"
        // Nachricht für die ein Optimalcode erzeugt werden soll
        private string _Message;
        public string Message {
            get { return _Message; }
            set { SetProperty<string>(ref _Message, value); }
            }

        // Enthält Eigenschften zu allen Zeichen in Message die zur Berechnung des Optimalcode benötigt werden.
        private List<MessageItem> _MessageItems;                
        public List<MessageItem> MessageItems
        {
            get { return _MessageItems; }
            set { SetProperty<List<MessageItem>>(ref _MessageItems , value); }
        }

        #endregion

        #region "Public Methods"
        /// <summary>
        /// Generiert die Grunddaten für den HuffmannCode
        /// </summary>
        public void GenerateHuffmannCode(string message)
        {
            MessageItems.Clear();
            this.Message = message;
            CalculateCharacterCounts();
            CalculateProbabilities();
            OrderMessageItems();
        }
        #endregion

        #region "Private Methods"
        /// <summary>
        /// Anzahl der Vorkommen aller Zeichen in message berechnen
        /// </summary>
        private void CalculateCharacterCounts()
        {
            for (int i = 0; i < Message.Length; i++)
            {
                string character = Message.Substring(i, 1);
                var exp = MessageItems.Find(MessageItem => MessageItem.Character == character);

                if (exp == null)
                {
                    // Element noch nicht vorhanden
                    exp = new MessageItem(character);
                    MessageItems.Add(exp);
                }

                exp.CharacterCountInMessage++;
            }
        }

        /// <summary>
        /// Wahrscheinlichkeit aller Zeichen berechnen
        /// </summary>
        private void CalculateProbabilities()
        {
            foreach (MessageItem messageItem in MessageItems)
            {
                messageItem.CalcProbability(this.Message.Length);
            }
        }

        /// <summary>
        /// Einträge in MessageItems nach Wahrscheinlichkeit sortieren
        /// </summary>
        private void OrderMessageItems()
        {
            MessageItems.Sort(new MessageItemProbabilityComparer().Compare);
        }
        #endregion
    }
}
