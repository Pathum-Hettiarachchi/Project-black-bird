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

        private void PatientListPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isDragging = true;
            clickPosition = e.GetPosition(MainCanvas);
            PatientListPanel.CaptureMouse();
        }

        private void PatientListPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                var currentPosition = e.GetPosition(MainCanvas);
                double deltaX = currentPosition.X - clickPosition.X;
                double deltaY = currentPosition.Y - clickPosition.Y;
                double newLeft = Canvas.GetLeft(PatientListPanel) + deltaX;
                double newTop = Canvas.GetTop(PatientListPanel) + deltaY;
                Canvas.SetLeft(PatientListPanel, newLeft);
                Canvas.SetTop(PatientListPanel, newTop);
                clickPosition = currentPosition;
            }
        }

        private void PatientListPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            PatientListPanel.ReleaseMouseCapture();
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
        
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dbHelper db = new dbHelper();
            RecentPatientsDataGrid.ItemsSource = db.GetRecentPatients().DefaultView;
        }

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
    }

   

}