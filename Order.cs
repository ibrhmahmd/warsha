﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace warsha
{
    public partial class Add_Order : Form
    {
        SqlConnection cn = new SqlConnection("Data Source=IBRAHIM;Initial Catalog=warsha;Integrated Security=True;Encrypt=False");
        public SqlDataAdapter dataAdapter;
        public DataSet dataSet;
        public DataTable orderDataTable;
        public DataTable ordersTable;

        public Add_Order()
        {
            InitializeComponent();
        }

       

        private void Add_Order_Load(object sender, EventArgs e)
        {
            loading_order_parts_table();
        }

        private void loading_order_parts_table()
        {

            //loading a single order data
            {
                // Initialize data adapter with select command
                dataAdapter = new SqlDataAdapter("select height, width, count, thickness, area, part_price from [dbo].[order_parts]", cn);

                // Initialize command builder to generate update/insert/delete commands
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);


                // Initialize DataSet and fill it
                dataSet = new DataSet();
                dataAdapter.Fill(dataSet, "order_parts");

                // Get the DataTable from DataSet
                orderDataTable = dataSet.Tables["order_parts"];

                // Bind DataGridView to DataTable
                edit_order_grid.DataSource = orderDataTable;


                // Allow user to add, delete, and edit rows
                edit_order_grid.AllowUserToAddRows = true;
                edit_order_grid.AllowUserToDeleteRows = true;
                edit_order_grid.EditMode = DataGridViewEditMode.EditOnEnter;


                // Subscribe to the CellValueChanged event
                edit_order_grid.CellValueChanged += Order_DataGrid_CellValueChanged;


                // Make only the first three columns editable, the rest read-only
                for (int i = 4; i < edit_order_grid.Columns.Count; i++)
                {
                    edit_order_grid.Columns[i].ReadOnly = true;
                }

                // Changing the header text of the columns
                edit_order_grid.Columns[0].HeaderText = "الطول";
                edit_order_grid.Columns[1].HeaderText = "العرض";
                edit_order_grid.Columns[2].HeaderText = "العدد";
                edit_order_grid.Columns[3].HeaderText = "السمك";
                edit_order_grid.Columns[4].HeaderText = "مساحه";
                edit_order_grid.Columns[5].HeaderText = "سعر القطعه";

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
            if (e.ColumnIndex == edit_order_grid.Columns["height"].Index ||
                e.ColumnIndex == edit_order_grid.Columns["width"].Index ||
                e.ColumnIndex == edit_order_grid.Columns["count"].Index)
            {
                // Get the current row
                DataGridViewRow row = edit_order_grid.Rows[e.RowIndex];

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



        private void reload_btn_Click(object sender, EventArgs e)
        {
            // Start a new instance of the application
            System.Diagnostics.Process.Start(Application.ExecutablePath);

            // Close the current instance
            Application.Exit();
        }


        private void back_Click(object sender, EventArgs e)
        {
            Home back_to_home = new Home();
            back_to_home.Show();
            this.Hide();
        }
    }
}
