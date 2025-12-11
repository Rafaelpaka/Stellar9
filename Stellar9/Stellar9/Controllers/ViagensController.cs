using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Stellar9.DTOs;
using Stellar9.modelos;
using Stellar9.Services;

namespace Stellar9.Controllers
{
    [ApiController]
    [Route("viagens")]
    public class ViagensController : ControllerBase
    {
        private readonly ViagemService _viagemService;

        public ViagensController(ViagemService viagemService)
        {
            _viagemService = viagemService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Viagem>), StatusCodes.Status200OK)]
        public IResult ObterViagens()
        {
            var viagens = _viagemService.ObterTodasViagens();
            return TypedResults.Ok(viagens);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Viagem), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MensagemErro), StatusCodes.Status404NotFound)]
        public IResult ObterViagemPorId(Guid id)
        {
            var viagem = _viagemService.ObterViagemPorId(id);

            if (viagem == null)
            {
                return TypedResults.NotFound(
                    new MensagemErro("Viagem não encontrada.", $"Nenhuma viagem com ID {id} foi encontrada.")
                );
            }

            return TypedResults.Ok(viagem);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Viagem), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(MensagemErro), StatusCodes.Status400BadRequest)]
        public IResult CriarViagem([FromBody] CriarViagemDto dto)
        {
            var (sucesso, mensagemErro, viagem) = _viagemService.CriarViagem(dto);

            if (!sucesso)
            {
                return TypedResults.BadRequest(
                    new MensagemErro(mensagemErro!, "Verifique os dados enviados.")
                );
            }

            return TypedResults.Created($"/viagens/{viagem!.Id}", viagem);
        }
    }
}