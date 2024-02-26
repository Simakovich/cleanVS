using TextCopy;

namespace DeleteFolders
{
    public class Programm
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Start Deleting bin/obj!");

            Console.Write("Remove bin/obj from: ");

            var path = string.Empty;
            string clipboardText = await ClipboardService.GetTextAsync();

            if (!string.IsNullOrEmpty(clipboardText) && IsValidPath(clipboardText))
            {
                Console.WriteLine("Текст из буфера обмена:");
                Console.WriteLine(clipboardText);
                path = clipboardText;
            }
            else
            {
                if(args != null && args.Any() && IsValidPath(args[0]))
                {
                    path = args[0];
                }
                else
                {
                    path = Console.ReadLine() ?? ".//..//";
                }
            }

            ////var customPath = Console
            var root = new DirectoryInfo(path);
            Console.WriteLine(root.FullName);

            Walk(path);
            Console.WriteLine("Done.");
            Console.WriteLine();
            Console.WriteLine();
            Console.ReadKey();
        }
        
        private static void Walk(string path)
        {
            if (Directory.Exists(path))
            {
                var root = new DirectoryInfo(path);
                DirectoryInfo[] subDirs = null;

                //получаем все подкаталоги
                try
                {
                    subDirs = root.GetDirectories();
                    //проходим по каждому подкаталогу
                    foreach (DirectoryInfo dirInfo in subDirs)
                    {
                        if (dirInfo.Name.Equals("obj") || dirInfo.Name.Equals("bin") || dirInfo.Name.Equals("LocalBuild") || dirInfo.Name.Equals(".vs") || dirInfo.Name.Equals(".idea"))
                        {
                            try
                            {
                                Directory.Delete(dirInfo.FullName, true);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(dirInfo.FullName + ":" + ex.Message);
                            }
                        }
                        else
                        {
                            Walk(dirInfo.FullName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Fail went to " + root + ":" + ex.Message);
                }
            }
        }

        private static bool IsValidPath(string path)
        {
            // Проверка на корректность формата пути
            if (string.IsNullOrWhiteSpace(path) || Path.GetInvalidPathChars().Any(c => path.Contains(c)))
            {
                return false;
            }

            // Проверка на существование пути
            return Directory.Exists(path) || File.Exists(path);
        }
    }
}