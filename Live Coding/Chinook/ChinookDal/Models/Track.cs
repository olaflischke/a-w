using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinookDal.Models
{
    public class Track
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual Album Album { get; set; }
        public virtual Genre Genre { get; set; }
        public virtual string Composer { get; set; }
        public virtual int Milliseconds { get; set; }
        public virtual int Bytes { get; set; }
        public virtual double UnitPrice { get; set; }
    }
}
