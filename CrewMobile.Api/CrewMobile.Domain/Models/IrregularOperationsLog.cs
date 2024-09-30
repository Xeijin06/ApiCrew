using System.ComponentModel.DataAnnotations;
using CrewMobile.Common.Models;

namespace CrewMobile.Domain.Models
{
    public class IrregularOperationsLog : IrregularOperationsLogCom
    {
        [Key]
        public int IrregularOperationsLogId { get; set; }
    }
}
