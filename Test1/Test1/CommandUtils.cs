using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using NUnit.Framework;

namespace Test1
{
    /// <summary>
	/// Class to help us execute programs/commands in a shell-like way.
	/// </summary>
	public static class CommandUtils
    {

        /// <summary>
        /// Executes the shell command
        /// </summary>
        /// <returns>std:out of the executed program</returns>
        /// <param name="fullCommand">full command, including absolute path and parameters</param>
        public static string ExecuteShell(string fullCommand)
        {
            char[] split = { ' ' };
            string[] parts = fullCommand.Split(split, 2);

            return ExecuteShell(parts[0], parts.Length > 1 ? parts[1] : "");
        }

        /// <summary>
        /// Executes the shell.
        /// </summary>
        /// <returns>std:out of the executed program</returns>
        /// <param name="fileName">absolute path to the desired executable</param>
        /// <param name="arguments">command line arguments for the executable.</param>
        public static string ExecuteShell(string fileName, string arguments)
        {
            var proc = new Process();
            proc.EnableRaisingEvents = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.ErrorDataReceived += ErrorHandler;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.FileName = fileName;
            proc.StartInfo.Arguments = arguments;
            proc.Start();
            proc.BeginErrorReadLine();
            proc.WaitForExit();
            return proc.StandardOutput.ReadToEnd();
        }

        /// <summary>
        /// Handles any std:err sent by ExecuteShell method.
        /// It prints executing program info and message to console
        /// </summary>
        /// <param name="sendingProcess">Sending process.</param>
        /// <param name="errLine">Error line.</param>
        private static void ErrorHandler(object sendingProcess, DataReceivedEventArgs errLine)
        {
            if (!String.IsNullOrEmpty(errLine.Data))
            {
                var p = sendingProcess as Process;
                Console.WriteLine(new string('-', 50));
                Console.WriteLine("Error when executing program: {0} {1}", p.StartInfo.FileName, p.StartInfo.Arguments);
                Console.WriteLine("Message is:");
                Console.WriteLine(errLine.Data);
                Console.WriteLine(new string('-', 50));
            }
        }
        
        
        
        
        //CommandUtils.Execute Shell is a simple boilerplate method that uses System.Process and returns the process's std out as a string
        //Constants.AdbExecutable points to the Android Debug Bridge executable file
        //If you have multiple devices connected, you need to pass in a device ID. If you have only one device, no need to pass anything in.
        //public static void Unlock(string deviceId = null)
        //{
        //    string deviceString = deviceId == null ? String.Empty : "-s " + deviceId + " ";
        //    //get dumpsys for power stats which includes screen on/off info
        //    //string power = deviceId == null ?
        //        //CommandUtils.ExecuteShell(Constants.AdbExecutable, "shell dumpsys power") :
        //        //CommandUtils.ExecuteShell(Constants.AdbExecutable, deviceString + "shell dumpsys power");

        //    //checks if screen is on/off. Two versions for different android versions.
        //    if (power.Contains("mScreenOn=false") || power.Contains("Display Power: state=OFF"))
        //    {
        //        //Sends keycode for power on
        //        //var on_res = CommandUtils.ExecuteShell(Constants.AdbExecutable, deviceString + "shell input keyevent 26");
        //        //Sends keycode for menu button. This will unlock stock android lockscreen. 
        //        //Does nothing if lockscreen is disabled
        //        //var menu_res = CommandUtils.ExecuteShell(Constants.AdbExecutable, deviceString + "shell input keyevent 82");
        //        //Assert.True(on_res == String.Empty || menu_res == String.Empty,
        //        //"There was a problem turning on the screen, msg: {0}, {1}", on_res, menu_res);
        //    }
        //}



    }

    //// Scrolls a given ScrollView a given amount. Doesn't check the amount.
    //void ScrollBy(AppResult scrollView, float amount)
    //{
    //    app.DragCoordinates(
    //        scrollView.Rect.CenterX, scrollView.Rect.CenterY + amount * 0.5f,
    //        scrollView.Rect.CenterX, scrollView.Rect.CenterY - amount * 0.5f);
    //}

    //// Scrolls a given amount down (can be negative), paging the results
    //void ScrollDown(AppResult scrollView, float amount)
    //{
    //    const float buffer = 50f;

    //    var usefulRoom = scrollView.Rect.Height - buffer * 2;
    //    var sign = Math.Sign(amount);
    //    var distance = Math.Abs(amount);
    //    while (distance > usefulRoom)
    //    {
    //        ScrollBy(scrollView, usefulRoom * sign);
    //        distance -= usefulRoom;
    //    }
    //    if (distance > 20f)
    //        ScrollBy(scrollView, distance * sign);
    //}

    //// Scrolls a given UIScrollView to center on the given element
    //// This is the usual entry point.
    //void ScrollToElement(AppResult scrollView, AppResult element)
    //{
    //    var amount = element.Rect.CenterY - scrollView.Rect.CenterY;
    //    ScrollDown(scrollView, amount);
    //}

}
