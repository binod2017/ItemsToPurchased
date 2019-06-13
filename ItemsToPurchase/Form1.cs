using System;
using System.Data;
using System.Windows.Forms;

namespace ItemsToPurchase
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Text = "Items To Purchase";
        }

        private int slno;
        private string itemname;
        private bool purchased;
        private DateTime dateCreated;
        private DateTime dateModified;

        private void Form1_Load(object sender, EventArgs e)
        {
            //Get all the items stored in the db
            DataTable data = DataAccess.GetAllItems();
            LoadDataGridView(data);
        }
        /// <summary>
        /// Load the Grid with items
        /// </summary>
        /// <param name="data"></param>
        private void LoadDataGridView(DataTable data)
        {
            dgvItems.DataSource = null;
            // Data grid view column setting
            dgvItems.DataSource = data;
            dgvItems.DataMember = data.TableName;
            dgvItems.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DataTable data = DataAccess.GetLastId();
            {
                slno = int.Parse(data.Rows[0]["Expr1000"].ToString()) + 1;
            }
            if (txtItemName.Text.Trim() != string.Empty)
            {
                itemname = txtItemName.Text;
                purchased = false;
                dateCreated = DateTime.Now;
                dateModified = DateTime.Now;

                Purchasing purchasing = new Purchasing()
                {
                    SlNo = slno,
                    ItemName = itemname,
                    Purchased = purchased,
                    DateCreated = dateCreated,
                    DateModified = dateModified
                };
                var success = DataAccess.AddItems(purchasing);
                if (success)
                {
                    //MessageBox.Show("Items Added Successfully");
                    LoadDataGridView(DataAccess.GetAllItems());
                }
                txtItemName.Text = "";
                txtItemName.Focus();
            }
            else
            {
                MessageBox.Show("Item Name cannot be empty");
                txtItemName.Focus();
            }
        }
        private void dgvItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int currentRow = dgvItems.SelectedCells[0].RowIndex;
            //MessageBox.Show("cell content click");
            try
            {
                string Id = dgvItems[0, currentRow].Value.ToString();
                slno = int.Parse(Id);
                bool result = (bool)dgvItems[2, currentRow].Value;
                if (result)
                {
                    purchased = false;
                }
                else { purchased = true; }
                dateModified = DateTime.Now;
                //btnActive.PerformClick(); 
                var status = DataAccess.ActiveDeactive(purchased, dateModified, slno);
                if (status)
                {
                    //MessageBox.Show("Updated Successfully");
                    dgvItems.DataSource = DataAccess.GetAllItems();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void dgvItems_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            try
            {
                if (dgv.SelectedRows.Count > 0)
                {
                    string Id = dgv.SelectedRows[0].Cells[0].Value.ToString();

                    slno = int.Parse(Id);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
