using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestingUI3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int number = 0;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShowFirstUI_Click(object sender, RoutedEventArgs e)
        {
            WelcomeText.Text = "Welcome to WPF UI";
            MainButton.Content = "Click me!";

            // Show Close Right elements and hide Open Right elements
            CloseRightLabel.Visibility = Visibility.Visible;
            CloseRightText.Visibility = Visibility.Visible;
            OpenRightLabel.Visibility = Visibility.Collapsed;
            OpenRightText.Visibility = Visibility.Collapsed;
        }

        private void ShowSecondUI_Click(object sender, RoutedEventArgs e)
        {
            WelcomeText.Text = "Welcome to WPF UI - SECONDUI";
            MainButton.Content = "Click me! SECONDUI";

            // Hide Close Right elements and show Open Right elements
            CloseRightLabel.Visibility = Visibility.Collapsed;
            CloseRightText.Visibility = Visibility.Collapsed;
            OpenRightLabel.Visibility = Visibility.Visible;
            OpenRightText.Visibility = Visibility.Visible;
        }

        // Toggle event handlers
        private void StatusToggle_Checked(object sender, RoutedEventArgs e)
        {
            StatusText.Text = "True";
        }

        private void StatusToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            StatusText.Text = "False";
        }

        private void ShotWhileMovingToggle_Checked(object sender, RoutedEventArgs e)
        {
            ShotWhileMovingText.Text = "True";
        }

        private void ShotWhileMovingToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            ShotWhileMovingText.Text = "False";
        }

        // View switching methods
        private void ShowUserInfoView_Click(object sender, RoutedEventArgs e)
        {
            HideAllViews();
            UserInfoView.Visibility = Visibility.Visible;
        }

        private void ShowContactUsView_Click(object sender, RoutedEventArgs e)
        {
            HideAllViews();
            ContactUsView.Visibility = Visibility.Visible;
        }

        private void ShowSettingsView_Click(object sender, RoutedEventArgs e)
        {
            HideAllViews();
            SettingsView.Visibility = Visibility.Visible;
        }

        private void ShowMacroSettingsView_Click(object sender, RoutedEventArgs e)
        {
            HideAllViews();
            MacroSettingsView.Visibility = Visibility.Visible;
        }

        private void ShowGlobalSettingsView_Click(object sender, RoutedEventArgs e)
        {
            HideAllViews();
            GlobalSettingsView.Visibility = Visibility.Visible;
        }
        private void DetectKeyPress(object sender, KeyEventArgs e)
        {
            // Get the pressed key name
            string keyName = e.Key.ToString();

            // Update the displayed key text
            SelectedKeyText.Text = keyName;

            // Close the popup
            KeyDetectionPopup.IsOpen = false;

            // Remove the temporary event handlers
            this.PreviewKeyDown -= DetectKeyPress;
            this.PreviewMouseDown -= DetectMousePress;

            // Mark the event as handled
            e.Handled = true;
        }

        private void DetectMousePress(object sender, MouseButtonEventArgs e)
        {
            // Get the pressed mouse button name
            string buttonName = e.ChangedButton.ToString();

            // Update the displayed key text
            SelectedKeyText.Text = buttonName;

            // Close the popup
            KeyDetectionPopup.IsOpen = false;

            // Remove the temporary event handlers
            this.PreviewKeyDown -= DetectKeyPress;
            this.PreviewMouseDown -= DetectMousePress;

            // Mark the event as handled
            e.Handled = true;
        }

        // Call this method to start detecting input
        private void StartKeyOrMouseDetection(object sender, RoutedEventArgs e)
        {
            KeyDetectionPopup.IsOpen = true;

            this.PreviewKeyDown += DetectKeyPress;
            this.PreviewMouseDown += DetectMousePress;
        }

        // Method to handle pixel range slider value change
        private void PixelRangeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Update the pixel range value text
            PixelRangeValue.Text = ((int)e.NewValue).ToString();
        }
        private void HideAllViews()
        {
            UserInfoView.Visibility = Visibility.Collapsed;
            ContactUsView.Visibility = Visibility.Collapsed;
            SettingsView.Visibility = Visibility.Collapsed;
            MacroSettingsView.Visibility = Visibility.Collapsed;
            GlobalSettingsView.Visibility = Visibility.Collapsed;
            DefaultView.Visibility = Visibility.Collapsed;
        }
        private void CloseRightToggle_Click(object sender, RoutedEventArgs e)
        {
            number++;
            if (number % 2 == 0)
            {
                CloseRightText.Text = "False";
            }
            else
            {
                CloseRightText.Text = "True";
            }
        }
    }
}
