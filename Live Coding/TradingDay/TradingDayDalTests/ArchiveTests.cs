using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using TradingDayDal;

namespace TradingDayDalTests
{
    [TestClass]
    public class ArchiveTests
    {
        string url;
        string connectionString;
        Configuration NHibernateConfig;
        ISessionFactory sessionFactory

        public ArchiveTests()
        {
            url = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-hist-90d.xml";

            connectionString = Properties.Settings.Default.ConString;

            NHibernateConfig = ConfigureNHibernate(connectionString);
            sessionFactory = NHibernateConfig.BuildSessionFactory();
        }

        [TestMethod]
        public void IsArchiveInitializing()
        {
            Archive archive = new Archive(url);

            Console.WriteLine($"USD vom 1. Tradingday: {archive.TradingDays.FirstOrDefault()?.ExchangeRates.LastOrDefault()?.EuroValue}");

            Assert.AreEqual(CountAttributes("time"), archive.TradingDays.Count());
        }

        [TestMethod]
        public void ArchiveDbTest()
        {
            (new SchemaExport(NHibernateConfig)).Execute(script: false, export: true, justDrop: false);

            using (ISession session = sessionFactory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    Archive archive = new Archive(url);

                    Stopwatch stopwatch = Stopwatch.StartNew();

                    foreach (TradingDay item in archive.TradingDays)
                    {
                        session.Save(item);
                    }

                    Console.WriteLine($"Before commit: {stopwatch.ElapsedMilliseconds:#,##0}");


                    session.Flush();
                    transaction.Commit();
                    
                    stopwatch.Stop();
                    Console.WriteLine($"After all: {stopwatch.ElapsedMilliseconds:#,##0}");
                }
            }
        }

        [TestMethod]
        public void ManuellerDbTest()
        {
            (new SchemaExport(NHibernateConfig)).Execute(script: false, export: true, justDrop: false);

            using (ISession session = sessionFactory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    TradingDay day = new TradingDay() { Date = DateTime.Today };

                    ExchangeRate rate1 = new ExchangeRate() { Symbol = "USD", EuroValue = 1.057, TradingDay = day };
                    ExchangeRate rate2 = new ExchangeRate() { Symbol = "ZAR", EuroValue = 16.9614, TradingDay = day };

                    day.ExchangeRates.Add(rate1);
                    day.ExchangeRates.Add(rate2);

                    //session.Save(rate1);
                    //session.Save(rate2);

                    session.Save(day);

                    transaction.Commit();

                    session.Flush();
                }
            }
        }

        private Configuration ConfigureNHibernate(string connectionString)
        {
            Configuration cfg = new Configuration();

            cfg.DataBaseIntegration(c =>
            {
                c.ConnectionString = connectionString;
                c.Driver<SqlClientDriver>();
                c.Dialect<MsSql2012Dialect>();
                c.IsolationLevel = IsolationLevel.RepeatableRead;
                c.LogSqlInConsole = false;
            });

            cfg.SessionFactory().GenerateStatistics();

            cfg.AddAssembly("TradingDayDal");

            return cfg;

        }



        private int CountAttributes(string attributeName)
        {
            return 63;
        }


    }
}
