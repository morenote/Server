using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Service
{
    /// <summary>
    /// 依赖注入服务
    /// </summary>
    public class DependencyInjectionService
    {
        public  static IServiceProvider IServiceProvider { get; set; }
        public IServiceProvider ServiceProvider
        {
            get => IServiceProvider;
        }

        public DependencyInjectionService()
        {
            
            
        }
    }
}
