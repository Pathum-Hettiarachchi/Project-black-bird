using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
namespace HMSDashboard
{

    /// <summary>
    /// Interaction logic for Patient_add.xaml
    /// </summary>
    public partial class Patient_add : Window
    {
        string fullName;
        string nic;
        DateTime? checkInDate = DateTime.Now;
        string disease;
        string bloodType;
        string bedNo;
        string gender;
        string age;
        string address;
        string city;
        byte[] profilePhoto;
        
        
        public Patient_add()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            
            string formattedDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");



            
            fullName = FullNameTextBox.Text;
            nic = NICTextBox.Text;
            disease = DiseaseTextBox.Text;
            bloodType = BloodTypeTextBox.Text;
            bedNo = BedNoTextBox.Text;
            gender = GenderTextBox.Text;
            age = AgeTextBox.Text;
            address = AddressTextBox.Text;
            city = CityTextBox.Text;
            
            // === Required Field Validations ===
            if (string.IsNullOrWhiteSpace(fullName))
            {
                MessageBox.Show("Full Name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(nic))
            {
                MessageBox.Show("NIC is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Regex for 12-digit NIC or 9-digit + 'v'/'V'
            Regex nicRegex = new Regex(@"^(\d{12}|\d{9}[vV])$");
            if (!nicRegex.IsMatch(nic))
            {
                MessageBox.Show("NIC format is invalid. Enter either 12 digits or 9 digits followed by 'V'.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            

            if (string.IsNullOrWhiteSpace(disease))
            {
                MessageBox.Show("Disease is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            
            // Call the function

            dbHelper createPatient = new dbHelper();
            createPatient.CreatePatient(fullName, nic, checkInDate, disease, bloodType, bedNo, gender, age, address, city,profilePhoto);
        }
        
        
        
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            
            string imagePath = "/20180125_001_1_.jpg"; // or the full path if needed
            BitmapImage bitmap = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));

            FullNameTextBox.Text = "";
                NICTextBox.Text = "";
                DiseaseTextBox.Text = "";
                BloodTypeTextBox.Text = "";
                BedNoTextBox.Text = "";
                GenderTextBox.Text = "";
                AgeTextBox.Text = "";
                AddressTextBox.Text = "";
                CityTextBox.Text = "";
                ProfileImage.Source = bitmap;




        }
        
        private void AddProfileImage(object sender, RoutedEventArgs e)
        {
            // Create an OpenFileDialog to choose a profile image
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif" // Filter for image files
            };

            // Show the dialog and check if the user selected a file
            if (openFileDialog.ShowDialog() == true)
            {
                // Load the selected image file into the ProfileImage control (Image element in XAML)
                Uri imageUri = new Uri(openFileDialog.FileName);
                BitmapImage bitmapImage = new BitmapImage(imageUri);

                ProfileImage.Source = bitmapImage; // Set the selected image to ProfileImage control
                profilePhoto = File.ReadAllBytes(openFileDialog.FileName);
                
            }
        }
        
        private void RemoveProfileImage(object sender, RoutedEventArgs e)
        {
            string imagePath = "/20180125_001_1_.jpg"; // or the full path if needed
            BitmapImage bitmap = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
            ProfileImage.Source = bitmap;
        }
        
    }
    }

