using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;

namespace DataServer
{
    class CheckPrimaryLife
    {
        private int period;
        private int delay;
        private bool isAlive;   
        private Timer timerReference;

        /**
         *  Single instance for the CheckPrimaryLife object
         **/
        private static CheckPrimaryLife instance = null;

        /**
         * CheckPrimaryLife constructor
         * 
         * @param bool isAlive
         **/
        public CheckPrimaryLife(bool isAlive)
        {
            this.isAlive = isAlive;
        }

        public int Period
        {
            get { return period; }
            set { period = value; }
        }

        public int Delay
        {
            get { return delay; }
            set { delay = value; }
        }

        public bool IsAlive
        {
            get { return isAlive; }
            set { isAlive = value; }
        }

        public Timer TimerReference
        {
            get { return timerReference; }
            set { timerReference = value; }
        }

        public static CheckPrimaryLife getInstance()
        {
            if (instance == null)
            {
                instance = new CheckPrimaryLife(false);
            }

            return instance;
        }

        private void TimerTask(object StateObject) 
        {
            // This function is responsible to verify if
            // the primary DataServer is alive.
        }

        public void execute()
        {
            // At this function the TimerTask is launched
            // and its period is setup to execute in Period
            // intervals.
        }


       


        


    }
}
