using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ValidPassword.API.ViewModel
{
    public class ReturnAuthViewModel
    {
        public ExecutionReturnViewModel ExecutionReturn { get; set; } = new ExecutionReturnViewModel();

        public string UserName { get; set; }

        public string Token { get; set; }
    }
}
