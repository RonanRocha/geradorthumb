using ImageMagick;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeradorThumb
{
    class Program
    {
        static void Main(string[] args)
        {


            Console.WriteLine("Add a a path of files ==================================");

            var path = Console.ReadLine();

            var files = GetFilesFromFolder(path);

            RealizeBackup(path);
            ResizeImages(files, path);

        }


        private static void RealizeBackup(string path)
        {
            var backupPath = path + "\\backup";

            if (!System.IO.Directory.Exists(backupPath))
            {
                System.IO.Directory.CreateDirectory(backupPath);
            }


            Console.WriteLine("Realizing backup of files ================================== waiting");


            if (System.IO.Directory.Exists(path))
            {
                string[] files = System.IO.Directory.GetFiles(path);

                // Copy the files and overwrite destination files if they already exist.
                foreach (string s in files)
                {

                    var fileName = System.IO.Path.GetFileName(s);
                    var destFile = System.IO.Path.Combine(backupPath, fileName);
                    System.IO.File.Copy(s, destFile, true); // cuidado está sobreescrevendo aqui caso você já tenha rodado
                }
            }


            Console.WriteLine("Realizes backup of files  \\o");

            
        }

        private static FileInfo[] GetFilesFromFolder(string path)
        {
            string[] extensions = new[] { ".jpeg", ".jpg", ".png" };
            DirectoryInfo dInfo = new DirectoryInfo(path);

            FileInfo[] files = dInfo.GetFiles()
                .Where(f => extensions.Contains(f.Extension.ToLower()))
                .ToArray();
            return files;
        }

        public static void ResizeImages(FileInfo[] Files, string path)
        {
            using (MagickImageCollection collection = new MagickImageCollection())
            {

                foreach (var file in Files)
                {

                    Console.WriteLine($"Add {file.Name} to magick \n");

                    // Add the first image
                    MagickImage image = new MagickImage(file);
                    image.Format = MagickFormat.Jpg;
                    image.Depth = 8;
                    collection.Add(image);
                }


                Console.WriteLine($"all files are added ====================================================== \n");


                //if (!Directory.Exists($"{path}\\200x200"))

                //    Console.WriteLine($"creating path thumbs \n");

                //Directory.CreateDirectory($"{path}\\200x200");

                //if (!Directory.Exists($"{path}\\60x60"))

                //    Console.WriteLine($"creating path thumbs \n");

                //Directory.CreateDirectory($"{path}\\60x60");



                foreach (MagickImage image in collection)
                {

                    //var teste = image.FileName.Split('\\');
                    //var filename = teste[teste.Length - 1];


                    //replace image caution  \0 aehooooo 
                    image.Resize(400, 400);
                    image.Write(image.FileName);


                    //image.Resize(200,200);
                    //image.Write($"{path}\\200x200\\200x200-" + filename);


                    //image.Resize(60, 60);
                    //image.Write($"{path}\\60x60\\60x60-" + filename);

                    Console.WriteLine($"adding {image.FileName} =============== wating \n");
                }
            }
        }
    }
}
