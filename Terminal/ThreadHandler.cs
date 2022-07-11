using System;
using System.Threading;
using System.Collections.Generic;

namespace Terminal
{
    class ThreadHandler
    {
        private bool breakRequest = false;
        private Thread keyOverride;
        public void InputBreaker()
        {
            Console.ReadKey();
            breakRequest = true;
        }
        public bool GetBreakRequest()
        {
            return breakRequest;
        }

        public void StartKeyOverride()
        {
            keyOverride = new Thread(KeyOverride);
            {
                keyOverride.Name = "keyOverride";
            }
            keyOverride.Start();
        }
        public void EndKeyOverride()
        {
            keyOverride.Abort();
        }

        public void KeyOverride()
        {
            //while (true)
            //{
            //    ConsoleKeyInfo key = Console.ReadKey();
            //    switch (key.Key)
            //    {
            //        case ConsoleKey.Backspace:
            //            Console.Write("\b");
            //            break;
            //    }
            //}
        }
    }
}
