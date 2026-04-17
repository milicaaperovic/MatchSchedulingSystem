using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MatchSchedulingSystem.Domain;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace MatchSchedulingSystem.Server
{
     class NitKlijenta
    {
        private NetworkStream tok;
        BinaryFormatter formater;

        public NitKlijenta(NetworkStream tok)
        {
            this.tok = tok;
            formater = new BinaryFormatter();
            ThreadStart ts = obradi;
            new Thread(ts).Start();
        }

        void obradi()
        {
            try
            {
                int operacija = 0;
                while(operacija != (int) Operation.End)
                {
                    TransferObject transfer = formater.Deserialize(tok) as TransferObject;
                    switch (transfer.Operation)
                    {
                        case Operation.GetAllTeams:
                            transfer.Response = Broker.dajSesiju().vratiSveReprezentacije();
                            formater.Serialize(tok, transfer);
                            break;
                        case Operation.SaveMatches:
                            transfer.Response = Broker.dajSesiju().sacuvajParove(transfer.Request as List<Par>);
                            formater.Serialize(tok, transfer);
                            break;
                        case Operation.End: operacija = 1;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
