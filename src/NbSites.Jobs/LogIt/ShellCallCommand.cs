using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Hangfire;

namespace NbSites.Jobs.LogIt
{
    public class ShellCallCommand
    {
        public BackupDbBatFile Args { get; set; }

        public Task Invoke(BackupDbBatFile backupBat)
        {
            if (backupBat == null)
            {
                throw new ArgumentException("参数不合法！必须提供非空的参数: " + typeof(BackupDbBatFile).Namespace);
            }

            var runResult = backupBat.Run();
            LogCommandHelper.Instance.Log(this.GetType(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " with args " + backupBat.FilePath);
            LogCommandHelper.Instance.Log(this.GetType(), runResult);

            return Task.CompletedTask;
        }

        public string Enqueue()
        {
            return BackgroundJob.Enqueue(() => Invoke(Args));
        }
    }

    public class BackupDbBatFile
    {
        public string FilePath { get; set; }

        public string Run()
        {
            if (!File.Exists(FilePath))
            {
                return "shell run failed: not exist: " + FilePath;
            }

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = FilePath,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();

            //process.StandardOutput.BaseStream.Flush();
            //var result = process.StandardOutput.ReadToEnd();

            //解决UTF-8的bat文件，中文乱码的问题
            using var reader = new StreamReader(process.StandardOutput.BaseStream, System.Text.Encoding.UTF8, true);
            reader.BaseStream.Flush();
            var result = reader.ReadToEnd();

            process.WaitForExit();
            return result;
        }
    }
}
