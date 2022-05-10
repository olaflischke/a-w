using ChinookDal.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ChinookUnitTests
{
    [TestClass]
    public partial class QueryTests
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
                Assert.IsFalse(NHibernateUtil.IsInitialized(first.Album));
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
            using (ISession session = sessionFactory.OpenSession())
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

        [TestMethod]
        public void GetRockTracksNativeSql()
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                var qTracks = session.CreateSQLQuery("SELECT * FROM Track AS tr INNER JOIN Genre as gr ON tr.GenreId = gr.GenreId WHERE gr.Name = 'Rock'")
                                       .AddEntity(typeof(Track)); // Ohne AddEntity: Rückgabewert object[]

                foreach (var item in qTracks.List<Track>())
                {
                    Console.WriteLine($"{item.Genre.Name}: {item.Name} - {item.Album.Title}");
                }

            }
        }

        [TestMethod]
        public void GetRockTracksCriteria()
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                var qTracks = session.CreateCriteria<Track>()
                                        .CreateCriteria("Genre")
                                            .Add(Expression.Like("Name", "Rock"));

                foreach (var item in qTracks.List<Track>())
                {
                    Console.WriteLine($"{item.Genre.Name}: {item.Name} - {item.Album.Title}");
                }
            }
        }

        [TestMethod]
        public void GetRockTracksLinq()
        {
            List<Track> tracks = null;

            using (ISession session = sessionFactory.OpenSession())
            {
                // using NHibernate.Linq;
                // using System.Linq;
                var qTracks = session.Query<Track>()
                                         .Fetch(tr => tr.Album)
                                            .ThenFetch(al => al.Artist)
                                          //.Fetch(tr => tr.Genre)
                                          .Where(tr => tr.Genre.Name == "Rock");
                // Projektion -> Ein größeres SQL-Statement
                //.Select(tr => new { Trackname = tr.Name, Albumname = tr.Album.Title });

                tracks = qTracks.ToList();
            }

            Console.WriteLine($"{tracks.Count} Tracks gefunden.");

            foreach (var item in tracks)
            {
                Console.WriteLine($"{item.Name} von Album {item.Album.Title} von {item.Album.Artist.Name}");
            }

        }


        [TestMethod]
        public void GetRockTracksLinqProjection()
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                // using NHibernate.Linq;
                // using System.Linq;
                var qTracks = session.Query<Track>()
                                        .Where(tr => tr.Genre.Name == "Rock")
                                        // Projektion -> Ein größeres SQL-Statement
                                        .Select(tr => new { Trackname = tr.Name, Albumname = tr.Album.Title });

                foreach (var item in qTracks)
                {
                    Console.WriteLine($"{item.Trackname} von {item.Albumname}");
                }
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
