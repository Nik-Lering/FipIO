using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace FipIO
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string FileName = "FipIO_Application_";

        //Grid GridBlock = this.FindName("MainArea") as Grid;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Output_all_issue(object sender, EventArgs e)
        {
            for(int i = 0; i<10; i++)
            {
                TextBlock tBlock = new TextBlock
                {
                    Name = MainWindow.FileName + i,
                    Text = i.ToString(),
                    Margin = new Thickness(0, 0 + i * 20, 0, 0),
                    //Width = 120,
                    //Height = 10
                };
                Grid.SetRow(tBlock, i);
                Grid.SetColumn(tBlock, 0);
                GridOfMain.Children.Add(tBlock);
                

            }
            for (int i = 0; i < 10; i++)
            {
                TextBlock tBlock = new TextBlock
                {
                    Name = MainWindow.FileName + i,
                    Text = i.ToString(),
                    Margin = new Thickness(0, 0 + i * 20, 0, 0),
                    //Width = 120,
                    //Height = 10
                };
                Grid.SetRow(tBlock, i);
                Grid.SetColumn(tBlock, 1);
                GridOfMain.Children.Add(tBlock);


            }
            //ScrollMainGrid.Content = tBlock;
            Issue_1.Text = "ffff";
        }

        private void Add_new_file(object sender, EventArgs e)
        {
            string FileName = FileEditOfPath.CreateFile();
            CreateFieldForEdit(FileName);
        }

        public void CreateFieldForEdit(string pathFile)
        {
            string[] readText = File.ReadAllLines(pathFile); // Получение того, что в файле
            //MainWindow mainWindow = new MainWindow();
            for (int i = 0; i < readText.Length; i++)
            {
                if (i == 1)
                {
                    CreateFormElemTextBlock("", i);
                }
                CreateFormElemTextBlock(readText[i], i);
                CreateFormElemTextBox(readText[i], i);
            }
        }

        private void CreateFormElemTextBlock(string textForOutput, int NumberForName)
        {
            TextBlock tBlock = new TextBlock
            {
                Name = MainWindow.FileName + NumberForName,
                Text = textForOutput.ToString(),
                Margin = new Thickness(0, 0 + NumberForName * 28, 0, 0),
                Height = 28,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top
            };
            Grid.SetRow(tBlock, NumberForName);
            Grid.SetColumn(tBlock, 0);
            GridOfMain.Children.Add(tBlock);
        }

        private void CreateFormElemTextBox(string textForOutput, int NumberForName)
        {
            TextBox tBlock = new TextBox
            {
                Name = MainWindow.FileName + "Textbox" + NumberForName,
                Text = textForOutput.ToString(),
                Margin = new Thickness(0, 0 + NumberForName * 28, 0, 0),
                Height = 28,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top
            };
            Grid.SetRow(tBlock, NumberForName);
            Grid.SetColumn(tBlock, 1);
            GridOfMain.Children.Add(tBlock);
        }

    }

    public class FileDirectoryClass
    {
        private static string PathToDirectrory = PathOfExeFile();

        public static string PathOfExeFile()  //Получить путь папке
        {
            return AppDomain.CurrentDomain.BaseDirectory + "\\FileForWork";
        }

        public static string[] FilesOfDirectoryWithPath()
        {
            if (!Directory.Exists(PathToDirectrory))
            {
                Directory.CreateDirectory(PathToDirectrory);
            }

            return Directory.GetFiles(PathToDirectrory);
             
        }

        public static string [] FilesOfDirectoryWithoutPath()
        {
            string[] FileWithoutPath = FilesOfDirectoryWithPath();
            if (FileWithoutPath.Length != 0)
            {
                for (int i = 0; i < FileWithoutPath.Length; i++)
                {
                    FileWithoutPath[i] = Path.GetFileNameWithoutExtension(FileWithoutPath[i]);
                }
            }
            return FileWithoutPath;
        }

    }

    public class FileEditOfPath : MainWindow 
    {
        private static string[] FilesForWork = FileDirectoryClass.FilesOfDirectoryWithoutPath();

        public static string CreateFile()
        {
            string FilePathAndName = CreateNewFileName();

            using (StreamWriter FileForReader = File.CreateText(FilePathAndName))
            {
                FileForReader.WriteLine("Заявка:");
                FileForReader.WriteLine("Папка:");
                FileForReader.WriteLine("Issue:");
                FileForReader.WriteLine("Merge:");
                FileForReader.WriteLine("Branch:");
                FileForReader.WriteLine("Описание:");
                FileForReader.WriteLine("Статус:");
            }

            return FilePathAndName;
            //EditLayoutXAMLclass EditLayout = new EditLayoutXAMLclass();
            //EditLayout.CreateFieldForEdit(FilePathAndName);

        }

        private static string CreateNewFileName()
        {
            int MaxNumberFile = MaxNumberFileOfDirectory();
            return FileDirectoryClass.PathOfExeFile() + "\\" + MainWindow.FileName + MaxNumberFile + ".txt";
        }

        private static int MaxNumberFileOfDirectory()
        {
            FilesForWork = FileDirectoryClass.FilesOfDirectoryWithoutPath();
            int NumberLastFile = 0;
            if (FilesForWork.Length != 0)
            {
                
                for (int i = 0; i < FilesForWork.Length; i++)
                {
                    int a = int.Parse(FilesForWork[i].Replace(MainWindow.FileName, ""));
                    if (int.Parse(FilesForWork[i].Replace(MainWindow.FileName, "")) > NumberLastFile) NumberLastFile = int.Parse(FilesForWork[i].Replace(MainWindow.FileName, ""));
                }
            }
            return NumberLastFile + 1;
        }
    }
    
}
