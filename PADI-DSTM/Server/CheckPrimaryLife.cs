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
        private Timer timer;

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
            this.timer = new Timer();
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

        public Timer Timer
        {
            get { return timer; }
            set { timer = value; }
        }

        public static CheckPrimaryLife getInstance()
        {
            if (instance == null)
            {
                instance = new CheckPrimaryLife(false);
            }

            return instance;
        }

        public void execute()
        {

        }


       


        


    }
}
