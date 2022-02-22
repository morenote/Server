

using Autofac.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Property
{
    /// <summary>
    /// 属性注入选择器
    /// </summary>
    public class AutowiredPropertySelector : IPropertySelector
    {
        public bool InjectProperty(PropertyInfo propertyInfo, object instance)
        {
            // 带有 AutowiredAttribute 特性的属性会进行属性注入
            return propertyInfo.CustomAttributes.Any(it => it.AttributeType == typeof(AutowiredAttribute));
        }
    }
}
