using Dnevnik.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dnevnik
{
    public class NewEntityField
    {
        [DisplayName("Название")]        
        public string Name { get; set; }

        [DisplayName("Важно/Неважно")]
        public bool Importance { get; set; }

        [DisplayName("Ссылка")]
        public bool IsLink { get; set; }
    }
}
