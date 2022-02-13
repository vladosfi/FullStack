﻿using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorldCities.Data;
using WorldCities.Data.Models;

namespace WorldCities.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public CountriesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: api/Countries
        [HttpGet]
        public async Task<ActionResult<ApiResult<Country>>> GetCountries(
            int pageIndex = 0,
            int pageSize = 10,
            string sortColumn = null,
            string sortOrder = null,
            string filterColumn = null,
            string filterQuery = null)
        {
            return await ApiResult<Country>.CreateAsync(
                            context.Countries,
                            pageIndex,
                            pageSize,
                            sortColumn,
                            sortOrder,
                            filterColumn,
                            filterQuery);
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Country>> GetCountry(int id)
        {
            var country = await context.Countries.FindAsync(id);

            if (country == null)
            {
                return NotFound();
            }

            return country;
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, Country country)
        {
            if (id != country.Id)
            {
                return BadRequest();
            }

            context.Entry(country).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountryExists(id))
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

        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Country>> PostCountry(Country country)
        {
            context.Countries.Add(country);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            context.Countries.Remove(country);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        [Route("IsDupeField")]
        public bool IsDupeField(
            int countryId,
            string fieldName,
            string fieldValue)
        {
            // Default approach (using strongly-typed LAMBA expressions)
            //switch (fieldName)
            //{
            // case "name":
            // return _context.Countries.Any(c => c.Name == fieldValue);
            // case "iso2":
            // return _context.Countries.Any(c => c.ISO2 == fieldValue);
            // case "iso3":
            // return _context.Countries.Any(c => c.ISO3 == fieldValue);
            // default:
            // return false;
            //}
            // Alternative approach (using System.Linq.Dynamic.Core)
            return (ApiResult<Country>.IsValidProperty(fieldName, true))
            ? context.Countries.Any(string.Format("{0} == @0 && Id != @1", fieldName), fieldValue, countryId)
            : false;
        }

        private bool CountryExists(int id)
        {
            return context.Countries.Any(e => e.Id == id);
        }
    }
}
