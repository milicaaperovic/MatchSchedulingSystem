using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchSchedulingSystem.Domain
{
    public enum Operation { 
        End = 1,
        GetAllTeams = 2,
        SaveMatches = 3,
        ValidateMatch = 4
    }
    [Serializable]
    public class TransferObject
    {
        public Operation Operation;
        public Object Request;
        public Object Response;
    }
}
