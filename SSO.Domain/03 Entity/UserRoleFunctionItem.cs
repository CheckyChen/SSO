using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Domain.Entity
{
    public class UserRoleFunctionItem
    {
        #region 角色功能ID
        /// <summary>
        /// 角色功能ID
        /// </summary>
        public int RoleFuncID { get; set; }
        #endregion

        #region 图标
        /// <summary>
        /// 图标
        /// </summary>
        public string FuncIcon { get; set; }
        #endregion

        #region 功能ID
        /// <summary>
        /// 功能ID
        /// </summary>
        public int FuncID { get; set; }
        #endregion

        #region 父级ID
        /// <summary>
        /// 父级ID
        /// </summary>
        public int ParentID { get; set; }
        #endregion

        #region 排序
        /// <summary>
        /// 排序
        /// </summary>
        public int SeqNo { get; set; }
        #endregion

        #region 角色ID
        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleID { get; set; }
        #endregion

        #region 功能/页面编码
        /// <summary>
        /// 功能/页面编码
        /// </summary>
        public string FuncCode { get; set; }
        #endregion

        #region 功能/页面名称
        /// <summary>
        /// 功能/页面名称
        /// </summary>
        public string FuncName { get; set; }
        #endregion

        #region 功能/页面类型
        /// <summary>
        /// 功能/页面类型
        /// </summary>
        public string FuncType { get; set; }
        #endregion

        #region 功能/页面类型名称
        /// <summary>
        /// 功能/页面类型名称
        /// </summary>
        public string FuncTypeName { get; set; }
        #endregion

        #region 系统编码
        /// <summary>
        /// 系统编码
        /// </summary>
        public string SystemCode { get; set; }
        #endregion

        #region 系统名称
        /// <summary>
        /// 系统名称
        /// </summary>
        public string SystemName { get; set; }
        #endregion

        #region 状态
        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }
        #endregion

        #region 创建时间
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
        #endregion

        #region 创建人
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }
        #endregion

        #region 修改时间
        /// <summary>
        /// 修改时间
        /// </summary>
        public string UpdateTime { get; set; }
        #endregion

        #region 修改人
        /// <summary>
        /// 修改人
        /// </summary>
        public string UpdateBy { get; set; }
        #endregion

        #region Url
        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }
        #endregion
    }
}
