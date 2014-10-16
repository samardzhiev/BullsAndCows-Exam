using BullsAndCows.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace BullsAndCows.WebAPI.DataModels
{
    public class GameDataModel
    {
        public static Expression<Func<Game, GameDataModel>> FromGame
        {
            get
            {
                return g => new GameDataModel
                {
                    Id = g.GameId,
                    Name = g.Name,
                    Red = g.RedPlayer.UserName,
                    Blue = g.BluePlayer.UserName ?? "No Blue Player yet",
                    DateCreated = DateTime.Now,
                    GameState = g.State.ToString()
                };
            }
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public string Blue { get; set; }

        public string Red { get; set; }

        public string GameState { get; set; }

        public DateTime DateCreated { get; set; }
    }
}