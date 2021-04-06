using CryptocurrencyTracker.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bittrex.Net;

namespace CryptocurrencyTracker.Services
{
    public class UpdateCryptoService : BackgroundService
    {
        private BittrexClient _client = new BittrexClient();

        protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // receive LastTradeRate in loop 
                await Task.Delay(Config.DbUpdateCryptoValuesPeriodMiliSec, stoppingToken);
                UpdateCryptoValues();
            }
        });

        async void UpdateCryptoValues()
        {
            Debug.WriteLine(DateTime.Now.ToString() + " Updating Bittrex crypto...");

            // get all crypto items from DB table `CryptoCurrencyItems`
            var context = new CryptoTrackerDbContext(Config.DbConnectionOptions);
            var currencyArray = context.CryptoCurrencyItems.ToList().Select(crypto => crypto);
            foreach (var cryptoItem in currencyArray)
            {
                if (cryptoItem.CryptoName != Config.CryptoBaseCurrency)
                {
                    var cryptoQueryStr = cryptoItem.CryptoName + "-" + Config.CryptoBaseCurrency;

                    var price = _client.GetTicker(cryptoQueryStr);
                    var value = price.Data.LastTradeRate;
                    // read and update DB
                    // get id of currency for adding values
                    var CryptoID = context.CryptoCurrencyItems.Where(crypto => crypto.CryptoName == cryptoItem.CryptoName).Select(c => c.ID).FirstOrDefault();
                    // add new value of current Cryptocurrency
                    CryptoCurrencyValue newValue = new CryptoCurrencyValue()
                    {
                        Id = 0,
                        CurrencyID = CryptoID,
                        HistoryDate = DateTime.Now,
                        MarketValue = value
                    };
                    context.Add(newValue);

                    // update tickers values
                    cryptoItem.TradeRateDate = DateTime.Now;
                    cryptoItem.LastTradeRate = price.Data.LastTradeRate;
                    Debug.WriteLine(DateTime.Now.ToString() + $" ID: {CryptoID} {cryptoItem.CryptoName + "-" + Config.CryptoBaseCurrency} {value}");
                }
            }

            await context.SaveChangesAsync();
            Debug.WriteLine(DateTime.Now.ToString() + " Updated.");
        }
    }
}
