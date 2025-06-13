using System.ComponentModel.DataAnnotations;

namespace CrewMobile.Common.Models
{
    public class GroupCom
    {
        [Display(Name = "Group Id")]
        public Guid GroupGuid { get; set; }
    }
}
