using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace Game_Sudoku
{
    class clsTime
    {
        #region Fields

        private Int64 m_milliseconds;
        private Int64 m_second;
        private Int64 m_minute;
        private Int64 m_hour;
        private static float interval = 1000f;

        private static clsTime m_instance;

        #endregion

        #region Properties

        public Int64 Hour
        {
            get { return m_hour; }
            set { m_hour = value; }
        }


        public Int64 Minute
        {
            get { return m_minute; }
            set { m_minute = value; }
        }


        public Int64 Second
        {
            get { return m_second; }
            set { m_second = value; }
        }


        public Int64 Milliseconds
        {
            get { return m_milliseconds; }
            set { m_milliseconds = value; }
        }


        #endregion

        #region Constructors & Methods

        private clsTime()
        {
            Second = 0;
            Minute = 0;
            Hour = 0;
            Milliseconds = 0;
        }

        public static clsTime getInstance()
        {
            if (m_instance == null)
            {
                m_instance = new clsTime();
            }

            return m_instance;
        }

        public void IncreaseTime(GameTime gameTime)
        {
            Milliseconds += (long)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (Milliseconds >= interval)
            {
                Second += 1;
                Milliseconds = 0;
                if (Second >= 60)
                {
                    Minute += 1;
                    Second = 0;
                    if (Minute >= 60)
                    {
                        Hour += 1;
                        Minute = 0;
                    }
                }

            }

        }

        public void ResetTime()
        {
            Second = 0;
            Minute = 0;
            Hour = 0;
            Milliseconds = 0;
        }

        public string GetTime()
        {
            return string.Format("{0:00}:{1:00}:{2:00}", this.Hour, this.Minute, this.Second);
        }
        #endregion

    }
}
