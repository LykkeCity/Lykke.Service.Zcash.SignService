using System.Threading.Tasks;
using Lykke.Service.Zcash.SignService.Core.Services;
using Lykke.Service.Zcash.SignService.Helpers;
using Lykke.Service.Zcash.SignService.Models;
using Lykke.Service.Zcash.SignService.Models.Sign;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.Zcash.SignService.Controllers
{
    [Route("/api/sign")]
    public class SignController : Controller
    {
        private readonly ITransactionService _transactionService;

        public SignController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(SignTransactionResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> SignTransaction([FromBody]SignTransactionRequest signRequest)
        {
            if (!ModelState.IsValid ||
                !ModelState.IsValidRequest(signRequest, _transactionService, out var tx, out var spentOutputs, out var branchId))
            {
                return BadRequest(ErrorResponse.Create(ModelState));
            }

            var signed = await _transactionService.SignAsync(tx, spentOutputs, signRequest.PrivateKeys, branchId);

            return Ok(new SignTransactionResponse()
            {
                SignedTransaction = signed
            });
        }
    }
}
