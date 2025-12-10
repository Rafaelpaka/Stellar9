using static Stellar9.modelos.Nave;

namespace Stellar9.DTOs
{
    public record CreateNaveDto(
    
        string Nome,
        string Modelo,
        decimal CapacidadeCargaKG,
        StatusNave Status = StatusNave.EmOrbita
        );

}
