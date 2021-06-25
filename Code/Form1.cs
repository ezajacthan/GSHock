using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace GShock
{
    public partial class GShock : Form
    {
        /// <summary>
        /// delegate declaration to hold operations performed by the clock
        /// </summary>
        public delegate void operation();

        //struct to hold entries in the state table
        public struct State
        {
            /// <summary>
            /// The next state to jump to
            /// </summary>
            public short nextState;

            /// <summary>
            /// The operation to be performed while in the current state
            /// </summary>
            public operation funDel;

            /// <summary>
            /// Creates a new State object 
            /// </summary>
            /// <param name="ns">The next state for the new obj</param>
            /// <param name="f">The operation to be performed when the new obj is reached</param>
            public State(short ns, operation f)
            {
                nextState = ns;
                funDel = f;
            }
        }

        const short CLOCKDISPLAY = 0;
        const short HOURSET = 1;
        const short MINSET = 2;
        const short SECSET = 3;
        const short STOPWATCHSTOP = 4;
        const short STOPWATCHRUN = 5;
        const short ALARMDISPLAY = 6;
        const short ALARMHOURSET = 7;
        const short ALARMMINSET = 8;
        const short ALARMSECSET = 9;

        private short currentState;
        private short hrs;
        private short milHrs;
        private short mins;
        private short secs;
        private short alarmHrs;
        private short alarmMilHrs;
        private short alarmMins;
        private short alarmSecs;
        private Stopwatch stopwatch;
        private bool isMilitaryTime;
        private bool isAlarmOn;
        private bool isAlarmRinging;
        private bool isLit = false;
        private Timer clockTimer;
        private Timer lightTimer;
        private Timer stopwatchTimer;
        private Timer alarmTimer;
        private DateTime lightTime;

        private void nop() { }
        operation NoOp; 

        private void LightDisplay() { isLit = true; lightTime = DateTime.UtcNow; }
        operation LightUp;

        private void ToggleMilitaryTime() 
        {
            isMilitaryTime = !isMilitaryTime;
            pmBox.Visible = !pmBox.Visible;
            textBox1.Text = GetTimeString();
        }
        operation ToggleMilTime;

        private void ToggleAlarmOnOff() 
        { 
            isAlarmOn = !isAlarmOn; 
            alarmBox.Checked = !alarmBox.Checked; 
        }
        operation ToggleAlarm;

        private void IncrementHrs() 
        { 
            milHrs++; 
            hrs++;
            if (hrs > 12) hrs = 1;
            if (milHrs > 23) milHrs = 0;
            textBox1.Text = GetTimeString();
        }
        operation IncHours;

        private void DecrementHours() 
        { 
            milHrs--; 
            hrs--;
            if (hrs < 1) hrs = 12;
            if (milHrs < 0) milHrs = 23;
            textBox1.Text = GetTimeString();
            TimeWrap();
        }
        operation DecHours;

        private void IncrementMins() 
        { 
            mins++;
            if (mins > 59) mins = 0;
            textBox1.Text = GetTimeString();
        }
        operation IncMins;

        private void DecrementMins() 
        {   
            mins--;
            if (mins < 0) mins = 59;
            textBox1.Text = GetTimeString();
        }
        operation DecMins;

        private void IncrementSecs() 
        { 
            secs++;
            if (secs > 59) secs = 0;
            textBox1.Text = GetTimeString();
        }
        operation IncSecs;

        private void DecrementSecs() 
        { 
            secs--;
            if (secs < 0) secs = 59;
            textBox1.Text = GetTimeString();
            TimeWrap();
        }
        operation DecSecs;

        private void IncrementAlarmHrs() 
        { 
            alarmHrs++;
            alarmMilHrs++;
            if (alarmHrs > 12) alarmHrs = 1;
            if (alarmMilHrs > 23) alarmMilHrs = 0;
            textBox1.Text = GetAlarmTimeString();
        }
        operation IncAlarmHours;

        private void DecrementAlarmHours() 
        { 
            alarmHrs--;
            alarmMilHrs--;
            if (alarmMilHrs < 0) alarmMilHrs = 23;
            if (alarmHrs < 1) alarmHrs = 12;
            textBox1.Text = GetAlarmTimeString();
        }
        operation DecAlarmHours;

        private void IncrementAlarmMins() 
        { 
            alarmMins++;
            if (alarmMins > 59) alarmMins = 0;
            textBox1.Text = GetAlarmTimeString();
        }
        operation IncAlarmMins;

        private void DecrementAlarmMins() 
        { 
            alarmMins--;
            if (alarmMins < 0) alarmMins = 59;
            textBox1.Text = GetAlarmTimeString();
        }
        operation DecAlarmMins;

        private void IncrementAlarmSecs() 
        { 
            alarmSecs++;
            if (alarmSecs > 59) alarmSecs = 0;
            textBox1.Text = GetAlarmTimeString();
        }
        operation IncAlarmSecs;

        private void DecrementAlarmSecs() 
        { 
            alarmSecs--;
            if (alarmSecs < 0) alarmSecs = 59;
            textBox1.Text = GetAlarmTimeString();
        }
        operation DecAlarmSecs;

        private void StartStopwatch() 
        {
            stopwatch.Start();
            stopwatchTimer.Start(); 
        }
        operation StartSW;

        private void StopStopwatch() { stopwatch.Stop(); stopwatchTimer.Stop(); }
        operation StopSW;

        private void ResetStopwatch() 
        { 
            StopStopwatch();
            //reset stopwatch count
            stopwatch = new Stopwatch();
            textBox1.Text = GetStopwatchTimeString();
        }
        operation ResetSW;

        private void RecordLap() { }
        operation LapRecord;


        private State[,] StateTable;

        /// <summary>
        /// Constructs a GShock object
        /// </summary>
        public GShock()
        {
            //initialize clock, alarm, stopwatch
            hrs = 12;
            milHrs = hrs;
            mins = 59;
            secs = 58;
            alarmHrs = 7;
            alarmMilHrs = alarmHrs;
            alarmMins = 30;
            alarmSecs = 0;
            isMilitaryTime = false;
            isAlarmOn = false;
            stopwatch = new Stopwatch();

            //generate timer for clock update
            clockTimer = new Timer();
            clockTimer.Tick += new EventHandler(ClockTimerEventHandler);
            clockTimer.Interval = 1000;
            clockTimer.Start();

            //generate timer for lighting up screen
            lightTimer = new Timer();
            lightTimer.Tick += new EventHandler(LightTimerEventHandler);
            lightTimer.Interval = 100;
            lightTimer.Start();

            //generate timer for stopwatch
            stopwatchTimer = new Timer();
            stopwatchTimer.Tick += new EventHandler(StopwatchTimerEventHandler);
            stopwatchTimer.Interval = 1;

            //generate timer for alarm
            alarmTimer = new Timer();
            alarmTimer.Tick += new EventHandler(AlarmTimerEventHandler);
            alarmTimer.Interval = 500;

            //initialize delegates
            NoOp = new operation(nop);
            LightUp = new operation(LightDisplay);
            ToggleMilTime = new operation(ToggleMilitaryTime);
            IncHours = new operation(IncrementHrs);
            DecHours = new operation(DecrementHours);
            IncMins = new operation(IncrementMins);
            DecMins = new operation(DecrementMins);
            IncSecs = new operation(IncrementSecs);
            DecSecs = new operation(DecrementSecs);
            IncAlarmHours = new operation(IncrementAlarmHrs);
            DecAlarmHours = new operation(DecrementAlarmHours);
            IncAlarmMins = new operation(IncrementAlarmMins);
            DecAlarmMins = new operation(DecrementAlarmMins);
            IncAlarmSecs = new operation(IncrementAlarmSecs);
            DecAlarmSecs = new operation(DecrementAlarmSecs);
            ToggleAlarm = new operation(ToggleAlarmOnOff);
            StartSW = new operation(StartStopwatch);
            StopSW = new operation(StopStopwatch);
            ResetSW = new operation(ResetStopwatch);
            LapRecord = new operation(RecordLap);

            //initialize state data
            currentState = CLOCKDISPLAY;

            //initialize state table
            StateTable = new State[10, 4]
                {
                    { new State(CLOCKDISPLAY, LightUp),  new State(CLOCKDISPLAY, ToggleMilTime), new State(HOURSET, NoOp),               new State(STOPWATCHSTOP, NoOp),},
                    { new State(HOURSET, LightUp),       new State(HOURSET, IncHours),           new State(HOURSET, DecHours),           new State(MINSET, NoOp),},
                    { new State(MINSET, LightUp),        new State(MINSET, IncMins),             new State(MINSET, DecMins),             new State(SECSET, NoOp),},
                    { new State(SECSET, LightUp),        new State(SECSET, IncSecs),             new State(SECSET, DecSecs),             new State(CLOCKDISPLAY, NoOp),},
                    { new State(STOPWATCHSTOP, LightUp), new State(STOPWATCHRUN, StartSW),       new State(STOPWATCHSTOP, ResetSW),      new State(ALARMDISPLAY, NoOp),},
                    { new State(STOPWATCHRUN, LightUp),  new State(STOPWATCHSTOP, StopSW),       new State(STOPWATCHRUN, LapRecord),     new State(STOPWATCHRUN, NoOp),},
                    { new State(ALARMDISPLAY, LightUp),  new State(ALARMDISPLAY, ToggleAlarm),   new State(ALARMHOURSET, NoOp),          new State(CLOCKDISPLAY, NoOp),},
                    { new State(ALARMHOURSET, LightUp),  new State(ALARMHOURSET, IncAlarmHours), new State(ALARMHOURSET, DecAlarmHours), new State(ALARMMINSET, NoOp),},
                    { new State(ALARMMINSET, LightUp),   new State(ALARMMINSET, IncAlarmMins),   new State(ALARMMINSET, DecAlarmMins),   new State(ALARMSECSET, NoOp),},
                    { new State(ALARMSECSET, LightUp),   new State(ALARMSECSET, IncAlarmSecs),   new State(ALARMSECSET, DecAlarmSecs),   new State(ALARMDISPLAY, NoOp),},
                };

            InitializeComponent();
            this.BackColor = Color.DarkGray;
            b1.Click += new EventHandler(b1_Click);
            b2.Click += new EventHandler(b2_Click);
            b3.Click += new EventHandler(b3_Click);
            b4.Click += new EventHandler(b4_Click);
            pmBox.Enabled = false;
            if (!isMilitaryTime && milHrs > 11) pmBox.Checked = true;
            else pmBox.Checked = false;
            textBox1.Text = GetTimeString();
        }

        /// <summary>
        /// Function to convert time to a string
        /// </summary>
        /// <returns>string representation of the current time</returns>
        public string GetTimeString()
        {
            string hrsText = "";
            string milhrsText = "";
            string minsText = "";
            string secsText = "";

            if (milHrs < 10) milhrsText = "0" + milHrs.ToString();
            else milhrsText = milHrs.ToString();

            if (hrs < 10) hrsText = "0" + hrs.ToString();
            else hrsText = hrs.ToString();

            if (mins < 10) minsText = "0" + mins.ToString();
            else minsText = mins.ToString();

            if (secs < 10) secsText = "0" + secs.ToString();
            else secsText = secs.ToString();

            if(currentState < 4)
            {
                if (!isMilitaryTime && milHrs > 11) pmBox.Checked = true;
                else pmBox.Checked = false;
            }
            

            if (isMilitaryTime)
            {
                return milhrsText + ":" + minsText + ":" + secsText;
            }
            else return hrsText + ":" + minsText + ":" + secsText;
        }

        /// <summary>
        /// Function to convert time to a string
        /// </summary>
        /// <returns>string representation of the current time</returns>
        public string GetAlarmTimeString()
        {
            string hrsText = "";
            string milhrsText = "";
            string minsText = "";
            string secsText = "";

            if (alarmHrs < 10) hrsText = "0" + alarmHrs.ToString();
            else hrsText = alarmHrs.ToString();

            if (alarmMilHrs < 10) milhrsText = "0" + alarmMilHrs.ToString();
            else milhrsText = alarmMilHrs.ToString();

            if (alarmMins < 10) minsText = "0" + alarmMins.ToString();
            else minsText = alarmMins.ToString();

            if (alarmSecs < 10) secsText = "0" + alarmSecs.ToString();
            else secsText = alarmSecs.ToString();

            if(currentState > 5)
            {
                if (!isMilitaryTime && alarmMilHrs > 11) pmBox.Checked = true;
                else pmBox.Checked = false;
            }
            

            if (isMilitaryTime)
            {
                return milhrsText + ":" + minsText + ":" + secsText;
            }
            else return hrsText + ":" + minsText + ":" + secsText;
        }

        /// <summary>
        /// Gets the stopwatch time and outputs it as a formatted string
        /// </summary>
        /// <returns>the time on the stopwatch as a string</returns>
        public string GetStopwatchTimeString()
        {
            TimeSpan elapsed = this.stopwatch.Elapsed;
            return string.Format("{0:00}:{1:00}.{2:000}",
                elapsed.Minutes, elapsed.Seconds, elapsed.Milliseconds);
        }

        /// <summary>
        /// Event handler for the timer
        /// </summary>
        private void ClockTimerEventHandler(Object obj, EventArgs args)
        {
            secs++;
            TimeWrap();
            if(isAlarmOn)
            {
                if(isMilitaryTime)
                {
                    if (milHrs == alarmMilHrs && mins == alarmMins && secs == alarmSecs)
                    {
                        isAlarmRinging = true;
                        alarmTimer.Start();
                    }
                }
                else
                {
                    if(hrs == alarmHrs && mins == alarmMins && secs == alarmSecs)
                    {
                        isAlarmRinging = true;
                        alarmTimer.Start();
                    }
                }
            }
            if (currentState < 4) textBox1.Text = GetTimeString();
        }

        private void LightTimerEventHandler(Object obj, EventArgs args)
        {
            if(isLit)
            {
                this.BackColor = Color.Yellow;
                if (DateTime.UtcNow - lightTime >= TimeSpan.FromSeconds(3))
                {
                    isLit = false;
                    this.BackColor = Color.DarkGray;
                }
            }
        }

        private void StopwatchTimerEventHandler(Object obj, EventArgs args)
        {
            textBox1.Text = GetStopwatchTimeString();
        }

        private void AlarmTimerEventHandler(Object obj, EventArgs args)
        {
            if (this.BackColor == Color.Red) this.BackColor = Color.DarkGray;
            else this.BackColor = Color.Red;
        }

        /// <summary>
        /// Function that handles the wrap of hrs/secs/mins
        /// </summary>
        private void TimeWrap()
        {
            if (secs > 59)
            {
                secs = 0;
                mins++;
                if (mins > 59)
                {
                    mins = 0;
                    hrs++;
                    milHrs++;
                    if (milHrs > 23)
                    {
                        milHrs = 0;
                    }
                    else if (milHrs < 0) milHrs = 23;
                    if (hrs > 12)
                    {
                        hrs = 1;
                    }
                    else if (hrs < 1) hrs = 12;
                }
                else if (mins < 0) mins = 59;
            }
            else if (secs < 0)
            {
                secs = 59;
            }

            if(currentState < 4)
            {
                if (!isMilitaryTime && milHrs > 11) pmBox.Checked = true;
                else pmBox.Checked = false;
            }
            
        }

        /// <summary>
        /// Helper function to change UI based on state change (called at button click)
        /// </summary>
        /// <param name="currState">the current state</param>
        /// <param name="nextState">the state to change to</param>
        private void changeState(short ns)
        {
            if(ns == 0)
            {
                clockTimer.Start();
                textBox1.Text = GetTimeString();
                if (!isMilitaryTime) pmBox.Visible = true;
                alarmBox.Visible = true;
                if (!isMilitaryTime && milHrs > 11) pmBox.Checked = true;
                else pmBox.Checked = false;
            }
            else if(ns>0 && ns <4)
            {
                clockTimer.Stop();
            }
            else if(ns == 4)
            {
                pmBox.Visible = false;
                alarmBox.Visible = false;
                textBox1.Text = GetStopwatchTimeString();
            }
            else if (ns>5) 
            {
                textBox1.Text = GetAlarmTimeString();
                if (!isMilitaryTime) pmBox.Visible = true;
                alarmBox.Visible = true;
                if (!isMilitaryTime && alarmMilHrs > 11) pmBox.Checked = true;
                else pmBox.Checked = false;
            }
        }

        /// <summary>
        /// Helper function to turn off the alarm if it is ringing and a button is pressed
        /// </summary>
        private void TurnOffAlarm()
        {
            alarmTimer.Stop();
            isAlarmRinging = false;
        }

        /// <summary>
        /// Event handler for the click of B1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void b1_Click(object sender, EventArgs e)
        {
            if (isAlarmRinging) TurnOffAlarm();
            else
            {
                State st = StateTable[currentState, 0];
                st.funDel();
                short nextState = st.nextState;
                changeState(nextState);
                currentState = nextState;
            }
        }

        /// <summary>
        /// Event handler for the click of B2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void b2_Click(object sender, EventArgs e)
        {
            if (isAlarmRinging) TurnOffAlarm();
            else
            {
                State st = StateTable[currentState, 1];
                st.funDel();
                short nextState = st.nextState;
                changeState(nextState);
                currentState = nextState;
            }
        }

        /// <summary>
        /// Event handler for the click of B3
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void b3_Click(object sender, EventArgs e)
        {
            if (isAlarmRinging) TurnOffAlarm();
            else
            {
                State st = StateTable[currentState, 2];
                st.funDel();
                short nextState = st.nextState;
                changeState(nextState);
                currentState = nextState;
            }
        }

        /// <summary>
        /// Event handler for the click of B4
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void b4_Click(object sender, EventArgs e)
        {
            if (isAlarmRinging) TurnOffAlarm();
            else
            {
                State st = StateTable[currentState, 3];
                st.funDel();
                short nextState = st.nextState;
                changeState(nextState);
                currentState = nextState;
            }
        }
    }
}
