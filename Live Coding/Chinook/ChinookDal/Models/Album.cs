using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinookDal.Models
{
   public class Album
    {
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual Artist Artist { get; set; }
        public virtual IList<Track> Tracks { get; set; } = new List<Track>();
    }
}
