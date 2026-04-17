using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatchSchedulingSystem.Server
{
    public partial class FormaServer : Form
    {
        Server s;
        Timer t;
        public FormaServer()
        {
            InitializeComponent();
        }

        private void FormaServer_Load(object sender, EventArgs e)
        {
            s = new Server();
            if (s.pokreniServer())
            {
                this.Text = "Server je pokrenut!";

                t = new Timer();
                t.Interval = 10000;
                t.Tick += osvezi;
                t.Start();
            }
        }

        private void osvezi(object sender, EventArgs e)
        {
            string uslov = "";

            if (cbDatum.Checked)
            {
                try
                {
                    DateTime datum = Convert.ToDateTime(txtDatum.Text);
                    uslov = " where cast(Datum as date)=cast('" + datum.ToString("yyyy-MM-dd") + "' as date) ";
                }
                catch (Exception)
                {
                    uslov = "";
                }
            }

            dataGridView1.DataSource = Broker.dajSesiju().vratiSveZaServer(uslov);

        }
    }
}
