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

        [HttpGet("{id}")]
        public string Get(int id)
        {

            var _valuesArray = _context.CryptoCurrencyValues.ToList()
                .Select(crypto => crypto).Where(c => c.CurrencyID == id);
            foreach (var item in _valuesArray)
            {
                var cc = _context.CryptoCurrencyItems.ToList().Select(crypto => crypto).Where(c => c.ID == item.CurrencyID).FirstOrDefault();
                item.CryptoName = cc.CryptoName;
            }
            return JsonConvert.SerializeObject(_valuesArray);
        }
    }
}
