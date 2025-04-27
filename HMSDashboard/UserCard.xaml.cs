using System.Windows;
using System.Windows.Controls;

namespace HMSDashboard
{
    public partial class UserCard : UserControl
    {
        private string Email = "Pathum H";  // Your private email

        public string UserName
        {
            get { return (string)GetValue(UserNameProperty); }
            set { SetValue(UserNameProperty, value); }
        }

        public static readonly DependencyProperty UserNameProperty =
            DependencyProperty.Register(
                "UserName",
                typeof(string),
                typeof(UserCard),
                new PropertyMetadata(string.Empty)
            );

        public UserCard()
        {
            InitializeComponent();
            this.DataContext = this;

            // Assign private Email to the UserName property
            this.UserName = Email;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SignIn signIn = new SignIn();
            signIn.Show();
            Window.GetWindow(this).Close();
            
        }

    }
}
