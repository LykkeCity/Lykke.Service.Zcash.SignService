using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Lykke.Service.Zcash.SignService.Core.Services;
using Lykke.Service.Zcash.SignService.Models;
using Lykke.Service.Zcash.SignService.Models.Sign;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.Zcash.SignService.Controllers
{
    [Route("api/sign")]
    public class SignController : Controller
    {
        private readonly ITransactionService _signService;

        public SignController(ITransactionService signService)
        {
            _signService = signService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(SignTransactionOkResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public IActionResult SignTransaction([FromBody]SignTransactionRequest signRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorResponse.Create(ModelState));
            }

            var tx = _signService.Sign(signRequest.Tx, signRequest.Coins, signRequest.Keys);

            return Ok(new SignTransactionOkResponse(tx));
        }
    }
}
