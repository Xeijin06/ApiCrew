using System.ComponentModel.DataAnnotations;

namespace CrewMobile.Common.Models
{
    public class GroupCom
    {
        [Display(Name = "Group Id")]
        public Guid GroupGuid { get; set; }

        [Display(Name = "Group Nmae")]
        public string? GroupName { get; set; }
    }
}
