


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

    byte[] globalFileName;
    string gloableFileExtension;

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

    private async Task<string> PickAndShow(PickOptions options)
    {
        try
        {
            var result = await FilePicker.Default.PickAsync(options);
            
            if (result != null)
                return result.FullPath;
            
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
        //PickOptions theoptions = new() {
        //PickerTitle = "Please select a file"
        //};
        //var fileResult = await PickAndShow(theoptions);
        //globalFileName = fileResult;

        var theoptions = new PickOptions
        {
            PickerTitle = "Please select a file"
        };

        globalFileName = await PickAndReadFile(theoptions);

        if (globalFileName != null)
        {
           

        //    // You now have the file content in the 'fileBytes' array
        //    // Do whatever you need with the file content
        //    string yp = "";
        //    byte[] encryptedBytes = AnsibleEncryption.Encrypt_Bytes(fileBytes, entry1.Text);

            //    MemoryStream stream = new MemoryStream(encryptedBytes);

            //    var theresult = await SaveFile(new CancellationToken(), stream);

            //    if (theresult.IsSuccessful)
            //        await Toast.Make($"File saved to {theresult.FilePath}").Show(new CancellationToken());
        }
        else
        {
        //    // Handle the case where the file wasn't picked or an error occurred
        //    string no = "";
        }

       // return fileBytes;
    }

    private async Task<byte[]> PickAndReadFile(PickOptions options)
    {
        try
        {
            var result = await FilePicker.Default.PickAsync(options);

            if (result != null)
            {
                string theName = result.FileName;
                gloableFileExtension = Path.GetExtension(theName);
                // On Android, the FullPath might not be a direct file path
                // Use the provided stream to read the file content instead
                using (var fileStream = await result.OpenReadAsync())
                {
                    var memoryStream = new MemoryStream();
                    await fileStream.CopyToAsync(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions here
            string dooo = "";
        }

        return null;
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
        
        //if (File.Exists(globalFileName))
        //{
        //    File.Delete(globalFileName);
        //}

        encrpyRadioButton.IsChecked = true;

        if (encrpyRadioButton.IsChecked)
        {
            try
            {
                //if (string.IsNullOrEmpty(globalFileName))
                //{
                //    //                        labelError.Text = "Selected file is empty.";
                //    //                        labelError.ForeColor = Color.Red;

                //    return;
                //}

                //if (globalFileName.StartsWith("$ANSIBLE_VAULT;"))
                //{
                //    //                        labelError.Text = "Selected file is encrypted.";
                //    //                        labelError.ForeColor = Color.Red;

                //    return;
                //}

                //  globalFileName = "/Android/data/com.companyname.mauiapp1/cache/2203693cc04e0be7f4f024d5f9499e13/88e76bf2b19a43bcbb4d1068dd5e9296/image.png";
                //var here = FileSystem.Current.AppDataDirectory;
                //string newt = "/data/user/0/com.companyname.mauiapp1/cache/2203693cc04e0be7f4f024d5f9499e13/f46da7fd10454bdd81980494eb1f718a/Screenshot_20230927_170318_One UI Home.jpg";

                //   var testing = await FileSystem.Current.OpenAppPackageFileAsync(newt);

                //     string fullFilePath = Path.GetFullPath(globalFileName);
                //     string directoryPath = Path.GetDirectoryName(fullFilePath);

                //      byte[] fileBytes = File.ReadAllBytes(globalFileName);
                //var test = FileSystem.OpenAppPackageFileAsync(globalFileName);

                if (gloableFileExtension != ".enm") {

                    byte[] encryptedBytes = AnsibleEncryption.Encrypt_Bytes(globalFileName, entry1.Text);

                    MemoryStream stream = new MemoryStream(encryptedBytes);

                    var theresult = await SaveFile(new CancellationToken(), stream);

                    if (theresult.IsSuccessful)
                        await Toast.Make($"File saved to {theresult.FilePath}").Show(new CancellationToken());
                }
                else
                {
                    validation.Text = "Can not encrpt a enm type file";
                    return;
                }
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
            // byte[] encryptedFileBytes = File.ReadAllBytes(globalFileName);

            if (gloableFileExtension == ".enm")
            {
                byte[] decryptedBytes = AnsibleEncryption.Decrypt_Bytes(globalFileName, entry1.Text);

                /// NEW CODE
                /// 
                MemoryStream stream = new MemoryStream(decryptedBytes);

                var theresult = await SaveFile(new CancellationToken(), stream);

                if (theresult.IsSuccessful)
                    await Toast.Make($"File saved to {theresult.FilePath}").Show(new CancellationToken());
            }
            else
            {
                validation.Text = "Please select a enm type file";
                return;
            }

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

