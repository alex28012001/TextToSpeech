using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Infastructure
{
    public class ResultDetails<TData> where TData : class
    {
        public ResultDetails(bool successed, string msg, string prop,TData data)
        {
            Successed = successed;
            Message = msg;
            Property = prop;
            Data = data;
        }
        public bool Successed { get; set; }
        public string Message { get; set; }
        public string Property { get; set; }
        public TData Data { get; set; }
    }
}
