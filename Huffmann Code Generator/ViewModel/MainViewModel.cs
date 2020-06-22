using Huffmann_Code_Generator.Command;
using Huffmann_Code_Generator.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace Huffmann_Code_Generator.ViewModel
{
    class MainViewModel : ViewModelBase
    {
        #region "Konstruktor"
        public MainViewModel()
        {
            HuffmannCodeGenerator = new HuffmannCodeGenerator();
            MessageItemsView = new ListCollectionView(HuffmannCodeGenerator.MessageItems);
            _ListViewSortProperty = "Character"; // Standardwert für Sortirung setzen
            _ListViewSortDirection = ListSortDirection.Ascending;

            // Comands initialisieren
            CalculateHuffmannCode = new RelayCommand(CalculateHuffmannCodeExecute, CalculateHuffmannCodeCanExecute);
        }
        #endregion

        #region "Private Fields"
        private HuffmannCodeGenerator HuffmannCodeGenerator { get; set; }
        #endregion

        #region "Properties"
        // Enthält die aktuell eingegebene Nachrcht deren Optimalcode ermittelt werden soll
        private string _Message;
        public string Message
        {
            get { return _Message; }
            set { SetProperty(ref _Message, value); }
        }

        //Enthält die Daten die zur Ermittlung des Optimalcodes berechnet wurden zur Anzeige an der ListView
        private ListCollectionView _MessageItemsView;
        public ListCollectionView MessageItemsView
        {
            get { return _MessageItemsView; }
            set { SetProperty(ref _MessageItemsView, value); }
        }

        // Enthält die Daten die zur Ermittlung des Optimalcodes berechnet wurden
        private ObservableCollection<MessageItem> _MessageItems;
        public ObservableCollection<MessageItem> MessageItems
        {
            get => _MessageItems;
            set
            {
                SetProperty(ref _MessageItems, value);
            }
        }

        /// <summary>
        /// Gibt an nach welchem Property die ListView sortiert werden soll
        /// </summary>
        private string _ListViewSortProperty;
        public string ListViewSortProperty
        {
            get => _ListViewSortProperty;
            set
            {
                SetProperty(ref _ListViewSortProperty, value);
                UpdateSorting();
            }
        }

        /// <summary>
        /// Gibt die Sortierrichtung an
        /// </summary>
        private ListSortDirection _ListViewSortDirection;
        public ListSortDirection ListViewSortDirection { get => _ListViewSortDirection;
            set
            {
                _ListViewSortDirection = value;
                UpdateSorting();
            }
        }

        #endregion

        #region "Commands"

        public ICommand CalculateHuffmannCode { get; private set; }

        /// <summary>
        /// Generiert den HuffmannCode
        /// </summary>
        /// <param name="obj"></param>
        private void CalculateHuffmannCodeExecute(object obj)
        {
            HuffmannCodeGenerator.GenerateHuffmannCode(Message);
            MessageItems = new ObservableCollection<MessageItem>(HuffmannCodeGenerator.MessageItems);
            MessageItemsView = new ListCollectionView(HuffmannCodeGenerator.MessageItems);
            UpdateSorting();
        }

        /// <summary>
        /// Prüft ob der HuffmannCode generiert werden kann - geht erst ab 2 Zeichen
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool CalculateHuffmannCodeCanExecute(object obj)
        {
            if (Message is null)
                return false;
            else
                return Message.Length >= 2;
        }
        #endregion

        /// <summary>
        /// Sortiert die Listview anhand der im ListViewSortProperty festgelegten Eigenschaft
        /// </summary>
        public void UpdateSorting()
        {
            MessageItemsView.SortDescriptions.Clear();
            MessageItemsView.SortDescriptions.Add(new System.ComponentModel.SortDescription(ListViewSortProperty, this.ListViewSortDirection));
        }
    }
}
