using Bittrex.Net;
using CryptocurrencyTracker.Models;
using CryptocurrencyTracker.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CryptocurrencyTracker.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class CryptoItemsController : ControllerBase
    {
        private BittrexClient _client = new BittrexClient();
        private readonly CryptoTrackerDbContext _context = new CryptoTrackerDbContext(Config.DbConnectionOptions);

        // GET: api/<CryptoItems>
        [HttpGet]
        public string Get() // IEnumerable<string> Get()
        {
            // read DB
            var _currencyArray = _context.CryptoCurrencyItems.ToList().Select(crypto => crypto.CryptoName);
            return JsonConvert.SerializeObject(_context.CryptoCurrencyItems, new DecimalFormatConverter());
        }

        // POST api/<CryptoItems>
        [HttpPost]
        public async Task<string> Post([FromBody] string newTicker)
        {
            Debug.WriteLine("POST: " + newTicker);
            newTicker = newTicker.ToUpper();
            if (!String.IsNullOrEmpty(newTicker) && newTicker != Config.CryptoBaseCurrency)
            {
                // get info about new crypto
                // get list of existings currencies
                bool cryptoIsValid = false;
                var allCryptos = await _client.GetSymbolsAsync();

                //{"Symbol":"XTZ-USDT","baseCurrencySymbol":"XTZ","quoteCurrencySymbol":"USDT","MinTradeSize":1.50000000,"Precision":8,"Status":"ONLINE","CreatedAt":"2019-12-17T17:55:35.663Z","Notice":"","ProhibitedIn":[],"AssociatedTermsOfService":[]}
                foreach (var item in allCryptos.Data)
                    if (item.BaseCurrency == newTicker || item.QuoteCurrency == newTicker)
                        cryptoIsValid = true;

                // if valid crypto save to DB
                if (cryptoIsValid)
                {
                    Debug.WriteLine("Symbol is valid: " + newTicker);
                    // create item
                    var cryptoQueryString = newTicker + "-" + Config.CryptoBaseCurrency;
                    var price = await _client.GetTickerAsync(cryptoQueryString);
                    var newItem = new CryptoCurrencyItem()
                    {
                        ID = 0,
                        CryptoName = newTicker,
                        LastTradeRate = price.Data.LastTradeRate,
                        TradeRateDate = DateTime.Now
                    };
                    //-------------------------------------------
                    bool cryptoExistsInDB = false;
                    var allCurrencyItems = _context.CryptoCurrencyItems.ToList()
                        .Select(crypto => crypto).Where(el => el.CryptoName == newTicker);
                    foreach (var item in allCurrencyItems)
                        if (item.CryptoName == newTicker) cryptoExistsInDB = true;
                    if (!cryptoExistsInDB)
                    {
                        _context.Add(newItem);
                        await _context.SaveChangesAsync();
                        Debug.WriteLine($"Saved new crypto {JsonConvert.SerializeObject(newItem)}");
                    }
                    else { Debug.WriteLine("Duplicate DB entry: " + newTicker); }
                }
            }
            // read DB & send new object list
            var _currencyArray = _context.CryptoCurrencyItems.ToList().Select(crypto => crypto);
            return JsonConvert.SerializeObject(_currencyArray, new DecimalFormatConverter());
        }

        // DELETE api/<CryptoItems>/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            Debug.WriteLine("Delete: " + id);
            // Remove crypto ticker
            var cryptoToDelete = _context.CryptoCurrencyItems.Where(c => c.ID == id).FirstOrDefault();
            _context.CryptoCurrencyItems.Remove(cryptoToDelete);
            // Remove related crypto values from DB
            var cryptoValsToDelete = _context.CryptoCurrencyValues.ToList().Where(c => c.CurrencyID == id);
            foreach (var item in cryptoValsToDelete)
                _context.CryptoCurrencyValues.Remove(item);

            _context.SaveChanges();

            // read DB & send new object list
            var _currencyArray = _context.CryptoCurrencyItems.ToList().Select(crypto => crypto);
            return JsonConvert.SerializeObject(_currencyArray, new DecimalFormatConverter());
        }
    }
}
