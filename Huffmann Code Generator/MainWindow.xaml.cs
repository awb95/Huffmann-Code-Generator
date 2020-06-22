using Huffmann_Code_Generator.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Huffmann_Code_Generator
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader;

            if(headerClicked != null)
            {
                var columnBinding = headerClicked.Column.DisplayMemberBinding as Binding;
                var sortBy = columnBinding?.Path.Path ?? headerClicked.Column.Header as string;
                var viewModel = (MainViewModel)this.FindResource("vm");

                if(viewModel != null)
                {
                    if(viewModel.ListViewSortProperty != sortBy)
                    {
                        // Neues SortProperty
                        viewModel.ListViewSortProperty = sortBy;                        
                    } else if(viewModel.ListViewSortProperty == sortBy && viewModel.ListViewSortDirection == ListSortDirection.Ascending)
                    {
                        // Gleiches SortProperty -> Reihenfolge ändern
                        viewModel.ListViewSortDirection = ListSortDirection.Descending;
                    }
                    else if (viewModel.ListViewSortProperty == sortBy && viewModel.ListViewSortDirection == ListSortDirection.Descending)
                    {
                        // Gleiches SortProperty -> Reihenfolge ändern
                        viewModel.ListViewSortDirection = ListSortDirection.Ascending;
                    }

                        
                }
            }
        }

    }
}
