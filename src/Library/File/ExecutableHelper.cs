using Microservice.Library.Extension;
using Microservice.Library.Extension.Helper;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Library.File
{
    /// <summary>
    /// 可执行文件帮助类
    /// </summary>
    public static class ExecutableHelper
    {
        /// <summary>
        /// 获取进程
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="arguments">启动参数</param>
        /// <param name="workingDirectory">工作目录</param>
        /// <param name="outputEncoding">输出流编码</param>
        /// <param name="errorEncoding">错误流编码</param>
        /// <returns></returns>
        public static Process GetProcess(string filename, string arguments, string workingDirectory = null, Encoding outputEncoding = null, Encoding errorEncoding = null)
        {
            var process = new Process();
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            if (outputEncoding != null)
                process.StartInfo.StandardOutputEncoding = outputEncoding;
            process.StartInfo.RedirectStandardError = true;
            if (errorEncoding != null)
                process.StartInfo.StandardErrorEncoding = errorEncoding;

            process.StartInfo.FileName = filename;

            process.StartInfo.Arguments = arguments;
            if (!workingDirectory.IsNullOrWhiteSpace())
                process.StartInfo.WorkingDirectory = workingDirectory;

            return process;
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="filename">文件</param>
        /// <param name="arguments">启动参数</param>
        /// <param name="input">输入内容</param>
        /// <param name="workingDirectory">工作目录</param>
        /// <param name="outputEncoding">输出流编码</param>
        /// <param name="errorEncoding">错误流编码</param>
        /// <returns>(<see cref="Process.StandardOutput"/>, <see cref="Process.StandardError"/>, <see cref="Process.ExitCode"/>)</returns>
        public static (string output, string error, int exitCode) SimpleCall(string filename, string arguments = null, string input = null, string workingDirectory = null, Encoding outputEncoding = null, Encoding errorEncoding = null)
        {
            var output = string.Empty;
            var error = string.Empty;
            var exitCode = 0;

            void outputReadLine(string value) => output += value + "\r\n";

            void errorReadLine(string value) => error += value + "\r\n";

            if (input != null)
            {
                void getInputWriteLine(Action<string> inputWriteLine) => inputWriteLine.Invoke(input);

                exitCode = Call(filename, arguments, getInputWriteLine, outputReadLine, errorReadLine, workingDirectory, outputEncoding, errorEncoding);
            }
            else
                exitCode = Call(filename, arguments, null, outputReadLine, errorReadLine, workingDirectory, outputEncoding, errorEncoding);

            return (output, error, exitCode);
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="filename">文件</param>
        /// <param name="arguments">启动参数</param>
        /// <param name="getInputWriteLine">读取输入内容</param>
        /// <param name="outputReadLine">写入输出</param>
        /// <param name="errorReadLine">写入错误</param>
        /// <param name="workingDirectory">工作目录</param>
        /// <param name="outputEncoding">输出流编码</param>
        /// <param name="errorEncoding">错误流编码</param>
        /// <returns><see cref="Process.ExitCode"/></returns>
        public static int Call(string filename, string arguments = null, Action<Action<string>> getInputWriteLine = null, Action<string> outputReadLine = null, Action<string> errorReadLine = null, string workingDirectory = null, Encoding outputEncoding = null, Encoding errorEncoding = null)
        {
            using (var process = GetProcess(filename, arguments, workingDirectory, outputEncoding, errorEncoding))
            {
                process.Start();

                if (getInputWriteLine != null)
                {
                    process.StandardInput.AutoFlush = true;
                    void inputWriteLine(string value)
                    {
                        process.StandardInput.WriteLine(value);
                        process.StandardInput.Flush();
                    }
                    getInputWriteLine.Invoke(inputWriteLine);
                }

                var writeOutputTask = outputReadLine == null ?
                    Task.CompletedTask :
                    Task.Run(() =>
                    {
                        while (!process.StandardOutput.EndOfStream)
                        {
                            var output = process.StandardOutput.ReadLine();
                            if (output == null)
                                continue;
                            outputReadLine.Invoke(output);
                        }
                    });

                var writeErrorTask = errorReadLine == null ?
                    Task.CompletedTask :
                    Task.Run(() =>
                    {
                        while (!process.StandardError.EndOfStream)
                        {
                            var error = process.StandardError.ReadLine();
                            if (error == null)
                                continue;
                            errorReadLine.Invoke(error);
                        }
                    });

                //var output = string.Empty;
                //var error = string.Empty;

                //var outputTaskAwaiter = process.StandardOutput.ReadToEndAsync().GetAwaiter();
                //outputTaskAwaiter.OnCompleted(() => { output = outputTaskAwaiter.GetResult(); });
                //var errorTaskAwaiter = process.StandardError.ReadToEndAsync().GetAwaiter();
                //errorTaskAwaiter.OnCompleted(() => { error = errorTaskAwaiter.GetResult(); });

                process.WaitForExit();

                Task.WaitAll(writeOutputTask, writeErrorTask);

                return process.ExitCode;
            }
        }

        /// <summary>
        /// 获取命令行文件名
        /// </summary>
        /// <returns></returns>
        public static string GetCmdFilename()
        {
            if (SystemInfoHelper.CurrentOS == OSPlatform.Windows)
                return "cmd.exe";
            else if (SystemInfoHelper.CurrentOS == OSPlatform.Linux || SystemInfoHelper.CurrentOS == OSPlatform.OSX)
                return Environment.GetEnvironmentVariable("SHELL");
            else
                throw new ApplicationException($"不支持在当前操作系统{SystemInfoHelper.CurrentOS}执行此操作.");
        }

        /// <summary>
        /// 获取命令行进程
        /// </summary>
        /// <param name="arguments">启动参数</param>
        /// <param name="workingDirectory">工作目录</param>
        /// <param name="outputEncoding">输出流编码</param>
        /// <param name="errorEncoding">错误流编码</param>
        /// <returns></returns>
        public static Process GetCmdProcess(string arguments, string workingDirectory = null, Encoding outputEncoding = null, Encoding errorEncoding = null)
        {
            return GetProcess(GetCmdFilename(), arguments, workingDirectory, outputEncoding, errorEncoding);
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <param name="arguments">参数</param>
        /// <param name="workingDirectory">工作目录</param>
        /// <param name="outputEncoding">输出流编码</param>
        /// <param name="errorEncoding">错误流编码</param>
        /// <returns>(<see cref="Process.StandardOutput"/>, <see cref="Process.StandardError"/>, <see cref="Process.ExitCode"/>)</returns>
        public static (string output, string error, int exitCode) CallCmd(string cmd, string arguments = null, string workingDirectory = null, Encoding outputEncoding = null, Encoding errorEncoding = null)
        {
            return SimpleCall(GetCmdFilename(), arguments, $"{cmd.TrimEnd('&')}&exit", workingDirectory, outputEncoding, errorEncoding);
        }
    }
}
