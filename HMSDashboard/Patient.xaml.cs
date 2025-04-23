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



    }
}
