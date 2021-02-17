using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Dnevnik
{
    /// <summary>
    /// Interaction logic for CreateInstanceOfEntity.xaml
    /// </summary>
    public partial class CreateInstanceOfEntityWindow : Window
    {   
        Database db;
        private string _tableTitle;
        public bool IsTextBoxReadOnly { get; set; }
        public CreateInstanceOfEntityWindow(string tableTitle, string userLogin)
        {
            InitializeComponent();
            DataContext = new EntityFieldsViewModel(userLogin, tableTitle);
            _tableTitle = tableTitle;           
        }

        public CreateInstanceOfEntityWindow(DocumentView document, string userLogin)
        {
            InitializeComponent();
            DataContext = new EntityFieldsViewModel(userLogin, document);
            _tableTitle = document.EntityName;                       
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {

            this.DialogResult = true;

            //var data = docViewModel.GetDocumentsForMainWindow(_tableTitle);
            //MainWindow mainWindow;
            //mainWindow.instancesListBox.ItemsSource = data;

        }
        
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

       
    }
}
