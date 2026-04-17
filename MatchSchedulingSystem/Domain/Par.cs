using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchSchedulingSystem.Domain
{
    [Serializable]
    public class Par
    {
        int id;
        Reprezentacija domacin;
        Reprezentacija gost;
        DateTime datum;

        [Browsable(false)]
        public int Id { get => id; set => id = value; }
        public Reprezentacija Domacin { get => domacin; set => domacin = value; }
        public Reprezentacija Gost { get => gost; set => gost = value; }

        [Browsable(false)]
        public DateTime Datum { get => datum; set => datum = value; }

        public override string ToString()
        {
            return $"{Domacin?.Naziv} vs {Gost?.Naziv} - {Datum:dd.MM.yyyy}";
        }
    }
}
