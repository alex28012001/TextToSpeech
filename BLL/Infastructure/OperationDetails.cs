using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Infastructure
{
    public class OperationDetails
    {
        public OperationDetails(bool successed,string msg,string prop)
        {
            Successed = successed;
            Message = msg;
            Property = prop;
        }
        public bool Successed { get; set; }
        public string Message { get; set; }
        public string Property { get; set; }
    }
}
