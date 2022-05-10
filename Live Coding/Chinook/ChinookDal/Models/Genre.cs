using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinookDal.Models
{
    public class Genre
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }

        public virtual IList<Track> Tracks { get; set; } = new List<Track>();
    }
}
