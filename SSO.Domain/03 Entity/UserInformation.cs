using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Domain.Entity
{
    public class UserInformation
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfo UserInfo { get; set; }

        /// <summary>
        /// 用户角色列表
        /// </summary>
        public UserRoleItem[] UserRoleInfos { get; set; }

        /// <summary>
        /// 用户角色功能列表
        /// </summary>
        public UserRoleFunctionItem[] UserRoleFunctionInfos { get; set; }

        /// <summary>
        /// 用户条件定义列表
        /// </summary>
        public UserConditionItem[] UserConditionInfos { get; set; }
    }
}
