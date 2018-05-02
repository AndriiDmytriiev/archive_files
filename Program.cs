using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Net.Mail;
using System.Security.Principal;

namespace ConsoleApplication
{
    class Program
    {
        static List<string> files = new List<string>();
        static List<string> list_exceptions = new List<string>();
        private static object MessageBox;
        public static Boolean IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                //Don't change FileAccess to ReadWrite, 
                //because if a file is in readOnly, it fails.
                stream = file.Open
                (
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.None
                );
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }



        static void getFilesRecursive(string sDir)
        {
            try
            {
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    getFilesRecursive(d);
                }
                foreach (var file in Directory.GetFiles(sDir))
                {
                    //This is where you would manipulate each file found, e.g.:
                    DoAction(file);
                }
            }
            catch (System.Exception e)
            {
                list_exceptions.Add(e.Message);
            }
        }

        static void DoAction(string filepath)
        {
            files.Add(filepath);
        }

        private static void SendEmail(string emailBody)
        {
            

            MailAddress from = new MailAddress("Someone@mail1.com", "SomeOne");
            MailAddress to = new MailAddress("info731408@ukr.net", "AppSupport");
            MailMessage message = new MailMessage(from, to);
            message.Subject = "Archive files";
            message.Body = emailBody;
           //message.Bcc.Add(bcc);
            SmtpClient client = new SmtpClient("smtp.gmail.com");
            client.Credentials = new System.Net.NetworkCredential("sendemails", "888787");
            client.EnableSsl = true;
            Console.WriteLine("Sending an e-mail message to {0} and {1}.",
                to.DisplayName, message.Bcc.ToString());
            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in CreateBccTestMessage(): {0}",
                            ex.ToString());
            }
        }

        static void Main(string[] args)
        {

           
            try
            {

              
                string startPath;
                string INIfolderPath;
                string INIfilePath;

                INIfolderPath = System.IO.Directory.GetCurrentDirectory();
                INIfolderPath = INIfolderPath + "\\data.ini";

                string zipPath;
                string fileNames = "";
                string extractPath;
                string s = "";
                string dt = DateTime.Now.ToShortDateString();
                dt = dt.Replace(".", "_");
                dt = dt.Replace(":", "_");
                using (StreamReader sr = File.OpenText(INIfolderPath))
                {   //ini data file
                    //1st line: source log files folder
                    s = sr.ReadLine();
                    var rx = new System.Text.RegularExpressions.Regex(";");
                    var array = rx.Split(s);

                    //if we have several log folders
                    if (array.Length > 1)
                    {
                        for (int i = 0; i < array.Length; i++)
                        {
                            startPath = array[i];
                            fileNames =  Environment.NewLine + fileNames + Environment.NewLine;
                            zipPath = startPath + "\\" + "archive_" + dt + "_" + i + ".zip";
                            fileNames = fileNames + zipPath + ":" + Environment.NewLine;
                            s = sr.ReadLine();
                            extractPath = s;

                            string newFile;
                                                        
                            using (ZipArchive archive = ZipFile.Open(zipPath, ZipArchiveMode.Update))
                            {
                                //archive.CreateEntryFromFile(newFile, "NewEntry.txt");
                            }
                            DateTime TodayDate = DateTime.Now;
                            DateTime WeekAgo = TodayDate.AddDays(-7);
                            foreach (string file in Directory.EnumerateFiles(startPath, "*.*"))
                            {
                                newFile = file;
                                using (ZipArchive archive = ZipFile.Open(zipPath, ZipArchiveMode.Update))
                                {
                                    string fileName = file.ToString().TrimEnd();

                                    string result;

                                    result = Path.GetFileName(fileName);
                                    DateTime fileCreatedDate = File.GetLastWriteTime(fileName);
                                    if (fileCreatedDate < WeekAgo)

                                    {
                                        long length = new System.IO.FileInfo(file).Length;
                                        if (length < 300000000)
                                        {
                                            archive.CreateEntryFromFile(newFile, result);
                                            File.Delete(file);
                                            fileNames = fileNames + Environment.NewLine + file + "; ";
                                        }    
                                    }
                                   
                                }
                                
                            }

                          
                        }

                    }
                    else
                    {   
                        startPath = array[0];
                        s = sr.ReadLine();
                        //3rd line: zip file
                        zipPath = startPath + "\\" + "archive_"  + dt + ".zip";
                        fileNames = Environment.NewLine + fileNames + zipPath + ":" + Environment.NewLine;
                        s = sr.ReadLine();
                        extractPath = s;
                        string newFile;

                       

                        using (ZipArchive archive = ZipFile.Open(zipPath, ZipArchiveMode.Update))
                        {
                            //archive.CreateEntryFromFile(newFile, "NewEntry.txt");
                        }
                        DateTime TodayDate = DateTime.Now;
                        DateTime WeekAgo = TodayDate.AddDays(-7);
                        foreach (string file in Directory.EnumerateFiles(startPath, "*.*"))
                        {
                            newFile = file;


                            //if (Directory.Exists("FOLDER_PATH"))
                            //{
                            //    var directory = new DirectoryInfo("FOLDER_PATH");
                            //    foreach (FileInfo file1 in directory.GetFiles())
                            //    {
                            //        if (!IsFileLocked(file1)) file1.Delete();
                            //    }
                            //}


                            using (ZipArchive archive = ZipFile.Open(zipPath, ZipArchiveMode.Update))
                            {


                                string fileName = file.ToString().TrimEnd();
                                
                                string result;

                                result = Path.GetFileName(fileName);
                                DateTime fileCreatedDate = File.GetLastWriteTime(fileName);
                                //string str = null;
                                //string retString = null;

                                //retString = fileName.Substring(fileName.Length - startPath.Length-2);
                                //retString = str.Substring(0, 3);
                                if (fileCreatedDate < WeekAgo)

                                {
                                    var directory = new DirectoryInfo(startPath);
                                   // foreach (FileInfo file1 in directory.GetFiles())
                                    {
                                        
                                        
                                        //if (!IsFileLocked(file1) //&& (result ==file1.ToString()))

                                        //file1.Delete();
                                        {
                                            long length = new System.IO.FileInfo(file).Length;
                                            if (length < 300000000)
                                            {
                                                archive.CreateEntryFromFile(newFile, result);
                                                File.Delete(file);
                                                fileNames = fileNames + Environment.NewLine + file + "; ";
                                            }
                                        }
                                    }
                                }

                            }

                        }

                    }
                }



                SendEmail("Server:" + Environment.MachineName + " " + (char)13 + (char)10 + fileNames);


            }
            catch (IOException e)
            {
                // Extract some information from this exception, and then   
                // throw it to the parent method.  
                if (e.Source != null)
                    Console.WriteLine("IOException source: {0}", e.Source);
                throw;
            }
        }
    }
}
