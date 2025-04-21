using System;
using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;

public class dbHelper
{
    private string connectionString = "server=localhost;database=hsm;uid=root;pwd=;";

    public void CreatePatient(
        string fullName,
        string nic,
        DateTime? checkInDate,
        string disease,
        string bloodType,
        string bedNo,
        string gender,
        string age,
        string address,
        string city,
        byte[] profilePhoto
    )
    {
        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                // Check if patient already exists by NIC
                string checkQuery = "SELECT COUNT(*) FROM patients WHERE NIC = @NIC";
                using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@NIC", nic);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    // If NIC exists, show a message and exit
                    if (count > 0)
                    {
                        MessageBox.Show("A patient with this NIC already exists.", "Duplicate Entry", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }

                // If NIC does not exist, proceed with insertion
                string insertQuery = @"
                    INSERT INTO patients 
                    (FullName, NIC, CheckInDate, Disease, BloodType, BedNo, Gender, Age, Address, City,ProfilePhoto) 
                    VALUES 
                    (@FullName, @NIC, @CheckInDate, @Disease, @BloodType, @BedNo, @Gender, @Age, @Address, @City,@ProfilePhoto);";

                MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn);

                insertCmd.Parameters.AddWithValue("@FullName", fullName);
                insertCmd.Parameters.AddWithValue("@NIC", nic);
                insertCmd.Parameters.AddWithValue("@CheckInDate", checkInDate.HasValue ? checkInDate.Value.ToString("yyyy-MM-dd") : (object)DBNull.Value);
                insertCmd.Parameters.AddWithValue("@Disease", disease);
                insertCmd.Parameters.AddWithValue("@BloodType", bloodType);
                insertCmd.Parameters.AddWithValue("@BedNo", bedNo);
                insertCmd.Parameters.AddWithValue("@Gender", gender);
                insertCmd.Parameters.AddWithValue("@Age", age);
                insertCmd.Parameters.AddWithValue("@Address", address);
                insertCmd.Parameters.AddWithValue("@City", city);
                insertCmd.Parameters.AddWithValue("@ProfilePhoto", profilePhoto ?? (object)DBNull.Value);
                

                int result = insertCmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("Patient created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Failed to create patient.", "Failure", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    
    
    public DataTable GetRecentPatients()
    {
        DataTable dataTable = new DataTable();

        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM patients ORDER BY CheckInDate DESC LIMIT 10";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dataTable);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error loading patients: " + ex.Message);
        }

        return dataTable;
    }

    
    
    
    
}
