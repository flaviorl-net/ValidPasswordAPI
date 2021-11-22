using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ValidPassword.API.ViewModel
{
    public class ReturnValidPasswordViewModel
    {
        public ExecutionReturnViewModel ExecutionReturn { get; set; } = new ExecutionReturnViewModel();

        public bool PasswordIsValid { get; set; }
    }
}
