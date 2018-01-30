using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudClassroom.Models
{
    public class ResponseModel
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
