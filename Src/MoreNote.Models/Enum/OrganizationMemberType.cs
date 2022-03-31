﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Enum
{
    /// <summary>
    /// 仓库访问者类型
    /// 
    /// 成员可以归为个人和团队
    /// 
    /// 团队是组织设置的一个管理架构维度，一个组织可以设置多个
    /// </summary>
    public enum OrganizationMemberType
    {
        Personal=0x01,
        Team=0x02

    }
}