using CryptocurrencyTracker.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bittrex.Net;
using Bittrex.Net.Objects;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;
using System.IO;

namespace CryptocurrencyTracker.Services
{
    public class UpdateCryptoService : BackgroundService
    {
        private BittrexClient _client = new BittrexClient();

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Timer _timer = new Timer(UpdateCryptoValues, null, 0, Config.DbUpdateCryptoValuesPeriodMiliSec);
            return Task.CompletedTask;
        }

        async void UpdateCryptoValues(object state)
        {
            Debug.WriteLine(DateTime.Now.ToString() + " Updating Bittrex crypto...");

            // get all crypto items from DB table `CryptoCurrencyItems`
            var _context = new CryptoTrackerDbContext(Config.DbConnectionOptions);
            var _currencyArray = _context.CryptoCurrencyItems.ToList().Select(crypto => crypto);
            foreach (var cryptoItem in _currencyArray)
            {
                if (cryptoItem.CryptoName != Config.CryptoBaseCurrency)
                {
                    var cryptoQueryStr = cryptoItem.CryptoName + "-" + Config.CryptoBaseCurrency;

                    var price = _client.GetTicker(cryptoQueryStr);
                    var value = price.Data.LastTradeRate;
                    // read and update DB
                    // get id of currency for adding values
                    var CryptoID = _context.CryptoCurrencyItems.Where(crypto => crypto.CryptoName == cryptoItem.CryptoName).Select(c => c.ID).FirstOrDefault();
                    // add new value of current Cryptocurrency
                    CryptoCurrencyValue newValue = new CryptoCurrencyValue()
                    {
                        Id = 0,
                        CurrencyID = CryptoID,
                        HistoryDate = DateTime.Now,
                        MarketValue = value
                    };
                    _context.Add(newValue);

                    // update tickers values
                    cryptoItem.TradeRateDate = DateTime.Now;
                    cryptoItem.LastTradeRate = price.Data.LastTradeRate;
                    Debug.WriteLine(DateTime.Now.ToString() + $" ID: {CryptoID} {cryptoItem.CryptoName + "-" + Config.CryptoBaseCurrency} {value}");
                }
            }

            await _context.SaveChangesAsync();
            Debug.WriteLine(DateTime.Now.ToString() + " Updated.");
        }
    }
}
