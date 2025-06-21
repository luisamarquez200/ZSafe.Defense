using Microsoft.AspNetCore.Mvc;
using Application.UseCases;
using Domain.Entities;
using Application.DTOs;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DefenseController : ControllerBase
    {
        private readonly DefenseStrategyService _service;

        public DefenseController(DefenseStrategyService service)
        {
            _service = service;
        }

        /// <summary>
        /// Calcula la mejor estrategia posible de defensa sin guardar en la base de datos.
        /// </summary>
        /// <returns>Lista de zombies eliminados con mejor puntaje</returns>
        [HttpGet("optimal-strategy")]
        [ProducesResponseType(typeof(List<Zombie>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetOptimalStrategy([FromQuery] int bullets, [FromQuery] int secondsAvailable)
        {
            if (bullets <= 0 || secondsAvailable <= 0)
                return BadRequest("Los valores deben ser mayores a cero.");

            var result = await _service.GetOptimalStrategy(bullets, secondsAvailable);
            return Ok(result);
        }

        /// <summary>
        /// Ejecuta una simulación completa y la guarda en base de datos.
        /// </summary>
        /// <returns>Simulación registrada con datos de zombies eliminados</returns>
        [HttpPost("simulate")]
        [ProducesResponseType(typeof(Simulation), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Simulate([FromQuery] int bullets, [FromQuery] int secondsAvailable)
        {
            if (bullets <= 0 || secondsAvailable <= 0)
                return BadRequest("Los valores deben ser mayores a cero.");

            var result = await _service.SaveSimulation(bullets, secondsAvailable);
            return Ok(result);
        }


        /// <summary>
        /// Registra una estrategia real con los parámetros dados y zombies seleccionados.
        /// </summary>
        [HttpPost("register-strategy")]
        [ProducesResponseType(typeof(Simulation), 200)]
        public async Task<IActionResult> RegisterRealStrategy([FromBody] RegisterStrategyDto dto)
        {
            if (dto.BalasDisponibles <= 0 || dto.TiempoDisponible <= 0 || dto.ZombieIds.Count == 0)
                return BadRequest("Datos inválidos para registrar la estrategia.");

            var result = await _service.RegisterRealStrategy(
                dto.BalasDisponibles,
                dto.TiempoDisponible,
                dto.ZombieIds
            );

            return Ok(result);
        }

        /// <summary>
        /// Retorna todas las simulaciones registradas (para el ranking).
        /// </summary>
        [HttpGet("all-simulations")]
        [ProducesResponseType(typeof(List<Simulation>), 200)]
        public async Task<IActionResult> GetAllSimulations()
        {
            var result = await _service.GetAllSimulations();
            return Ok(result);
        }


    }
}