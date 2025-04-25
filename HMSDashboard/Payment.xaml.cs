using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MySql.Data.MySqlClient;

namespace HMSDashboard
{
    public partial class Payment : Window
    {
        public Payment()
        {
            InitializeComponent();
            // Set default date to today
            AppointmentDatePicker.SelectedDate = DateTime.Today;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void newPatient(object sender, RoutedEventArgs e)
        {
            // Handle new patient logic here
        }

        private void findAppointment(object sender, RoutedEventArgs e)
        {
            string docName = DocNameTextBox.Text.Trim();
            DateTime? selectedDate = AppointmentDatePicker.SelectedDate;
            string AppointmentNo = AppointmentNoTextBox.Text.Trim();
            AppointmentsContainer.Children.Clear();


            try
            {
                string doctorNIC = string.Empty;
                List<Appointment> appointments = new List<Appointment>();

                using (MySqlConnection conn = new MySqlConnection("server=localhost;database=hsm;uid=root;pwd=;"))
                {
                    conn.Open();

                    // Get Doctor NIC
                    string getNICQuery = "SELECT NIC FROM doctors WHERE FullName = @DocName";
                    using (MySqlCommand cmd = new MySqlCommand(getNICQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@DocName", docName);

                        var result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            doctorNIC = result.ToString();
                        }
                        else
                        {
                            MessageBox.Show("Doctor not found!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }

                    // Fetch appointments
                    string fetchAppointmentsQuery;

                    if ((string.IsNullOrWhiteSpace(AppointmentNo)))
                    {
                        fetchAppointmentsQuery = @"SELECT 
                                    DocName,
                                    PatientName,
                                    MobileNumber,
                                    AppointmentDate,
                                    AppointmentNo,
                                    PaymentStatus
                                FROM appointments
                                WHERE (DocNIC = @DocNIC AND AppointmentDate = @AppDate AND AppointmentNo = @AppointmentNo)
                                   OR (DocNIC = @DocNIC AND AppointmentDate = @AppDate)";
                    }
                    else
                    {
                        fetchAppointmentsQuery = @"SELECT 
                                    DocName,
                                    PatientName,
                                    MobileNumber,
                                    AppointmentDate,
                                    AppointmentNo,
                                    PaymentStatus
                                FROM appointments
                                WHERE DocNIC = @DocNIC AND AppointmentDate = @AppDate AND AppointmentNo = @AppointmentNo ";
                    }


                    using (MySqlCommand cmd = new MySqlCommand(fetchAppointmentsQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@DocNIC", doctorNIC);
                        cmd.Parameters.AddWithValue("@AppDate", selectedDate.Value.Date);
                        cmd.Parameters.AddWithValue("@AppointmentNo", AppointmentNo);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                MessageBox.Show("No appointments found for this doctor on the selected date.");
                                return;
                            }

                            while (reader.Read())
                            {
                                var appointment = new Appointment
                                {
                                    DoctorName = reader["DocName"].ToString(),
                                    PatientName = reader["PatientName"].ToString(),
                                    Mobile = reader["MobileNumber"].ToString(),
                                    AppointmentDate = Convert.ToDateTime(reader["AppointmentDate"]),
                                    AppointmentNo = reader["AppointmentNo"].ToString(),
                                    PaymentStatus = reader["PaymentStatus"].ToString()
                                };

                                appointments.Add(appointment);
                            }
                        }
                    }

                    conn.Close();
                }

                foreach (var appointment in appointments)
                {
                    // StackPanel with margin (internal layout)
                    StackPanel appointmentPanel = new StackPanel();

                    appointmentPanel.Children.Add(new TextBlock
                    {
                        Text = "Doctor: " + appointment.DoctorName,
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(0, 0, 0, 5)
                    });
                    appointmentPanel.Children.Add(new TextBlock { Text = "Patient: " + appointment.PatientName, Margin = new Thickness(0, 0, 0, 2) });
                    appointmentPanel.Children.Add(new TextBlock { Text = "Mobile: " + appointment.Mobile, Margin = new Thickness(0, 0, 0, 2) });
                    appointmentPanel.Children.Add(new TextBlock { Text = "Date: " + appointment.AppointmentDate.ToShortDateString(), Margin = new Thickness(0, 0, 0, 2) });
                    appointmentPanel.Children.Add(new TextBlock { Text = "Appointment No: " + appointment.AppointmentNo, Margin = new Thickness(0, 0, 0, 5) });
                    appointmentPanel.Children.Add(new TextBlock { Text = "Payment Status: " + appointment.PaymentStatus, Tag = "PaymentStatusTextBlock", Margin = new Thickness(0, 0, 0, 5) });

                    // Create and style button
                    Button paymentButton = new Button
                    {
                        Content = appointment.PaymentStatus == "Completed" ? "Payment Complete" : "Pay",
                        Background = appointment.PaymentStatus == "Completed" ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red),
                        Foreground = Brushes.White,
                        Padding = new Thickness(10, 5, 10, 5),
                        Margin = new Thickness(0, 5, 0, 0),
                        Width = 150,
                        Height = 30,
                        BorderThickness = new Thickness(0),
                        IsEnabled = appointment.PaymentStatus != "Completed"
                    };
                    paymentButton.Click += (s, e) =>
                    {
                        try
                        {
                            string connectionString = "server=localhost;database=hsm;uid=root;pwd=;";
                            using (MySqlConnection conn = new MySqlConnection(connectionString))
                            {
                                conn.Open();

                                string query = @"UPDATE appointments 
                             SET PaymentStatus = 'Completed' 
                             WHERE DocName = @DoctorName 
                               AND AppointmentDate = @AppointmentDate 
                               AND AppointmentNo = @AppointmentNo";

                                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                                {
                                    cmd.Parameters.AddWithValue("@DoctorName", appointment.DoctorName);
                                    cmd.Parameters.AddWithValue("@AppointmentDate", appointment.AppointmentDate.Date);
                                    cmd.Parameters.AddWithValue("@AppointmentNo", appointment.AppointmentNo);

                                    int rowsAffected = cmd.ExecuteNonQuery();

                                    if (rowsAffected > 0)
                                    {
                                        MessageBox.Show("Payment status updated to Completed!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                                        // Update UI
                                      //  PaymentStatusTextBlock.Text = "Payment Status: ";
                                        paymentButton.Content = "Payment Complete";
                                        paymentButton.Background = new SolidColorBrush(Colors.Green);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Update failed. No matching appointment found.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    };



                    paymentButton.Style = (Style)Application.Current.Resources["RoundedButtonStyle"];
                    appointmentPanel.Children.Add(paymentButton);

                    // Wrap the StackPanel in a Border to add padding & background
                    Border appointmentBorder = new Border
                    {
                        Background = Brushes.White,
                        CornerRadius = new CornerRadius(8),
                        BorderBrush = new SolidColorBrush(Color.FromRgb(211, 211, 211)), // Light Gray
                        BorderThickness = new Thickness(1),
                        Padding = new Thickness(10),
                        Margin = new Thickness(10),
                        Child = appointmentPanel
                    };

                    AppointmentsContainer.Children.Add(appointmentBorder);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection Error: " + ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    // Appointment class to hold data for each appointment
    public class Appointment
    {
        public string DoctorName { get; set; }
        public string PatientName { get; set; }
        public string Mobile { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string AppointmentNo { get; set; }
        public string PaymentStatus { get; set; }
    }
}
