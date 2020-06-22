using Huffmann_Code_Generator.ViewModel;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace Huffmann_Code_Generator.Model
{
    public class MessageItem : ViewModelBase //muss INotifyPropertyChanged implementieren damit änderungen an die observableCollection weitergeleitet werden -> DiagramControl
    {

        #region "Konstruktor"
        public MessageItem() { }

        public MessageItem(string character)
        {
            Character = character;
        }
        #endregion

        #region "Properties"
        // Buchstabe
        private string _character;
        public string Character { get => _character; set => SetProperty(ref _character, value); }

        // Anzahl wie oft der Buchstabe in der Nachricht vorkommt
        private long _CharacterCountInMessage;
        public long CharacterCountInMessage { get => _CharacterCountInMessage; set => SetProperty(ref _CharacterCountInMessage, value); }

        // Wahrscheinlichkeit des Buchstabens in der Nachricht
        private double _Probability;
        public double Probability { get => _Probability; set => SetProperty(ref _Probability, value); }

        // Optimalcode
        private string _Optimalcode;
        public string Optimalcode { get => _Optimalcode; set => SetProperty(ref _Optimalcode, value); }
        #endregion

        #region "Methods"
        /// <summary>
        /// Allgemeine Berechnung der Wahrscheinlichkeit
        /// </summary>
        /// <param name="chracterCount"></param>
        /// <param name="messageLength"></param>
        /// <returns></returns>
        public static double CalcProbability(long chracterCount, long messageLength)
        {
            return (double)chracterCount / (double)messageLength;
        }

        /// <summary>
        /// Wahrscheinlichkeit des aktuellen Buchstabens Berechnen
        /// </summary>
        /// <param name="messageLength">Gesamantzahl Zechen in der Nachricht</param>
        public void CalcProbability(int messageLength)
        {
            Probability = CalcProbability(CharacterCountInMessage, messageLength);
        }
        #endregion

    }
}