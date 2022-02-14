using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WorldCities.Data;
using WorldCities.Data.Models;

namespace WorldCities.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public CitiesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: api/Cities
        // GET: api/Cities
        // GET: api/Cities/?pageIndex=0&pageSize=10
        [HttpGet]
        public async Task<ActionResult<ApiResult<CityDTO>>> GetCities(
               int pageIndex = 0,
               int pageSize = 10,
               string sortColumn = null,
               string sortOrder = null,
               string filterColumn = null,
               string filterQuery = null)
        {
            return await ApiResult<CityDTO>.CreateAsync(
                context.Cities
                .Select(c => new CityDTO()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Lat = c.Lat,
                    Lon = c.Lon,
                    CountryId = c.Country.Id,
                    CountryName = c.Country.Name
                }),
                pageIndex, 
                pageSize, 
                sortColumn, 
                sortOrder, 
                filterColumn, 
                filterQuery);
        }

        // GET: api/Cities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<City>> GetCity(int id)
        {
            var city = await context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }
            return city;
        }

        // PUT: api/Cities/5
        // To protect from overposting attacks, please enable the
        // specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCity(int id, City city)
        {
            if (id != city.Id)
            {
                return BadRequest();
            }

            context.Entry(city).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // POST: api/Cities
        // To protect from overposting attacks, please enable the
        // specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<City>> PostCity(City city)
        {
            context.Cities.Add(city);
            await context.SaveChangesAsync();
            return CreatedAtAction("GetCity", new { id = city.Id }, city);
        }

        // DELETE: api/Cities/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<City>> DeleteCity(int id)
        {
            var city = await context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }
            context.Cities.Remove(city);
            await context.SaveChangesAsync();
            return city;
        }

        [HttpPost]
        [Route("IsDupeCity")]
        public bool IsDupeCity(City city)
        {
            return context.Cities.Any(
                e => e.Name == city.Name
                    && e.Lat == city.Lat
                    && e.Lon == city.Lon
                    && e.CountryId == city.CountryId
                    && e.Id != city.Id
                    );
        }

        private bool CityExists(int id)
        {
            return context.Cities.Any(e => e.Id == id);
        }
    }
}