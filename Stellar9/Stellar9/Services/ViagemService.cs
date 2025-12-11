using Stellar9.DTOs;
using Stellar9.modelos;
using Stellar9.Repositories;
using static Stellar9.modelos.Nave;

namespace Stellar9.Services
{
    
    public class ViagemService
    {
        private readonly PortoRepository _repository;

        public ViagemService(PortoRepository repository)
        {
            _repository = repository;
        }

        
        public (bool Sucesso, string? MensagemErro, Viagem? Viagem) CriarViagem(CriarViagemDto dto)
        {
            
            if (string.IsNullOrWhiteSpace(dto.Origem))
            {
                return (false, "A origem da viagem é obrigatória.", null);
            }

           
            if (string.IsNullOrWhiteSpace(dto.Destino))
            {
                return (false, "O destino da viagem é obrigatório.", null);
            }

            
            if (dto.Origem.Trim().Equals(dto.Destino.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                return (false, "Origem e destino não podem ser o mesmo local.", null);
            }

           
            if (dto.DataPartida < DateTime.UtcNow)
            {
                return (false, "A data de partida não pode ser no passado. Ainda não inventamos viagem no tempo!", null);
            }

            
            var destinosProibidos = new[] { "Sol", "Sun", "Buraco Negro", "Black Hole" };
            if (destinosProibidos.Any(d => dto.Destino.Contains(d, StringComparison.OrdinalIgnoreCase)))
            {
                return (false, "Destino extremamente perigoso! A SpacePort 9 valoriza a vida da tripulação.", null);
            }

            
            if (dto.ViagemId == null || !dto.ViagemId.Any())
            {
                return (false, "Uma viagem precisa ter pelo menos uma nave.", null);
            }

            
            foreach (var naveId in dto.ViagemId)
            {
                if (!_repository.NaveExiste(naveId))
                {
                    return (false, $"Nave com ID {naveId} não encontrada.", null);
                }
            }

            
            var navesIndisponiveis = new List<string>();
            foreach (var naveId in dto.ViagemId)
            {
                var nave = _repository.ObterNaveID(naveId);
                if (nave != null && nave.Status != StatusNave.EmOrbita)
                {
                    navesIndisponiveis.Add($"{nave.Nome} ({nave.Status})");
                }
            }

            if (navesIndisponiveis.Any())
            {
                return (false,
                    $"As seguintes naves não estão disponíveis (devem estar em órbita): {string.Join(", ", navesIndisponiveis)}",
                    null);
            }

           
            var viagem = new Viagem
            {
                Id = Guid.NewGuid(),
                Origem = dto.Origem.Trim(),
                Destino = dto.Destino.Trim(),
                DataPartida = dto.DataPartida,
                NavesIds = new List<Guid>(dto.ViagemId)
            };

            _repository.AdicionarViagem(viagem);

           
            foreach (var naveId in dto.ViagemId)
            {
                var nave = _repository.ObterNaveID(naveId);
                if (nave != null)
                {
                    nave.Status = StatusNave.Viajando;
                }
            }

            return (true, null, viagem);
        }

       
        public IEnumerable<Viagem> ObterTodasViagens()
        {
            return _repository.ObterTodasViagens();
        }

       
        public Viagem? ObterViagemPorId(Guid id)
        {
            return _repository.ObterViagemPorId(id);
        }
    }
}