using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace HMSDashboard
{
    public partial class Loading : Window
    {
        private DispatcherTimer letterTimer;
        private DispatcherTimer animationEndTimer;
        private string fullText = "Wellness";
        private int currentLetterIndex = 0;

        private readonly string[] letterColors = {
            "#e74c3c", "#f39c12", "#f1c40f", "#2ecc71",
            "#3498db", "#9b59b6", "#1abc9c", "#e67e22"
        };

        public Loading()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Start quicker (0.2s delay)
            DispatcherTimer startDelay = new DispatcherTimer();
            startDelay.Interval = TimeSpan.FromMilliseconds(200);
            startDelay.Tick += (s, args) =>
            {
                startDelay.Stop();
                StartLetterBlink();
            };
            startDelay.Start();
        }

        private void StartLetterBlink()
        {
            currentLetterIndex = 0;
            LetterStackPanel.Children.Clear();

            letterTimer = new DispatcherTimer();
            letterTimer.Interval = TimeSpan.FromMilliseconds(200); // faster per letter
            letterTimer.Tick += ShowNextLetter;
            letterTimer.Start();
        }

        private void ShowNextLetter(object sender, EventArgs e)
        {
            if (currentLetterIndex < fullText.Length)
            {
                char letter = fullText[currentLetterIndex];

                TextBlock letterText = new TextBlock
                {
                    Text = letter.ToString(),
                    FontSize = 48,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(letterColors[currentLetterIndex])),
                    Margin = new Thickness(5, 0, 5, 0),
                    RenderTransform = new TranslateTransform()
                };

                var bounce = new DoubleAnimation
                {
                    From = -5,
                    To = 5,
                    Duration = TimeSpan.FromMilliseconds(300),
                    AutoReverse = true,
                    RepeatBehavior = RepeatBehavior.Forever
                };

                (letterText.RenderTransform as TranslateTransform)?.BeginAnimation(TranslateTransform.YProperty, bounce);

                LetterStackPanel.Children.Add(letterText);
                currentLetterIndex++;
            }
            else
            {
                letterTimer.Stop();

                // Let animation run for 2.5s then stop and open Main
                animationEndTimer = new DispatcherTimer();
                animationEndTimer.Interval = TimeSpan.FromMilliseconds(2500);
                animationEndTimer.Tick += EndAnimationAndOpenMain;
                animationEndTimer.Start();
            }
        }

        private void EndAnimationAndOpenMain(object sender, EventArgs e)
        {
            animationEndTimer.Stop();

            // Stop animations
            foreach (TextBlock tb in LetterStackPanel.Children)
            {
                tb.RenderTransform.BeginAnimation(TranslateTransform.YProperty, null);
            }

            // Replace with single final static word
            LetterStackPanel.Children.Clear();
            LetterStackPanel.Children.Add(new TextBlock
            {
                Text = fullText,
                FontSize = 48,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBED3FF")),
                HorizontalAlignment = HorizontalAlignment.Center
            });

            // Open MainWindow after 1s
            DispatcherTimer openMainTimer = new DispatcherTimer();
            openMainTimer.Interval = TimeSpan.FromMilliseconds(1000);
            openMainTimer.Tick += (s2, e2) =>
            {
                openMainTimer.Stop();
                new SignIn().Show();
                this.Close();
            };
            openMainTimer.Start();
        }
    }
}
