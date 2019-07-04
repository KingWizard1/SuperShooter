using System;
using UnityEngine;

namespace SuperShooter
{

    public class GameClock : MonoBehaviour
    {

        public class Clock
        {
            private DateTime now;
            private TimeSpan timeNow;
            private TimeSpan gameTime;
            private int minutesPerDay; //Realtime minutes per game-day (1440 would be realtime)

            public Clock(int minPerDay)
            {
                minutesPerDay = minPerDay;
            }

            public TimeSpan GetTime()
            {
                now = DateTime.Now;
                timeNow = now.TimeOfDay;
                double hours = timeNow.TotalMinutes % minutesPerDay;
                double minutes = (hours % 1) * 60;
                double seconds = (minutes % 1) * 60;
                gameTime = new TimeSpan((int)hours, (int)minutes, (int)seconds);

                return gameTime;
            }
        }

        Clock clock;

        void Start()
        {
            clock = new Clock(24);
        }

        void FixedUpdate()
        {
            Debug.Log(clock.GetTime().ToString());
        }

    }
}