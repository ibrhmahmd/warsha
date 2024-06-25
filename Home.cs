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
using System.Security.Cryptography.X509Certificates;
using System.Net;
using static TheArtOfDevHtmlRenderer.Adapters.RGraphicsPath;

namespace warsha
{
    public partial class Home : Form
    {
        SqlConnection cn = new SqlConnection("Data Source=IBRAHIM;Initial Catalog=warsha;Integrated Security=True;Encrypt=False");
        public SqlDataAdapter dataAdapter;
        public DataSet dataSet;
        public DataTable orderDataTable;
        public DataTable ordersTable;
        public DataTable customersTable;
        public string cust_name_to_create_its_orderstable;
        public string sqrmtr_prc_create_its_orderstable;

        public Home()
        {
            InitializeComponent();
        }
        private void adding_customer_Load(object sender, EventArgs e)
        {
            loading_orders_grid();
            loading_customers_grid();
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

            //redirecting to another page
            //Add_Order go_to_order = new Add_Order();
            //go_to_order.Show();
            //this.Hide();

            // Adding the customer credentials to the customers table
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
                        string insert_query = "INSERT INTO [dbo].[customers] (name, phone, joined, balance) VALUES (@name, @phone, @joined, @balance)";
                        SqlCommand insert_user_table = new SqlCommand(insert_query, cn);
                        insert_user_table.Parameters.AddWithValue("@name", CustName.Text);
                        insert_user_table.Parameters.AddWithValue("@phone", CustPhone.Text);
                        insert_user_table.Parameters.AddWithValue("@balance", CustBalance.Text);
                        insert_user_table.Parameters.AddWithValue("@joined", DateTime.Now); // Adding the current date and time
                        insert_user_table.ExecuteNonQuery();
                        MessageBox.Show("USER ADDED", "Popup", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cust_name_to_create_its_orderstable = CustName.Text;
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
            string CustTableName = "orders_of_" + CustName.Text;
            loading_customers_grid();

            // Creating a table for this customer to gather his orders
            {
                creating_an_orders_table_by_the_name_of_customer(CustTableName);
            }
        }

        private void creating_an_orders_table_by_the_name_of_customer(string cust_name_to_create_orderstable)
        {
            // Ensure the customer name does not contain SQL-injection characters or invalid table name characters
            string sanitizedCustomerName = cust_name_to_create_orderstable.Replace("'", "''");
            // Use square brackets around the table name to handle special characters
            string tableName = "[" + sanitizedCustomerName + "]";

            // Define the SQL query to create the table
            string createTableQuery = $@"CREATE TABLE {tableName}
                                (order_id INT NOT NULL, 
                                customer_name INT NULL,
                                order_number VARCHAR(50) NULL,
                                date_added DATE NULL,
                                discount DECIMAL(5, 2) NULL,
                                squaremeter_price DECIMAL(10, 2) NULL,
                                total_price_of_the_order DECIMAL(10, 2) NULL,
                                order_name VARCHAR(50) NULL);";

            try
            {
                // Open the connection
                cn.Open();

                // Create and execute the SQL command
                SqlCommand createTableCommand = new SqlCommand(createTableQuery, cn);
                createTableCommand.ExecuteNonQuery();

                // Inform the user of success
                //MessageBox.Show("Done creating the orders table for: " + cust_name_to_create_orderstable);
            }
            catch (SqlException ex)
            {
                // Log or handle the SQL exception
                MessageBox.Show("SQL Error: " + ex.Message);
            }
            finally
            {
                // Ensure the connection is closed
                if (cn.State == System.Data.ConnectionState.Open)
                {
                    cn.Close();
                }
            }
        }







        private void add_order_btn_click(object sender, EventArgs e)
        {

            // Change the parent to the form (or any other container)
            add_order_btn.Parent = this; // 'this' refers to the form

            add_order_btn.BringToFront();

            add_order_btn.BackColor = Color.Black;
            add_order_btn.BorderColor = Color.White;



            // Define the new location and size for this button 
            Point newLocation = new Point(2000, 35);
            Size newSize = new Size(400, 70);

            // Set the location and size of this button
            add_order_btn.Location = newLocation;
            add_order_btn.Size = newSize;



            //showing the order adding groubbox
            adding_groubbox.Visible = true;
            adding_groubbox.BringToFront();

            // Define the new location for addding customer group 
            Point newLocation_for_adding_groubbox = new Point(100, 200);
            //adding_groubbox.Location = newLocation_for_adding_groubbox;

            //// Define the new size for addding customer group 
            Size newSize_for_adding_groubbox = new Size(1700, 200);
            //adding_groubbox.Size = newSize_for_adding_groubbox;
        }






        private void loading_orders_grid()
        {
            //loding list of prvios orders

            // Initialize data adapter with select command
            dataAdapter = new SqlDataAdapter("select  order_name, order_number, date_added, total_price_of_the_order  from [dbo].[orders] ", cn);

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

        private void loading_customers_grid()
        {
            try
            {
                // Initialize data adapter with select command
                dataAdapter = new SqlDataAdapter("SELECT * FROM [dbo].[customers]", cn);

                // Initialize command builder to generate update/insert/delete commands
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);

                // Initialize DataSet and fill it
                dataSet = new DataSet();
                dataAdapter.Fill(dataSet, "customers");

                // Get the DataTable from DataSet
                ordersTable = dataSet.Tables["customers"];

                // Bind DataGridView to DataTable
                customers_grid.DataSource = ordersTable;

                // Change the header text of the DataGridView columns
                customers_grid.Columns[0].HeaderText = "ID";
                customers_grid.Columns[1].HeaderText = "الاسم";
                customers_grid.Columns[2].HeaderText = "رقم الهاتف";
                customers_grid.Columns[3].HeaderText = "التاريخ";
                // Add more columns as needed

                // Optionally, you can set column widths or other properties here
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }





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

        private void CustName_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void ordersgrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void reload_btn_click(object sender, EventArgs e)
        {
            // Start a new instance of the application
            System.Diagnostics.Process.Start(Application.ExecutablePath);

            // Close the current instance
            Application.Exit();
        }

        private void add_order_group_Click_1(object sender, EventArgs e)
        {

        }

        private void orders_group_Click(object sender, EventArgs e)
        {

        }

        private void guna2GroupBox1_Click(object sender, EventArgs e)
        {

        }

        private void customers_grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}