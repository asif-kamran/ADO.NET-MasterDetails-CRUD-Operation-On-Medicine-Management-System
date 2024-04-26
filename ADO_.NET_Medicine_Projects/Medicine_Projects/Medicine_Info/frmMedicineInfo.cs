using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Medicine_Projects.Medicine_Info
{
    public partial class frmMedicineInfo : Form
    {
        SqlConnection sqlcon = new SqlConnection(@"Data Source=DESKTOP-FHNMU6U\KAMRAN;Initial Catalog=medioneDB;Integrated Security=True");
        SqlTransaction tran;
        public frmMedicineInfo()
        {
            InitializeComponent();
        }

        private void frmMedicineInfo_Load(object sender, EventArgs e)
        {
            LoadGrid();
            LoadFormCombo();
            LoadCompanyCombo();
        }
        private void LoadGrid()
        {
            sqlcon.Open();
            SqlDataAdapter sqlda = new SqlDataAdapter(@"select medicineId,medicineName,companyName,formName,medicineImage,MRP from medicine m join companyInfo c on m.companyId=c.companyId join medicineForm f on m.formId=f.formId", sqlcon);
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            dataGridView2.DataSource = dt;
            sqlcon.Close();
        }
        private void LoadCompanyCombo()
        {
            sqlcon.Open();
            SqlDataAdapter sqlda = new SqlDataAdapter("select * from companyInfo", sqlcon);
            DataSet ds = new DataSet();
            sqlda.Fill(ds);
            DataRow newRow = ds.Tables[0].NewRow();
            newRow[0] = "-1";
            newRow[1] = "---Select Company---";
            ds.Tables[0].Rows.InsertAt(newRow, 0);
            cmbCoompanyName.DataSource = ds.Tables[0];
            cmbCoompanyName.DisplayMember = "companyName";
            cmbCoompanyName.ValueMember = "companyId";
            sqlcon.Close();
        }
        private void LoadFormCombo()
        {
            sqlcon.Open();
            SqlDataAdapter sqlda = new SqlDataAdapter("select * from medicineForm", sqlcon);
            DataSet ds = new DataSet();
            sqlda.Fill(ds);
            DataRow newRow = ds.Tables[0].NewRow();
            newRow[0] = "-1";
            newRow[1] = "---Select Form---";
            ds.Tables[0].Rows.InsertAt(newRow, 0);
            cmbFormName.DataSource = ds.Tables[0];
            cmbFormName.DisplayMember = "formName";
            cmbFormName.ValueMember = "formId";
            sqlcon.Close();
        }
        
        private void clearData()
        {
            txtId.Clear();
            txtMedicineName.Clear();
            cmbCoompanyName.SelectedIndex = 0;
            cmbFormName.SelectedIndex = 0;
            txtPicture.Clear();
            pictureBox1.Image = null;
            txtMrp.Clear();
            if (dataGridView1.DataSource == null)
            {
                dataGridView1.Rows.Clear();

            }
            else
            {
                dataGridView1.DataSource = (dataGridView1.DataSource as DataTable).Clone();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clearData();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Image img = Image.FromFile(openFileDialog1.FileName);
                this.pictureBox1.Image = img;
                txtPicture.Text = openFileDialog1.FileName;
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                Image img = Image.FromFile(txtPicture.Text);
                MemoryStream ms = new MemoryStream();
                img.Save(ms, ImageFormat.Bmp);
                sqlcon.Open();

                tran=sqlcon.BeginTransaction();

                SqlCommand sqlcmd = new SqlCommand("insert into medicine(medicineId,medicineName,companyId,formId,medicineImage,MRP) values (@i,@n,@ci,@fi,@p,@m)",sqlcon,tran);
                sqlcmd.Parameters.AddWithValue("@i", txtId.Text);
                sqlcmd.Parameters.AddWithValue("@n", txtMedicineName.Text);
                sqlcmd.Parameters.AddWithValue("@ci", cmbCoompanyName.SelectedValue);
                sqlcmd.Parameters.AddWithValue("@fi", cmbFormName.SelectedValue);
                sqlcmd.Parameters.Add(new SqlParameter("@p", SqlDbType.VarBinary) { Value = ms.ToArray() });
                sqlcmd.Parameters.AddWithValue("@m", txtMrp.Text);          
                sqlcmd.ExecuteNonQuery();
                foreach (DataGridViewRow dgvRow in dataGridView1.Rows) {
                    if (dgvRow.IsNewRow) break;
                    else
                    {
                        SqlCommand sqlcmd1 = new SqlCommand(@"insert into medicineSupplier(medicineId,supplierName,supplierAddress,disAmount) values(@mi,@sn,@sa,@d)",sqlcon,tran);
                        sqlcmd1.Parameters.AddWithValue("@mi", Convert.ToInt32(txtId.Text));
                        sqlcmd1.Parameters.AddWithValue("@sn", dgvRow.Cells["dgvtxtSupplierName"].Value);
                        sqlcmd1.Parameters.AddWithValue("@sa", dgvRow.Cells["dgvtxtAddress"].Value);
                        sqlcmd1.Parameters.AddWithValue("@d",Convert.ToInt32(dgvRow.Cells["dgvtxtDisAmount"].Value));
                        sqlcmd1.ExecuteNonQuery();
                    }
                }

                tran.Commit();

                MessageBox.Show("Insert Successfull", "Insert Alert");
                clearData();
                sqlcon.Close();
            }
            catch(Exception ex)
            {
                tran.Rollback();

                MessageBox.Show("Invalid Input\n"+ex.Message);
                sqlcon.Close();
            }
            LoadGrid();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells[0].Value);
            sqlcon.Open();
            SqlDataAdapter sda = new SqlDataAdapter(@"select medicineId,medicineName,companyId,formId,medicineImage,mrp from medicine where medicineId=" + id + "", sqlcon);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                txtId.Text = dt.Rows[0][0].ToString();
                txtMedicineName.Text = dt.Rows[0][1].ToString();
                cmbCoompanyName.SelectedValue = dt.Rows[0][2].ToString();
                cmbFormName.SelectedValue = dt.Rows[0][3].ToString();
                // txtMedicineName.Text = dt.Rows[0][4].ToString();
                MemoryStream ms = new MemoryStream((byte[])dt.Rows[0][4]);
                Image img = Image.FromStream(ms);
                pictureBox1.Image = img;
                txtMrp.Text = dt.Rows[0][5].ToString();
            }
            sqlcon.Close();

            sqlcon.Open();
            SqlDataAdapter sda1 = new SqlDataAdapter(@"select id,supplierName,supplierAddress,disAmount from medicineSupplier where medicineId=" + id + "", sqlcon);
            DataTable dt1 = new DataTable();
            sda1.Fill(dt1);
            dataGridView1.DataSource = dt1;

            sqlcon.Close();

        }
        private void loadGridData(int id)
        {
            sqlcon.Open();
            SqlDataAdapter sda = new SqlDataAdapter(@"select medicineId,medicineName,companyId,formId,medicineImage,mrp from medicine where medicineId=" + id + "", sqlcon);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                txtId.Text = dt.Rows[0][0].ToString();
                txtMedicineName.Text = dt.Rows[0][1].ToString();
                cmbCoompanyName.SelectedValue = dt.Rows[0][2].ToString();
                cmbFormName.SelectedValue = dt.Rows[0][3].ToString();
                // txtMedicineName.Text = dt.Rows[0][4].ToString();
                MemoryStream ms = new MemoryStream((byte[])dt.Rows[0][4]);
                Image img = Image.FromStream(ms);
                pictureBox1.Image = img;
                txtMrp.Text = dt.Rows[0][5].ToString();
            }
            sqlcon.Close();

            sqlcon.Open();
            SqlDataAdapter sda1 = new SqlDataAdapter(@"select id,supplierName,supplierAddress,disAmount from medicineSupplier where medicineId=" + id + "", sqlcon);
            DataTable dt1 = new DataTable();
            sda1.Fill(dt1);
            dataGridView1.DataSource = dt1;

            sqlcon.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                sqlcon.Open();
                tran = sqlcon.BeginTransaction();
                if (txtPicture.Text != "")
                {
                    Image img = Image.FromFile(txtPicture.Text);
                    MemoryStream ms = new MemoryStream();
                    img.Save(ms, ImageFormat.Bmp);
                    SqlCommand sqlcmd = new SqlCommand(@"update medicine set medicineName=@n,companyId=@ci ,
                    formId=@fi ,medicineImage=@p,MRP=@m where medicineId=@i",sqlcon,tran);
                    sqlcmd.Parameters.AddWithValue("@i", txtId.Text);
                    sqlcmd.Parameters.AddWithValue("@n", txtMedicineName.Text);
                    sqlcmd.Parameters.AddWithValue("@ci", cmbCoompanyName.SelectedValue);
                    sqlcmd.Parameters.AddWithValue("@fi", cmbFormName.SelectedValue);
                    sqlcmd.Parameters.Add(new SqlParameter("@p", SqlDbType.VarBinary) { Value = ms.ToArray() });
                    sqlcmd.Parameters.AddWithValue("@m", txtMrp.Text);
                    sqlcmd.ExecuteNonQuery();
                }
                else
                {
                    SqlCommand sqlcmd = new SqlCommand(@"update medicine set medicineName=@n,companyId=@ci ,
                    formId=@fi ,MRP=@m where medicineId=@i",sqlcon,tran);
                    sqlcmd.Parameters.AddWithValue("@i", txtId.Text);
                    sqlcmd.Parameters.AddWithValue("@n", txtMedicineName.Text);
                    sqlcmd.Parameters.AddWithValue("@ci", cmbCoompanyName.SelectedValue);
                    sqlcmd.Parameters.AddWithValue("@fi", cmbFormName.SelectedValue);
                    sqlcmd.Parameters.AddWithValue("@m", txtMrp.Text);
                    sqlcmd.ExecuteNonQuery();
                }
                foreach (DataGridViewRow dgvRow in dataGridView1.Rows)
                {
                    if (dgvRow.IsNewRow) break;
                    else
                    {
                        if (dgvRow.Cells["dgvtxtId"].Value.ToString()!="")
                        {
                            SqlCommand sqlcmd1 = new SqlCommand(@"update medicineSupplier set supplierName=@sn, supplierAddress=@sa ,disAmount=@d where id=@id", sqlcon,tran);
                            sqlcmd1.Parameters.AddWithValue("@id", Convert.ToInt32(dgvRow.Cells["dgvtxtId"].Value));
                            sqlcmd1.Parameters.AddWithValue("@sn", dgvRow.Cells["dgvtxtSupplierName"].Value);
                            sqlcmd1.Parameters.AddWithValue("@sa", dgvRow.Cells["dgvtxtAddress"].Value);
                            sqlcmd1.Parameters.AddWithValue("@d", Convert.ToInt32(dgvRow.Cells["dgvtxtDisAmount"].Value)); sqlcmd1.ExecuteNonQuery();
                        }
                        else
                        {
                            SqlCommand sqlcmd1 = new SqlCommand(@"insert into medicineSupplier(medicineId,supplierName,supplierAddress,disAmount) values(@mi,@sn,@sa,@d)", sqlcon,tran);
                            sqlcmd1.Parameters.AddWithValue("@mi", Convert.ToInt32(txtId.Text));
                            sqlcmd1.Parameters.AddWithValue("@sn", dgvRow.Cells["dgvtxtSupplierName"].Value);
                            sqlcmd1.Parameters.AddWithValue("@sa", dgvRow.Cells["dgvtxtAddress"].Value);
                            sqlcmd1.Parameters.AddWithValue("@d", Convert.ToInt32(dgvRow.Cells["dgvtxtDisAmount"].Value));
                            sqlcmd1.ExecuteNonQuery();
                        }
                    }
                }
                tran.Commit();
                MessageBox.Show("Update SuccessFully!!", "Update Message");
                sqlcon.Close();
            }
            catch(Exception ex)
            {
                tran.Rollback();
                MessageBox.Show("Invalid Input!!\n"+ex.Message);
                sqlcon.Close();
            }
            LoadGrid();
            loadGridData(Convert.ToInt32(txtId.Text));
            dataGridView2.CurrentCell = dataGridView2.Rows[Convert.ToInt32(txtId.Text)-1].Cells[0];
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                sqlcon.Open();

                tran = sqlcon.BeginTransaction();

                SqlCommand sqlcmd1 = new SqlCommand(@"delete from medicineSupplier where medicineId=" + Convert.ToInt32(txtId.Text) + " ",sqlcon,tran);
                sqlcmd1.ExecuteNonQuery();
                
                SqlCommand sqlcmd = new SqlCommand(@"delete from medicine where medicineId=" + txtId.Text + " ",sqlcon,tran);
                sqlcmd.ExecuteNonQuery();
                                
                tran.Commit();
                
                MessageBox.Show("Delete Successfully !!", "Delete Message");
                clearData();
                sqlcon.Close();
            }
            catch(Exception ex)
            {
                tran.Rollback();
                MessageBox.Show("Data Not Valid !!"+ex.Message);
                sqlcon.Close();
            }
            LoadGrid();
        }
    }
}
