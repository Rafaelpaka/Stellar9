using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Stellar9.Services;

namespace Stellar9.Controllers
{
    
    [ApiController]
    [Route("admin")]
    public class AdminController : ControllerBase
    {
        private readonly NaveService _naveService;

        public AdminController(NaveService naveService)
        {
            _naveService = naveService;
        }

        [HttpGet("exportar-dados")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IResult ExportarDados()
        {
            var json = _naveService.ExportarNavesComoJson();

            return TypedResults.Content(json, "application/json");
        }

        [HttpGet("exportar-dados/download")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IResult ExportarDadosComoArquivo()
        {
            var json = _naveService.ExportarNavesComoJson();

            var bytes = System.Text.Encoding.UTF8.GetBytes(json);
            var fileName = $"naves-export-{DateTime.UtcNow:yyyyMMdd-HHmmss}.json";

            return TypedResults.File(
                bytes,
                contentType: "application/json",
                fileDownloadName: fileName
            );
        }
    }
}