using System.Threading.Tasks;
using Lykke.Service.Zcash.SignService.Core.Services;
using Lykke.Service.Zcash.SignService.Models;
using Lykke.Service.Zcash.SignService.Models.Sign;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.Zcash.SignService.Controllers
{
    [Route("/api/sign")]
    public class SignController : Controller
    {
        private readonly ITransactionService _signService;

        public SignController(ITransactionService signService)
        {
            _signService = signService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(SignTransactionResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> SignTransaction([FromBody]SignTransactionRequest signRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorResponse.Create(ModelState));
            }

            return Ok(new SignTransactionResponse()
            {
                SignedTransaction = await _signService.Sign(signRequest.Tx, signRequest.SpentOutputs, signRequest.PrivateKeys)
            });
        }
    }
}
