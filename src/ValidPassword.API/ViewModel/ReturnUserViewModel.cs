using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ValidPassword.API.ViewModel
{
    public class ReturnUserViewModel
    {
        public ExecutionReturnViewModel ExecutionReturn { get; set; } = new ExecutionReturnViewModel();

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }
    }
}
