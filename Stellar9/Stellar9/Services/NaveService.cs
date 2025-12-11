using Stellar9.DTOs;
using Stellar9.modelos;
using Stellar9.Repositories;
using static Stellar9.modelos.Nave;

namespace Stellar9.Services
{
    public class NaveService
    {
        private readonly PortoRepository _repository;

        public NaveService(PortoRepository repository)
        {

            _repository = repository;

        }

        public IEnumerable<Nave> NavesPorStatus(StatusNave? status = null) {

            var naves = _repository.ObterNaves();

            if (status != null) {
            
            naves = naves.Where(n => n.Status == status.Value );
            }

            return naves;
        }

        public Nave? ObterNaveID(Guid id) { 
        
            return _repository.ObterNaveID(id);

        }

        public (bool Sucesso, string? MensagemErro, Nave? nave) CriarNave(CreateNaveDto dto) {

            if (dto.CapacidadeCargaKG < 0) {

                return(false, "A capacidade não pode ser negativa", null);
            }

            if (string.IsNullOrWhiteSpace(dto.Nome)) {

                return (false, "O nome da nave não pode ser vazio", null);
            }
            if (string.IsNullOrWhiteSpace(dto.Modelo))
            {
                return (false, "O modelo da nave é obrigatório.", null);
            }

            var nave = new Nave
            {
                Id = Guid.NewGuid(),
                Nome = dto.Nome.Trim(),
                Modelo = dto.Modelo.Trim(),
                CapacidadeCargaKG = dto.CapacidadeCargaKG,
                Status = dto.Status
            };
            _repository.AdicionarNave(nave);

            return (true, null, nave);
        }


        public (bool Sucesso, string? MensagemErro, Nave? Nave) AtualizarStatus(Guid id, StatusNave novostatus) {
        
            var nave = _repository.ObterNaveID(id);

            if (nave == null) {

                return (false, "Nave não encontrada.", null);
            }

            if (nave.Status == StatusNave.Manutencao && novostatus == StatusNave.Viajando) {

                return (false, "Nave em manutenção, é necessario aguardar a manutenção ser realizada para viajar", null);
            }

            nave.Status = novostatus;

            return (true, null, nave);
        }

        public (bool Sucesso, string? MensagemErro) RemoverNave(Guid id)
        {
            var nave = _repository.ObterNaveID(id);

            if (nave == null)
            {
                return (false, "Nave não encontrada.");
            }
            if (_repository.ViagemFutura(id))
            {
                return (false,
                    "Não é possível remover uma nave que está programada para viagens futuras. Cancele as viagens primeiro.");
            }

            _repository.RemoverNave(nave);

            return (true, null);
        }

        public string ExportarNavesComoJson()
        {
            var naves = _repository.ObterNaves();

          
            var options = new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
            };

            return System.Text.Json.JsonSerializer.Serialize(naves, options);
        
    } 
    }
}
