using BullsAndCows.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace BullsAndCows.WebAPI.Controllers
{
    public class BaseApiController : ApiController
    {
        protected IBullsAndCowsData data;

        public BaseApiController(IBullsAndCowsData data)
        {
            this.data = data;
        }

        protected bool IsValidNumber(string number)
        {
            int parsedNumber;
            if (int.TryParse(number, out parsedNumber))
            {
                return true;
            }

            return false;
        }
    }
}