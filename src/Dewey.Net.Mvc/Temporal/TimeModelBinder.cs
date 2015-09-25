using Dewey.Net.Temporal;
using System.Web.Mvc;

namespace Dewey.Net.Mvc.Temporal
{
    public class TimeModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var key = bindingContext.ModelName;

            var val = bindingContext.ValueProvider.GetValue(key);

            if (val != null) {
                var s = val.AttemptedValue as string;

                if (s != null) {
                    return new Time(s);
                }
            }
            return null;
        }
    }
}
