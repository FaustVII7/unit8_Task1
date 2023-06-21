using System;
using System.IO;

namespace FolderCleaner
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите путь до папки для очистки:");
            string folderPath = Console.ReadLine();

            try
            {
                DirectoryInfo directory = new DirectoryInfo(folderPath);
                if (!directory.Exists)
                {
                    Console.WriteLine("Папки по заданному пути не существует.");
                    return;
                }

                CleanFolder(directory, TimeSpan.FromMinutes(30));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static void CleanFolder(DirectoryInfo directory, TimeSpan threshold)
        {
            foreach (FileInfo file in directory.GetFiles())
            {
                if (DateTime.UtcNow - file.LastAccessTimeUtc > threshold)
                {
                    file.Delete();
                    Console.WriteLine($"Удалён файл: {file.FullName}");
                }
            }

            foreach (DirectoryInfo subDirectory in directory.GetDirectories())
            {
                CleanFolder(subDirectory, threshold);

                if (subDirectory.GetFiles().Length == 0 && subDirectory.GetDirectories().Length == 0 &&
                    DateTime.UtcNow - subDirectory.LastAccessTimeUtc > threshold)
                {
                    subDirectory.Delete();
                    Console.WriteLine($"Удалена пустая папка: {subDirectory.FullName}");
                }
            }
        }
    }
}