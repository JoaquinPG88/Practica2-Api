using BusinessLogic.Managers;
using Microsoft.AspNetCore.Mvc;
using Services.Models;
using System.Collections.Generic;

namespace Practica2.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly PatientManager _pm;

        public PatientsController(PatientManager pm)
        {
            _pm = pm;
        }

        [HttpGet]
        public ActionResult<List<Patient>> GetAll()
        {
            return Ok(_pm.GetAllPatients());
        }

        [HttpGet("{ci}")]
        public ActionResult<Patient> Get(string ci)
        {
            var p = _pm.GetPatient(ci);
            if (p == null) return NotFound("Patient not found");
            return Ok(p);
        }

        [HttpPost]
        public ActionResult<Patient> Create([FromBody] Patient p)
        {
            try
            {
                var created = _pm.CreatePatient(p);
                return CreatedAtAction(nameof(Get), new { ci = created.CI }, created);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPut("{ci}")]
        public ActionResult<Patient> Update(string ci, [FromBody] Patient upd)
        {
            try
            {
                var updated = _pm.UpdatePatient(ci, upd);
                return Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Patient not found");
            }
        }

        [HttpDelete("{ci}")]
        public IActionResult Delete(string ci)
        {
            try
            {
                _pm.DeletePatient(ci);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Patient not found");
            }
        }
    }
}

