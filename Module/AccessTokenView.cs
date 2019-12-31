using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module
{
    public class AccessTokenView
    {
        public bool IsSuccess { get; set; }
        public AccessTokenModel SuccessModel { get; set; }
        public ErrorAccessTokenModel ErrorModel { get; set; }
    }
}
