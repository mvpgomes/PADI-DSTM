using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataServer
{
    class SayIamAlive
    {
        private int period;
        private int delay;
        private Timer timerReference;

        /**
         * SayImAlive single instance  
         **/
        private static SayIamAlive instance = null;

        public SayIamAlive(){}

        public Timer TimerReference
        {
            get { return timerReference; }
            set { timerReference = value; }
        }


        public int Delay
        {
            get { return delay; }
            set { delay = value; }
        }

        public int Period
        {
            get { return period; }
            set { period = value; }
        }

        public static SayIamAlive getInstance()
        {
            if (instance == null)
            {
                instance = new SayIamAlive();
            }

            return instance;
        }

        private void TimerTask(object StateObject)
        {
            // This function is responsible to send
            // I'm alives to the replica
        }

        public void execute()
        {
            // At this function the TimerTask is launched
            // and its period is setup to execute in Period
            // intervals.
        }

    }
}
