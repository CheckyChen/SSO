using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Domain.Entity
{
    /// <summary>
    /// 用户凭证
    /// </summary>
    public class SSO_Cert
    {
        /// <summary>
        /// 登录令牌
        /// </summary>
        public string token { set; get; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string usercode { set; get; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime timeout { set; get; }
    }
}
