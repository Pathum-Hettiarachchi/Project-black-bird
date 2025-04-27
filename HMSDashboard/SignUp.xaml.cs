using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HMSDashboard
{
    /// <summary>
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : Window
    {
        public SignUp()
        {
            InitializeComponent();
        }


        private bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedRole = ((ComboBoxItem)role.SelectedItem)?.Content.ToString();
            string emailValue = email.Text;
            string passwordValue = password.Password;
            string rePasswordValue = repassword.Password;

            if (!IsValidEmail(emailValue))
            {
                MessageBox.Show("Invalid email format.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (passwordValue.Length < 8)
            {
                MessageBox.Show("Password must be at least 8 characters long.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (passwordValue != rePasswordValue)
            {
                MessageBox.Show("Passwords do not match.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DatabaseHelper db = new DatabaseHelper();
            string hashedPassword = PasswordHelper.HashPassword(passwordValue);
            db.InsertUser(emailValue, selectedRole, hashedPassword);
           // MessageBox.Show("Registration Successful");
            new MainWindow().Show();
            this.Close();

        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            SignIn signInWindow = new SignIn();
            signInWindow.Show();
            this.Close();
        }




    }
}
