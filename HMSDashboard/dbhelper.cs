using System;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media.Imaging;
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
        byte[] profilePhoto,
        string MobileNumber,
        bool isAdmitPatient
    )
    {

        string patientStatus;
        if (isAdmitPatient)
        {

            patientStatus = "Admitted";
        }
        else {

            patientStatus = "Discharged";
        }

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
                    (FullName, NIC, CheckInDate, Disease, BloodType, BedNo, Gender, Age, Address, City,ProfilePhoto,Status,MobileNumber) 
                    VALUES 
                    (@FullName, @NIC, @CheckInDate, @Disease, @BloodType, @BedNo, @Gender, @Age, @Address, @City,@ProfilePhoto,@Status,@MobileNumber);";

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
                insertCmd.Parameters.AddWithValue("@Status", patientStatus); 
                insertCmd.Parameters.AddWithValue("@MobileNumber",MobileNumber);



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
    
    
    
    
    public void CreateDoctor(
    string fullName,
    string nic,
    string gender,
    string age,
    string phone,
    string specialization,
    string qualification,
    byte[] profilePhoto
)
{
    try
    {
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            conn.Open();

            // Check if doctor already exists by NIC
            string checkQuery = "SELECT COUNT(*) FROM doctors WHERE NIC = @NIC";
            using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
            {
                checkCmd.Parameters.AddWithValue("@NIC", nic);
                int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (count > 0)
                {
                    MessageBox.Show("A doctor with this NIC already exists.", "Duplicate Entry", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            // If NIC does not exist, proceed with insertion
            string insertQuery = @"
                INSERT INTO doctors 
                (FullName, NIC, Gender, Age, Phone, Specialization, Qualification, ProfilePhoto, Status) 
                VALUES 
                (@FullName, @NIC, @Gender, @Age, @Phone, @Specialization, @Qualification, @ProfilePhoto, @Status);";

            MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn);

            insertCmd.Parameters.AddWithValue("@FullName", fullName);
            insertCmd.Parameters.AddWithValue("@NIC", nic);
            insertCmd.Parameters.AddWithValue("@Gender", gender);
            insertCmd.Parameters.AddWithValue("@Age", age);
            insertCmd.Parameters.AddWithValue("@Phone", phone);
            insertCmd.Parameters.AddWithValue("@Specialization", specialization);
            insertCmd.Parameters.AddWithValue("@Qualification", qualification);
            insertCmd.Parameters.AddWithValue("@ProfilePhoto", profilePhoto ?? (object)DBNull.Value);
            insertCmd.Parameters.AddWithValue("@Status", "Active");

            int result = insertCmd.ExecuteNonQuery();

            if (result > 0)
            {
                MessageBox.Show("Doctor added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Failed to add doctor.", "Failure", MessageBoxButton.OK, MessageBoxImage.Warning);
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
    
    
    
    
    // get patient data by NIC
    public (bool found, string fullName, string gender, string age, string address, string bloodType, string status, BitmapImage profileImage,DateTime checkInDate,string Disease) GetPatientDataByNic(string nic)
{
    string query = "SELECT FullName, NIC, Gender, Age, Address, BloodType, ProfilePhoto, Status,checkInDate,Disease FROM patients WHERE NIC = @NIC";

    try
    {
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            conn.Open();
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@NIC", nic);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string fullName = reader["FullName"]?.ToString() ?? "null";
                        string gender = reader["Gender"]?.ToString() ?? "null";
                        string age = reader["Age"]?.ToString() ?? "null";
                        string address = reader["Address"]?.ToString() ?? "null";
                        string bloodType = reader["BloodType"]?.ToString() ?? "null";
                        string status = reader["Status"]?.ToString() ?? "null";
                        DateTime checkInDate = Convert.ToDateTime(reader["CheckInDate"]);
                        string disease = reader["Disease"]?.ToString() ?? "null";


                        BitmapImage profileImage = null;
                        if (reader["ProfilePhoto"] != DBNull.Value)
                        {
                            byte[] imageBytes = (byte[])reader["ProfilePhoto"];
                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            {
                                BitmapImage bitmap = new BitmapImage();
                                bitmap.BeginInit();
                                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                                bitmap.StreamSource = ms;
                                bitmap.EndInit();
                                bitmap.Freeze();
                                profileImage = bitmap;
                            }
                        }
                        else
                        {
                            profileImage = new BitmapImage(new Uri("pack://application:,,,/20180125_001_1_.jpg"));
                        }

                        return (true, fullName, gender, age, address, bloodType, status, profileImage,checkInDate,disease);
                    }
                    else
                    {
                        return (false, null, null, null, null, null, null, null,DateTime.MinValue,null);
                    }
                }
            }
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show("Error fetching patient data: " + ex.Message);
        return (false, null, null, null, null, null, null, null,DateTime.MinValue,null);
    }
}
    
    
    
    
    // update admit patient
    
    public void AdmitPatientByFindNic(string nic, string bedNo, string disease, DateTime checkInDate, string status)
    {
        string query = "UPDATE patients SET BedNo = @BedNo, Disease = @Disease, CheckInDate = @CheckInDate, Status = @Status WHERE NIC = @NIC";
        

        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@BedNo", bedNo);
                    cmd.Parameters.AddWithValue("@Disease", disease);
                    cmd.Parameters.AddWithValue("@CheckInDate", checkInDate);
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@NIC", nic);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Patient admitted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Admit failed. NIC not found or already admitted.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("An error occurred while admitting the patient:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    
    
    // patient discharge by nic
    
    public void DischargePatientByNic(string nic, DateTime checkInDate, string status)
    {
        string query = "UPDATE patients SET BedNo = NULL, Disease = NULL, CheckInDate = @CheckInDate, Status = @Status WHERE NIC = @NIC";

        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CheckInDate", checkInDate);
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@NIC", nic);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Patient discharged successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Discharge failed. NIC not found or already discharged.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("An error occurred while discharging the patient:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    
    
    
    //  GetPatientStatusByNIC-----
    public string GetPatientStatusByNIC(string nic)
{
    // Validate NIC with regex: 12 digits or 9 digits + 'V' or 'v'
    if (!Regex.IsMatch(nic, @"^(\d{12}|\d{9}[vV])$"))
    {
        MessageBox.Show("Invalid NIC format.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return null;
    }

    string connectionString = "server=localhost;database=hsm;uid=root;pwd=;";
    string query = "SELECT Status FROM patients WHERE NIC = @NIC";

    try
    {
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            conn.Open();

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@NIC", nic);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string status = reader["Status"].ToString();

                        if (status == "Discharged")
                        {
                            return "Discharged";
                        }
                        else if (status == "Admitted")
                        {
                            return "Admitted";
                        }
                        else
                        {
                            MessageBox.Show($"Unknown status '{status}' for this patient.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return null;
                        }
                    }
                    else
                    {
                        MessageBox.Show("No patient found with that NIC. Please create a new record.", "Not Found", MessageBoxButton.OK, MessageBoxImage.Information);
                        return null;
                    }
                }
            }
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show("Error while searching patient: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return null;
    }
}



    // Generic method to execute COUNT(*) queries
    public int GetCountFromQuery(string query)
    {
        int count = 0;

        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    count = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error executing count query: " + ex.Message);
        }

        return count;
    }







}
