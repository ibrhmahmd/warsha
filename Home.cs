using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.VisualBasic.ApplicationServices;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data.Common;

namespace warsha
{
    public partial class Home : Form
    {
        SqlConnection cn = new SqlConnection("Data Source=IBRAHIM;Initial Catalog=warsha;Integrated Security=True;Encrypt=False");
        public SqlDataAdapter dataAdapter;
        public DataSet dataSet;
        public DataTable orderDataTable;

        public Home()
        {
            InitializeComponent();
        }
        private void adding_customer_Load(object sender, EventArgs e)
        {

            // Initialize data adapter with select command
            dataAdapter = new SqlDataAdapter("select height, width, thickness, area, part_price, total_price_of_parts from [dbo].[order_parts]", cn);

            // Initialize command builder to generate update/insert/delete commands
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);

            // Initialize DataSet and fill it
            dataSet = new DataSet();
            dataAdapter.Fill(dataSet, "order_parts");

            // Get the DataTable from DataSet
            orderDataTable = dataSet.Tables["order_parts"];

            // Bind DataGridView to DataTable
            Order_DataGrid.DataSource = orderDataTable;

            // Allow user to add, delete, and edit rows
            Order_DataGrid.AllowUserToAddRows = true;
            Order_DataGrid.AllowUserToDeleteRows = true;
            Order_DataGrid.EditMode = DataGridViewEditMode.EditOnEnter;

            // Make only the first three columns editable, the rest read-only
            for (int i = 3; i < Order_DataGrid.Columns.Count; i++)
            {
                Order_DataGrid.Columns[i].ReadOnly = true;
            }

            // Changing the header text of the columns
            Order_DataGrid.Columns[0].HeaderText = "Height";
            Order_DataGrid.Columns[1].HeaderText = "Width";
            Order_DataGrid.Columns[2].HeaderText = "Thickness";
            Order_DataGrid.Columns[3].HeaderText = "Area";
            Order_DataGrid.Columns[4].HeaderText = "Price";
            Order_DataGrid.Columns[5].HeaderText = "Total $";

            // Add save changes button
            SaveButton.Text = "Save Changes";
            SaveButton.Click += SaveButton_Click;
            Controls.Add(SaveButton);
        }



        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                dataAdapter.Update(dataSet, "order_parts");
                MessageBox.Show("Changes saved successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving changes: " + ex.Message);
            }
        }











        private void ADD_cust_Click(object sender, EventArgs e)
        {
            try
            {
                cn.Open();

                string redundet_check_query = "SELECT name FROM [dbo].[customers] WHERE name = @name";
                string redundet_phone_check_query = "SELECT name FROM [dbo].[customers] WHERE phone = @phone";

                SqlCommand redundet_name_check = new SqlCommand(redundet_check_query, cn);
                SqlCommand redundet_phone_check = new SqlCommand(redundet_phone_check_query, cn);

                redundet_name_check.Parameters.AddWithValue("@name", CustName.Text);
                redundet_phone_check.Parameters.AddWithValue("@phone", CustPhone.Text);

                bool isDuplicate = false;

                using (SqlDataReader name_reader = redundet_name_check.ExecuteReader())
                {
                    if (name_reader.Read())
                    {
                        MessageBox.Show("This name is registered before", "Popup", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CustName.Text = string.Empty;
                        isDuplicate = true;
                    }
                }

                if (!isDuplicate)
                {
                    using (SqlDataReader phone_reader = redundet_phone_check.ExecuteReader())
                    {
                        if (phone_reader.Read())
                        {
                            MessageBox.Show("This phone is registered before", "Popup", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CustPhone.Text = string.Empty;
                            isDuplicate = true;
                        }
                    }
                }

                if (!isDuplicate)
                {
                    string insert_query = "INSERT INTO [dbo].[customers] (name, phone, joined) VALUES (@name, @phone, @joined)";
                    SqlCommand insert_user_table = new SqlCommand(insert_query, cn);
                    insert_user_table.Parameters.AddWithValue("@name", CustName.Text);
                    insert_user_table.Parameters.AddWithValue("@phone", CustPhone.Text);
                    insert_user_table.Parameters.AddWithValue("@joined", DateTime.Now); // Adding the current date and time

                    insert_user_table.ExecuteNonQuery();
                    MessageBox.Show("USER ADDED", "Popup", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.Close();
            }
        }

    


        //private void Order_DataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    dataAdapter = new SqlDataAdapter("select * from [dbo].[order_parts]", cn);

        //    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);

        //    oder_data_table = new DataTable();
        //    dataAdapter.Fill(oder_data_table);

        //    Order_DataGrid.DataSource = oder_data_table;
        //    Order_DataGrid.AllowUserToAddRows = true;
        //    Order_DataGrid.AllowUserToDeleteRows = true;
        //    Order_DataGrid.EditMode = DataGridViewEditMode.EditOnEnter;

        //    // Save changes button
        //    Button saveButton = new Button();
        //    saveButton.Text = "Save Changes";
        //    saveButton.Click += SaveButton_Click;
        //    Controls.Add(saveButton);
        //}



        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

      

       
    }
}