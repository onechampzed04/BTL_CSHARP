using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTL_2.Model
{
    public class FuncResult<T>
    {
        public EnumErrorCode ErrorCode { get; set; }
        public string ErrorDesc { get; set; }
        public T Data { get; set; }
    }
}
