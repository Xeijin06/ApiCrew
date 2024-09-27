using System.ComponentModel.DataAnnotations;
using CrewMobile.Common.Models;

namespace CrewMobile.Domain.Models
{

    public class ProccessFileLog : ProccessFileLogCom
    {
        [Key]
        public int ProccessFileLogId { get; set; }
    }
}