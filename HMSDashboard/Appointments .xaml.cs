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
    /// Interaction logic for Appointments.xaml
    /// </summary>
    public partial class Appointments : Window
    {
        public Appointments()
        {
            InitializeComponent();
        }



        private void MainWindow_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();

        }

      


        private void newPatient(object sender, RoutedEventArgs e)
        {

        

        }

        private void findAppointment(object sender, RoutedEventArgs e)
{
    // 1. Get values from input controls
    string docName = DocNameTextBox.Text.Trim();
    DateTime? selectedDate = AppointmentDatePicker.SelectedDate;

    if (string.IsNullOrEmpty(docName))
    {
        MessageBox.Show("Please enter Doctor Name.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
    }
    else
    {
        if (selectedDate == null)
        {
            MessageBox.Show("Please select a date.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        else
        {
            if (selectedDate.Value.Date < DateTime.Today)
            {
                MessageBox.Show("Please select a valid date (today or future).", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }
    }

    
    try
    {
        string doctorNIC = string.Empty;

        // 2. Connect to DB and get NIC of the doctor by name
        using (MySqlConnection conn = new MySqlConnection("server=localhost;database=hsm;uid=root;pwd=;"))
        {
            conn.Open();

            // Get Doctor NIC
            string getNICQuery = "SELECT NIC FROM doctors WHERE FullName = @DocName";
            using (MySqlCommand cmd = new MySqlCommand(getNICQuery, conn))
            {
                cmd.Parameters.AddWithValue("@DocName", docName);

                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    doctorNIC = result.ToString();
                    

                }
                else
                {
                    MessageBox.Show("Doctor not found!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            // 3. Count appointments for the doctor on the selected date
            string countAppointmentsQuery = "SELECT COUNT(*) FROM appointments WHERE DocNIC = @DocNIC AND AppointmentDate = @AppDate";
            using (MySqlCommand cmd = new MySqlCommand(countAppointmentsQuery, conn))
            {
                cmd.Parameters.AddWithValue("@DocNIC", doctorNIC);
                cmd.Parameters.AddWithValue("@AppDate", selectedDate.Value.Date);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                int appointmentNumber = count + 1;
                DateTime appointmentDate = selectedDate.Value.Date;


                // 4. Show result
                //MessageBox.Show($"Dr. {docName} has {count} appointments on {selectedDate.Value.ToShortDateString()}.", "Appointment Count", MessageBoxButton.OK, MessageBoxImage.Information);
                
                PlaceAppointment placeAppointment = new PlaceAppointment(appointmentNumber, docName, doctorNIC,appointmentDate);
                placeAppointment.Show();
            }

            conn.Close();
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show("Connection Error: " + ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}

        private void Patient_Click(object sender, RoutedEventArgs e)
        {
            Patient patient = new Patient();
            patient.Show();
            this.Close();
        }

        private void Doctor_Click(object sender, RoutedEventArgs e)
        {
            DoctorAdd doctorAdd = new DoctorAdd();
            doctorAdd.Show();
            this.Close();
        }

        private void Payement_Click(object sender, RoutedEventArgs e)
        {
            Payment payement = new Payment();
            payement.Show();
            this.Close();
        }
    }
}
