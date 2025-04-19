using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace ProjectBlackBird
{
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
            /*MessageBox.Show($"Role: {selectedRole}\nEmail: {emailValue}\nPassword: {passwordValue}", "Registration Successful", MessageBoxButton.OK, MessageBoxImage.Information);*/
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            SignIn signInWindow = new SignIn();
            signInWindow.Show();
            this.Close();
        }
        
        
        
        
        
        
        
        
    }
}