using System;
using System.Collections.Generic;
using System.Text;

namespace ValidPassword.Tests
{
    public class BadRequestResponse
    {
        public string type { get; set; }
        public string title { get; set; }
        public int status { get; set; }
        public string traceId { get; set; }
        public Errors errors { get; set; }
    }

    public class Errors
    {
        public List<string> Password { get; set; }
        public List<string> UserName { get; set; }
    }
}
