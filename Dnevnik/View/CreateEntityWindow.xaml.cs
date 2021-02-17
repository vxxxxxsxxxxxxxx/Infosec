using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for CreateEntityWindow.xaml
    /// </summary>
    public partial class CreateEntityWindow : Window
    {
        Database db;
        public IEnumerable<NewEntityField> Entities;
        public CreateEntityWindow(string userLogin)
        {
            InitializeComponent();
            newEntityFieldsGrid.DataContext = new EntityViewModel();
            db = new Database(userLogin);
        }

        // This snippet is much safer in terms of preventing unwanted
        // Exceptions because of missing [DisplayNameAttribute].
        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyDescriptor is PropertyDescriptor descriptor)
            {
                e.Column.Header = descriptor.DisplayName ?? descriptor.Name;
            }
        }

        public IEnumerable<NewEntityField> getData()
        {
            Entities = newEntityFieldsGrid.ItemsSource as IEnumerable<NewEntityField>;

            return Entities;
        }

        

        public IEnumerable<NewEntityField> getFields()
        {
            List<NewEntityField> fieldsCollection = getData().ToList();
            if (fieldsCollection.Count == 0)
                WarningLabel.Content = "Заполните таблицу для полей";
            //MessageBox.Show("заполни таблицу  э", "!", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                foreach (var field in fieldsCollection)
                {
                    if (Validation.IsValid(field.Name))
                        yield return field;
                    else
                        throw new Exception("Название не валидно. Можно использовать только латинские буквы, цифры и нижнее подчеркивание");
                }
            }
            
        }
        public IEnumerable<string> getImportantFieldsNames()
        {
            List<NewEntityField> fieldsCollection = getData().ToList();
            
            foreach (var field in fieldsCollection)
            {
                if (field.Importance)
                    yield return field.Name;
            }
        }
        public bool[] getImportantFields()
        {
            List<NewEntityField> fieldsCollection = getData().ToList();

            bool[] mas = new bool[fieldsCollection.Count()];
            int i = 0;
            foreach (var field in fieldsCollection)
            {
                if (field.Importance)
                    mas[i] = true;
                else
                    mas[i] = false;

                i++;
            }
            return mas;
        }

        private void CreateEntity_Click(object sender, RoutedEventArgs e)
        {
            try
            {               
                if (String.IsNullOrEmpty(EntityNameTextBox.Text))
                {
                    EntityNameTextBox.BorderBrush = Brushes.Red;
                    EntityNameTextBox.BorderThickness = new Thickness(3);
                    WarningLabel.Content = "Укажите название новой сущности";

                }
                else
                {
                    if (!Validation.IsValid(EntityNameTextBox.Text))
                        throw new Exception("Имя таблицы не валидно. Можно использовать только латинские буквы, цифры и нижнее подчеркивание");
                    WarningLabel.Content = "";
                    EntityNameTextBox.BorderThickness = new Thickness(1);
                    EntityNameTextBox.BorderBrush = Brushes.Gray;
                    if (getFields().Count() != 0)
                    {
                        if  (getImportantFieldsNames().Count() != 0) 
                            CreateNewEntity();
                        else
                            WarningLabel.Content = "Выберите хотя бы одно важное поле";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "не ура", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            
        }

        private void CreateNewEntity()
        {
            try
            {
                bool success = db.CreateNewEntity(EntityNameTextBox.Text, getFields());
                if (success)
                {
                    MessageBox.Show("Успешный успех", "Ура!", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("што-то пошло не так", "не ура", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "не ура", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
