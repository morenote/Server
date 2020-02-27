using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;


namespace MoreNote.Common.ModelBinder
{
    public class Hex2LongModelBinder : IModelBinder
    {
        /// <summary>
        /// 将前端传输hex字符串转换为long类型
        /// </summary>
        /// <param name="bindingContext"></param>
        /// <returns></returns>
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(long)) return Task.CompletedTask;
            if (!bindingContext.BindingSource.CanAcceptDataFrom(BindingSource.Custom)) return Task.CompletedTask;
            var formName = bindingContext.ModelName;
            string stringValue = null;
            try
            {
                stringValue = bindingContext.HttpContext.Request.Form[formName];
            }catch(Exception ex)
            {

            }
           
            if (string.IsNullOrEmpty(stringValue))
            {
                 stringValue = bindingContext.HttpContext.Request.Query[formName];
            }
          
            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, stringValue, stringValue);

            // Attempt to parse the long                
            if (long.TryParse(s:stringValue,style:NumberStyles.HexNumber,null, out long valueAsLong))
            {
                bindingContext.Result = ModelBindingResult.Success(valueAsLong);
            }
            return Task.CompletedTask;
        }
    }
}
