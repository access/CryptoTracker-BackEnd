using CryptocurrencyTracker.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;

namespace CryptocurrencyTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CryptoValuesController : ControllerBase
    {
        private readonly CryptoTrackerDbContext _context = new CryptoTrackerDbContext(Config.DbConnectionOptions);
        [HttpGet]
        public string Get()
        {
            // read DB
            var valuesArray = _context.CryptoCurrencyValues.ToList().Select(crypto => crypto);
            return JsonConvert.SerializeObject(valuesArray);
        }

        [HttpGet("{id}/{period}")]
        public async Task<string> Get(int id, int period = 21600)
        {
            Debug.WriteLine($"{id} : {period}");
            var startDate = DateTime.Now.AddSeconds(-period);
            var valuesArray = _context.CryptoCurrencyValues.ToList()
                .Where(crypto => crypto.CurrencyID == id)
                .Where(crypto => crypto.HistoryDate >= startDate); // for setted period, default 6 hours = 21600 sec.
            foreach (var item in valuesArray)
            {
                var cc = _context.CryptoCurrencyItems.ToList()
                    .Where(crypto => crypto.ID == item.CurrencyID)
                    .FirstOrDefault();
                item.CryptoName = cc.CryptoName;
            }
            return JsonConvert.SerializeObject(valuesArray);
        }
    }
}
