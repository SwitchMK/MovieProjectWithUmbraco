using MovieProjectWithUmbraco.Extensions;
using MovieProjectWithUmbraco.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace MovieProjectWithUmbraco.Controllers.Films.Soundtracks
{
    public class SoundtrackController : SurfaceController
    {
        private const string PathToSoundtracksFolder = "~/Views/Partials/Film/Soundtrack/";

        public ActionResult RenderSoundtracksList()
        {
            var soundtrackList = GetSoundtrackList();

            return PartialView(PathToSoundtracksFolder + "_SoundtrackList.cshtml", soundtrackList);
        }

        private SoundtracksList GetSoundtrackList()
        {
            var soundtrackList = CurrentPage.Children.FirstOrDefault(d => d.DocumentTypeAlias == "soundtracksList");

            if (soundtrackList == null)
                return null;

            return new SoundtracksList
            {
                ImagePath = soundtrackList.GetCropUrl(propertyAlias: "image", imageCropMode: ImageCropMode.Max),
                Title = soundtrackList.GetPropertyValue<string>("title"),
                TotalDuration = soundtrackList.GetPropertyValue<int>("totalDuration"),
                Soundtracks = GetSoundtracks(soundtrackList).ToList()
            };
        }

        private IEnumerable<Soundtrack> GetSoundtracks(IPublishedContent page)
        {
            foreach (var soundtrack in page.Children)
            {
                yield return new Soundtrack
                {
                    Title = soundtrack.GetPropertyValue<string>("title"),
                    Composer = soundtrack.GetLinkResponses("composer"),
                    Duration = soundtrack.GetPropertyValue<int>("duration"),
                    ExternalSourceUrl = soundtrack.GetPropertyValue<string>("externalSource")
                };
            }
        }
    }
}