using System.Collections.Generic;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace AirportExplorer.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public string MapboxAccessToken { get; }

        public IndexModel(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;

            MapboxAccessToken = configuration["Mapbox:AccessToken"];
        }

        public void OnGet()
        {

        }

        // http://0.0.0.0:5000/?handler=airports
        public IActionResult OnGetAirports()
        {
            var configuration = new Configuration
            {
                BadDataFound = context =>
                {
                }
            };
            using (var sr = new StreamReader(Path.Combine(_hostingEnvironment.WebRootPath, "airports.dat")))
            using (var reader = new CsvReader(sr, configuration))
            {

                var featureCollection = new FeatureCollection();
                while (reader.Read())
                {
                    string name = reader.GetField<string>(1);
                    string iateCode = reader.GetField<string>(4);
                    double latitude = reader.GetField<double>(6);
                    double longitude = reader.GetField<double>(7);

                    featureCollection.Features.Add(new Feature(
                        new Point(new Position(latitude, longitude)),
                        new Dictionary<string, object>
                        {
                            { "name", name },
                            { "iataCode", iateCode }
                        }));
                }
                return new JsonResult(featureCollection);
            }
        }
    }
}