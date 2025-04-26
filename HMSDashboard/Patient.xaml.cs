using System.Windows;
using MySql.Data.MySqlClient;

namespace HMSDashboard
{
    /// <summary>
    /// Interaction logic for Patient.xaml
    /// </summary>
    public partial class Patient : Window
    {
        public Patient()
        {
            InitializeComponent();
        }




        //On_Loaded funcrion
        private void On_Loaded(object sender, RoutedEventArgs e)
        {

            dbHelper db = new dbHelper();
            RecentPatientsDataGrid.ItemsSource = db.GetRecentPatients().DefaultView;


        }



        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();

        }

        private void findPatient(object sender, RoutedEventArgs e)
        {
            dbHelper findStatus = new dbHelper();

            string nic = searchBox.Text.Trim();
            string status = findStatus.GetPatientStatusByNIC(nic);

            if (status == "Discharged")
            {
                PatientAdmit admit = new PatientAdmit(nic);
                admit.Show();
            }
            else if (status == "Admitted")
            {
                DischargePatient discharge = new DischargePatient(nic);
                discharge.Show();
            }

           

            
        }


        private void newPatient(object sender, RoutedEventArgs e)
        {

            Patient_add patientadd = new Patient_add();
            patientadd.Show();
            this.Close();

        }

        private void Doctor_Click(object sender, RoutedEventArgs e)
        {
            DoctorAdd doctorAdd = new DoctorAdd();
            doctorAdd.Show();
            this.Close();
        }

        private void Appointment_Click(object sender, RoutedEventArgs e)
        {
            Appointments appointment = new Appointments();
            appointment.Show();
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
