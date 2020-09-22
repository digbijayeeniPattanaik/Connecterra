using API.Data;
using Infrastructure.Entities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class FarmContextSeed
    {
        public static async Task SeedAsync(FarmContext farmContext, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!farmContext.Farms.Any())
                {
                    var farmData = File.ReadAllText("../Infrastructure/SeedData/farms.json");
                    var farms = JsonConvert.DeserializeObject<List<Farm>>(farmData);
                    farmContext.Farms.AddRange(farms);
                    await farmContext.SaveChangesAsync();
                }

                if (!farmContext.Cows.Any())
                {
                    var cowsData = File.ReadAllText("../Infrastructure/SeedData/cows.json");
                    var cows = JsonConvert.DeserializeObject<List<Cow>>(cowsData);
                    farmContext.Cows.AddRange(cows);
                    await farmContext.SaveChangesAsync();
                }

                if (!farmContext.Sensors.Any())
                {
                    var sensorsData = File.ReadAllText("../Infrastructure/SeedData/sensors.json");
                    var sensors = JsonConvert.DeserializeObject<List<Sensor>>(sensorsData);
                    farmContext.Sensors.AddRange(sensors);
                    await farmContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<FarmContextSeed>();
                logger.LogError(ex, "Failed while seeding");
            }
        }
    }
}
