using System.Windows;


namespace HMSDashboard
{
    /// <summary>
    /// Interaction logic for DischargePatient.xaml
    /// </summary>
    public partial class DischargePatient : Window
    {

        private string nicValue;
        public DischargePatient(string nic)
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
            var patientDataResult = getPatientData.GetPatientDataByNic(nic);

            if (patientDataResult.found)
            {
                txtFullName.Text = patientDataResult.fullName;
                txtNIC.Text = "NIC: " + nic;
                txtGender.Text = patientDataResult.gender;
                txtAge.Text = patientDataResult.age;
                txtAddress.Text = patientDataResult.address;
                txtBloodType.Text = patientDataResult.bloodType;
                txtStatus.Text = "Status: " + patientDataResult.status;
                txtDisease.Text = "Disease: " + patientDataResult.Disease;
                profileImageBrush.ImageSource = patientDataResult.profileImage;
                txtCheckedIn.Text = "Admitted Date: " + patientDataResult.checkInDate.ToString("yyyy-MM-dd");

            }
            else
            {
                MessageBox.Show("Patient not found.");
            }

        }
        

        private void Close_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }

        private void DischargePatient_Click(object sender, RoutedEventArgs e)
        {
            //string TxtDischargeDiscription = txtDischargeDiscription.Text;
            DateTime checkInDate = DateTime.Now;
            string status = "Discharged ";


            dbHelper DischargePatientByFindNic = new dbHelper();
            DischargePatientByFindNic.DischargePatientByNic(nicValue,checkInDate,status);
            this.Close();

        }
    }

}
