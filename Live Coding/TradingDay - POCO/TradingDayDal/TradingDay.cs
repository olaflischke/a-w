using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TradingDayDal
{
    public class TradingDay
    {
        public TradingDay(XElement tradingDayNode)
        {
            this.Date = Convert.ToDateTime(tradingDayNode.Attribute("time").Value);

            var q = tradingDayNode.Elements().Select(el => new ExchangeRate()
            {
                Symbol = el.Attribute("currency").Value,
                EuroValue = Convert.ToDouble(el.Attribute("rate").Value, NumberFormatInfo.InvariantInfo),
                TradingDay=this
            });

            this.ExchangeRates = q.ToList();
        }

        public DateTime Date { get; set; }
        public List<ExchangeRate> ExchangeRates { get; set; }
    }
}
