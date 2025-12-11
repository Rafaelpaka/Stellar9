using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Stellar9.DTOs;
using Stellar9.modelos;
using Stellar9.Services;
using static Stellar9.modelos.Nave;

namespace Stellar9.Controllers
{
    [ApiController]
    [Route("naves")]
    public class NavesController : ControllerBase
    {
        private readonly NaveService _naveService;

        public NavesController(NaveService naveService)
        {
            _naveService = naveService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Nave>), StatusCodes.Status200OK)]
        public IResult ObterNaves([FromQuery] StatusNave? status = null)
        {
           
            var naves = _naveService.NavesPorStatus(status);
            return TypedResults.Ok(naves);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Nave), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MensagemErro), StatusCodes.Status404NotFound)]
        public IResult ObterNavePorId(Guid id)
        {
            var nave = _naveService.ObterNaveID(id);

            if (nave == null)
            {
                return TypedResults.NotFound(
                    new MensagemErro("Nave não encontrada.", $"Nenhuma nave com ID {id} foi encontrada.")
                );
            }

            return TypedResults.Ok(nave);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Nave), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(MensagemErro), StatusCodes.Status400BadRequest)]
        public IResult CriarNave([FromBody] CreateNaveDto dto)
        {
            var (sucesso, mensagemErro, nave) = _naveService.CriarNave(dto);

            if (!sucesso)
            {
                return TypedResults.BadRequest(
                    new MensagemErro(mensagemErro!, "Verifique os dados enviados.")
                );
            }

            return TypedResults.Created($"/naves/{nave!.Id}", nave);
        }

        [HttpPut("{id:guid}/status")]
        [ProducesResponseType(typeof(Nave), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MensagemErro), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(MensagemErro), StatusCodes.Status404NotFound)]
        public IResult AtualizarStatus(Guid id, [FromBody] AtualizarStatusDto dto)
        {
            var (sucesso, mensagemErro, nave) = _naveService.AtualizarStatus(id, dto.NovoStatus);

            if (!sucesso)
            {
                if (mensagemErro!.Contains("não encontrada"))
                {
                    return TypedResults.NotFound(new MensagemErro(mensagemErro));
                }

                return TypedResults.BadRequest(new MensagemErro(mensagemErro));
            }

            return TypedResults.Ok(nave);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(MensagemErro), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(MensagemErro), StatusCodes.Status404NotFound)]
        public IResult RemoverNave(Guid id)
        {
            var (sucesso, mensagemErro) = _naveService.RemoverNave(id);

            if (!sucesso)
            {
                if (mensagemErro!.Contains("não encontrada"))
                {
                    return TypedResults.NotFound(new MensagemErro(mensagemErro));
                }

                return TypedResults.BadRequest(new MensagemErro(mensagemErro));
            }

            return TypedResults.NoContent();
        }
    }
}