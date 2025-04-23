using System.Windows;
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
            string nic = searchBox.Text.Trim();
            // PatientAdmit patientAdmit = new PatientAdmit(nic);  // Pass NIC here
            //patientAdmit.Show();

            DischargePatient dischargePatient = new DischargePatient(nic);
            dischargePatient.Show();

        }

        private void newPatient(object sender, RoutedEventArgs e)
        {

            Patient_add patientadd = new Patient_add();
            patientadd.Show();
            this.Close();

        }



    }
}
