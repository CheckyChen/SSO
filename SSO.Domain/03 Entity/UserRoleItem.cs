﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSO.Domain.Entity
{
    public class UserRoleItem
    {
        #region 用户角色ID
        /// <summary>
        /// 用户角色ID
        /// </summary>
        public int UserRoleID { get; set; }
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

        #region 用户显示名称
        /// <summary>
        /// 用户显示名称
        /// </summary>
        public string DisplayName { get; set; }
        #endregion

        #region 工号
        /// <summary>
        /// 工号
        /// </summary>
        public string EmpNo { get; set; }
        #endregion

        #region 用户类型编码
        /// <summary>
        /// 用户类型编码
        /// </summary>
        public string UserType { get; set; }
        #endregion

        #region 用户类型名称
        /// <summary>
        /// 用户类型名称
        /// </summary>
        public string UserTypeName { get; set; }
        #endregion

        #region 角色ID
        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleID { get; set; }
        #endregion

        #region 是否超级管理员
        /// <summary>
        /// 是否超级管理员
        /// </summary>
        public Boolean IsSuperAdmin { get; set; }
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

        #region 角色名称
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }
        #endregion

        #region 角色描述
        /// <summary>
        /// 角色描述
        /// </summary>
        public string RoleDescription { get; set; }
        #endregion
    }
}
