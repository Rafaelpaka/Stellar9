

namespace Stellar9.DTOs
{
    public record CriarViagemDto(
        string Origem,
        string Destino,
        DateTime DataPartida,
        List<Guid> ViagemId
        );
}
