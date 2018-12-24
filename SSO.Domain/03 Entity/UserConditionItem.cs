using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Domain.Entity
{
    public class UserConditionItem
    {
        #region 用户条件表ID
        /// <summary>
        /// 用户条件表ID
        /// </summary>
        public int UserConditionID { get; set; }
        #endregion

        #region 用户ID
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }
        #endregion

        #region 用户名称
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
        #endregion

        #region 条件ID
        /// <summary>
        /// 条件ID
        /// </summary>
        public int ConditionID { get; set; }
        #endregion

        #region 条件名称
        /// <summary>
        /// 条件名称
        /// </summary>
        public string ConditionName { get; set; }
        #endregion

        #region 条件编码
        /// <summary>
        /// 条件编码
        /// </summary>
        public string ConditionCode { get; set; }
        #endregion

        #region 查询字符串
        /// <summary>
        /// 查询字符串
        /// </summary>
        public string QueryString { get; set; }
        #endregion

        #region 列编码
        /// <summary>
        /// 列编码
        /// </summary>
        public string ColumnCode { get; set; }
        #endregion

        #region 列名称
        /// <summary>
        /// 列名称
        /// </summary>
        public string ColumnName { get; set; }
        #endregion

        #region 用户条件编码
        /// <summary>
        /// 用户条件编码
        /// </summary>
        public string ItemCode { get; set; }
        #endregion

        #region 用户条件名称名称
        /// <summary>
        /// 用户条件名称名称
        /// </summary>
        public string ItemName { get; set; }
        #endregion

        #region 系统编码
        /// <summary>
        /// 系统编码
        /// </summary>
        public string SystemCode { get; set; }
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

        #region 条件类型ID
        /// <summary>
        /// 条件类型ID
        /// </summary>
        public string ConditionType { get; set; }
        #endregion

        #region 条件类型名称
        /// <summary>
        /// 条件类型名称
        /// </summary>
        public string ConditionTypeName { get; set; }
        #endregion
    }
}
