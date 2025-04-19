using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace ProjectBlackBird
{
    
    public partial class SignIn : Window
    {
        public SignIn()
        {
            InitializeComponent();
        }

        private bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }

        // This is triggered when the "Login" button is clicked
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedRole = ((ComboBoxItem)role.SelectedItem)?.Content.ToString();
            string emailValue = email.Text;
            string passwordValue = password.Password;

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

            // If everything is valid, show the entered details
           // MessageBox.Show($"Role: {selectedRole}\nEmail: {emailValue}\nPassword: {passwordValue}", "Login Successful", MessageBoxButton.OK, MessageBoxImage.Information);
           
           
           // Login User

           DatabaseHelper login = new DatabaseHelper();
           if ((login.ValidateLogin(emailValue, passwordValue)== true))
           {
               MessageBox.Show("Login successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
               // implement after login function
           }
           







        }

        // This is triggered when the "Sign Up" hyperlink is clicked
        private void SignUpHyperlink_Click(object sender, RoutedEventArgs e)
        {
            SignUp signUpWindow = new SignUp();
            signUpWindow.Show();
            this.Close();
        }
    }
    
    

    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}