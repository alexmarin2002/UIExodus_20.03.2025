using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace TestingUI3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int number = 0;
        private FileSharing fileSharing;

        // Dictionary to map settings to their numerical values (0-30)
        private Dictionary<string, int> settingValues = new Dictionary<string, int>();

        public MainWindow()
        {
            try
            {

                // Initialize component first
                InitializeComponent();
                TextBlock resBlock = this.FindName("ResolutionTextBlock") as TextBlock;
                if (resBlock != null)
                {
                    resBlock.Text = $"{SystemParameters.PrimaryScreenWidth}x{SystemParameters.PrimaryScreenHeight}";
                }
                // Initialize settings dictionary
                InitializeSettingValues();
                // Initialize file sharing - this creates/reads the settings file
                fileSharing = new FileSharing();
                Console.WriteLine($"Settings file located at: {fileSharing.GetSettingsFilePath()}");
                //LAUNCH SETTINGS-------------------------------------------------------------------------------------------------
                if (StatusText != null)
                    StatusText.Text = "False";

                if (fileSharing != null)
                    fileSharing.UpdateSetting(FileSharing.SettingIndex.Status, settingValues["StatusFalse"]);

                //LAUNCH SETTINGS-------------------------------------------------------------------------------------------------


                // Set initial values for controls based on file settings
                SetInitialValues();

                // Set visibility of GlobalSettingsView as the default view
                HideAllViews();
                GlobalSettingsView.Visibility = Visibility.Visible;

                // Set the window title to indicate file sharing mode
                this.Title = "TestingUI3 - File Sharing Mode";


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Critical error during initialization: {ex.Message}");
            }

        }

        private void InitializeSettingValues()
        {
            try
            {
                // Map toggle settings with distinct values
                settingValues["StatusTrue"] = 10;
                settingValues["StatusFalse"] = 0;
                settingValues["ShotWhileMovingTrue"] = 15;
                settingValues["ShotWhileMovingFalse"] = 5;

                // Map shooting method with distinct values
                settingValues["PressButton"] = 12;
                settingValues["HoldButton"] = 24;

                // Map color detection with distinct values
                settingValues["Red"] = 7;
                settingValues["Purple"] = 14;
                settingValues["Yellow"] = 21;

                // Map pixel range values (1-10) to distinct values between 0-30
                settingValues["PixelRange1"] = 1;
                settingValues["PixelRange2"] = 2;
                settingValues["PixelRange3"] = 3;
                settingValues["PixelRange4"] = 4;
                settingValues["PixelRange5"] = 5;
                settingValues["PixelRange6"] = 6;
                settingValues["PixelRange7"] = 7;
                settingValues["PixelRange8"] = 8;
                settingValues["PixelRange9"] = 9;
                settingValues["PixelRange10"] = 10;

                //Map Audio Range Values
                settingValues["AudioRange0"] = 0;
                settingValues["AudioRange1"] = 1;
                settingValues["AudioRange2"] = 2;
                settingValues["AudioRange3"] = 3;
                settingValues["AudioRange4"] = 4;
                settingValues["AudioRange5"] = 5;
                settingValues["AudioRange6"] = 6;
                settingValues["AudioRange7"] = 7;
                settingValues["AudioRange8"] = 8;
                settingValues["AudioRange9"] = 9;
                settingValues["AudioRange10"] = 10;

                // Map shooting delay with distinct values
                settingValues["VANDAL"] = 5;
                settingValues["GUARDIAN"] = 9;
                settingValues["SHERIF"] = 13;
                settingValues["PHANTOM"] = 17;
                settingValues["GHOST"] = 22;
                settingValues["NO DELAY"] = 28;

                // Default weapon delays
                settingValues["DefaultVandal"] = 6;
                settingValues["DefaultGuardian"] = 11;
                settingValues["DefaultSheriff"] = 16;
                settingValues["DefaultPhantom"] = 20;
                settingValues["DefaultGhost"] = 25;
                settingValues["DefaultNoDelay"] = 100;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in InitializeSettingValues: {ex.Message}");
            }
        }

        private void SetInitialValues()
        {
            try
            {
                if (fileSharing == null)
                {
                    Console.WriteLine("FileSharing not initialized, using default values");
                    return;
                }

                // Load settings from file for UI initialization

                // Status toggle
                bool status = fileSharing.ReadSetting(FileSharing.SettingIndex.Status) == 10;
                if (StatusToggle != null)
                {
                    StatusToggle.IsChecked = status;
                    if (StatusText != null)
                        StatusText.Text = status ? "True" : "False";
                }

                // ShotWhileMoving toggle
                bool shotWhileMoving = fileSharing.ReadSetting(FileSharing.SettingIndex.ShotWhileMoving) == 15;
                if (ShotWhileMovingToggle != null)
                {
                    ShotWhileMovingToggle.IsChecked = shotWhileMoving;
                    if (ShotWhileMovingText != null)
                        ShotWhileMovingText.Text = shotWhileMoving ? "True" : "False";
                }

                // Pixel Range slider
                int pixelRangeValue = fileSharing.ReadSetting(FileSharing.SettingIndex.PixelRange);

                PixelRangeSlider.Value = pixelRangeValue;

                if (PixelRangeSlider != null)
                {
                    if (PixelRangeValue != null)
                        PixelRangeValue.Text = pixelRangeValue.ToString();
                }

                // Audio Range slider
                int audioRangeValue = fileSharing.ReadSetting(FileSharing.SettingIndex.AudioRange);

                AudioRangeSlider.Value = audioRangeValue;

                if (AudioRangeSlider != null)
                {
                    if (AudioRangeValue != null)
                        AudioRangeValue.Text = audioRangeValue.ToString()+"0%";
                }

                // Shooting Method radio buttons
                int shootingMethod = fileSharing.ReadSetting(FileSharing.SettingIndex.ShootingMethod);
                if (PressButtonOption != null)
                    PressButtonOption.IsChecked = (shootingMethod == settingValues["PressButton"]);
                if (HoldButtonOption != null)
                    HoldButtonOption.IsChecked = (shootingMethod == settingValues["HoldButton"]);

                // If neither matches, default to Press Button
                if (shootingMethod != settingValues["PressButton"] &&
                    shootingMethod != settingValues["HoldButton"] &&
                    PressButtonOption != null)
                {
                    PressButtonOption.IsChecked = true;
                }

                // Color Detection radio buttons
                int colorDetection = fileSharing.ReadSetting(FileSharing.SettingIndex.ColorDetection);
                if (RedOption != null)
                    RedOption.IsChecked = (colorDetection == settingValues["Red"]);
                if (PurpleOption != null)
                    PurpleOption.IsChecked = (colorDetection == settingValues["Purple"]);
                if (YellowOption != null)
                    YellowOption.IsChecked = (colorDetection == settingValues["Yellow"]);

                // If no match, default to Red
                if (colorDetection != settingValues["Red"] &&
                    colorDetection != settingValues["Purple"] &&
                    colorDetection != settingValues["Yellow"] &&
                    RedOption != null)
                {
                    RedOption.IsChecked = true;
                }

                // Shooting Delay radio buttons
                int shootingDelay = fileSharing.ReadSetting(FileSharing.SettingIndex.ShootingDelay);
                if (VandalOption != null)
                    VandalOption.IsChecked = (shootingDelay == settingValues["VANDAL"]);
                if (GuardianOption != null)
                    GuardianOption.IsChecked = (shootingDelay == settingValues["GUARDIAN"]);
                if (SherifOption != null)
                    SherifOption.IsChecked = (shootingDelay == settingValues["SHERIF"]);
                if (PhantomOption != null)
                    PhantomOption.IsChecked = (shootingDelay == settingValues["PHANTOM"]);
                if (GhostOption != null)
                    GhostOption.IsChecked = (shootingDelay == settingValues["GHOST"]);
                if (NoDelayOption != null)
                    NoDelayOption.IsChecked = (shootingDelay == settingValues["NO DELAY"]);

                // If no match, default to Vandal
                if (shootingDelay != settingValues["VANDAL"] &&
                    shootingDelay != settingValues["GUARDIAN"] &&
                    shootingDelay != settingValues["SHERIF"] &&
                    shootingDelay != settingValues["PHANTOM"] &&
                    shootingDelay != settingValues["GHOST"] &&
                    shootingDelay != settingValues["NO DELAY"] &&
                    VandalOption != null)
                {
                    VandalOption.IsChecked = true;
                }

                // Weapon delay text boxes
                if (VandalDelayTextBox != null)
                    VandalDelayTextBox.Text = fileSharing.ReadSetting(FileSharing.SettingIndex.WeaponDelayVandal).ToString();
                if (GuardianDelayTextBox != null)
                    GuardianDelayTextBox.Text = fileSharing.ReadSetting(FileSharing.SettingIndex.WeaponDelayGuardian).ToString();
                if (SheriffDelayTextBox != null)
                    SheriffDelayTextBox.Text = fileSharing.ReadSetting(FileSharing.SettingIndex.WeaponDelaySheriff).ToString();
                if (PhantomDelayTextBox != null)
                    PhantomDelayTextBox.Text = fileSharing.ReadSetting(FileSharing.SettingIndex.WeaponDelayPhantom).ToString();
                if (GhostDelayTextBox != null)
                    GhostDelayTextBox.Text = fileSharing.ReadSetting(FileSharing.SettingIndex.WeaponDelayGhost).ToString();
                if (NoDelayTextBox != null)
                    NoDelayTextBox.Text = fileSharing.ReadSetting(FileSharing.SettingIndex.WeaponDelayNone).ToString();

                // Key bindings
                int selectedKey = fileSharing.ReadSetting(FileSharing.SettingIndex.SelectedKey);
                int weaponDelayKey = fileSharing.ReadSetting(FileSharing.SettingIndex.WeaponDelayKey);

                if (SelectedKeyText != null)
                {
                    string hexString = selectedKey > 0 ? selectedKey.ToString("X") : "None";
                    char keyChar = selectedKey > 0 ? (char)selectedKey : ' ';
                    SelectedKeyText.Text = $"{keyChar} (0x{hexString})";
                }

                if (SelectedKeyText2 != null)
                {
                    string hexString2 = weaponDelayKey > 0 ? weaponDelayKey.ToString("X") : "None";
                    char keyChar2 = weaponDelayKey > 0 ? (char)weaponDelayKey : ' ';
                    SelectedKeyText2.Text = $"{keyChar2} (0x{hexString2})";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SetInitialValues: {ex.Message}");
            }
        }

        private void ShowFirstUI_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (WelcomeText != null)
                    WelcomeText.Text = "Welcome to WPF UI";
                if (MainButton != null)
                    MainButton.Content = "Click me!";

                // Show Close Right elements and hide Open Right elements
                if (CloseRightLabel != null)
                    CloseRightLabel.Visibility = Visibility.Visible;
                if (CloseRightText != null)
                    CloseRightText.Visibility = Visibility.Visible;
                if (OpenRightLabel != null)
                    OpenRightLabel.Visibility = Visibility.Collapsed;
                if (OpenRightText != null)
                    OpenRightText.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ShowFirstUI_Click: {ex.Message}");
            }
        }

        private void ShowSecondUI_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (WelcomeText != null)
                    WelcomeText.Text = "Welcome to WPF UI - SECONDUI";
                if (MainButton != null)
                    MainButton.Content = "Click me! SECONDUI";

                // Hide Close Right elements and show Open Right elements
                if (CloseRightLabel != null)
                    CloseRightLabel.Visibility = Visibility.Collapsed;
                if (CloseRightText != null)
                    CloseRightText.Visibility = Visibility.Collapsed;
                if (OpenRightLabel != null)
                    OpenRightLabel.Visibility = Visibility.Visible;
                if (OpenRightText != null)
                    OpenRightText.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ShowSecondUI_Click: {ex.Message}");
            }
        }

        // Toggle event handlers with file sharing
        private void StatusToggle_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (StatusText != null)
                    StatusText.Text = "True";

                if (fileSharing != null)
                    fileSharing.UpdateSetting(FileSharing.SettingIndex.Status, settingValues["StatusTrue"]);

                Console.WriteLine("Status changed: True");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in StatusToggle_Checked: {ex.Message}");
            }
        }

        private void StatusToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (StatusText != null)
                    StatusText.Text = "False";

                if (fileSharing != null)
                    fileSharing.UpdateSetting(FileSharing.SettingIndex.Status, settingValues["StatusFalse"]);

                Console.WriteLine("Status changed: False");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in StatusToggle_Unchecked: {ex.Message}");
            }
        }

        private void ShotWhileMovingToggle_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ShotWhileMovingText != null)
                    ShotWhileMovingText.Text = "True";

                if (fileSharing != null)
                    fileSharing.UpdateSetting(FileSharing.SettingIndex.ShotWhileMoving, settingValues["ShotWhileMovingTrue"]);

                Console.WriteLine("Shot While Moving changed: True");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ShotWhileMovingToggle_Checked: {ex.Message}");
            }
        }

        private void ShotWhileMovingToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ShotWhileMovingText != null)
                    ShotWhileMovingText.Text = "False";

                if (fileSharing != null)
                    fileSharing.UpdateSetting(FileSharing.SettingIndex.ShotWhileMoving, settingValues["ShotWhileMovingFalse"]);

                Console.WriteLine("Shot While Moving changed: False");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ShotWhileMovingToggle_Unchecked: {ex.Message}");
            }
        }

        // View switching methods
        private void ShowUserInfoView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HideAllViews();
                if (UserInfoView != null)
                    UserInfoView.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ShowUserInfoView_Click: {ex.Message}");
            }
        }

        private void ShowContactUsView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HideAllViews();
                if (ContactUsView != null)
                    ContactUsView.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ShowContactUsView_Click: {ex.Message}");
            }
        }

        private void ShowSettingsView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HideAllViews();
                if (SettingsView != null)
                    SettingsView.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ShowSettingsView_Click: {ex.Message}");
            }
        }

        private void ShowMacroSettingsView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HideAllViews();
                if (MacroSettingsView != null)
                    MacroSettingsView.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ShowMacroSettingsView_Click: {ex.Message}");
            }
        }

        private void ShowGlobalSettingsView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HideAllViews();
                if (GlobalSettingsView != null)
                    GlobalSettingsView.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ShowGlobalSettingsView_Click: {ex.Message}");
            }
        }

        // Key detection handlers with file sharing
        private void DetectKeyPress(object sender, KeyEventArgs e)
        {
            try
            {
                // Store the current button for reference
                Button sourceButton = KeyDetectionPopup.Tag as Button;
                if (sourceButton == null)
                {
                    if (KeyDetectionPopup != null)
                        KeyDetectionPopup.IsOpen = false;
                    return;
                }

                // Get the pressed key virtual key code in hexadecimal
                int hexValue = KeyInterop.VirtualKeyFromKey(e.Key);

                // Update the displayed key text and save to file
                string keyName = e.Key.ToString();
                string hexString = $"0x{hexValue:X}"; // Convert to hex format

                if (sourceButton == KeyDetectionButton && SelectedKeyText != null)
                {
                    SelectedKeyText.Text = $"{keyName} ({hexString})";
                    if (fileSharing != null)
                        fileSharing.UpdateSetting(FileSharing.SettingIndex.SelectedKey, hexValue);
                }
                else if (sourceButton == KeyDetectionButton2 && SelectedKeyText2 != null)
                {
                    SelectedKeyText2.Text = $"{keyName} ({hexString})";
                    if (fileSharing != null)
                        fileSharing.UpdateSetting(FileSharing.SettingIndex.WeaponDelayKey, hexValue);
                }

                // Close the popup
                if (KeyDetectionPopup != null)
                    KeyDetectionPopup.IsOpen = false;

                // Remove the temporary event handlers
                this.PreviewKeyDown -= DetectKeyPress;
                this.PreviewMouseDown -= DetectMousePress;

                // Mark the event as handled
                e.Handled = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DetectKeyPress: {ex.Message}");
                this.PreviewKeyDown -= DetectKeyPress;
                this.PreviewMouseDown -= DetectMousePress;
            }
        }

        private void DetectMousePress(object sender, MouseButtonEventArgs e)
        {
            try
            {
                // Store the current button for reference
                Button sourceButton = KeyDetectionPopup.Tag as Button;
                if (sourceButton == null)
                {
                    if (KeyDetectionPopup != null)
                        KeyDetectionPopup.IsOpen = false;
                    return;
                }

                // Map mouse buttons to their virtual key codes (VK codes)
                int hexValue = e.ChangedButton switch
                {
                    MouseButton.Left => 0x01,    // Left Mouse Button
                    MouseButton.Right => 0x02,   // Right Mouse Button
                    MouseButton.Middle => 0x04,  // Middle Mouse Button
                    MouseButton.XButton1 => 0x05, // Extra Mouse Button 1
                    MouseButton.XButton2 => 0x06, // Extra Mouse Button 2
                    _ => 0x00 // Unknown button
                };

                string buttonName = e.ChangedButton.ToString();
                string hexString = $"0x{hexValue:X}"; // Convert to hex format

                // Update the displayed key text and save to file
                if (sourceButton == KeyDetectionButton && SelectedKeyText != null)
                {
                    SelectedKeyText.Text = $"{buttonName} ({hexString})";
                    if (fileSharing != null)
                        fileSharing.UpdateSetting(FileSharing.SettingIndex.SelectedKey, hexValue);
                }
                else if (sourceButton == KeyDetectionButton2 && SelectedKeyText2 != null)
                {
                    SelectedKeyText2.Text = $"{buttonName} ({hexString})";
                    if (fileSharing != null)
                        fileSharing.UpdateSetting(FileSharing.SettingIndex.WeaponDelayKey, hexValue);
                }

                // Close the popup
                if (KeyDetectionPopup != null)
                    KeyDetectionPopup.IsOpen = false;

                // Remove the temporary event handlers
                this.PreviewKeyDown -= DetectKeyPress;
                this.PreviewMouseDown -= DetectMousePress;

                // Mark the event as handled
                e.Handled = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DetectMousePress: {ex.Message}");
                this.PreviewKeyDown -= DetectKeyPress;
                this.PreviewMouseDown -= DetectMousePress;
            }
        }


        // Call this method to start detecting input
        private void StartKeyOrMouseDetection(object sender, RoutedEventArgs e)
        {
            try
            {
                // Store the button that triggered this for reference in event handlers
                if (KeyDetectionPopup != null)
                {
                    KeyDetectionPopup.Tag = sender;

                    // Show the popup
                    KeyDetectionPopup.IsOpen = true;

                    // Add temporary event handlers
                    this.PreviewKeyDown += DetectKeyPress;
                    this.PreviewMouseDown += DetectMousePress;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in StartKeyOrMouseDetection: {ex.Message}");
            }
        }

        // Method to handle pixel range slider value change with file sharing
        private void PixelRangeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                // Get the value directly from the slider (1-10)
                int pixelRange = (int)e.NewValue;

                // Update the pixel range value text
                if (PixelRangeValue != null)
                {
                    PixelRangeValue.Text = pixelRange.ToString();
                }

                // Calculate the mapped value and update in file
                string key = $"PixelRange{pixelRange}";
                if (settingValues.ContainsKey(key) && fileSharing != null)
                {
                    int mappedValue = settingValues[key];
                    fileSharing.UpdateSetting(FileSharing.SettingIndex.PixelRange, mappedValue);
                    Console.WriteLine($"Pixel Range changed to: {pixelRange} (mapped: {mappedValue})");
                }
                else
                {
                    Console.WriteLine($"Pixel Range changed to: {pixelRange}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in PixelRangeSlider_ValueChanged: {ex.Message}");
            }
        }

        private void AudioRangeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                // Get the value directly from the slider (1-10)
                int audioRange = (int)e.NewValue;

                // Update the pixel range value text
                if (AudioRangeValue != null)
                {
                    AudioRangeValue.Text = audioRange.ToString()+"0%";
                }

                // Calculate the mapped value and update in file
                string key = $"AudioRange{audioRange}";
                if (settingValues.ContainsKey(key) && fileSharing != null)
                {
                    int mappedValue = settingValues[key];
                    fileSharing.UpdateSetting(FileSharing.SettingIndex.AudioRange, mappedValue);
                    Console.WriteLine($"A Range changed to: {audioRange} (mapped: {mappedValue})");
                }
                else
                {
                    Console.WriteLine($"Audio Range changed to: {audioRange}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AudioRangeSlider_ValueChanged: {ex.Message}");
            }
        }


        private void HideAllViews()
        {
            try
            {
                if (UserInfoView != null)
                    UserInfoView.Visibility = Visibility.Collapsed;
                if (ContactUsView != null)
                    ContactUsView.Visibility = Visibility.Collapsed;
                if (SettingsView != null)
                    SettingsView.Visibility = Visibility.Collapsed;
                if (MacroSettingsView != null)
                    MacroSettingsView.Visibility = Visibility.Collapsed;
                if (GlobalSettingsView != null)
                    GlobalSettingsView.Visibility = Visibility.Collapsed;
                if (DefaultView != null)
                    DefaultView.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in HideAllViews: {ex.Message}");
            }
        }

        private void CloseRightToggle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                number++;
                if (CloseRightText != null)
                {
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CloseRightToggle_Click: {ex.Message}");
            }
        }

        // Method to handle shooting method selection changes with file sharing
        private void ShootingMethod_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                RadioButton radioButton = sender as RadioButton;
                if (radioButton != null && radioButton.IsChecked == true && fileSharing != null)
                {
                    int value = 0;
                    string method = "";

                    if (radioButton == PressButtonOption)
                    {
                        value = settingValues["PressButton"];
                        method = "Press Button";
                    }
                    else if (radioButton == HoldButtonOption)
                    {
                        value = settingValues["HoldButton"];
                        method = "Hold Button";
                    }

                    // Update the setting in the file
                    fileSharing.UpdateSetting(FileSharing.SettingIndex.ShootingMethod, value);
                    Console.WriteLine($"Shooting Method changed: {method} ({value})");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ShootingMethod_Checked: {ex.Message}");
            }
        }

        // Method to handle color detection selection changes with file sharing
        private void ColorDetection_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                RadioButton radioButton = sender as RadioButton;
                if (radioButton != null && radioButton.IsChecked == true && fileSharing != null)
                {
                    int value = 0;
                    string color = "";

                    if (radioButton == RedOption)
                    {
                        value = settingValues["Red"];
                        color = "Red";
                    }
                    else if (radioButton == PurpleOption)
                    {
                        value = settingValues["Purple"];
                        color = "Purple";
                    }
                    else if (radioButton == YellowOption)
                    {
                        value = settingValues["Yellow"];
                        color = "Yellow";
                    }

                    // Update the setting in the file
                    fileSharing.UpdateSetting(FileSharing.SettingIndex.ColorDetection, value);
                    Console.WriteLine($"Color Detection changed: {color} ({value})");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ColorDetection_Checked: {ex.Message}");
            }
        }
        private void SyncAllSettings_Click(object sender, RoutedEventArgs e)
        {
            if (fileSharing == null)
            {
                MessageBox.Show("File sharing is not initialized.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Synchronize all current UI settings to the file

                // Status Setting
                if (StatusToggle != null && StatusToggle.IsChecked.HasValue)
                {
                    int statusValue = StatusToggle.IsChecked.Value ? settingValues["StatusTrue"] : settingValues["StatusFalse"];
                    fileSharing.UpdateSetting(FileSharing.SettingIndex.Status, statusValue);
                }

                // Shot While Moving Setting
                if (ShotWhileMovingToggle != null && ShotWhileMovingToggle.IsChecked.HasValue)
                {
                    int shotWhileMovingValue = ShotWhileMovingToggle.IsChecked.Value ?
                        settingValues["ShotWhileMovingTrue"] :
                        settingValues["ShotWhileMovingFalse"];
                    fileSharing.UpdateSetting(FileSharing.SettingIndex.ShotWhileMoving, shotWhileMovingValue);
                }

                // Shooting Method
                if (PressButtonOption != null && PressButtonOption.IsChecked == true)
                {
                    fileSharing.UpdateSetting(
                        FileSharing.SettingIndex.ShootingMethod,
                        settingValues["PressButton"]
                    );
                }
                else if (HoldButtonOption != null && HoldButtonOption.IsChecked == true)
                {
                    fileSharing.UpdateSetting(
                        FileSharing.SettingIndex.ShootingMethod,
                        settingValues["HoldButton"]
                    );
                }

                // Color Detection
                if (RedOption != null && RedOption.IsChecked == true)
                {
                    fileSharing.UpdateSetting(
                        FileSharing.SettingIndex.ColorDetection,
                        settingValues["Red"]
                    );
                }
                else if (PurpleOption != null && PurpleOption.IsChecked == true)
                {
                    fileSharing.UpdateSetting(
                        FileSharing.SettingIndex.ColorDetection,
                        settingValues["Purple"]
                    );
                }
                else if (YellowOption != null && YellowOption.IsChecked == true)
                {
                    fileSharing.UpdateSetting(
                        FileSharing.SettingIndex.ColorDetection,
                        settingValues["Yellow"]
                    );
                }

                // Shooting Delay
                if (VandalOption != null && VandalOption.IsChecked == true)
                {
                    fileSharing.UpdateSetting(
                        FileSharing.SettingIndex.ShootingDelay,
                        settingValues["VANDAL"]
                    );
                }
                else if (GuardianOption != null && GuardianOption.IsChecked == true)
                {
                    fileSharing.UpdateSetting(
                        FileSharing.SettingIndex.ShootingDelay,
                        settingValues["GUARDIAN"]
                    );
                }
                else if (SherifOption != null && SherifOption.IsChecked == true)
                {
                    fileSharing.UpdateSetting(
                        FileSharing.SettingIndex.ShootingDelay,
                        settingValues["SHERIF"]
                    );
                }
                else if (PhantomOption != null && PhantomOption.IsChecked == true)
                {
                    fileSharing.UpdateSetting(
                        FileSharing.SettingIndex.ShootingDelay,
                        settingValues["PHANTOM"]
                    );
                }
                else if (GhostOption != null && GhostOption.IsChecked == true)
                {
                    fileSharing.UpdateSetting(
                        FileSharing.SettingIndex.ShootingDelay,
                        settingValues["GHOST"]
                    );
                }
                else if (NoDelayOption != null && NoDelayOption.IsChecked == true)
                {
                    fileSharing.UpdateSetting(
                        FileSharing.SettingIndex.ShootingDelay,
                        settingValues["NO DELAY"]
                    );
                }

                // Pixel Range
                if (PixelRangeSlider != null)
                {
                    int pixelRange = (int)PixelRangeSlider.Value;
                    string key = $"PixelRange{pixelRange}";
                    if (settingValues.ContainsKey(key))
                    {
                        fileSharing.UpdateSetting(
                            FileSharing.SettingIndex.PixelRange,
                            settingValues[key]
                        );
                    }
                }
                if (AudioRangeSlider != null)
                {
                    int audioRange = (int)AudioRangeSlider.Value;
                    string key = $"AudioRange{audioRange}";
                    if (settingValues.ContainsKey(key))
                    {
                        fileSharing.UpdateSetting(
                            FileSharing.SettingIndex.AudioRange,
                            settingValues[key]
                        );
                    }
                }

                // Weapon Delays
                if (VandalDelayTextBox != null &&
                    int.TryParse(VandalDelayTextBox.Text, out int vandalDelay))
                {
                    fileSharing.UpdateSetting(
                        FileSharing.SettingIndex.WeaponDelayVandal,
                        Math.Clamp(vandalDelay, 0, 30)
                    );
                }

                if (GuardianDelayTextBox != null &&
                    int.TryParse(GuardianDelayTextBox.Text, out int guardianDelay))
                {
                    fileSharing.UpdateSetting(
                        FileSharing.SettingIndex.WeaponDelayGuardian,
                        Math.Clamp(guardianDelay, 0, 30)
                    );
                }

                if (SheriffDelayTextBox != null &&
                    int.TryParse(SheriffDelayTextBox.Text, out int sheriffDelay))
                {
                    fileSharing.UpdateSetting(
                        FileSharing.SettingIndex.WeaponDelaySheriff,
                        Math.Clamp(sheriffDelay, 0, 30)
                    );
                }

                if (PhantomDelayTextBox != null &&
                    int.TryParse(PhantomDelayTextBox.Text, out int phantomDelay))
                {
                    fileSharing.UpdateSetting(
                        FileSharing.SettingIndex.WeaponDelayPhantom,
                        Math.Clamp(phantomDelay, 0, 30)
                    );
                }

                if (GhostDelayTextBox != null &&
                    int.TryParse(GhostDelayTextBox.Text, out int ghostDelay))
                {
                    fileSharing.UpdateSetting(
                        FileSharing.SettingIndex.WeaponDelayGhost,
                        Math.Clamp(ghostDelay, 0, 30)
                    );
                }

                if (NoDelayTextBox != null &&
                    int.TryParse(NoDelayTextBox.Text, out int noDelay))
                {
                    fileSharing.UpdateSetting(
                        FileSharing.SettingIndex.WeaponDelayNone,
                        Math.Clamp(noDelay, 0, 30)
                    );
                }

                // Key Selections
                if (SelectedKeyText != null && !string.IsNullOrEmpty(SelectedKeyText.Text) &&
                    SelectedKeyText.Text != "None")
                {
                    int selectedKeyValue = Math.Abs(SelectedKeyText.Text.GetHashCode() % 31);
                    fileSharing.UpdateSetting(FileSharing.SettingIndex.SelectedKey, selectedKeyValue);
                }

                if (SelectedKeyText2 != null && !string.IsNullOrEmpty(SelectedKeyText2.Text) &&
                    SelectedKeyText2.Text != "None")
                {
                    int weaponDelayKeyValue = Math.Abs(SelectedKeyText2.Text.GetHashCode() % 31);
                    fileSharing.UpdateSetting(FileSharing.SettingIndex.WeaponDelayKey, weaponDelayKeyValue);
                }

                // Update last sync time
                if (LastSyncText != null)
                {
                    LastSyncText.Text = $"Last synchronized: {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                }

                // Update file path information
                if (FilePathText != null)
                {
                    try
                    {
                        FilePathText.Text = $"Settings file: {fileSharing.GetSettingsFilePath()}";
                    }
                    catch (Exception pathEx)
                    {
                        FilePathText.Text = $"Could not retrieve file path: {pathEx.Message}";
                    }
                }

                // Show success message
                MessageBox.Show(
                    "All settings have been synchronized to the file.",
                    "Sync Successful",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
            catch (Exception ex)
            {
                // Show error message if sync fails
                MessageBox.Show(
                    $"Failed to synchronize settings: {ex.Message}",
                    "Sync Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }
        // Method to handle shooting delay selection changes with file sharing
        private void ShootingDelay_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                RadioButton radioButton = sender as RadioButton;
                if (radioButton != null && radioButton.IsChecked == true && fileSharing != null)
                {
                    int value = 0;
                    string weapon = "";

                    if (radioButton == VandalOption)
                    {
                        value = settingValues["VANDAL"];
                        weapon = "VANDAL";
                    }
                    else if (radioButton == GuardianOption)
                    {
                        value = settingValues["GUARDIAN"];
                        weapon = "GUARDIAN";
                    }
                    else if (radioButton == SherifOption)
                    {
                        value = settingValues["SHERIF"];
                        weapon = "SHERIF";
                    }
                    else if (radioButton == PhantomOption)
                    {
                        value = settingValues["PHANTOM"];
                        weapon = "PHANTOM";
                    }
                    else if (radioButton == GhostOption)
                    {
                        value = settingValues["GHOST"];
                        weapon = "GHOST";
                    }
                    else if (radioButton == NoDelayOption)
                    {
                        value = settingValues["NO DELAY"];
                        weapon = "NO DELAY";
                    }

                    // Update the setting in the file
                    fileSharing.UpdateSetting(FileSharing.SettingIndex.ShootingDelay, value);
                    Console.WriteLine($"Shooting Delay changed: {weapon} ({value})");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ShootingDelay_Checked: {ex.Message}");
            }
        }

        // Method to handle weapon delay text changes with file sharing
        private void WeaponDelay_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (fileSharing == null)
                    return;

                TextBox textBox = sender as TextBox;
                if (textBox != null)
                {
                    // Try to parse the text to an integer
                    if (int.TryParse(textBox.Text, out int value))
                    {
                        // Clamp the value to 0-30
                        value = Math.Clamp(value, 0, 1000);
                        // Update the appropriate setting based on which TextBox was changed
                        if (textBox == VandalDelayTextBox)
                        {
                            fileSharing.UpdateSetting(FileSharing.SettingIndex.WeaponDelayVandal, value);
                            Console.WriteLine($"Vandal delay set to: {value}");
                        }
                        else if (textBox == GuardianDelayTextBox)
                        {
                            fileSharing.UpdateSetting(FileSharing.SettingIndex.WeaponDelayGuardian, value);
                            Console.WriteLine($"Guardian delay set to: {value}");
                        }
                        else if (textBox == SheriffDelayTextBox)
                        {
                            fileSharing.UpdateSetting(FileSharing.SettingIndex.WeaponDelaySheriff, value);
                            Console.WriteLine($"Sheriff delay set to: {value}");
                        }
                        else if (textBox == PhantomDelayTextBox)
                        {
                            fileSharing.UpdateSetting(FileSharing.SettingIndex.WeaponDelayPhantom, value);
                            Console.WriteLine($"Phantom delay set to: {value}");
                        }
                        else if (textBox == GhostDelayTextBox)
                        {
                            fileSharing.UpdateSetting(FileSharing.SettingIndex.WeaponDelayGhost, value);
                            Console.WriteLine($"Ghost delay set to: {value}");
                        }
                        else if (textBox == NoDelayTextBox)
                        {
                            fileSharing.UpdateSetting(FileSharing.SettingIndex.WeaponDelayNone, value);
                            Console.WriteLine($"No Delay value set to: {value}");
                        }
                    }
                    else
                    {
                        // Handle invalid input
                        string defaultKey = "";
                        FileSharing.SettingIndex settingIndex = FileSharing.SettingIndex.WeaponDelayVandal;
                        bool validSetting = true;

                        if (textBox == VandalDelayTextBox)
                        {
                            defaultKey = "DefaultVandal";
                            settingIndex = FileSharing.SettingIndex.WeaponDelayVandal;
                        }
                        else if (textBox == GuardianDelayTextBox)
                        {
                            defaultKey = "DefaultGuardian";
                            settingIndex = FileSharing.SettingIndex.WeaponDelayGuardian;
                        }
                        else if (textBox == SheriffDelayTextBox)
                        {
                            defaultKey = "DefaultSheriff";
                            settingIndex = FileSharing.SettingIndex.WeaponDelaySheriff;
                        }
                        else if (textBox == PhantomDelayTextBox)
                        {
                            defaultKey = "DefaultPhantom";
                            settingIndex = FileSharing.SettingIndex.WeaponDelayPhantom;
                        }
                        else if (textBox == GhostDelayTextBox)
                        {
                            defaultKey = "DefaultGhost";
                            settingIndex = FileSharing.SettingIndex.WeaponDelayGhost;
                        }
                        else if (textBox == NoDelayTextBox)
                        {
                            defaultKey = "DefaultNoDelay";
                            settingIndex = FileSharing.SettingIndex.WeaponDelayNone;
                        }
                        else
                        {
                            validSetting = false;
                        }

                        int defaultValue = 0;

                        // Reset to default if key exists
                        if (validSetting && !string.IsNullOrEmpty(defaultKey) && settingValues.ContainsKey(defaultKey))
                        {
                            defaultValue = settingValues[defaultKey];
                            textBox.Text = defaultValue.ToString();
                            fileSharing.UpdateSetting(settingIndex, defaultValue);
                        }
                        else
                        {
                            // Fallback to 0 if no default found
                            textBox.Text = "0";
                            if (validSetting)
                                fileSharing.UpdateSetting(settingIndex, 0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in WeaponDelay_TextChanged: {ex.Message}");
            }
        }

        // Window closing event to clean up resources
        protected override void OnClosed(EventArgs e)
        {
            try
            {
                // Clean up file sharing resources
                if (fileSharing != null)
                {
                    fileSharing.Dispose();
                    fileSharing = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in OnClosed: {ex.Message}");
            }

            base.OnClosed(e);
        }
    }
}