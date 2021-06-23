using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Globalization;
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
            if (bindingContext.ModelType != typeof(long?)) return Task.CompletedTask;
            if (!bindingContext.BindingSource.CanAcceptDataFrom(BindingSource.Custom)) return Task.CompletedTask;
            var modelName = bindingContext.ModelName;
            // Try to fetch the value of the argument by name
            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }
            bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

            var value = valueProviderResult.FirstValue;

            // Check if the argument value is null or empty
            if (string.IsNullOrEmpty(value))
            {
                return Task.CompletedTask;
            }

            // Attempt to parse the long?
            if (long.TryParse(s: value, style: NumberStyles.HexNumber, null, out long valueAsLong))
            {
                bindingContext.Result = ModelBindingResult.Success(valueAsLong);
            }
            return Task.CompletedTask;
        }
    }
}