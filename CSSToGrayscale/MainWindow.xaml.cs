using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;


namespace CSSToGrayscale
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


        }
    
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            //dialog.InitialDirectory = "C:\\Users";
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                try
                {   // Open the text file using a stream reader.

                    string dirPath = dialog.FileName + @"\";
                    DirectoryInfo d = new DirectoryInfo(dirPath);//Assuming Test is your Folder
                    FileInfo[] Files = d.GetFiles("*.css"); //Getting Text files



                    foreach (FileInfo file in Files)
                    {
                        string line = File.ReadAllText(dirPath + file.Name);

                        var foundIndexes = new List<int>();

                        lblStatus.Content = lblStatus.Content + "File " + dirPath + file.Name + " read succesfully. \r\n";
                        for (int i = line.IndexOf('#'); i > -1; i = line.IndexOf('#', i + 1))
                        {
                            foundIndexes.Add(i);
                        }

                        for (int index = 0; index < line.Length; index++)
                        {
                            if (line[index].ToString() == "#")
                            {
                                string colorCode = line.Substring(index, 7);
                                Regex regex = new Regex(@"#[0-9a-fA-F]{6}");
                                Match match = regex.Match(colorCode);
                                if (match.Success) //if color in #xxxxxx format
                                {
                                    //lblStatus.Content = colorCode;
                                    int R = Convert.ToInt32("0x" + colorCode[1] + colorCode[2], 16);
                                    int G = Convert.ToInt32("0x" + colorCode[3] + colorCode[4], 16);
                                    int B = Convert.ToInt32("0x" + colorCode[5] + colorCode[6], 16);
                                    int min = Math.Min(R, Math.Min(G, B));
                                    string updatedString = "#" + min.ToString("X") + min.ToString("X") + min.ToString("X");
                                    if (colorCode != updatedString)
                                    {
                                        //line = line.Replace(colorCode, updatedString);
                                        line = line.Substring(0, index) + updatedString + line.Substring(index + 7, line.Length - (index + 7));
                                        //line = Regex.Replace(line, colorCode, updatedString);
                                    }


                                }
                                else
                                {
                                    Regex regex2 = new Regex(@"^#[0-9a-fA-F]{3}\s");
                                    Match match2 = regex2.Match(colorCode);
                                    if (match2.Success) //if color in #xxx format
                                    {
                                        int R = Convert.ToInt32("0x" + colorCode[1], 16);
                                        int G = Convert.ToInt32("0x" + colorCode[2], 16);
                                        int B = Convert.ToInt32("0x" + colorCode[3], 16);
                                        int min = Math.Min(R, Math.Min(G, B));

                                        string updatedString = "#" + min.ToString("X") + min.ToString("X") + min.ToString("X") + colorCode[4] + colorCode[5] + colorCode[6];
                                        //lblStatus.Content = updatedString;
                                        if (colorCode != updatedString)
                                        {
                                            line = line.Substring(0, index) + updatedString + line.Substring(index + 7, line.Length - (index + 7));
                                        }


                                    }
                                }

                            }
                        }



                        File.WriteAllText(dirPath + file.Name, line);
                        lblStatus.Content = lblStatus.Content + "\r\n All files updated succesfully";
                    }




                }
                catch (Exception f)
                {
                    lblStatus.Content = lblStatus.Content + "\r\n Error during file reading.";
                    MessageBox.Show(f.Message);
                }

            }

            
        }



    }
}
