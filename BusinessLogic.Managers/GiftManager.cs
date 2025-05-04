using Services.Models;
using Services.ExternalServices;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.Managers
{
    public class GiftManager
    {
        private readonly ElectronicStoreService _ess = new();
        public async Task<Electronic> GetGiftForPatientAsync()
        {
            var list = await _ess.GetAllElectronicItems();
            return list.FirstOrDefault();
        }
    }
}
