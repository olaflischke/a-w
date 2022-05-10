using ChinookDal.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using System;
using System.Data;

namespace ChinookUnitTests
{
    [TestClass]
    public class QueryTests
    {
        Configuration configuration;
        ISessionFactory sessionFactory;

        public QueryTests()
        {
            configuration = ConfigureNHibernate();
            sessionFactory = configuration.BuildSessionFactory();
        }

        [TestMethod]
        public void FirstTrackGotten()
        {
            Track first = null;

            using (ISession session = sessionFactory.OpenSession())
            {
                first = session.Get<Track>(1);
                Assert.IsFalse( NHibernateUtil.IsInitialized(first.Album));
            }

            //Console.WriteLine($"Gefundener Track: {first.Name}"); // von Album {first.Album.Title}");
        }

        [TestMethod]
        public void FirstTrackLoaded()
        {
            Track first = null;

            using (ISession session = sessionFactory.OpenSession())
            {
                first = session.Load<Track>(1);
                Assert.IsFalse(NHibernateUtil.IsInitialized(first.Album));
                //Console.WriteLine($"Gefundener Track: {first.Name}"); // von Album {first.Album.Title}");
            }

        }

        [TestMethod]
        public void FirstAlbumGotten()
        {
            Album album = null;
            using (ISession session=sessionFactory.OpenSession())
            {
                album = session.Get<Album>(1);
                Assert.IsFalse(NHibernateUtil.IsInitialized(album.Tracks));
            }
        }

        [TestMethod]
        public void FirstAlbumLoaded()
        {
            Album album = null;
            using (ISession session = sessionFactory.OpenSession())
            {
                album = session.Load<Album>(1);
                Assert.IsFalse(NHibernateUtil.IsInitialized(album.Tracks));
            }
        }


        public static Configuration ConfigureNHibernate()
        {
            var cfg = new Configuration();

            cfg.DataBaseIntegration(x =>
            {
                x.ConnectionString = Properties.Settings.Default.ChinookConString;
                x.Driver<SqlClientDriver>();
                x.Dialect<MsSql2012Dialect>();
                x.IsolationLevel = IsolationLevel.RepeatableRead;
                x.LogSqlInConsole = true;
                x.Timeout = 10;
                x.BatchSize = 10;
            });

            cfg.SessionFactory().GenerateStatistics();

            cfg.AddAssembly("ChinookDal");

            return cfg;
        }

    }
}
