using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WorldCities.Data;
using OfficeOpenXml;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using WorldCities.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace WorldCities.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment env;

        public SeedController(
            ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment env)
        {
            this.context = context;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.env = env;
        }

        [HttpGet]
        public async Task<ActionResult> Import()
        {
            var path = Path.Combine(env.ContentRootPath, String.Format("Data/Source/worldcities.xlsx"));

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var ep = new ExcelPackage(stream))
                {
                    // get the first worksheet
                    var ws = ep.Workbook.Worksheets[0];
                    // initialize the record counters
                    var nCountries = 0;
                    var nCities = 0;

                    #region Import all Countries
                    // create a list containing all the countries already existing into the Database (it will be empty on first run).
                    var lstCountries = context.Countries.ToList();

                    // iterates through all rows, skipping the first one
                    //for (int nRow = 2; nRow <= ws.Dimension.End.Row; nRow++)
                    for (int nRow = 2; nRow <= 1000; nRow++)
                    {
                        var row = ws.Cells[nRow, 1, nRow, ws.Dimension.End.Column];
                        var name = row[nRow, 5].GetValue<string>();

                        // Did we already created a country with that name?
                        if (lstCountries.Where(c => c.Name == name).Count() == 0)
                        {
                            // create the Country entity and fill it with xlsx data
                            var country = new Country();
                            country.Name = name;
                            country.ISO2 = row[nRow, 6].GetValue<string>();
                            country.ISO3 = row[nRow, 7].GetValue<string>();
                            // save it into the Database
                            context.Countries.Add(country);
                            await context.SaveChangesAsync();
                            // store the country to retrieve its Id later on
                            lstCountries.Add(country);
                            // increment the counter
                            nCountries++;
                        }
                    }
                    #endregion
                    #region Import all Cities
                    // iterates through all rows, skipping the first one
                    for (int nRow = 2;
                    //nRow <= ws.Dimension.End.Row; nRow++)
                    nRow <= 1000; nRow++)
                    {
                        var row = ws.Cells[nRow, 1, nRow, ws.Dimension.End.Column];
                        // create the City entity and fill it with xlsx data
                        var city = new City();
                        city.Name = row[nRow, 1].GetValue<string>();
                        city.Name_ASCII = row[nRow, 2].GetValue<string>();
                        city.Lat = row[nRow, 3].GetValue<decimal>();
                        city.Lon = row[nRow, 4].GetValue<decimal>();
                        // retrieve CountryId
                        var countryName = row[nRow, 5].GetValue<string>();
                        var country = lstCountries.Where(c => c.Name == countryName).FirstOrDefault();
                        city.CountryId = country.Id;
                        // save the city into the Database
                        context.Cities.Add(city);
                        await context.SaveChangesAsync();
                        // increment the counter
                        nCities++;
                    }
                    #endregion
                    return new JsonResult(new
                    {
                        Cities = nCities,
                        Countries = nCountries
                    });
                }
            }
        }

        [HttpGet]
        public async Task<ActionResult> CreateDefaultUsers()
        {
            throw new NotImplementedException();
        }
    }
}