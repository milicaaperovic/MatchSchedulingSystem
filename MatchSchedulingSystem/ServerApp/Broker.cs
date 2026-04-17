using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MatchSchedulingSystem.Domain;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Collections;

namespace MatchSchedulingSystem.Server
{
    public class Broker
    {
        SqlCommand komanda;
        SqlConnection konekcija;
        SqlTransaction transakcija;

        void konektujSe()
        {
            konekcija = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=IspitNov2018;Integrated Security=True");
            komanda = konekcija.CreateCommand();
        }

        Broker()
        {
            konektujSe();
        }

        //singleton
        static Broker instanca;
        public static Broker dajSesiju()
        {
            if (instanca == null) instanca = new Broker();
            return instanca;
        }

        public List<Reprezentacija> vratiSveReprezentacije()
        {
            List<Reprezentacija> lista = new List<Reprezentacija>();

            try
            {
                konekcija.Open();
                komanda.CommandText = "Select * from Reprezentacija";
                SqlDataReader citac = komanda.ExecuteReader();
                while (citac.Read())
                {
                    Reprezentacija r = new Reprezentacija();
                    r.Id = citac.GetInt32(0);
                    r.Naziv = citac.GetString(1);
                    lista.Add(r);
                }
                citac.Close();
                return lista;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
            finally { if (konekcija != null) konekcija.Close(); }
        }

        public int vratiSifruPara()
        {
            try
            {
                komanda.CommandText = "Select max(ID) from Par";
                try
                {
                    int sifra = Convert.ToInt32(komanda.ExecuteScalar());
                    return sifra + 1;
                }
                catch (Exception)
                {

                    return 1;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string sacuvajParove(List<Par> lista)
        {
            try
            {
                konekcija.Open();
                transakcija = konekcija.BeginTransaction();
                komanda = new SqlCommand("", konekcija, transakcija);
                foreach (Par p in lista)
                {
                    if (proveriPar(p))
                    {
                        transakcija.Rollback(); return "Par vec postoji!";
                    }
                    if (proveriReprezentaciju(p))
                    {
                        transakcija.Rollback(); return "Neko vec igra istog dana!";
                    }
                    if (p.Domacin.Id == p.Gost.Id)
                    {
                        transakcija.Rollback(); return p.Domacin.Naziv+" " + " ne moze da igra sama sa sobom!";
                    }
                    p.Id = vratiSifruPara();
                    komanda.CommandText = "Insert into Par values (" + p.Id + "," + p.Domacin.Id + "," + p.Gost.Id + ",'" + p.Datum.ToString("yyyy-MM-dd") + "')";
                    komanda.ExecuteNonQuery();
                }
                transakcija.Commit();
                return "Sacuvano!";
            }
            catch (Exception ex)
            {
                if(transakcija != null)
                {
                    transakcija.Rollback();
                }
                
                return "Nije sacuvano!";
            }
            finally { if (konekcija != null) konekcija.Close(); }
        }


        //provera nad bazom
        public bool proveriPar(Par p)
        {
            try
            {
                komanda.CommandText = "Select * from Par where cast(Datum as date)=cast('" + p.Datum.ToString("yyyy-MM-dd") + "' as date) and Domacin=" + p.Domacin.Id + " " +
                    "and Gost=" + p.Gost.Id + "";
                SqlDataReader citac = komanda.ExecuteReader();
                if (citac.Read())
                {
                    citac.Close();
                    return true;
                }
                citac.Close();
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        public bool proveriReprezentaciju(Par p)
        {
            try
            {
                komanda.CommandText = "Select * from Par where cast(Datum as date)=cast('" + p.Datum.ToString("yyyy-MM-dd") + "' as date) and (Domacin=" + p.Domacin.Id + " " +
                    "or Gost=" + p.Gost.Id +" or Gost="+p.Domacin.Id+" or Domacin="+p.Gost.Id+" )";
                SqlDataReader citac = komanda.ExecuteReader();
                if (citac.Read())
                {
                    citac.Close();
                    return true;
                }
                citac.Close();
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        public List<MatchView> vratiSveZaServer(string uslov)
        {
            List<MatchView> lista = new List<MatchView>();

            try
            {
                konekcija.Open();
                komanda.CommandText = "SELECT d.Naziv,g.Naziv,p.Datum from Par p inner join Reprezentacija d on p.Domacin=d.ID inner join Reprezentacija g on p.Gost=g.ID" + uslov;
                SqlDataReader citac = komanda.ExecuteReader();
                while (citac.Read())
                {
                    MatchView match = new MatchView();
                    match.Domacin = citac.GetString(0);
                    match.Gost = citac.GetString(1);
                    match.Datum = citac.GetDateTime(2);
                    lista.Add(match);
                }
                citac.Close();
                return lista;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
            finally { if (konekcija != null) konekcija.Close(); }
        }



    }
}
