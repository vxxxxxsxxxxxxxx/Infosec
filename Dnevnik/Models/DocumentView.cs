﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Dnevnik
{
    public class DocumentView: INotifyPropertyChanged
    {
        public DocumentView(int id, string entityName, string annotationFields)
        {
            DocumentID = id;
            EntityName = entityName;
            AnnotationFields = annotationFields;
        }
        private int _documentID;
        private string _entityName;
        private string _annotationFields;
        [Key]
        public int DocumentID
        {
            get { return _documentID; }
            set
            {
                _documentID = value;
                OnPropertyChanged("DocumentID");
            }
        }
        public string EntityName
        {
            get { return _entityName; }
            set
            {
                _entityName = value;
                OnPropertyChanged("EntityName");
            }
        }
        public string AnnotationFields
        {
            get { return _annotationFields; }
            set
            {
                _annotationFields = value;
                OnPropertyChanged("AnnotationFields");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
