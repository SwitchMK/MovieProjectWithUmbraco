﻿using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Web.Models;

namespace MovieProjectWithUmbraco.Models
{
    public class UsersModel
    {
        public IEnumerable<UserInfo> UsersInfo { get; set; }
    }
}