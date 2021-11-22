using System;
using System.Collections.Generic;
using System.Text;

namespace ValidPassword.Domain.ValueObjects
{
    public class ValidationError
    {
        public string Message { get; set; }
        public ValidationError(string message)
        {
            this.Message = message;
        }
    }
}
