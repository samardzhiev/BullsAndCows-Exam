namespace BullsAndCows.WebAPI.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Microsoft.AspNet.Identity;

    using BullsAndCows.Data;
    using BullsAndCows.WebAPI.DataModels;
    using BullsAndCows.Models;
    using BullsAndCows.GameLogic;


    public class GuessController : BaseApiController
    {
        public GuessController(IBullsAndCowsData data)
            : base(data)
        {

        }

        [Authorize]
        [HttpPost]
        public HttpResponseMessage Guess(int id, GuessInputDataModel model)
        {
            var currentUserId = this.User.Identity.GetUserId();
            var game = this.data.Games.Find(id);

            if (game == null)
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, "No such game");
            }

            if (!ModelState.IsValid)
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid input data.");
            }

            if (!this.IsValidNumber(model.Number))
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, "The number should contain only digits.");
            }

            if (game.BluePlayer == null)
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, "This game has no second player.");
            }

            if (game.RedPlayerId != currentUserId && game.BluePlayerId != currentUserId)
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, "This is not your game.");
            }

            if (currentUserId == game.RedPlayerId && game.State != GameState.RedInTurn)
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, "This is not your turn.");
            }

            if (currentUserId == game.BluePlayerId && game.State != GameState.BlueInTurn)
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, "This is not your turn.");
            }
            var guess = new Guess();
            if (currentUserId == game.RedPlayerId)
            {
                guess = GuessVerifier.GetResult(game.BluePlayerNumber, model.Number);
                guess.DateMade = DateTime.Now;
                guess.GameId = game.GameId;
                guess.Number = model.Number;
                guess.UserName = currentUserId == game.BluePlayerId ? game.BluePlayer.UserName : game.RedPlayer.UserName;
                game.RedPlayerGuesses.Add(guess);
                game.State = game.State == GameState.BlueInTurn ? GameState.RedInTurn : GameState.BlueInTurn;
                if (guess.CowsCount == 4)
                {
                    game.State = GameState.WonByRedPlayer;
                }
                
            }
            else
            {
                guess = GuessVerifier.GetResult(game.RedPlayerNumber, model.Number);
                game.BluePlayerGuesses.Add(guess);
                game.State = game.State == GameState.BlueInTurn ? GameState.RedInTurn : GameState.BlueInTurn;
                if (guess.CowsCount == 4)
                {
                    game.State = GameState.WonByBluePlayer;
                }
            }

            this.data.SaveChanges();

            return this.Request.CreateResponse(HttpStatusCode.OK, guess);
        }
    }
}