using System.Collections.Generic;

namespace MovieProjectWithUmbraco.Models
{
    public class DetailedUserInfo
    {
        public BasicInfoModel BasicInfo { get; set; }
        public ContactInfoModel ContactInfo { get; set; }
        public IEnumerable<FilmInfo> FilmsInfo { get; set; }
    }
}