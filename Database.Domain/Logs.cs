using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Domain
{
    public class Logs
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string CurrentExeption { get; set; }
        public string ExceptionLogger { get; set; }
        public string Url { get; set; }


    }
}
