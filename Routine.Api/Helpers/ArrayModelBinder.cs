using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.Reflection;

namespace Routine.Api.Helpers
{
    public class ArrayModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (!bindingContext.ModelMetadata.IsEnumerableType) {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }
            //ModelName = ids
            //the guids string with "," as seperator
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).ToString();

            if (string.IsNullOrWhiteSpace(value)) {
                bindingContext.Result = ModelBindingResult.Success(null);
            }

            //elementType is Guid
            var elementType = bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];
            var converter = TypeDescriptor.GetConverter(elementType);

            //array of strings
            var values = value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => converter.ConvertFromString(x.Trim())).ToArray();
            //copy to convert to Guid array
            var typeValues = Array.CreateInstance(elementType, values.Length);
            values.CopyTo(typeValues, 0);
            bindingContext.Model = typeValues;
            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
            return Task.CompletedTask;

        }
    }
}
