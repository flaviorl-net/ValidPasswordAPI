using System;
using System.Collections.Generic;
using System.Text;

namespace ValidPassword.Domain.Entities
{
    public class User
    {
        public int ID { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }

    }
}
