namespace BullsAndCows.WebAPI.DataModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    public class GuessInputDataModel
    {
        [MinLength(4)]
        [MaxLength(4)]
        public string Number { get; set; }
    }
}