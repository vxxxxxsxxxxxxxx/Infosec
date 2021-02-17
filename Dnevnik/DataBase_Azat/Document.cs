using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dnevnik
{
    public class Document
    {
        public OrderedDictionary Fields { get; set; }
        public int[] AnnotationFields { get; set; }

        public Document(params int[] annotationFields)
        {
            Fields = new OrderedDictionary();
            AnnotationFields = annotationFields;
        }

        public Document(OrderedDictionary dictionary)
        {
            Fields = dictionary;
        }
    }
}
