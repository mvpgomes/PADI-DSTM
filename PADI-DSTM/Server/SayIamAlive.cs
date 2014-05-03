using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;

namespace DataServer
{
    class SayIamAlive
    {
        private int period;
        private int delay;
        private Timer timer;

        /**
         * SayImAlive single instance  
         **/
        private static SayIamAlive instance = null;

        public SayIamAlive()
        {
            this.Timer = new Timer();
        }

        public Timer Timer
        {
            get { return timer; }
            set { timer = value; }
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

        public void execute()
        {

        }

    }
}
