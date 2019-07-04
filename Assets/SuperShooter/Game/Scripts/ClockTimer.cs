using System;
using UnityEngine;
using NaughtyAttributes;

namespace SuperShooter
{

    public class ClockTimer : MonoBehaviour
    {

        public int startTime = 0;

        public int finishTime = 60;

        public bool isCountdown = false;


        // ------------------------------------------------- //

        [ShowNativeProperty] public bool isRunning { get; private set; } = false;
        [ShowNativeProperty] public float currentTime { get; private set; } = 0f;
        [ShowNativeProperty] public string currentTimeString { get; private set; } = "0:00";

        // ------------------------------------------------- //

        void Start()
        {
            currentTime = startTime;
        }

        // ------------------------------------------------- //

        public void StartCountdown(int fromSeconds)
        {
            Stop();
            startTime = fromSeconds;
            currentTime = fromSeconds;
            finishTime = 0;
            isCountdown = true;
            isRunning = true;
        }

        public void Stop()
        {
            isRunning = true;
        }

        // ------------------------------------------------- //

        public void Update()
        {
            if (!isRunning)
                return;


            if (isCountdown) {
                if (currentTime > finishTime)
                    currentTime -= Time.deltaTime;
            }
            else {
                throw new NotImplementedException("Counting UP is not implemented yet.");
                currentTime += Time.deltaTime;
            }


            // Convert to string
            var time = TimeSpan.FromSeconds(currentTime);
            currentTimeString = time.ToString(@"mm\:ss");


        }

        // ------------------------------------------------- //

    }
}