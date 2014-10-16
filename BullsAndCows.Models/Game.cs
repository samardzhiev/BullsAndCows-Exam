namespace BullsAndCows.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class Game
    {
        private string redPlayerNumber;
        private string bluePlayerNumber;

        public Game()
        {
            this.State = GameState.WaitingForOpponent;
            this.BluePlayerGuesses = new HashSet<Guess>();
            this.RedPlayerGuesses = new HashSet<Guess>();
        }

        [Required]
        public int GameId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public GameState State { get; set; }

        [Required]
        public string RedPlayerId { get; set; }

        public virtual User RedPlayer { get; set; }

        [Required]
        public string RedPlayerNumber
        {
            get
            {
                return this.redPlayerNumber;
            }
            set
            {
                if (!this.IsValidNumber(value))
                {
                    throw new ArgumentException("The provided nuber doesn't contain digits only.");
                }

                this.redPlayerNumber = value;
            }
        }

        public string BluePlayerId { get; set; }

        public virtual User BluePlayer { get; set; }

        public string BluePlayerNumber
        {
            get
            {
                return this.bluePlayerNumber;
            }
            set
            {
                if (!this.IsValidNumber(value))
                {
                    throw new ArgumentException("The provided nuber doesn't contain digits only.");
                }

                this.bluePlayerNumber = value;
            }
        }

        public DateTime DateCreated { get; set; }

        public virtual ICollection<Guess> RedPlayerGuesses { get; set; }
        public virtual ICollection<Guess> BluePlayerGuesses { get; set; }

        private bool IsValidNumber(string number)
        {
            int parsedNumber;
            if (number == null)
            {
                return true;
            }

            if (int.TryParse(number, out parsedNumber))
            {
                return true;
            }

            return false;
        }

    }
}
