using Umbraco.Web;
using System.Web.Optimization;
using System;
using MovieProjectWithUmbraco.DI_Configuration;

namespace MovieProjectWithUmbraco
{
    public class Global : UmbracoApplication
    {
        protected override void OnApplicationStarted(object sender, EventArgs e)
        {
            AutofacConfig.ConfigureContainer();

            base.OnApplicationStarted(sender, e);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}