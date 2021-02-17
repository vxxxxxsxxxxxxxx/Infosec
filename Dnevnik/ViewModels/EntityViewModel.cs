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
        private ObservableCollection<NewEntity> _entity;
        public EntityViewModel()
        {
            _entity = new ObservableCollection<NewEntity>();
        }
        
        public ObservableCollection<NewEntity> Entity => _entity;
    }
}
