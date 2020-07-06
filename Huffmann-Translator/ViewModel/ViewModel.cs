using Huffmann_Translator.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Huffmann_Translator.Model;
using Microsoft.Win32;
using System.Data.OleDb;
using System.Windows.Data;
using System.ComponentModel;

namespace Huffmann_Translator.ViewModel
{
	class ViewModel : ViewModelBase
    {

        #region "Konstruktor"
		public ViewModel()
		{
			Model = new Model.Model();
			Kompress = new RelayCommand(this.KompressExecute, this.KompressCanExecute);
			Dekompress = new RelayCommand(this.DekompressExecute, this.DekompressCanExecute);
			OpenCodeTable = new RelayCommand(this.OpenCodeTableExecute, null);
			CodeTable = Model.AsciiToHuffmann;			
		}
		#endregion

		#region "Private Fields"
		private Model.Model Model;
        #endregion

        #region "Properties"
        private string _TextAscii;
		public string TextAscii
		{
			get { return _TextAscii; }
			set { SetProperty(ref _TextAscii, value); }
		}

		private string _TextHuffmann;
		public string TextHuffmann
		{
			get { return _TextHuffmann; }
			set { SetProperty(ref _TextHuffmann ,value); }
		}
			

		// TODO: Dictionary unterstützte die schnittstelle IPropertyChanged nicht -> kiene GUI aktualisierung bei neuer Codetabelle
		private Dictionary<string, string> _CodeTable;
		public Dictionary<string, string> CodeTable
		{
			get { return _CodeTable; }
			set { SetProperty(ref _CodeTable, value); }
		}

        #endregion

        #region "Commands"
		public ICommand Kompress { get; private set; }

		private void KompressExecute(object obj)
		{
			this.TextHuffmann = Model.Ascii_Huffmann(this.TextAscii);
		}

		private bool KompressCanExecute(object obj)
		{
			return CodeTable != null;
		}

		public ICommand Dekompress { get; private set; }

		private void DekompressExecute(object obj)
		{
			this.TextAscii = Model.Huffmann_Ascii(this.TextHuffmann);
		}

		private bool DekompressCanExecute(object obj)
		{
			return CodeTable != null;
		}

		public ICommand OpenCodeTable { get; private set; }

		private void OpenCodeTableExecute(object obj)
		{
			OpenFileDialog OFD = new OpenFileDialog()
			{
				CheckFileExists = true,
				Filter = Resources.Language.ofd_filter_csv_files + " (*.csv)|*.csv|" + Resources.Language.ofd_filter_all_files + " (*.*)|*.*",
				Multiselect = false,
				Title = Resources.Language.ofd_select_codetable
			};

			
			if (OFD.ShowDialog().HasValue)
			{
				this.Model.ReadCodeTable(OFD.FileName);
				CodeTable = null;
				CodeTable = Model.AsciiToHuffmann;
			}
		}

		#endregion

	}
}
