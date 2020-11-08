using System.Web;
using System.Web.Mvc;

namespace Assignment3_B_Diarra
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
