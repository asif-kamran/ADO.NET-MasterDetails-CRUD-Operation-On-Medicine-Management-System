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
using System.Xml.Linq;

namespace Medicine_Projects.Medicine_Company
{
    public partial class frmMedicineCompany : Form
    {
        SqlConnection sqlcon = new SqlConnection(@"Data Source=DESKTOP-FHNMU6U\KAMRAN;Initial Catalog=medioneDB;Integrated Security=True");
        public frmMedicineCompany()
        {
            InitializeComponent();
        }

        private void frmMedicineCompany_Load(object sender, EventArgs e)
        {
            LoadGrid();
        }
        private void LoadGrid()
        {
            SqlDataAdapter sqlda = new SqlDataAdapter("select * from companyInfo", sqlcon);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcon;
                sqlcmd.CommandText = "insert into companyInfo(companyId,companyName) values(" + txtId.Text + ",'" + txtCompanyName.Text + "')";
                sqlcmd.ExecuteNonQuery();
                MessageBox.Show("Company Inserted SuccessFull !!", "Insert Message");
                LoadGrid();
                txtId.Text = "";
                txtCompanyName.Text = "";
                sqlcon.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Insert Valid Data!!\n"+ex.Message);
                sqlcon.Close();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtId.Clear();
            txtCompanyName.Clear();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcon;
                sqlcmd.CommandText = "update companyInfo set companyName='" + txtCompanyName.Text
                    + "' where companyId=" + txtId.Text + "";
                sqlcmd.ExecuteNonQuery();
                MessageBox.Show("Company Update SuccessFull !!", "Update Message");
                LoadGrid();
                txtId.Text = "";
                txtCompanyName.Text = "";
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Insert Valid Data!!\n" + ex.Message);
                sqlcon.Close();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcon;
                sqlcmd.CommandText = "delete from companyInfo where companyId=" + txtId.Text + "";
                sqlcmd.ExecuteNonQuery();
                MessageBox.Show("Company Delete SuccessFull !!", "Delete Message");
                LoadGrid();
                txtId.Text = "";
                txtCompanyName.Text = "";
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Insert Valid Data!!\n" + ex.Message);
                sqlcon.Close();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
            sqlcon.Open();
            SqlDataAdapter sda = new SqlDataAdapter("select companyId,companyName from companyInfo where companyId=" + id + " ", sqlcon);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                txtId.Text = dt.Rows[0][0].ToString();
                txtCompanyName.Text = dt.Rows[0][1].ToString();
            }
            sqlcon.Close();
        }
    }
}
