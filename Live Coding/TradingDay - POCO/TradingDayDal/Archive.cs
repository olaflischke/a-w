using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TradingDayDal
{
    public class Archive
    {
        public Archive(string url)
        {
            this.TradingDays=GetData(url);

            //SaveToDb();
        }

        private void SaveToDb()
        {
            throw new NotImplementedException();
        }

        private List<TradingDay> GetData(string url)
        {
            XDocument document = XDocument.Load(url);

            var q = document.Root.Descendants().Where(nd => nd.Name.LocalName == "Cube" && nd.Attributes().Any(at => at.Name == "time"))
                                            .Select(nd => new TradingDay(nd));

            return q.ToList();
        }

        public List<TradingDay> TradingDays { get; set; }
    }
}
