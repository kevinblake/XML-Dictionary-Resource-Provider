using System;
using System.Web.UI.WebControls;
using Localisation;

namespace Web.Sample
{
    public partial class Default : LocalisedPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                _changeLocation.SelectedValue = LocalisationSettings.CurrentLocale;
            }
        }

        protected void SelectLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {

            LocalisationSettings.CurrentLocale = ((DropDownList) sender).SelectedValue;
            Response.Redirect(Request.Url.PathAndQuery);
        }
    }
}