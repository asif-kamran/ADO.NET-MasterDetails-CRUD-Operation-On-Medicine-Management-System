using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Medicine_Projects.Medicine_Form
{
    public partial class frmMedicineForm : Form
    {
        SqlConnection sqlcon = new SqlConnection(@"Data Source=DESKTOP-FHNMU6U\KAMRAN;Initial Catalog=medioneDB;Integrated Security=True");
        public frmMedicineForm()
        {
            InitializeComponent();
        }

        private void frmMedicineForm_Load(object sender, EventArgs e)
        {
            LoadGrid();
        }
        private void LoadGrid()
        {
            SqlDataAdapter sqlda = new SqlDataAdapter("select * from medicineForm", sqlcon);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            dataGridView2.DataSource = dt;
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcon;
                sqlcmd.CommandText = "insert into medicineForm(formId,formName) values(" + txtId.Text + ",'" + txtFomName.Text + "')";
                sqlcmd.ExecuteNonQuery();
                MessageBox.Show("Insert Successfully", "Insert Message");
                LoadGrid();
                txtId.Text = "";
                txtFomName.Text = "";              
                sqlcon.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Insert Valid Input\n"+ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcon;
                sqlcmd.CommandText = "update medicineForm set formName='" + txtFomName.Text
                    + "' where formId=" + txtId.Text + "";
                sqlcmd.ExecuteNonQuery();
                MessageBox.Show("Data Update Successfully","Update info");
                LoadGrid();
                txtId.Text = "";
                txtFomName.Text = "";
                sqlcon.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Data Update Successfully!!\n"+ex.Message);
                sqlcon.Close();
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells[0].Value);
            sqlcon.Open();
            SqlDataAdapter sda = new SqlDataAdapter("select formId,formName from medicineForm where formId=" + id + " ", sqlcon);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                txtId.Text = dt.Rows[0][0].ToString();
                txtFomName.Text = dt.Rows[0][1].ToString();
            }
            sqlcon.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtId.Clear();
            txtFomName.Clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcon;
                sqlcmd.CommandText = "delete from medicineForm where formId=" + txtId.Text + "";
                sqlcmd.ExecuteNonQuery();
                MessageBox.Show("Data Delete Successfully!!","Delete Message");
                LoadGrid();
                txtId.Text = "";
                txtFomName.Text = "";
                sqlcon.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Data Delete Successfully!!\n"+ex.Message);
            }
        }
    }
}
