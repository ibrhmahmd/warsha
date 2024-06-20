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

namespace warsha
{
    public partial class adding_customer : Form
    {
        SqlConnection cn = new SqlConnection("Data Source=IBRAHIM;Initial Catalog=warsha;Integrated Security=True;Encrypt=False");

        public adding_customer()
        {
            InitializeComponent();
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


        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void adding_customer_Load(object sender, EventArgs e)
        {

        }
    }
}