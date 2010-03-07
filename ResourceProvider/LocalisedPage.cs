using System.Web.UI;
using System.Web;
using System.Threading;
using System.Globalization;

namespace Localisation
{
    public class LocalisedPage : Page
    {
        protected override void InitializeCulture()
        {
            if (HttpContext.Current.Session["CurrentLocale"] == null)
            {
                HttpContext.Current.Session["CurrentLocale"] = Thread.CurrentThread.CurrentCulture.ToString();
            }
            else
            {
                var selectedLanguage = HttpContext.Current.Session["CurrentLocale"].ToString();
                Page.UICulture = selectedLanguage;
                Page.Culture = selectedLanguage;

                Thread.CurrentThread.CurrentCulture =
                    CultureInfo.CreateSpecificCulture(selectedLanguage);
                Thread.CurrentThread.CurrentUICulture = new
                    CultureInfo(selectedLanguage);
            }

            base.InitializeCulture();
        }


    }
}
