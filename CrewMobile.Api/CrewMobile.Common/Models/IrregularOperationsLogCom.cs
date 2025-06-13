using System;
using System.ComponentModel.DataAnnotations;

namespace CrewMobile.Common.Models
{
    public class IrregularOperationsLogCom
    {
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "Was Success")]
        public bool WasSuccess { get; set; }

        public string Steps { get; set; }
    }
}