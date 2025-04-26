using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace HMSDashboard
{
    public partial class MainWindow : Window
    {
        private bool isDragging = false;
        private Point clickPosition;

        public MainWindow()
        {
            InitializeComponent();
        }

        
        // payemet button click
        private void paymentClick(object sender, RoutedEventArgs e)
        {

            Payment payment = new Payment();
            payment.Show();
            this.Close();

        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
/*         Patient_add patientAddWindow = new Patient_add();
            patientAddWindow.Show();
            this.Close();*/

            Patient patientDashboard = new Patient();
            patientDashboard.Show();
            this.Close();


         
        }
        
        
      /*  private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dbHelper db = new dbHelper();
            RecentPatientsDataGrid.ItemsSource = db.GetRecentPatients().DefaultView;
        }*/

        private void DoctorClick(object sender, RoutedEventArgs e)
        {

            DoctorAdd addDoctor = new DoctorAdd();
            addDoctor.Show();
            this.Close();

        }

        private void AppintmentClick(object sender, RoutedEventArgs e)
        {
            Appointments Appointments =new Appointments();
            Appointments.Show();
            this.Close();

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Loading loadin =new Loading();
            loadin.Show();
           
        }





        //On_Loaded funcrion
        private void On_Loaded(object sender, RoutedEventArgs e)
        {
          
            dbHelper getCount = new dbHelper();
            appointmentCount.Text = $"0 {getCount.GetCountFromQuery("SELECT COUNT(*) FROM appointments WHERE appointmentDate >= CURDATE()")}";
            docCount.Text = $"0 {getCount.GetCountFromQuery("SELECT COUNT(*) FROM doctors")}";
            AdmittedPatientCount.Text = $"0 {getCount.GetCountFromQuery("SELECT COUNT(*) FROM patients WHERE Status = 'Admitted'")}";



            dbHelper db = new dbHelper();
            RecentPatientsDataGrid.ItemsSource = db.GetRecentPatients().DefaultView;


        }


    }



}