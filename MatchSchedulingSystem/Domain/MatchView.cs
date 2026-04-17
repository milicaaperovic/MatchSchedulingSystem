using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchSchedulingSystem.Domain
{
    public class MatchView
    {
        string domacin;
        string gost;
        DateTime datum;

        public string Domacin { get => domacin; set => domacin = value; }
        public string Gost { get => gost; set => gost = value; }

        [Browsable(false)]
        public DateTime Datum { get => datum; set => datum = value; }

        [DisplayName("Date")]
        public string DateFormatted
        {
            get { return datum.ToString("dd.MM.yyyy"); }
        }


    }
}
