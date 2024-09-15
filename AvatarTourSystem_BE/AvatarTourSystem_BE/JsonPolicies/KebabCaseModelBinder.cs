using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AvatarTourSystem_BE.JsonPolicies
{
    public class KebabCaseModelBinder: IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            var form = bindingContext.HttpContext.Request.Form;
            var model = Activator.CreateInstance(bindingContext.ModelType);

            foreach (var property in bindingContext.ModelType.GetProperties())
            {
                var kebabCaseName = ToKebabCase(property.Name);
                if (form.TryGetValue(kebabCaseName, out var value))
                {
                    var convertedValue = Convert.ChangeType(value.ToString(), property.PropertyType);
                    property.SetValue(model, convertedValue);
                }
            }

            bindingContext.Result = ModelBindingResult.Success(model);
            return Task.CompletedTask;
        }

        private string ToKebabCase(string str)
        {
            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "-" + x : x.ToString())).ToLower();
        }
    }
}
