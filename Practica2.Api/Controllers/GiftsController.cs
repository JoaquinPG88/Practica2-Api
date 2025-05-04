using BusinessLogic.Managers;
using Microsoft.AspNetCore.Mvc;
using Services.Models;
using System.Threading.Tasks;

namespace Practica2.Api.Controllers
{
    [ApiController]
    [Route("api/patients/{ci}/gift")]
    public class GiftsController : ControllerBase
    {
        private readonly PatientManager _pm;
        private readonly GiftManager _gm;

        public GiftsController(PatientManager pm, GiftManager gm)
        {
            _pm = pm;
            _gm = gm;
        }

        [HttpGet]
        public async Task<ActionResult<Electronic>> GetGift(string ci)
        {
            var patient = _pm.GetPatient(ci);
            if (patient == null)
                return NotFound("Patient not found");

            var gift = await _gm.GetGiftForPatientAsync();
            if (gift == null)
                return NotFound("No gifts available");

            return Ok(gift);
        }
    }
}
