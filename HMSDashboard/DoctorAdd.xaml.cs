using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace HMSDashboard
{
    public partial class DoctorAdd : Window
    {
        string fullName;
        string nic;
        string gender;
        string age;
        string phone;
        string specialization;
        string qualification;
        byte[] profilePhoto;

        public DoctorAdd()
        {
            InitializeComponent();
        }

        private void Dashbord_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void PatientClick(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            fullName = FullNameTextBox.Text;
            nic = NICTextBox.Text;
            gender = GenderComboBox.Text;
            age = AgeTextBox.Text;
            phone = PhoneTextBox.Text;
            specialization = SpecializationTextBox.Text;
            qualification = QualificationTextBox.Text;

            // === Validations ===
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

            Regex nicRegex = new Regex(@"^(\d{12}|\d{9}[vV])$");
            if (!nicRegex.IsMatch(nic))
            {
                MessageBox.Show("NIC format is invalid. Enter either 12 digits or 9 digits followed by 'V'.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // TODO: Save to database (you can implement CreateDoctor in dbHelper)
            dbHelper db = new dbHelper();
            db.CreateDoctor(fullName, nic, gender, age, phone, specialization, qualification, profilePhoto);

            //MessageBox.Show("Doctor saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            ClearFormFields();
        }

        private void AddProfileImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(openFileDialog.FileName, UriKind.Absolute);
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                ProfileImage.Source = bitmapImage;
                profilePhoto = File.ReadAllBytes(openFileDialog.FileName);
            }
        }

        private void RemoveProfileImage(object sender, RoutedEventArgs e)
        {
            string imagePath = "pack://application:,,,/20180125_001_1_.jpg"; // default fallback image
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();

            ProfileImage.Source = bitmap;
            profilePhoto = null;
        }

        private void ClearFormFields()
        {
            FullNameTextBox.Text = "";
            NICTextBox.Text = "";
            GenderComboBox.Text = "";
            AgeTextBox.Text = "";
            PhoneTextBox.Text = "";
            SpecializationTextBox.Text = "";
            QualificationTextBox.Text = "";

            RemoveProfileImage(null, null);
        }
    }
}
