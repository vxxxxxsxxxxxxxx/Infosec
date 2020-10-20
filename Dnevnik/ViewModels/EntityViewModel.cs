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
        private ObservableCollection<Entity> _entity;
        public EntityViewModel()
        {
            _entity = new ObservableCollection<Entity>();
        }
        
        public ObservableCollection<Entity> Entity => _entity;
    }
}
