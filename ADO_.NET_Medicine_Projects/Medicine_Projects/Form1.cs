using Medicine_Projects.Medicine_Company;
using Medicine_Projects.Medicine_Form;
using Medicine_Projects.Medicine_Info;
using Medicine_Projects.Purchase_Info;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Medicine_Projects
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void insertUpdateDeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMedicineCompany fmc = new frmMedicineCompany();
            fmc.Show();
            fmc.MdiParent = this;
        }

        private void insertUpdateDeleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmMedicineForm fmc = new frmMedicineForm();
            fmc.Show();
            fmc.MdiParent = this;
        }

        private void insertUpdateDeleteToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            frmMedicineInfo fmc = new frmMedicineInfo();
            fmc.Show();
            fmc.MdiParent = this;
        }

        private void insertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPurchaseInfo fmc = new frmPurchaseInfo();
            fmc.Show();
            fmc.MdiParent = this;
        }

        private void medicineInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            medicineInfoReport mir = new medicineInfoReport();
            mir.Show();
            mir.MdiParent = this;
        }
    }
}
