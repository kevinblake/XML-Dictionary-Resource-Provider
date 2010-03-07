using System.Web;
using System.Threading;

namespace Localisation
{
    public static class LocalisationSettings
    {
        /// <summary>
        /// Returns the current set locale.  Uses the value from the web.config globalization setting if none has been set.
        /// </summary>
        public static string CurrentLocale
        {
            get
            {
                if (HttpContext.Current.Session == null)
                {
                    return Thread.CurrentThread.CurrentUICulture.ToString();
                }
                return HttpContext.Current.Session["CurrentLocale"] == null ? Thread.CurrentThread.CurrentUICulture.ToString() : HttpContext.Current.Session["CurrentLocale"].ToString();
            }
            set
            {
                HttpContext.Current.Session["CurrentLocale"] = value;
            }
        }



    }
}
