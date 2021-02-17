using Dnevnik.Models;
using Dnevnik.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Dnevnik
{    
    public class EntityFieldsViewModel : INotifyPropertyChanged
    {
        Database db;

        RelayCommand openLinkedTableCommand;
        RelayCommand returnToParenTableCommand;
        RelayCommand openChooseLinkedTableWindowCommand;
        private string tableTitle;        
        private List<DocumentView> displayedDocuments = new List<DocumentView>();
        
        public Dictionary<string, string> parentLinkFieldIds = new Dictionary<string, string>(); 
        public Dictionary<string, string> currentLinkFieldIds = new Dictionary<string, string>(); 
        string FileName;
        public EntityFieldsViewModel(string fileName, string title)
        {                      
            db = new Database(fileName);
            tableTitle = title;
            FileName = fileName;
            SetFieldsToDisplay();
        }
        public EntityFieldsViewModel(string fileName, DocumentView doc)
        {
            db = new Database(fileName);
            tableTitle = doc.EntityName;
            FileName = fileName;
            SetFieldsToDisplay(doc.DocumentID);
        }
        public RelayCommand OpenLinkedTableCommand
        {
            get
            {
                return openLinkedTableCommand ??
                  (openLinkedTableCommand = new RelayCommand((o) =>
                  {
                      
                      if (SelectedField != null && SelectedField.IsLink)
                      {
                          IsBackButtonVisible = true;
                          tableTitle = SelectedField.Title;

                          // find row id by tablename and clicked row value
                          //SelectedField.FValue                          
                          if (!currentLinkFieldIds.ContainsKey(tableTitle + "_id") || currentLinkFieldIds[tableTitle + "_id"] == null)
                          {
                                throw new Exception("Attempting to open linked table, but no linked field found for " + SelectedField.Title);
                          }
                          try
                          {
                              int docId = Convert.ToInt32(currentLinkFieldIds[tableTitle + "_id"]);
                              SetFieldsToDisplay(docId);
                          } catch (FormatException ex)
                          {
                              throw new Exception(ex.Message);
                          }
                          
                                              
                      }                      
                  }));
            }
        }


        public RelayCommand OpenChooseLinkedTableWindowCommand
        {
            get
            {
                return openChooseLinkedTableWindowCommand ??
                  (openChooseLinkedTableWindowCommand = new RelayCommand((o) =>
                  {
                      if (SelectedField != null && SelectedField.IsLink)
                      {
                          SelectFromLookupWindow selectFromLookupWindow = new SelectFromLookupWindow(FileName, SelectedField.Title);
                          if (selectFromLookupWindow.ShowDialog().Value)
                          {
                              LinkField row = selectFromLookupWindow.ViewModel.SelectedRow;
                              if (row != null)
                              {
                                  var tempFields = Fields;                                 
                                  Field fieldToEdit = tempFields.Where(x => x.Title == SelectedField.Title).First();
                                  currentLinkFieldIds[SelectedField.Title + "_id"] = row.LinkedRowId;
                                  parentLinkFieldIds[SelectedField.Title + "_id"] = row.LinkedRowId;

                                  fieldToEdit.FValue = row.Name;
                                  SetNewFields(tempFields.ToList());
                              }
                          }
                      }
                  }));
            }
        }
        public RelayCommand ReturnToParenTableCommand
        {
            get
            {
                return returnToParenTableCommand ??
                  (returnToParenTableCommand = new RelayCommand((o) =>
                  {
                      
                      if (displayedDocuments.Count > 1)
                      {
                          var lastIndex = displayedDocuments.Count - 1;
                          var doc = displayedDocuments[lastIndex - 1];
                          displayedDocuments.RemoveAt(lastIndex);
                          IsBackButtonVisible = displayedDocuments.Count > 1;

                          tableTitle = doc.EntityName;
                          var newFields = GetDocumentByID(tableTitle, doc.DocumentID);
                          SetNewFields(newFields);
                      }
                  }));
            }
        }

        internal void SetFieldsToDisplay(int docId = 0)
        {
            List<Field> newFields;
            
            if (docId == 0)
            {
                newFields = GetFieldsList(tableTitle);
            }
            else
            {
                newFields = GetDocumentByID(tableTitle, docId);
            }
            displayedDocuments.Add(new DocumentView(docId, tableTitle));
            SetNewFields(newFields);
        }

        private void SetNewFields(List<Field> newFields)
        {
            Fields = new ObservableCollection<Field>();
            foreach (var field in newFields)
            {
                Fields.Add(field);
            }
        }


        private bool _isBackButtonVisible = false;
        public bool IsBackButtonVisible
        {
            get { return _isBackButtonVisible; }
            set
            {
                _isBackButtonVisible = value;
                OnPropertyChanged("IsBackButtonVisible");
            }
        }

        private ObservableCollection<Field> _fields;
        public ObservableCollection<Field> Fields
        {
            get { return _fields; }
            set
            {
                _fields = value;
                OnPropertyChanged("Fields");
            }
        }

        private Field selectedField;
        public Field SelectedField
        {
            get { return selectedField; }
            set
            {
                selectedField = value;
                OnPropertyChanged("SelectedField");
                if (Fields.Count != 0)
                {                     
                    var tempFields = Fields.Select(c => { c.IsOpenLookupButtonsVisible = false;
                        c.IsOpenLinkedRowButtonVisible = false; 
                        return c; }).ToList();
                    Field fieldToEdit = tempFields.Where(x => x.Title == SelectedField.Title).First();

                    if (fieldToEdit.IsLink && displayedDocuments.Count == 1)
                    {
                        fieldToEdit.IsOpenLookupButtonsVisible = true;
                    }

                    if (fieldToEdit.IsLink)
                    {
                        fieldToEdit.IsOpenLinkedRowButtonVisible = true;
                    }                    
                    SetNewFields(tempFields);
                }
               
            }
        }

        public List<Field> GetFieldsList(string tableTitle)
        {
            List<Field> listOfDocs = new List<Field>();
            var docs = db.GetFieldNameListOfEntity(tableTitle);
            var linkFields = db.getLinkFields(tableTitle);
            List<string> lis = docs.ToList();

            for (int i = 1; i < docs.Count(); i++)
            {
                if (lis[i].EndsWith("_id"))
                {                   
                    continue;
                }
                Field field = new Field(lis[i]);
                var isLinkField = linkFields.Where(x => x == field.Title).Any();
                field.IsLink = isLinkField;
                listOfDocs.Add(field);
            }

            return listOfDocs;
        }

        public List<Field> GetDocumentByID(string tableTitle, int id)
        {
            List<Field> listOfFieldsValues = new List<Field>();
            var document = db.GetDocumentByID(tableTitle, id);
            Field doc_field;
            List<string> values = new List<string>();

            var linkFields = db.getLinkFields(tableTitle);
            currentLinkFieldIds = new Dictionary<string, string>();
            foreach (DictionaryEntry field in document)
            {
                values = field.Value as List<string>;
                for (int i = 0; i < values.Count(); i++)
                {
                    if (field.Key.ToString() == "id")
                    {                     
                        continue;
                    }
                    if (field.Key.ToString().EndsWith("_id"))
                    {
                        if (displayedDocuments.Count == 0)
                        {
                            parentLinkFieldIds.Add(field.Key.ToString(), values[i].ToString());
                        }
                        currentLinkFieldIds.Add(field.Key.ToString(), values[i].ToString());
                        continue;
                    }
                    
                    var isLinkField = linkFields.Where(x => x == field.Key.ToString()).Any();                    
                    doc_field = new Field(field.Key.ToString(), values[i].ToString(), isLinkField);
                    listOfFieldsValues.Add(doc_field);
                }

            }
            return listOfFieldsValues;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
