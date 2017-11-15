﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinanceMs.Common
{
    /// <summary>
    /// 是否明细
    /// </summary>
    public enum EnumIsDetail
    {
        /// <summary>
        /// 是明细，无子节点
        /// </summary>
        yes = 0,
        /// <summary>
        /// 不是明细，有子节点
        /// </summary>
        no = 1
    }

    /// <summary>
    /// 导入信息存在的情况
    /// </summary>
    public enum ResInfoState
    {
        /// <summary>
        /// 导入数据没问题
        /// </summary>
        ok = 0,
        /// <summary>
        /// 父级code为空，但级数大于1，直接生成1级
        /// </summary>
        parentCodeEmpty = 1,
        /// <summary>
        /// 父级code不存在，直接生成1级
        /// </summary>
        parentCodeFail = 2,
        /// <summary>
        /// 父级存在，但是级数和父级级数相差不是1
        /// </summary>
        layerFail = 3
    }

    /// <summary>
    /// 审批状态
    /// </summary>
    public enum EnumAuditState
    {
        /// <summary>
        /// 0制单
        /// </summary>
        prepare = 0,
        /// <summary>
        /// 提交
        /// </summary>
        submit = 1,
        /// <summary>
        /// 2通过
        /// </summary>
        pass = 2,
        /// <summary>
        /// 3驳回
        /// </summary>
        reject = 3
    }

    /// <summary>
    /// 停用标志
    /// </summary>
    public enum EnumTYBZ
    {
        /// <summary>
        /// 启用
        /// </summary>
        enabled = 0,
        /// <summary>
        /// 停用
        /// </summary>
        disable = 1,
    }

}
