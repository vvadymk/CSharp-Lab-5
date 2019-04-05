using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Management;

namespace KochmarLab5
{
    internal class Process
    {


        internal PerformanceCounter RamCounter { get; }

        internal PerformanceCounter CpuCounter { get; }

        public string Name
        {
            get;
        }

        public int Id
        {
            get;
        }

        public bool IsActive
        {
            get;
        }

        public int CpuTaken
        {
            get;
            set;
        }

        public int RamTaken
        {
            get;
            set;
        }

        public int ThreadsNumber
        {
            get;
            set;
        }

        public string Username
        {
            get;
        }

        public string FilePath
        {
            get;
        }

        public string RunOn
        {
            get;
        }


        internal Process(System.Diagnostics.Process systemProcess)
        {
            RamCounter = new PerformanceCounter("Process", "Working Set", systemProcess.ProcessName);
            CpuCounter = new PerformanceCounter("Process", "% Processor Time", systemProcess.ProcessName);
            Name = systemProcess.ProcessName;
            Id = systemProcess.Id;
            IsActive = systemProcess.Responding;
            CpuTaken = (int)CpuCounter.NextValue();
            RamTaken = (int)(RamCounter.NextValue() / 1024 / 1024);
            ThreadsNumber = systemProcess.Threads.Count;
            Username = GetProcessOwner(systemProcess.Id);
            try
            {
                FilePath = systemProcess.MainModule.FileName;
            }
            catch (Win32Exception e)
            {
                FilePath = e.Message;
            }
            try
            {
                RunOn = systemProcess.StartTime.ToString();
            }
            catch (Win32Exception e)
            {
                RunOn = e.Message;
            }
        }

        private static string GetProcessOwner(int processId)
        {
            string query = "Select * From Win32_Process Where ProcessID = " + processId;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection processList = searcher.Get();

            foreach (ManagementObject obj in processList)
            {
                string[] argList = { string.Empty, string.Empty };
                int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                if (returnVal == 0)
                {
                    return argList[1] + "\\" + argList[0];
                }
            }

            return "";
        }
    }
}
