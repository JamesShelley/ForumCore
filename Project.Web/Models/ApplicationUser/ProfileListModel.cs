using System.Collections;
using System.Collections.Generic;

namespace StopGambleProject.Models.ApplicationUser
{
    public class ProfileListModel
    {
        public IEnumerable<ProfileModel> Profiles { get; set; }
        public string UserId { get; set; }
    }
}