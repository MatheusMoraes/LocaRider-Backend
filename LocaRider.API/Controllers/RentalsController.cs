using LocaRider.Application.DTO.Motorcycle;
using LocaRider.Application.DTO.Rental;
using LocaRider.Application.Interfaces.Motorcycle;
using LocaRider.Application.Interfaces.Rentals;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LocaRider.API.Controllers
{
    [Route("locacao")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalsService _rentalsService;
        public RentalsController(IRentalsService rentalsService)
        {
            _rentalsService = rentalsService;
        }

        /// <summary>
        /// Alugar uma moto
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> CreateRental(RentalDTO rentalDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { mensagem = "Dados Inválidos" });

            var createdRental = await _rentalsService.CreateRentalAsync(rentalDTO);

            // Retorna o objeto criado com status 201
            return Created();
        }

        /// <summary>
        /// Consultar locação por id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<RentalDTO>> GetRentalById([Required] string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { mensagem = "Dados Inválidos" });

            var rental = await _rentalsService.GetRentalByIdAsync(id);

            if (rental is null)
                return NotFound("Locação não encontrada.");

            return Ok(rental);
        }

        /// <summary>
        /// Informar data de devolução e calcular valor
        /// </summary>
        [HttpPut("{id}/devolucao")]
        public async Task<IActionResult> UpdateRentalDevolutionDate([Required] string id, [FromBody][Required] RentalDevolutionDTO rentalDevolutionDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { mensagem = "Dados inválidos" });

            var updated = await _rentalsService.SetReturnDateAsync(id, rentalDevolutionDTO);
            if (!updated) return NotFound("Locação não encontrada.");

            return Ok(new { mensagem = "Data de devolução informada com sucesso" });
        }
    }
}
