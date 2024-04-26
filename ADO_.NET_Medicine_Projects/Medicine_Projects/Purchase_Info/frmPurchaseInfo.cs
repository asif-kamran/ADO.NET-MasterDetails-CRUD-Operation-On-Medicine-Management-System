using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Medicine_Projects.Purchase_Info
{
    public partial class frmPurchaseInfo : Form
    {
        SqlConnection sqlcon = new SqlConnection(@"Data Source=DESKTOP-FHNMU6U\KAMRAN;Initial Catalog=medioneDB;Integrated Security=True");
        public frmPurchaseInfo()
        {
            InitializeComponent();
        }

        private void frmPurchaseInfo_Load(object sender, EventArgs e)
        {
            LoadMedicineCombo();
            LoadGrid();
        }
        private void LoadGrid()
        {
            SqlDataAdapter sqlda = new SqlDataAdapter(@"select purchaseId,medicine.medicineName,quantity,unitPrice,purchaseDate from purchase join medicine on purchase.medicineId=medicine.medicineId", sqlcon);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        private void LoadMedicineCombo()
        {
            sqlcon.Open();
            SqlDataAdapter sqlda = new SqlDataAdapter("select medicineId,medicineName from medicine", sqlcon);
            DataSet ds = new DataSet();
            sqlda.Fill(ds);
            DataRow newRow = ds.Tables[0].NewRow();
            newRow[0] = "-1";
            newRow[1] = "---Select Medicine---";
            ds.Tables[0].Rows.InsertAt(newRow, 0);
            cmbMedicineName.DataSource = ds.Tables[0];
            cmbMedicineName.DisplayMember = "medicineName";
            cmbMedicineName.ValueMember = "medicineId";

            sqlcon.Close();

        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcon;
                sqlcmd.CommandText = @"insert into purchase(purchaseId,medicineId,quantity,unitPrice,purchaseDate) values (" + txtId.Text + "," + cmbMedicineName.SelectedValue + "," + txtQty.Text + "," + txtPrice.Text + ",'" + dateTimePicker1.Value + "')";
                sqlcmd.ExecuteNonQuery();
                MessageBox.Show("Data Insert Succesfully !!", "Message");
                LoadGrid();
                sqlcon.Close();
                txtId.Clear();
                txtPrice.Clear();
                txtQty.Clear();
                cmbMedicineName.SelectedIndex = -1;
                this.dateTimePicker1.Value = DateTime.Now;

                sqlcon.Close();
                

            }
            catch (Exception ex)
            {
                MessageBox.Show("Data Insert is Not Valid !!"+ex.Message);
                sqlcon.Close();
            }
            
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            sqlcon.Close();
            txtId.Clear();
            txtPrice.Clear();
            txtQty.Clear();
            cmbMedicineName.SelectedIndex = -1;
            this.dateTimePicker1.Value=DateTime.Now;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcon;
                sqlcmd.CommandText = "delete from purchase where purchaseId=" + txtId.Text + " ";
                sqlcmd.ExecuteNonQuery();
                MessageBox.Show("Delete Successfully !!", "Delete Message");

                sqlcon.Close();
                txtId.Clear();
                txtPrice.Clear();
                txtQty.Clear();
                cmbMedicineName.SelectedIndex = -1;
                this.dateTimePicker1.Value = DateTime.Now;
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Data Not Valid !!" + ex.Message);
                sqlcon.Close();
            }
            LoadGrid();

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
            sqlcon.Open();
            SqlDataAdapter sda = new SqlDataAdapter(@"select purchaseId,medicineId,quantity,unitPrice,purchaseDate from purchase where purchaseId="+id+"", sqlcon);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                txtId.Text = dt.Rows[0][0].ToString();
                cmbMedicineName.SelectedValue = dt.Rows[0][1].ToString();
                txtQty.Text = dt.Rows[0][2].ToString();
                txtPrice.Text = dt.Rows[0][3].ToString();
                dateTimePicker1.Value = Convert.ToDateTime(dt.Rows[0][4].ToString());
            }
            sqlcon.Close();
        }
    }
}
