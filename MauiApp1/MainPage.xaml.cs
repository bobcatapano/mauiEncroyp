


using Microsoft.Maui;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Storage;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Maui.Alerts;
using System.ComponentModel.Design;

namespace MauiApp1;

public partial class MainPage : ContentPage
{
    Entry entry1;
    Entry entry2;

    RadioButton encrpyRadioButton;
    RadioButton decrpyRadioButton;
    Label validation;
 
    string globalFileName;

    public MainPage()
    {
        validation = new Label { Text = "" };
        entry1 = new Entry { Placeholder = "Enter password" };
        entry1.TextChanged += OnEntryTextChanged;
        entry1.Completed += OnEntryCompleted;

        entry2 = new Entry { Placeholder = "Re-Enter password" };
        entry2.TextChanged += OnEntryTextChanged2;
        entry2.Completed += OnEntryCompleted2;

        var buttonsGrid = new Grid() { HeightRequest = 40.0 };
        buttonsGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1.0, GridUnitType.Auto) });
        buttonsGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(30.0, GridUnitType.Absolute) });
        buttonsGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1.0, GridUnitType.Auto) });

        var radioButtonsGrid = new Grid() { HeightRequest = 40.0 };
        radioButtonsGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1.0, GridUnitType.Auto) });
        radioButtonsGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(30.0, GridUnitType.Absolute) });
        radioButtonsGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1.0, GridUnitType.Auto) });

        var browseButton = new Button() { WidthRequest = 100, Text = "Browse" };
        browseButton.Clicked += OnBrowseButtonClicked;
        Grid.SetColumn(browseButton, 0);
        buttonsGrid.Children.Add(browseButton);

        var cryptButton = new Button() { WidthRequest = 200, Text = "Encrypt/Decrypt" };
        cryptButton.Clicked += OnCryptButtonClicked;
        Grid.SetColumn(cryptButton, 2);
        buttonsGrid.Children.Add(cryptButton);

        encrpyRadioButton = new RadioButton() { Content = "Encrypt" };
        encrpyRadioButton.IsChecked = true;
        Grid.SetColumn(encrpyRadioButton, 0);
        radioButtonsGrid.Children.Add(encrpyRadioButton);

        decrpyRadioButton = new RadioButton() { Content = "Decrypt" };
        Grid.SetColumn(decrpyRadioButton, 2);
        radioButtonsGrid.Children.Add(decrpyRadioButton);

        var stackLayout = new VerticalStackLayout
        {
            Padding = new Thickness(30, 60, 30, 30),
            Children = { entry1, entry2, validation, buttonsGrid, radioButtonsGrid }
        };

        this.Content = stackLayout;
    }

    private void OnEntryCompleted2(object sender, EventArgs e)
    {
      //  throw new NotImplementedException();
    }

    private void OnEntryTextChanged2(object sender, TextChangedEventArgs e)
    {
      //  throw new NotImplementedException();
    }

    private void OnEntryCompleted(object sender, EventArgs e)
    {
       // throw new NotImplementedException();
    }

    private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
    {
     //   throw new NotImplementedException();
    }

    private async Task<FileResult> PickAndShow(PickOptions options)
    {
        try
        {
            var result = await FilePicker.Default.PickAsync(options);
            
            if (result != null)
                return result;
            
        }
        catch (Exception ex) {
            return null;
        }
        return null;
    }

    private async Task<FileSaverResult> SaveFile(CancellationToken cancellationToken, MemoryStream stream)
    {
        try
        {
            var result = await FileSaver.Default.SaveAsync("test.enm", stream, cancellationToken);
            if (result != null)
            {
                return result;
            }
        }
        catch (OperationCanceledException)
        {
            // The user canceled the operation
        }
        return null;
    }
    async void OnBrowseButtonClicked(object sender, EventArgs e)
    {
        PickOptions theoptions = new() {
        PickerTitle = "Please select a file"
        };
        var fileResult = await PickAndShow(theoptions);
        globalFileName = fileResult.FullPath;
    }

    async void OnCryptButtonClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(entry1.Text) || string.IsNullOrEmpty(entry2.Text))
        {
            validation.Text = "Please enter a password";
            return;
        }
        else if (entry1.Text != entry2.Text)
        {
            validation.Text = "Passwords do not match";
            return;
        }
        else
        {
            validation.Text = "Password validated";
        }
        
        if (File.Exists(globalFileName))
        {
            File.Delete(globalFileName);
        }

        encrpyRadioButton.IsChecked = true;

        if (encrpyRadioButton.IsChecked)
        {
            try
            {
                if (string.IsNullOrEmpty(globalFileName))
                {
                    //                        labelError.Text = "Selected file is empty.";
                    //                        labelError.ForeColor = Color.Red;

                    return;
                }

                if (globalFileName.StartsWith("$ANSIBLE_VAULT;"))
                {
                    //                        labelError.Text = "Selected file is encrypted.";
                    //                        labelError.ForeColor = Color.Red;

                    return;
                }

                byte[] fileBytes = File.ReadAllBytes(globalFileName);
                
                string fullFilePath = Path.GetFullPath(globalFileName);
                string directoryPath = Path.GetDirectoryName(fullFilePath);
                 
                byte[] encryptedBytes = AnsibleEncryption.Encrypt_Bytes(fileBytes, entry1.Text);

                MemoryStream stream = new MemoryStream(encryptedBytes);

                var theresult = await SaveFile(new CancellationToken(), stream);
                
                if (theresult.IsSuccessful)
                   await Toast.Make($"File saved to {theresult.FilePath}").Show(new CancellationToken());           
            }
            catch (Exception ex)
            {
                var test = ex;
                //                    labelError.Text = "There was an error encrypting the file.";
                //                    labelError.ForeColor = Color.Red;
            }
        }

        if (decrpyRadioButton.IsChecked)
        {
            //try
            //{
            //     string fullFilePath = Path.GetFullPath(globalFileName);

            //     if (!fullFilePath.EndsWith(".enm"))
            //     {
            //        //                        labelError.Text = "Only .enm files can be decrypted.";
            //        //                        labelError.ForeColor = Color.Red;

            //          return;
            //     }

            //     if (useBinaryCheckBox.IsChecked)
            //     {
                    byte[] encryptedFileBytes = File.ReadAllBytes(globalFileName);
                    byte[] decryptedBytes = AnsibleEncryption.Decrypt_Bytes(encryptedFileBytes, entry1.Text);

                    /// NEW CODE
                    /// 
                    MemoryStream stream = new MemoryStream(decryptedBytes);

                    var theresult = await SaveFile(new CancellationToken(), stream);

                    if (theresult.IsSuccessful)
                        await Toast.Make($"File saved to {theresult.FilePath}").Show(new CancellationToken());

            ////

            //         string newFilePath = new string(fullFilePath.SkipLast(4).ToArray());
            //         if (!File.Exists(newFilePath))
            //         {
            //             File.WriteAllBytes(newFilePath, decryptedBytes);

            //            //                            labelError.Text = "File decrypted.";
            //            //                            labelError.ForeColor = Color.Green;

            //            //                            textBoxFileName.Text = "";
            //            //                            textBoxPW.Text = "";
            //            //                            textBoxRPW.Text = "";

            //             return;
            //         }
            //        else
            //        {
            //            string decryptedFilePath = newFilePath + ".decrypted";
            //            if (!File.Exists(decryptedFilePath))
            //            {
            //                File.WriteAllBytes(decryptedFilePath, decryptedBytes);

            //                //                                labelError.Text = "File Decrypted";
            //                //                                labelError.ForeColor = Color.Green;

            //                //                                textBoxFileName.Text = "";
            //                //                                textBoxPW.Text = "";
            //                //                                textBoxRPW.Text = "";

            //                return;
            //            }
            //            else
            //            {
            //                //                                //ToDo: Iterate like encrypt
            //                int iteration = 0;
            //                decryptedFilePath = fullFilePath + iteration + ".enm";
            //                while (File.Exists(decryptedFilePath))
            //                {
            //                    iteration++;
            //                    decryptedFilePath = fullFilePath + iteration + ".enm";
            //                }

            //                File.WriteAllBytes(decryptedFilePath, decryptedBytes);

            //                //                                textBoxFileName.Text = fullFilePath + ".enm";

            //                //                                labelError.Text = "File Decrypted";
            //                //                                labelError.ForeColor = Color.Green;

            //                //                                textBoxFileName.Text = "";
            //                //                                textBoxPW.Text = "";
            //                //                                textBoxRPW.Text = "";

            //                return;

            //            }
            //        }

            //     }
            //     else
            //     {
            //          string fileText = File.ReadAllText(globalFileName);
            //          var keyVaultResult = AnsibleEncryption.Decrypt(fileText, editor.Text);

            //          string newFilePath = new string(fullFilePath.SkipLast(4).ToArray());
            //          if (!File.Exists(newFilePath))
            //          {
            //               File.WriteAllText(newFilePath, keyVaultResult);

            //            //                            labelError.Text = "File decrypted.";
            //            //                            labelError.ForeColor = Color.Green;

            //            //                            textBoxFileName.Text = "";
            //            //                            textBoxPW.Text = "";
            //            //                            textBoxRPW.Text = "";
            //           }
            //           else
            //           {
            //               string decryptedFilePath = newFilePath + ".decrypted";
            //               if (!File.Exists(decryptedFilePath))
            //               {
            //                   File.WriteAllText(decryptedFilePath, keyVaultResult);

            //                //                                labelError.Text = "File Decrypted";
            //                //                                labelError.ForeColor = Color.Green;

            //                //                                textBoxFileName.Text = "";
            //                //                                textBoxPW.Text = "";
            //                //                                textBoxRPW.Text = "";
            //               }
            //               else
            //               {
            //                   int iteration = 0;
            //                   decryptedFilePath = fullFilePath + iteration + ".decrypted";
            //                   while (File.Exists(decryptedFilePath))
            //                   {
            //                       iteration++;
            //                       decryptedFilePath = fullFilePath + iteration + ".decrypted";
            //                   }

            //                   File.WriteAllText(decryptedFilePath, keyVaultResult);

            //                //                                textBoxFileName.Text = fullFilePath + ".decrypted";

            //                //                                labelError.Text = "File encrypted.";
            //                //                                labelError.ForeColor = Color.Green;

            //                //                                textBoxFileName.Text = "";
            //                //                                textBoxPW.Text = "";
            //                //                                textBoxRPW.Text = "";
            //            }
            //        }
            //     }

            // }
            // catch 
            // {
            ////                    labelError.Text = "Error decrypting file.";
            ////                    labelError.ForeColor = Color.Red;
            // }
        }
    }
}

