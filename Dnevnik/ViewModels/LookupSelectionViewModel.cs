using Dnevnik.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Dnevnik.ViewModels
{
    public class LookupSelectionViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<LinkField> _rows;
        public ObservableCollection<LinkField> Rows
        {
            get { return _rows; }
            set
            {
                _rows = value;
                OnPropertyChanged("Rows");
            }
        }

        private LinkField _selectedRow;
        public LinkField SelectedRow
        {
            get { return _selectedRow; }
            set
            {
                _selectedRow = value;
                OnPropertyChanged("SelectedRow");
            }
        }

        Database db;
        string TableName;
       
        public LookupSelectionViewModel(string fileName, string tableName)
        {
            db = new Database(fileName);
            TableName = tableName;
            SetTableRows();
        }

        private void SetTableRows()
        {
            var rows = GetTableRows();
            Rows = new ObservableCollection<LinkField>();
            foreach (var row in rows)
            {
                Rows.Add(row);
            }
        }

        private LinkField[] GetTableRows()
        {
            return db.GetFirstImportantRowsForTable(TableName);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
