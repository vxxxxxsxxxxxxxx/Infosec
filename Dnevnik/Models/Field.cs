using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dnevnik
{
    //простенький класс для бинда данных (в шаблонах gridcolumn забинжены поля этого класса)
    public class Field
    {
        public Field(string title)
        {
            Title = title;
            FValue = "";
            IsLink = false;
        }
        public Field(string title, string value)
        {
            Title = title;
            FValue = value;
            IsLink = false;
        }
        public Field(string title, string value, bool isLink)
        {
            Title = title;
            FValue = value;
            IsLink = isLink;
        }
        public string Title { get; set; }
        public string FValue { get; set; }
        public bool IsLink { get; set; }
        public bool IsOpenLookupButtonsVisible { get; set; } = false;
        public bool IsOpenLinkedRowButtonVisible { get; set; } = false;
    }
}
