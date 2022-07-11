using System;
using System.Net.NetworkInformation;
using System.Threading;


namespace Terminal
{
    class NetworkHandler
    {
        public string IPConfig()
        {
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            string output = string.Empty;
            foreach (NetworkInterface networkInterface in interfaces)
            {
                string defaultGateway = string.Empty;
                string ipClass = string.Empty;
                switch (networkInterface.GetIPProperties().UnicastAddresses[0].IPv4Mask.ToString())
                {
                    case "255.0.0.0":
                        ipClass = "A";
                        break;

                    case "255.255.0.0":
                        ipClass = "B";
                        break;

                    case "255.255.255.0":
                        ipClass = "C";
                        break;
                }
                try
                {
                    defaultGateway = networkInterface.GetIPProperties().GatewayAddresses[0].Address.ToString();
                }
                catch
                {
                    defaultGateway = "";
                }
                output += "\n------------------------------------------------------\n";
                output += String.Format("| {0, -50} |", networkInterface.Name);
                output += "\n------------------------------------------------------\n";
                try
                {
                    output += String.Format("| {0, -50} |", "IP Address: " + networkInterface.GetIPProperties().UnicastAddresses[1].Address.ToString());
                }
                catch
                {
                    output += String.Format("| {0, -50} |", "IP Address: " + networkInterface.GetIPProperties().UnicastAddresses[0].Address.ToString());
                }
                output += String.Format("\n| {0, -45} |", "\tDefault Gateway: " + defaultGateway);
                output += String.Format("\n| {0, -45} |", "\tSubnet Mask: " + networkInterface.GetIPProperties().UnicastAddresses[0].IPv4Mask.ToString());
                output += String.Format("\n| {0, -45} |", "\tIP Class: " + ipClass);
                output += String.Format("\n| {0, -45} |", "\tOperational Status: " + networkInterface.OperationalStatus.ToString());
                output += "\n------------------------------------------------------\n\n";
            }
            return output;
        }

        public void Ping(string target, int times)
        {
            ConsoleHandler consoleHandler = new ConsoleHandler();
            string output = "\nPinging target " + target + " " + times + " times\n";
            try
            {
                ThreadHandler threadHandler = new ThreadHandler();
                Thread thread = new Thread(threadHandler.InputBreaker)
                {
                    Name = "inputBreaker"
                };
                thread.Start();
                int totalTime = 0;
                int totalPings = 0;
                for (int i = 0; i < times; i++)
                {
                    Ping ping = new Ping();
                    PingReply pingReply = ping.Send(target);
                    if (pingReply.Status == IPStatus.Success)
                    {
                        output += "Reply from " + target + ": Time = " + pingReply.RoundtripTime + "\n";
                        totalTime += Convert.ToInt32(pingReply.RoundtripTime);
                    }
                    else
                    {
                        output += "Target did not respond!\n";
                    }
                    consoleHandler.Output(output, ConsoleColor.White);
                    output = string.Empty;
                    Thread.Sleep(1000);
                    totalPings = i;
                    if (threadHandler.GetBreakRequest())
                    {
                        i = times;
                    }
                }
                consoleHandler.Output(output += "\nAverage ping time: " + Convert.ToInt32(totalTime / (totalPings + 1)) + "ms\n", ConsoleColor.White);
                thread.Abort();
            }
            catch
            {
                output += "Target was invalid!\n";
                consoleHandler.Output(output, ConsoleColor.White);
            }
        }

        public void NetCheck(string target)
        {

        }
    }
}
