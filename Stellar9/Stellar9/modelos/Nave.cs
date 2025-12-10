namespace Stellar9.modelos
{
    public class Nave
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nome { get; set; } = string.Empty;

        public string Modelo { get; set; } = string.Empty;

        public decimal CapacidadeCargaKG { get; set; } = 0;

        public enum StatusNave{
            EmOrbita,
            Viajando,
            Manutencao
        }

    }
}
