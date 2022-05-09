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
            this.TradingDays = GetData(url);

            //SaveToDb();
        }

        private void SaveToDb()
        {
            throw new NotImplementedException();
        }

        private List<TradingDay> GetData(string url)
        {
            XDocument document = XDocument.Load(url);

            var q = document.Root.Descendants()
                                            //.Where(nd => CheckLocalName(nd, "Cube")) // && nd.Attributes().Any(at => at.Name == "time"))
                                            .Where(nd => nd.Name.LocalName == "Cube" && nd.Attributes().Any(at => at.Name == "time"))
                                            .Select(nd => new TradingDay(nd));

            return q.ToList();
        }

        private bool CheckLocalName(XElement element, string name)
        {
            if (element.Name.LocalName == name)
                return true;
            return false;

        }

        public List<TradingDay> TradingDays { get; set; }
    }
}
