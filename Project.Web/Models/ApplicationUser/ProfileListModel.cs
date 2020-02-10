using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StopGambleProject.Models.ApplicationUser
{
    public class ProfileListModel
    {
        public IEnumerable<ProfileModel> Profiles { get; set; }
    }
}