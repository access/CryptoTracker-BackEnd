using CryptocurrencyTracker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text.Json;
using System.Diagnostics;

namespace CryptocurrencyTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CryptoValuesController : ControllerBase
    {
        private readonly CryptoTrackerDbContext _context = new CryptoTrackerDbContext(Config.DbConnectionOptions);
        [HttpGet]
        public string Get() // IEnumerable<string> Get()
        {
            // read DB
            var _valuesArray = _context.CryptoCurrencyValues.ToList().Select(crypto => crypto);
            return JsonConvert.SerializeObject(_valuesArray);
        }

        [HttpGet("{id}/{period}")]
        public async Task<string> Get(int id, int period = 21600)
        {
            Debug.WriteLine($"{id} : {period}");
            var startDate = DateTime.Now.AddSeconds(-period);
            var _valuesArray = _context.CryptoCurrencyValues.ToList()
                //.Select(crypto => crypto)
                .Where(crypto => crypto.CurrencyID == id)
                .Where(crypto => crypto.HistoryDate >= startDate); // for setted period, default 6 hours = 21600 sec.
            foreach (var item in _valuesArray)
            {
                var cc = _context.CryptoCurrencyItems.ToList()
                    //.Select(crypto => crypto)
                    .Where(crypto => crypto.ID == item.CurrencyID)
                    .FirstOrDefault();
                item.CryptoName = cc.CryptoName;
            }
            return JsonConvert.SerializeObject(_valuesArray);
        }
    }
}
