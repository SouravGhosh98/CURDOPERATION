using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace curdoperation
{
    public partial class CURDOPERATION : Form
    {
        private SqlConnection connection;
        string connectionstring= "Data Source=DESKTOP-EQOL92T\\SQLEXPRESS;Initial Catalog=CURD;Integrated Security=True";
        bool s=false;
        public CURDOPERATION()
        {
            InitializeComponent();
            try
            { 
                connection = new SqlConnection(connectionstring);  //DESKTOP-EQOL92T\SQLEXPRESS
                connection.Open();
                dataGridresult.CellClick += dataGridresult_CellClick;
                loaddata();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error connecting to the Database:"+ex.Message);    
            }
        }
        private void btninsert_Click(object sender, EventArgs e)
        {
            string insertQuery = "INSERT INTO Companies (id,name,AGE) VALUES (@id,@name,@AGE)";
            using (SqlCommand comd = new SqlCommand(insertQuery, connection))
            {
                comd.Parameters.AddWithValue("@id", textBoxID.Text);
                comd.Parameters.AddWithValue("@name", textBoxname.Text);
                comd.Parameters.AddWithValue("@AGE", textBoxAge.Text);
                s=ValidateInput();
                if(s==true)
                {
                   int rowsAffected = comd.ExecuteNonQuery();
                     if (rowsAffected > 0)
                     { 
                        MessageBox.Show("Data inserted successfully.");
                        s=false;
                     }
                    else
                    { 
                       MessageBox.Show("Failed to insert data.");
                    }
                  loaddata();
                }
            }
        }
        private void btndelet_Click(object sender, EventArgs e)
        {
            string deleteQuery = "DELETE FROM Companies WHERE ID = @id";
            using (SqlCommand comd = new SqlCommand(deleteQuery, connection))
            {
                comd.Parameters.AddWithValue("@id", textBoxID.Text);
                int rowsAffected = comd.ExecuteNonQuery();
                if (rowsAffected > 0)
                { 
                    MessageBox.Show("Data deleted successfully.");
                    textBoxID.Text=" ";
                    textBoxname.Text=" ";
                    textBoxAge.Text=" ";
                }
                else
                { 
                    MessageBox.Show("Failed to delete data.");
                    textBoxID.Text=" ";
                    textBoxname.Text=" ";
                    textBoxAge.Text=" ";
                }
            }
            loaddata();
        }
        private void btnupdate_Click(object sender, EventArgs e)
        {
            bool k=false;
            string updateQuery = "UPDATE Companies SET name = @name, AGE = @AGE WHERE id = @ID";
             s=ValidateInput();
             if (s==true)
             {
                using (SqlCommand comd = new SqlCommand(updateQuery, connection))
                {
                   comd.Parameters.AddWithValue("@name", textBoxname.Text);
                   comd.Parameters.AddWithValue("@AGE", textBoxAge.Text);
                   comd.Parameters.AddWithValue("@id", textBoxID.Text);
                    k=ValidateInput();
                    {
                        if(k==true)
                        {
                         int rowsAffected = comd.ExecuteNonQuery();
                          if (rowsAffected > 0)
                          { 
                             MessageBox.Show("Data updated successfully.");
                          }
                         else
                         { 
                             MessageBox.Show("Failed to update data.");
                         }
                        }
                    }
                }
                loaddata();
             }
        }
        private void btnsearch_Click(object sender, EventArgs e)
        {
            string searchQuery = "SELECT * FROM Companies WHERE id = @id";
                using (SqlCommand comd = new SqlCommand(searchQuery, connection))
                {
                  comd.Parameters.AddWithValue("@id", textBoxID.Text);
                   using (SqlDataReader reader = comd.ExecuteReader())
                   {
                     if (reader.Read())
                     {
                        int id = Convert.ToInt32(reader["id"]);
                        string name = reader["name"].ToString();
                        int age = Convert.ToInt32(reader["AGE"]);
                        MessageBox.Show($"ID: {id}\n Name: {name}\n Age: {age}");
                     }
                     else
                     {
                        textBoxID.Text=" ";
                        textBoxname.Text=" ";
                        textBoxAge.Text=" ";
                        MessageBox.Show("Data not found.");
                     }
                   }
                }
               loaddata();
        }
        private void loaddata()
        {
                string selectQuery = "SELECT * FROM Companies";
                using (SqlDataAdapter adapter = new SqlDataAdapter(selectQuery, connection))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridresult.DataSource = dataTable;
                }
        }
        private bool ValidateInput()
        {
               if (string.IsNullOrWhiteSpace(textBoxname.Text))
               {
                   MessageBox.Show("Please enter a name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                   textBoxname.Text="\0";
                   return false;
               }
               foreach (char character in textBoxname.Text)
               {
                  if (!char.IsLetter(character))
                  {
                      MessageBox.Show("Name should contain only letters.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                      textBoxname.Text="\0";
                      return false;
                  }
               }
               if (!int.TryParse(textBoxID.Text, out _))
               {
                  MessageBox.Show("Please enter a valid ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                  textBoxID.Text="\0";
                  return false;
               }
               if (!int.TryParse(textBoxAge.Text, out _))
               {
                  MessageBox.Show("Please enter a valid age.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                  textBoxAge.Text="\0";
                  return false;
               }  
              return true;
        }
        private void dataGridresult_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) 
            {
               DataGridViewRow selectedRow = dataGridresult.Rows[e.RowIndex];
               textBoxID.Text = selectedRow.Cells["id"].Value.ToString();
               textBoxname.Text = selectedRow.Cells["name"].Value.ToString();
               textBoxAge.Text = selectedRow.Cells["AGE"].Value.ToString();
            }
        }

    }
}
