using MagicVilla_VillaAPI.Data;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("/api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok(VillaStore.villaList);
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(200, Type =typeof(VillaDTO))]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            // add validation and return error codes conditionally
            if (id == 0) return BadRequest();
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            if (villa == null) return NotFound();

            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO villaDTO)
        {
            // if (!ModelState.IsValid) return BadRequest(ModelState);
            if (villaDTO == null) return BadRequest(villaDTO);
            if (villaDTO.Id > 0) return StatusCode(StatusCodes.Status500InternalServerError);

            villaDTO.Id = VillaStore.villaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            VillaStore.villaList.Add(villaDTO);

            return CreatedAtRoute("GetVilla", new { id = villaDTO.Id }, villaDTO);
        }
    }
}