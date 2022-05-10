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
    partial class QueryTests
    {
        [TestMethod]
        public void UpdateTrackTest()
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                Track track = session.Get<Track>(1);
                track.Name = "Opus 51";

                session.Save(track);

                session.Flush();
            }
        }

        [TestMethod]
        public void UpdateMit2Sessions()
        {
            Track track1 = null;

            using (ISession session1 = sessionFactory.OpenSession())
            {
                track1 = session1.Get<Track>(1);
            }

            Console.WriteLine($"{track1.Name} geladen und detached (Session.Close()).");

            Console.WriteLine("Ändere Trackname...");
            track1.Name = "Opus 317";

            using (ISession session2 = sessionFactory.OpenSession())
            {
                Track track2 = session2.Get<Track>(1);
                session2.Merge(track1);

                Console.WriteLine($"Speichere {track1.Name}");
                //session2.Save(track); // Exception, weil INSERT
                //session2.Update(track); // Funktioniert für bestehende Daten
                //session2.SaveOrUpdate(track1); // Funktioniert für beide Situationen
                
                session2.Flush();
            }

        }

    }
}
