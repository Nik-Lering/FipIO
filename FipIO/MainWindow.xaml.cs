using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace FipIO
{
    [global::System.Configuration.SettingsSerializeAs(global::System.Configuration.SettingsSerializeAs.Binary)]
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string PathDirectoryApp = Environment.CurrentDirectory;

        string KeyForDecoda;

        string[] ArrStatusTask = {"Заявка:",
        "Папка:",
                "Issue:",
                "Merge:",
                "Branch:",
                "Описание:",
                "Статус:"
        };
        static string[] FileForWriter;
        static string[] FileOfDirectory;
        static string FilePathFolderBegin;
        static string FilePathFolderEnd;
        public static string FileName = "FipIO_Application_";
        static string PathOfFile = PathDirectoryApp + "/FipIO Task/" + FileName;

        //Grid GridBlock = this.FindName("MainArea") as Grid;

        public MainWindow()
        {
            InitializeComponent();
            
            if(!Directory.Exists(PathDirectoryApp+"/FipIO Task"))
            {
                Directory.CreateDirectory(PathDirectoryApp + "/FipIO Task");
            }
            SavePropertis();

        }

        private void SavePropertis()
        {
            FipIO_2.Text = "Last: " + System.IO.Path.GetFileNameWithoutExtension(Properties.Settings.Default.vhosts_nginx) + " / " + System.IO.Path.GetFileNameWithoutExtension(Properties.Settings.Default.httpd_vhosts);
            FipIO_1.Text = "Last: " + System.IO.Path.GetFileNameWithoutExtension(Properties.Settings.Default.folderForCopyBegin);
            FipIO_3.Text = "Last: " + System.IO.Path.GetFileNameWithoutExtension(Properties.Settings.Default.folderForCopyEnd);
        }
        private void Output_all(object sender, RoutedEventArgs e)
        {

            TextBlock tBlock;
            System.Windows.Controls.TextBox tBox;
            //string[] ParserText;
            int RowCount = 0;
            //int CountFile = 1;
            int NumberFile = 0;
            FileOfDirectory = Directory.GetFiles(PathDirectoryApp + "/FipIO Task");
            for (int file = 0; file < FileOfDirectory.Length; file++)
            {
                string FileNameLocal = System.IO.Path.GetFileNameWithoutExtension(FileOfDirectory[file]);
                NumberFile = Convert.ToInt32(FileNameLocal.Replace(FileName, ""));

                string[] readText = File.ReadAllLines(FileOfDirectory[file]);

                for(int i = 0; i < ArrStatusTask.Length; i++)
                {
                    RowDefinition rDefin = new RowDefinition();
                    rDefin.MaxHeight = 50;
                    FieldForTask.RowDefinitions.Add(rDefin);
                    //ParserText = ParserStringText(readText[i]);

                    tBlock = CreateTextBlock(ArrStatusTask[i], RowCount, 0, NumberFile, i);
                    FieldForTask.Children.Add(tBlock);
                    tBox = CreateTextBox(readText[i], RowCount, 1, NumberFile, i);
                    FieldForTask.Children.Add(tBox);
                    RowCount++;
                }

                tBlock = CreateTextBlock("", RowCount, 0, NumberFile, 0);
                FieldForTask.Children.Add(tBlock);
                tBlock = CreateTextBlock("", RowCount, 1, NumberFile, 0);
                FieldForTask.Children.Add(tBlock);
                RowCount++;
                //CountFile++;
            }
        }

        static TextBlock CreateTextBlock(string textFile, int Row, int Column, int CountFIle, int LineIsArr)
        {
            TextBlock tBlock = new TextBlock
            {
                Text = textFile,
                Name = "Fip_IO" + CountFIle + "_" + LineIsArr
                //Margin = new Thickness(0, 0 + Margin * 20, 0, 0),

            };
            
            //Grid.SetRow(tBlock, i);
            Grid.SetColumn(tBlock, Column);
            Grid.SetRow(tBlock, Row);
            return tBlock;
        }

        static System.Windows.Controls.TextBox CreateTextBox(string textFile, int Row, int Column, int CountFIle, int LineIsArr)
        {
            System.Windows.Controls.TextBox tBox = new System.Windows.Controls.TextBox
            {
                Text = textFile,
                Name = "Fip_IO" + CountFIle + "_" + LineIsArr
                //Margin = new Thickness(0, 0 + Margin * 20, 0, 0),

            };
            tBox.TextChanged += WriterFile;
            //Grid.SetRow(tBlock, i);
            Grid.SetColumn(tBox, Column);
            Grid.SetRow(tBox, Row);
            return tBox;
        }

        private static void WriterFile(object sender, TextChangedEventArgs e)
        {
            string TextBoxName = ((sender as System.Windows.Controls.TextBox).Name).Remove(0,6);
            string[] FileNameAndLine = TextBoxName.Split('_');

            string[] readText = File.ReadAllLines(PathOfFile +  FileNameAndLine[0] + ".txt");
            readText[Convert.ToInt32(FileNameAndLine[1])] = (sender as System.Windows.Controls.TextBox).Text;
            StreamWriter FileForWriteText = new StreamWriter(PathOfFile + FileNameAndLine[0] + ".txt", false);

            for (int i = 0; i < readText.Length; i++)
            {
                FileForWriteText.WriteLine(readText[i]);
            }
            FileForWriteText.WriteLine();
            FileForWriteText.Close();


        }

        static string[] ParserStringText(string textFile)
        {
            //string[] ParserText;
            string[] ParserText = textFile.Split(new char[] {'-'}, 2);
            return ParserText;
        }

        private void CreateNewTask(object sender, RoutedEventArgs e)
        {
            CreateNewFile();
        }

        private void CreateNewFile()
        {
            int NumberLastFile = MaxCountFile();

            using (StreamWriter FileForReader = File.CreateText(PathDirectoryApp + "/FipIO Task/" + FileName + NumberLastFile + ".txt"))
            {
                FileForReader.WriteLine("Заявка:");
                FileForReader.WriteLine("Папка:");
                FileForReader.WriteLine("Issue:");
                FileForReader.WriteLine("Merge:");
                FileForReader.WriteLine("Branch:");
                FileForReader.WriteLine("Описание:");
                FileForReader.WriteLine("Статус:");
            }
        }

        static int MaxCountFile()
        {
            int max = 0;
            int lastNumberFile;

            FileOfDirectory = Directory.GetFiles(PathDirectoryApp + "/FipIO Task");

            for (int i = 0; i < FileOfDirectory.Length; i++)
            {
                string FileNameLocal = System.IO.Path.GetFileNameWithoutExtension(FileOfDirectory[i]);
                lastNumberFile = Convert.ToInt32(FileNameLocal.Replace(FileName,""));
                if(lastNumberFile > max)
                {
                    max = lastNumberFile;
                }
            }

            return max + 1;
        }

        private void OpenFile(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog OPF = new Microsoft.Win32.OpenFileDialog();
            OPF.Multiselect = true;
            if (OPF.ShowDialog() == true)
            {
                FileForWriter = OPF.FileNames;
            }
        }

        private void SelectBeginDir(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog OPF_folder = new FolderBrowserDialog();

            if (OPF_folder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FilePathFolderBegin = OPF_folder.SelectedPath;
            }
        }

        private void SelectEndDir(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog OPF_folder_end = new FolderBrowserDialog();

            if (OPF_folder_end.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FilePathFolderEnd = OPF_folder_end.SelectedPath;
            }
        }

        private void CreateCopy(object sender, RoutedEventArgs e)
        {
            if (FileForWriter != null)
            {
                for (int i = 0; i < FileForWriter.Length; i++)
                {
                    string FileName = System.IO.Path.GetFileName(FileForWriter[i]);
                    if (FileName == "httpd-vhosts.conf")
                    {
                        Properties.Settings.Default.httpd_vhosts = FileForWriter[i];
                    }
                    else if (FileName == "vhosts-nginx.conf")
                    {
                        Properties.Settings.Default.vhosts_nginx = FileForWriter[i];
                    }
                }
            }

            if (FilePathFolderBegin != null)
            {
                Properties.Settings.Default.folderForCopyBegin = FilePathFolderBegin;
            }

            if (FilePathFolderEnd != null)
            {
                Properties.Settings.Default.folderForCopyEnd = FilePathFolderEnd;
            }

            SavePropertis();

            Properties.Settings.Default.Save();
            AddTextInTextFile(Properties.Settings.Default.folderForCopyEnd, DirectoryName.Text);
            CopyFolder();
            ComandLine();
        }

        public void ComandLine()
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "cmd.exe";
            psi.Arguments = "/k " + "cd /d " + Properties.Settings.Default.folderForCopyEnd + "/" + DirectoryName.Text + " & composer update";
            DirectoryName.Text = psi.Arguments;
            Process.Start(psi);
        }
        public void AddTextInTextFile(string PathDirectory, string NameLocalServer)
        {
            string[] FilesPath = {Properties.Settings.Default.vhosts_nginx,
                Properties.Settings.Default.httpd_vhosts
            };

            if (!String.IsNullOrEmpty(FilesPath[0]) && !String.IsNullOrEmpty(FilesPath[1]))
            {
                string[] httpd_vhosts = {"<VirtualHost *:8000>",
                                    "ServerName "+NameLocalServer+".loc",
                                    "ServerAlias " + NameLocalServer + ".t199",
                                    "DocumentRoot \"" + PathDirectory + "/" + NameLocalServer + "/www/\"",
                                    "AddType application/x-httpd-php .php",
                                    "Action  application/x-httpd-php /php56/php-cgi.exe",
                                    "Alias /_syscss/ \"" + PathDirectory + "/" + NameLocalServer + "/vendor/web-autoresource/war-lib-6/src/_syscss/\"",
                                    "Alias /_syslib/ \"" + PathDirectory + "/" + NameLocalServer + "/vendor/web-autoresource/war-lib-6/src/_syslib/\"",
                                    "Alias /_sysimg/ \"" + PathDirectory + "/" + NameLocalServer + "/vendor/web-autoresource/war-lib-6/src/_sysimg/\"",
                                    "Alias /_phplib/ \"" + PathDirectory + "/" + NameLocalServer + "/vendor/web-autoresource/war-lib-6/src/_UNIVERSAL/\"",
                                    "</VirtualHost>"
            };


                string[] vhosts_nginx_text = {"server {",
                                    "server_name "+ NameLocalServer+".loc " + NameLocalServer + ".t199;",
                                    "include php_war6.conf;",
                                    "include global.conf;",
                                    "root " + PathDirectory + "/" + NameLocalServer+"/www;",
                                    "set $libs " + PathDirectory + "/" + NameLocalServer +"/vendor/web-autoresource/war-lib-6/src;",
                                    "set $sysmod " + PathDirectory + "/" + NameLocalServer + "/vendor/web-autoresource-modules;",
                                    "}"
            };

                for (int file = 0; file < FilesPath.Length; file++)
                {
                    StreamWriter FileForWriteText = new StreamWriter(FilesPath[file], true);
                    string[] textForWrite;
                    FileForWriteText.WriteLine();
                    if (file == 0) textForWrite = vhosts_nginx_text;
                    else textForWrite = httpd_vhosts;

                    for (int i = 0; i < textForWrite.Length; i++)
                    {
                        FileForWriteText.WriteLine(textForWrite[i]);
                    }
                    FileForWriteText.WriteLine();
                    FileForWriteText.Close();
                }

            }
            else
            {
                System.Windows.MessageBox.Show("Вы не выбрали один (или оба) из файла ('httpd - vhosts.conf' или 'vhosts - nginx.conf')");
            }

        }

        private void CopyFolder()
        {
            if (!String.IsNullOrEmpty(Properties.Settings.Default.folderForCopyEnd) && !String.IsNullOrEmpty(Properties.Settings.Default.folderForCopyBegin))
            {
                DirectoryCopy(Properties.Settings.Default.folderForCopyBegin, Properties.Settings.Default.folderForCopyEnd + "/" + DirectoryName.Text, true);
            }
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = System.IO.Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = System.IO.Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
    
}
