namespace TradingDayDal
{
    /// <summary>
    /// Stellt einen Wechselkurs dar
    /// </summary>
    public class ExchangeRate
    {
        public virtual int Id { get; set; }
        public virtual double EuroValue { get; set; }
        public virtual string Symbol { get; set; }
        public virtual TradingDay TradingDay { get; set; }
    }
}