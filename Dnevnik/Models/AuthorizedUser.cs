using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dnevnik
{
    public class AuthorizedUser : IDataErrorInfo
    {
        [Key]
        public int UserID { get; set; }
        [DisplayName("Имя")]
        public string Name { get; set; }
        [DisplayName("Фамилия")]
        public string LastName { get; set; }
        [DisplayName("Дата рождения")]        
        public DateTime DateOfBirth { get; set; }
        [DisplayName("Логин")]
        public string Login { get; set; }
        [DisplayName("Пароль")]
        public string Password { get; set; }

        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {                    
                    case "Name":
                        //Обработка ошибок для свойства Name
                        break;
                    case "LastName":
                        //smth
                        break;
                    case "Login":
                        //Обработка ошибок для свойства Position
                        break;
                    case "Password":
                        //Обработка ошибок для свойства Position
                        break;
                }
                return error;
            }
        }
        public string Error => throw new NotImplementedException();
    }
}
