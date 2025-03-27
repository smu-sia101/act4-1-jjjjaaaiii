using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace PetNameGenerators.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PetNameGeneratorController : ControllerBase
    {
        private static readonly Dictionary<string, string[]> PetNames = new()
        {
            { "dog", new[] { "Buddy", "Max", "Charlie", "Rocky", "Rex" } },
            { "cat", new[] { "Whiskers", "Mittens", "Luna", "Simba", "Tiger" } },
            { "bird", new[] { "Tweety", "Sky", "Chirpy", "Raven", "Sunny" } }
        };

        [HttpPost("generate")]
        public IActionResult GeneratePetName([FromBody] PetNameRequest request)
        {
            if (request.AnimalType == null)
            {
                return BadRequest(new { error = "The 'animalType' field is required." });
            }

            if (!PetNames.ContainsKey(request.AnimalType.ToLower()))
            {
                return BadRequest(new { error = "Invalid animal type. Allowed values: dog, cat, bird." });
            }

            if (request.TwoPart.HasValue && request.TwoPart.Value.GetType() != typeof(bool))
            {
                return BadRequest(new { error = "The 'twoPart' field must be a boolean (true or false)." });
            }

            var selectedNames = PetNames[request.AnimalType.ToLower()];
            var random = new Random();

            string petName = request.TwoPart == true
                ? selectedNames[random.Next(selectedNames.Length)] + selectedNames[random.Next(selectedNames.Length)]
                : selectedNames[random.Next(selectedNames.Length)];

            return Ok(new { name = petName });
        }
    }

    public class PetNameRequest
    {
        public string AnimalType { get; set; }
        public bool? TwoPart { get; set; }
    }
}
