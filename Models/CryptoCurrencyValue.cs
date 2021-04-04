using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CryptocurrencyTracker.Models
{
    //[Keyless]
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
