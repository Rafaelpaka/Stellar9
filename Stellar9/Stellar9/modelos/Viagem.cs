namespace Stellar9.modelos
{
    public class Viagem
    {
        public Guid Id {  get; set; } = Guid.NewGuid();
        public string Origem { get; set; } = string.Empty;
        public string Destino {  get; set; } = string.Empty;
        public DateTime DataPartida { get; set; }

        public List<Guid> ViagemId = new List<Guid>();
    }
}
