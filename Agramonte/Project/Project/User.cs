using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class User
    {
        public String username;
        public String password;

        public User(String username, String password)
        {
            this.username = username;
            this.password = password;
        }

        public String getUserName()
        {
            return username;
        }
        public String getPassWord()
        {
            return password;
        }
    }
}
