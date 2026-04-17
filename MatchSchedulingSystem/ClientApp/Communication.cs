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

namespace MatchSchedulingSystem.Client
{
    public class Communication
    {
        TcpClient klijent;
        NetworkStream tok;
        BinaryFormatter formater;

        public bool poveziSeNaServer()
        {
            try
            {
                klijent = new TcpClient("localhost", 20000);
                tok = klijent.GetStream();
                formater = new BinaryFormatter();

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public void kraj()
        {
              //slanje
              TransferObject transfer = new TransferObject();
              transfer.Operation = Operation.End;
              formater.Serialize(tok, transfer);
            
        }

        public List<Reprezentacija> vratiReprezentacije()
        {
            //slanje
            TransferObject transfer = new TransferObject();
            transfer.Operation = Operation.GetAllTeams;
            formater.Serialize(tok, transfer);

            //prijem
            transfer = formater.Deserialize(tok) as TransferObject;
            return transfer.Response as List<Reprezentacija>;  
        }

        public string sacuvajParove(List<Par> lista)
        {
            //slanje
            TransferObject transfer = new TransferObject();
            transfer.Operation = Operation.SaveMatches;
            transfer.Request = lista;
            formater.Serialize(tok, transfer);

            //prijem
            transfer = formater.Deserialize(tok) as TransferObject;
            return transfer.Response.ToString();
        }
    }
}
