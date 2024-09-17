
namespace Differencial.Web
{
    public class ModelBindersConfig
    {
        public static void RegisterModelBinders()
        {
            ModelBinders.Binders.Add(typeof(decimal?), new DecimalModelBinder());
            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());
        }
    }
}
