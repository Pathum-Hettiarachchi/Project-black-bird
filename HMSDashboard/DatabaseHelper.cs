using System;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text;
using HMSDashboard;

public static class PasswordHelper
{
    public static string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = sha256.ComputeHash(bytes);

            // Convert byte array to hex string
            StringBuilder builder = new StringBuilder();
            foreach (byte b in hash)
            {
                builder.Append(b.ToString("x2")); // lowercase hex
            }
            return builder.ToString();
        }
    }
}



public class DatabaseHelper
{
    private string connectionString = "server=localhost;database=hsm;uid=root;pwd=;";


    // Register User



    public bool InsertUser(string email, string role, string password)
    {
        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                // Check if user already exists
                string checkQuery = "SELECT COUNT(*) FROM users WHERE email = @Email";
                MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@Email", email);

                int userExists = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (userExists > 0)
                {
                    MessageBox.Show("User already exists.", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false; // ? Stop and return false
                }
                else
                {
                    // Insert new user
                    string insertQuery = "INSERT INTO users (email, role, password) VALUES (@Email, @Role, @Password)";
                    MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@Email", email);
                    insertCmd.Parameters.AddWithValue("@Role", role);
                    insertCmd.Parameters.AddWithValue("@Password", password);

                    int rowsAffected = insertCmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("User registered successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        return true; // ? Success
                    }
                    else
                    {
                        MessageBox.Show("User registration failed.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false; // ? Failed
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return false; // ? Error occurred
        }
    }

    // login user handler



    public bool ValidateLogin(string email, string enteredPassword)
    {
        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT password FROM users WHERE email = @Email";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);

                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    string storedHashedPassword = result.ToString();
                    string enteredHashedPassword = PasswordHelper.HashPassword(enteredPassword);

                    if (storedHashedPassword == enteredHashedPassword)
                    {
                       
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Invalid password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("User not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Login failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }
    }
    

}

