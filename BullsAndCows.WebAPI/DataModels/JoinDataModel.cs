using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BullsAndCows.WebAPI.DataModels
{
    public class JoinDataModel
    {
        [Required]
        [MinLength(4)]
        [MaxLength(4)]
        public string Number { get; set; }
    }
}