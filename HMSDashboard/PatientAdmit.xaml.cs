using System.Windows;


namespace HMSDashboard
{
    public partial class PatientAdmit : Window
    {
        private string nicValue;
        public PatientAdmit(string nic)
        {
            InitializeComponent();
            nicValue = nic;
        }

        private void onLoaded(object sender, RoutedEventArgs e)
        {
            string nic = nicValue;

            if (string.IsNullOrEmpty(nic))
            {
                MessageBox.Show("Please enter a NIC.");
                return;
            }

        /*get data from databse by NIC*/
      
            dbHelper getPatientData = new dbHelper();
            var patientDataResult =getPatientData.GetPatientDataByNic(nic);

            if (patientDataResult.found)
            {
                txtFullName.Text = patientDataResult.fullName;
                txtNIC.Text = "NIC: " + nic;
                txtGender.Text = patientDataResult.gender;
                txtAge.Text = patientDataResult.age;
                txtAddress.Text = patientDataResult.address;
                txtBloodType.Text = patientDataResult.bloodType;
                txtStatus.Text = "Status: " + patientDataResult.status;
                profileImageBrush.ImageSource = patientDataResult.profileImage;
            }
            else
            {
                MessageBox.Show("Patient not found.");
            }
  












            /*
            string connectionString = "server=localhost;database=hsm;uid=root;pwd=;";
            string query = "SELECT FullName, NIC, Gender, Age, Address, BloodType,ProfilePhoto,Status FROM patients WHERE NIC = @NIC";

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
                                // Fetch the data
                                string fullName = reader["FullName"].ToString();
                                string gender = reader["Gender"].ToString();
                                string age = reader["Age"].ToString();
                                string address = reader["Address"].ToString();
                                string bloodType = reader["BloodType"].ToString();
                                string status = reader["Status"].ToString();

                                // display UI

                                txtFullName.Text = fullName;
                                txtNIC.Text = "NIC: " + nic;
                                txtGender.Text = gender;
                                txtAge.Text = age;
                                txtAddress.Text = address;
                                txtBloodType.Text = bloodType;
                                txtStatus.Text = "Status: " + status;
                                
                                
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

                                        profileImageBrush.ImageSource = bitmap; 
                                    }
                                }
                                else
                                {
                                    // Use default image
                                    profileImageBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/20180125_001_1_.jpg"));
                                }


                                // Show the data in a MessageBox
                                /*string patientData = $"Full Name: {fullName}\nNIC: {nic}\nGender: {gender}\nAge: {age}\nAddress: {address}\nBlood Type: {bloodType}\nStatus: {status}";
                                MessageBox.Show(patientData, "Patient Details", MessageBoxButton.OK, MessageBoxImage.Information);#1#
                            }
                            else
                            {
                                MessageBox.Show("Patient not found.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching patient data: " + ex.Message);
            }*/
        }

        
        // when admit update bed and disease
        private void AdmitPatient_Click(object sender, RoutedEventArgs e)
        {
            string bedNo = txtBedNo.Text.Trim();
            string disease = txtDisease.Text.Trim();
            DateTime checkInDate = DateTime.Now;
            string status = "Admitted";


            dbHelper AdmitPatientByFindNic = new dbHelper();
            AdmitPatientByFindNic.AdmitPatientByFindNic(nicValue,bedNo,disease,checkInDate,status);

            

            /*string connectionString = "server=localhost;database=hsm;uid=root;pwd=;";
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
                        cmd.Parameters.AddWithValue("@NIC", nicValue);  // This should be passed from constructor

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Patient admitted successfully!");
                        }
                        else
                        {
                            MessageBox.Show("Failed to admit patient. Check NIC.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while admitting patient: " + ex.Message);
            }*/
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }
        
    }
}
