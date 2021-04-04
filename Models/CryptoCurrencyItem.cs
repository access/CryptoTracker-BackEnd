using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CryptocurrencyTracker.Models
{
    public class CryptoCurrencyItem
    {
        public int ID { get; set; }
        public string CryptoName { get; set; }
        public decimal LastTradeRate { get; set; }
        public DateTime TradeRateDate { get; set; }
        [NotMapped]
        public string BaseCrypto = Config.CryptoBaseCurrency;
    }
}
