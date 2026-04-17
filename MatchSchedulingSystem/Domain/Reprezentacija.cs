using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchSchedulingSystem.Domain
{
    [Serializable]
    public class Reprezentacija
    {
        public override string ToString()
        {
            return Naziv;
        }

        int id;
        string naziv;


        public int Id { get => id; set => id = value; }
        public string Naziv { get => naziv; set => naziv = value; }

        public Reprezentacija Objekat { get => this;  }
        
    }
}
