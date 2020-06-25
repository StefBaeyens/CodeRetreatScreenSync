using Xamarin.Forms;

namespace CodeRetreatScreenSync.Services.Navigation
{
    public class NavigationContext
    {
        public static readonly BindableProperty ParamProperty =
            BindableProperty.CreateAttached("Param", typeof(object), typeof(NavigationContext), null,
                defaultBindingMode: BindingMode.OneWayToSource);

        public static object GetParam(BindableObject view)
        {
            return view.GetValue(ParamProperty);
        }

        public static void SetParam(BindableObject view, object value)
        {
            view.SetValue(ParamProperty, value);
        }
    }
}
