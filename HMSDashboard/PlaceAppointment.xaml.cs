using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace HMSDashboard
{
    /// <summary>
    /// Interaction logic for PlaceAppointment.xaml
    /// </summary>
    public partial class PlaceAppointment : Window
    {
        private string docName;
        private string docNic;
        private DateTime appointmentDate;
        private string appointmentNumber;
        public PlaceAppointment(int InputappointmentNumber, string InputdocName, string InputdocNIC, DateTime InputappointmentDate)
        {
         docName = InputdocName;
         docNic = InputdocNIC;
            InitializeComponent();
            DoctorNameTextBox.Text =docName ;
            appointmentDate = InputappointmentDate;
            appointmentNumber = InputappointmentNumber.ToString();
            AppointmentNumberTextBlock.Text = appointmentNumber;
            AppointmentDate.Text = appointmentDate.ToString("yyyy-MM-dd");


        }

       private void appointmentSaveToDb(object sender, RoutedEventArgs e)
{
    string patientNIC = PatientNICTextBox.Text.Trim();
    TimeSpan appointmentTime = new TimeSpan(16, 0, 0); // 4:00 PM fixed time
    string status = "Scheduled";
    string paymentStatus = "Uncomplete";

    if (string.IsNullOrEmpty(patientNIC))
    {
        MessageBox.Show("Please enter a valid Patient NIC.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
    }

    try
    {
        using (MySqlConnection conn = new MySqlConnection("server=localhost;database=hsm;uid=root;pwd=;")) // replace with your connection
        {
            conn.Open();

            // Get Patient details
            string queryPatient = "SELECT FullName, MobileNumber FROM patients WHERE NIC = @nic";
            MySqlCommand cmdPatient = new MySqlCommand(queryPatient, conn);
            cmdPatient.Parameters.AddWithValue("@nic", patientNIC);

            string patientName = "";
            string mobileNumber = "";

            using (MySqlDataReader reader = cmdPatient.ExecuteReader())
            {
                if (reader.Read())
                {
                    patientName = reader["FullName"].ToString();
                    mobileNumber = reader["MobileNumber"].ToString();
                }
                else
                {
                    MessageBox.Show("Patient not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            // Insert appointment
            string insertQuery = @"INSERT INTO appointments 
                (AppointmentNo,AppointmentDate, AppointmentTime, DocNIC, DocName, PatientNIC, PatientName, MobileNumber, Status, PaymentStatus) 
                VALUES (@No,@date, @time, @docNIC, @docName, @patientNIC, @patientName, @mobileNumber, @status, @paymentStatus)";

            MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn);
            insertCmd.Parameters.AddWithValue("@No", appointmentNumber);
            insertCmd.Parameters.AddWithValue("@date", appointmentDate);
            insertCmd.Parameters.AddWithValue("@time", appointmentTime);
            insertCmd.Parameters.AddWithValue("@docNIC", docNic);
            insertCmd.Parameters.AddWithValue("@docName", docName);
            insertCmd.Parameters.AddWithValue("@patientNIC", patientNIC);
            insertCmd.Parameters.AddWithValue("@patientName", patientName);
            insertCmd.Parameters.AddWithValue("@mobileNumber", mobileNumber);
            insertCmd.Parameters.AddWithValue("@status", status);
            insertCmd.Parameters.AddWithValue("@paymentStatus", paymentStatus);

            int rowsAffected = insertCmd.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                MessageBox.Show("Appointment saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Failed to save appointment.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.Close();
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}

    }
}
