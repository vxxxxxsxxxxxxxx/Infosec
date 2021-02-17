using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dnevnik
{
    public class AuthorizedUsersViewModel
    {
        private UsersDatabase db;
        IEnumerable<AuthorizedUser> usersList;
        public AuthorizedUsersViewModel()
        {
            db = new UsersDatabase();
        }
        //public bool Register()
        //{
        //    return db.AuthorizedUsers;
        //}

    }
}
