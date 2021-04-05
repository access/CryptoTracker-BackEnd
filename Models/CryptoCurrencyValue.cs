using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptocurrencyTracker.Models
{
    public class CryptoCurrencyValue
    {
        public int Id { get; set; }
        public int CurrencyID { get; set; }
        public decimal MarketValue { get; set; }
        public DateTime HistoryDate { get; set; }
        [NotMapped]
        public string CryptoName = string.Empty;
        [NotMapped]
        public string BaseCrypto = Config.CryptoBaseCurrency;
    }
}
