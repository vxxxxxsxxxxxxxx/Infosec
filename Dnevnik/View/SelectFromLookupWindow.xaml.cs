using Dnevnik.ViewModels;
using System;
using System.Collections.Generic;
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

namespace Dnevnik.View
{
    /// <summary>
    /// Логика взаимодействия для SelectFromLookupWindow.xaml
    /// </summary>
    public partial class SelectFromLookupWindow : Window
    {
        public LookupSelectionViewModel ViewModel;
        public SelectFromLookupWindow(string fileName, string tableName)
        {            
            InitializeComponent();

            ViewModel = new LookupSelectionViewModel(fileName, tableName);
            DataContext = ViewModel;            
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
