using CantineKata.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CantineKata.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RepasController : Controller
    {
        private readonly IMediator _mediator;

        public RepasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("crediter-compte")]
        public async Task<IActionResult> CrediterCompte([FromBody] CrediterCompteCommand command)
        {

            if (command == null) return BadRequest("commande invalide");

            var result = await _mediator.Send(command);

            if(result)
                return Ok("Crédit effectué avec succés");

            return NotFound("Client non trouvé");
        }

        [HttpPost("payer-repas")]
        public async Task<IActionResult> PayerRepas([FromBody] PayerRepasCommand command)
        {
            if (command == null) 
                return BadRequest("commande invalide");

            try
            {
                var ticket = await _mediator.Send(command);
                return Ok(ticket);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
