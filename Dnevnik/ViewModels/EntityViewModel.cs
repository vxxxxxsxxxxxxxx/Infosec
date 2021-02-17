using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Dnevnik.Models;

namespace Dnevnik
{
    class EntityViewModel
    {
        private ObservableCollection<NewEntityField> _entity;
        public EntityViewModel()
        {
            _entity = new ObservableCollection<NewEntityField>();
        }
        
        public ObservableCollection<NewEntityField> Entity => _entity;
    }
}
