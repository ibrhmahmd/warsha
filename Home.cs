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
        public DataTable ordersTable;

        public Home()
        {
            InitializeComponent();
        }
        private void adding_customer_Load(object sender, EventArgs e)
        {

            //loding list of prvios orders
            {
                // Initialize data adapter with select command
                dataAdapter = new SqlDataAdapter("select  order_name, order_number, date_added, total_price_of_the_order  from [dbo].[orders]\r\n", cn);

                // Initialize command builder to generate update/insert/delete commands
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);


                // Initialize DataSet and fill it
                dataSet = new DataSet();
                dataAdapter.Fill(dataSet, "orders");

                // Get the DataTable from DataSet
                ordersTable = dataSet.Tables["orders"];

                // Bind DataGridView to DataTable
                ordersgrid.DataSource = ordersTable;


                // Changing the header text of the columns
                ordersgrid.Columns[0].HeaderText = "الاسم";
                ordersgrid.Columns[1].HeaderText = "رقم الطلب";
                ordersgrid.Columns[2].HeaderText = "التاريخ";
                ordersgrid.Columns[3].HeaderText = "السعر";
            }


            //loading a single order data
            {
                // Initialize data adapter with select command
                dataAdapter = new SqlDataAdapter("select height, width,count, thickness, area, part_price from [dbo].[order_parts]", cn);

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


                // Subscribe to the CellValueChanged event
                Order_DataGrid.CellValueChanged += Order_DataGrid_CellValueChanged;


                // Make only the first three columns editable, the rest read-only
                for (int i = 4; i < Order_DataGrid.Columns.Count; i++)
                {
                    Order_DataGrid.Columns[i].ReadOnly = true;
                }

                // Changing the header text of the columns
                Order_DataGrid.Columns[0].HeaderText = "الطول";
                Order_DataGrid.Columns[1].HeaderText = "العرض";
                Order_DataGrid.Columns[2].HeaderText = "العدد";
                Order_DataGrid.Columns[3].HeaderText = "السمك";
                Order_DataGrid.Columns[4].HeaderText = "مساحه";
                Order_DataGrid.Columns[5].HeaderText = "سعر القطعه";

                // Initialize and configure SaveButton
                Button SaveButton = new Button();
                SaveButton.Text = "Save Changes";
                SaveButton.Location = new Point(10, 10); // Adjust the location as needed
                SaveButton.Click += SaveButton_Click;

                // Add save button to the form
                Controls.Add(SaveButton);
            }
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

        private void Order_DataGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Ensure the row index is within the valid range
            if (e.RowIndex < 0 || e.RowIndex >= Order_DataGrid.Rows.Count)
            {
                return; // Exit the method if the row index is out of range
            }

            // Check if the changed cell is in the Height or Width column
            if (e.ColumnIndex == Order_DataGrid.Columns["height"].Index ||
                e.ColumnIndex == Order_DataGrid.Columns["width"].Index ||
                e.ColumnIndex == Order_DataGrid.Columns["count"].Index)
            {
                // Get the current row
                DataGridViewRow row = Order_DataGrid.Rows[e.RowIndex];

                // Retrieve the Height and Width values
                if (decimal.TryParse(row.Cells["height"].Value?.ToString(), out decimal height) &&
                    decimal.TryParse(row.Cells["width"].Value?.ToString(), out decimal width) &&
                    decimal.TryParse(row.Cells["count"].Value?.ToString(), out decimal count))
                {

                    cn.Open();
                    //getting the price of th squere meter 
                    SqlCommand get_price = new SqlCommand("select squaremeter_price from [dbo].[orders] ", cn);
                    SqlDataReader get_price_reader = get_price.ExecuteReader();
                    // Check if there are rows returned from the query
                    if (get_price_reader.Read())
                    {
                        // Retrieve the value from the SqlDataReader
                        // Assuming squaremeter_price is an integer in the database
                        decimal sqrmtr_price = get_price_reader.GetDecimal(0); // Assuming squaremeter_price is the first column

                        // Now you can use sqrmtr_price as needed
                        SQR_MTR_Price.Text = sqrmtr_price + "$ ";

                        // Calculate the Area & price
                        decimal area = (height * width) * count;
                        decimal part_price = Math.Round(area * sqrmtr_price);
                        cn.Close();

                        cn.Open();
                        //upload the price to the database
                        SqlCommand upload_price = new SqlCommand("select squaremeter_price from [dbo].[orders] ", cn);
                        upload_price.ExecuteNonQuery();
                        cn.Close();


                        // Update the Area cell value
                        row.Cells["area"].Value = area;
                        row.Cells["part_price"].Value = part_price;
                    }

                }
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



        private void add_order_btn_click(object sender, EventArgs e)
        {
            ////this.Hide();
            ////Add_Order add_order_page = new Add_Order();
            ////add_order_page.Show();

            // Change the parent to the form (or any other container)
            add_order_btn.Parent = this; // 'this' refers to the form

            add_order_btn.BringToFront();

            add_order_btn.BackColor = Color.Black;
            add_order_btn.BorderColor = Color.White;



            // Define the new location and size
            Point newLocation = new Point(2200, 35);
            Size newSize = new Size(250, 70);




            // Set the location and size of the form
            add_order_btn.Location = newLocation;
            add_order_btn.Size = newSize;
            adding_groubbox.Visible = true;

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

        private void adding_groubbox_Click(object sender, EventArgs e)
        {

        }

        private void Order_DataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void add_order_group_Click(object sender, EventArgs e)
        {

        }

        private void order_name_TextChanged(object sender, EventArgs e)
        {

        }
    }
}