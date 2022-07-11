using Microsoft.VisualBasic.Devices;
using System;
using System.Diagnostics;

namespace Terminal
{
    class ResourceHandler
    {
        PerformanceCounter cpuCounter;
        PerformanceCounter ramCounter;
        int pollingRate = 500;
        int ramAmount;
        public ResourceHandler()
        {
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            ramAmount = Convert.ToInt32(new ComputerInfo().TotalPhysicalMemory / 1000000);
        }

        public int GetRAMAmount()
        {
            return ramAmount;
        }

        public string GetRAMUsage()
        {
            return ((GetRAMAmount() - ramCounter.NextValue()) / GetRAMAmount() * 100).ToString();
        }

        public string GetCPUUsage()
        {
            return cpuCounter.NextValue().ToString();
        }

        public void SetPollingRate(int pollingRate)
        {
            this.pollingRate = pollingRate;
        }

        public int GetPollingRate()
        {
            return pollingRate;
        }
    }
}
