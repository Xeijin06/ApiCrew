using System.ComponentModel.DataAnnotations;
using CrewMobile.Common.Models;
using Newtonsoft.Json;

namespace CrewMobile.Domain.Models
{
    public class Passanger : PassangerCom
    {
        [Key]
        public int PassangerId { get; set; }

        public int IrregularOperationId { get; set; }

        [JsonIgnore]
        public virtual IrregularOperation IrregularOperation { get; set; }

    }
}
