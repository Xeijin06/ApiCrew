using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CrewMobile.Common.Models;
using Newtonsoft.Json;

namespace CrewMobile.Domain.Models
{
    public class IrregularOperation : IrregularOperationCom
    {
        [Key]
        public int IrregularOperationId { get; set; }

        [JsonIgnore]
        public virtual ICollection<Passanger> Passangers { get; set; }
    }
}
