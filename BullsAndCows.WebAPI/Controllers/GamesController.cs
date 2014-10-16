using BullsAndCows.Data;
using BullsAndCows.WebAPI.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using BullsAndCows.Models;
using System.Net;
using System.Net.Http;
using BullsAndCows.GameLogic;

namespace BullsAndCows.WebAPI.Controllers
{
    public class GamesController : BaseApiController
    {
        private const int numberOfResultEntities = 10;

        public GamesController(IBullsAndCowsData data)
            : base(data)
        {

        }

        [Authorize]
        [HttpPost]
        public HttpResponseMessage Create(CreateGameDataModel createGameDataModel)
        {
            if (!ModelState.IsValid)
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (!IsValidNumber(createGameDataModel.Number))
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, "The provided numbers are not digits only.");
            }

            Game game = new Game()
            {
                Name = createGameDataModel.Name,
                RedPlayerId = this.User.Identity.GetUserId(),
                State = GameState.WaitingForOpponent,
                RedPlayerNumber = createGameDataModel.Number,
                DateCreated = DateTime.Now
            };

            this.data.Games.Add(game);
            this.data.SaveChanges();

            GameDataModel model = new GameDataModel()
            {
                Id = game.GameId,
                Name = game.Name,
                Red = this.User.Identity.GetUserName(),
                Blue = "No blue player yet",
                DateCreated = game.DateCreated,
                GameState = GameState.WaitingForOpponent.ToString()
            };

            var response = this.Request.CreateResponse(HttpStatusCode.Created, model);
            return response;
        }

        [Route("Authenticated")]
        [Authorize]
        [HttpGet]
        public HttpResponseMessage FindPersonalGames()
        {
            return FindPersonalGames(1);
        }

        [Route("Authenticated")]
        [Authorize]
        [HttpGet]
        public HttpResponseMessage FindPersonalGames(int page)
        {
            if (page <= 0)
            {
                var badResponse = this.Request.CreateResponse(HttpStatusCode.BadRequest, "The page number should be positive");
                return badResponse;
            }

            var result = this.data.Games.All()
                .Where(g => (g.State == GameState.WaitingForOpponent) ||
                    this.User.Identity.GetUserId() == g.RedPlayer.Id && (g.State != GameState.WonByRedPlayer || g.State != GameState.WonByBluePlayer) ||
                    this.User.Identity.GetUserId() == g.BluePlayer.Id && (g.State != GameState.WonByRedPlayer || g.State != GameState.WonByBluePlayer))
                .OrderBy(g => g.State)
                .ThenBy(g => g.Name)
                .ThenBy(g => g.DateCreated)
                .ThenBy(g => g.RedPlayer.UserName)
                .Skip((page - 1) * numberOfResultEntities)
                .Take(10)
                .Select(GameDataModel.FromGame);

            var response = this.Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage FindGames()
        {
            return FindGames(1);
        }

        [HttpGet]
        public HttpResponseMessage FindGames(int page)
        {
            if (page <= 0)
            {
                var badResponse = this.Request.CreateResponse(HttpStatusCode.BadRequest, "The page number should be positive");
                return badResponse;
            }

            var result = this.data.Games.All()
                .Where(g => g.State == GameState.WaitingForOpponent)
                .OrderBy(g => g.State)
                .ThenBy(g => g.Name)
                .ThenBy(g => g.DateCreated)
                .ThenBy(g => g.RedPlayer.UserName)
                .Skip((page - 1) * numberOfResultEntities)
                .Take(10)
                .Select(GameDataModel.FromGame);

            var response = this.Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [HttpPut]
        [Authorize]

        public HttpResponseMessage Join(int id, JoinDataModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid data provided.");
            }

            if (!IsValidNumber(model.Number))
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid number provided.");
            }

            var currentUserId = this.User.Identity.GetUserId();
            var game = this.data.Games.Find(id);

            if (game.BluePlayer != null)
            {
                if (game.BluePlayer.Id == currentUserId || game.RedPlayer.Id == currentUserId)
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, "You are already joined in this game");
                }
            }
            else if (game.RedPlayer.Id == currentUserId)
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, "You are already joined in this game");
            }

            game.BluePlayerNumber = model.Number;
            game.BluePlayerId = currentUserId;
            this.data.SaveChanges();

            game.State = DesideFirstPlayerGuess.Deside;
            this.data.SaveChanges();

            var responseDetails = new
            {
                result = string.Format("You joined game {0}", game.Name)
            };

            return this.Request.CreateResponse(HttpStatusCode.OK, responseDetails);
        }
        
    }
}