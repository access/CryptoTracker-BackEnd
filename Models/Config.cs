using Bittrex.Net;
using Bittrex.Net.Objects;
using CryptoExchange.Net.Authentication;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CryptocurrencyTracker.Models
{
    public static class Config
    {
        public readonly static string DbConnectionString = @"Server=localhost\TEW_SQLEXPRESS; Database=CryptocurrencyTracker; Integrated Security=True;";
        public readonly static DbContextOptions<CryptoTrackerDbContext> DbConnectionOptions = new DbContextOptionsBuilder<CryptoTrackerDbContext>().UseSqlServer(DbConnectionString).Options;
        public static int DbUpdateCryptoValuesPeriodMiliSec = 5000;
        public readonly static string BittrexApiKey = "41d1acdd825c462aaeb8365a36ab5687";
        public readonly static string BittrexApiSecret = "be03e357232b4c56b37cefdbdaa89774";
        public readonly static string CryptoBaseCurrency = "BTC";

        static Config()
        {
            BittrexClient.SetDefaultOptions(new BittrexClientOptions()
            {
                ApiCredentials = new ApiCredentials(BittrexApiKey, BittrexApiSecret),
            });
        }
    }
}
