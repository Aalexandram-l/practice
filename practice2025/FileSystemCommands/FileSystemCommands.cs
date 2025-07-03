using CommandLib;
using System;
using System.IO;

namespace FileSystemCommands
{
    public class DirectorySizeCommand : ICommand
    {
        private string folderPath;
        
        public DirectorySizeCommand(string path)
        {
            folderPath = path;
        }
        
        public void Execute()
        {
            if (!Directory.Exists(folderPath))
            {
                Console.WriteLine("Directory not found"); 
                return;
            }

            long size = 0;
            string[] files = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);
            
            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                size += fileInfo.Length;
            }
            
            Console.WriteLine($"Directory size: {size} bytes"); 
        }
    }

    public class FindFilesCommand : ICommand
    {
        private string folderPath;
        private string filePattern;
        
        public FindFilesCommand(string path, string pattern)
        {
            folderPath = path;
            filePattern = pattern;
        }
        
        public void Execute()
        {
            if (!Directory.Exists(folderPath))
            {
                Console.WriteLine("Directory not found"); 
                return;
            }

            string[] files = Directory.GetFiles(folderPath, filePattern, SearchOption.AllDirectories);
            
            Console.WriteLine($"Found {files.Length} files:"); 
            foreach (string file in files)
            {
                Console.WriteLine(file);
            }
        }
    }
}