using MatchSchedulingSystem.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatchSchedulingSystem.Client
{
    public partial class FormaKlijent : Form
    {
        Communication communication;
        BindingList<Par> listaParova;
        public FormaKlijent()
        {
            InitializeComponent();
        }

        void popuniGrid()
        {
            DataGridViewComboBoxColumn domacin = new DataGridViewComboBoxColumn();
            DataGridViewComboBoxColumn gost = new DataGridViewComboBoxColumn();

            domacin.HeaderText = "Domacin";
            domacin.Name = "domacin";
            domacin.DataPropertyName = "Domacin";
            domacin.DataSource = communication.vratiReprezentacije();
            domacin.ValueMember = "Objekat";
            domacin.DisplayMember = "Naziv";

            gost.HeaderText = "Gost";
            gost.Name = "gost";
            gost.DataPropertyName = "Gost";
            gost.DataSource = communication.vratiReprezentacije();
            gost.ValueMember = "Objekat";
            gost.DisplayMember = "Naziv";

            dataGridView1.Columns.Add(domacin);
            dataGridView1.Columns.Add(gost);

            dataGridView1.AutoGenerateColumns = false;
            listaParova = new BindingList<Par>();
            dataGridView1.DataSource = listaParova;
        }

        private void FormaKlijent_Load(object sender, EventArgs e)
        {
            communication = new Communication();
            if (communication.poveziSeNaServer())
            {
                this.Text = "Povezan!";

                popuniGrid();
            }

        }

        private void btnDodaj_Click(object sender, EventArgs e)
        {
            Par p = new Par();
            try
            {
                p.Datum = Convert.ToDateTime(txtDatum.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Datum nije validan!");
                return;
            }
            listaParova.Add(p);
        }

        private void btnObrisi_Click(object sender, EventArgs e)
        {
            try
            {
                Par p = dataGridView1.CurrentRow.DataBoundItem as Par;
                listaParova.Remove(p);
            }
            catch (Exception)
            {
                MessageBox.Show("Niste odabrali par za brisanje!");
            }
        }

        private void btnSacuvaj_Click(object sender, EventArgs e)
        {
            string poruka = communication.sacuvajParove(new List<Par>(listaParova));
            
                MessageBox.Show(poruka);
           
        }
    }
}
