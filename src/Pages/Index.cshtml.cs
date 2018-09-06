﻿using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace AirportExplorer.Pages
{
    public class IndexModel : PageModel
    {
        public string MapboxAccessToken { get; }

        public IndexModel(IConfiguration configuration)
        {
            MapboxAccessToken = configuration["Mapbox:AccessToken"];
        }

        public void OnGet()
        {

        }
    }
}
