
namespace BullsAndCows.Models
{
    using System;
    using System.Linq;

    public class Guess
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Number { get; set; }

        public DateTime DateMade { get; set; }

        public int CowsCount { get; set; }

        public int BullsCount { get; set; }

        public int GameId { get; set; }

        public virtual Game Game { get; set; }
    }

}
