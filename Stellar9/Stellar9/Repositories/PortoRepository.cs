
using Stellar9.modelos;
namespace Stellar9.Repositories
{
    public class PortoRepository
    {
        private readonly List<Nave> naves = new List<Nave>();
        private readonly List<Viagem> viagens = new List<Viagem>();


        public IEnumerable<Nave> ObterNaves() {
        
            
        return naves;
        }

        public Nave? ObterNaveID(Guid id) {

            return naves.FirstOrDefault(n => n.Id == id);
        }

        public void AdicionarNave(Nave nave) {
        
            naves.Add(nave);
        }

        public bool NaveExiste(Guid id) {
        
            return naves.Any(n => n.Id == id);
        }

        public void RemoverNave(Nave nave) {
            naves.Remove(nave);
        }

        public IEnumerable<Viagem> ObterTodasViagens(){

            return viagens;
        }

        public Viagem? ObterViagemPorId(Guid id) {
        
        return viagens.FirstOrDefault(n => n.Id == id);
        }

        public void AdicionarViagem(Viagem viagem) {
        
            viagens.Add(viagem);
        }
        public bool ViagemFutura(Guid naveId) {
        
            DateTime agora = DateTime.Now;
            return viagens.Any(n => n.DataPartida >  agora && n.NavesIds.Contains(naveId));
        
        }
    }
}
