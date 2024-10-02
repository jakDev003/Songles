using songles.Data;
using songles.Data.Models;
using System.Diagnostics;

namespace songles.Service
{
    internal class DiskJockey
    {
        private readonly Model context;
        private Song? currentSong;

        /// <summary>
        /// Plays songs from the database according to the user's preferences
        /// </summary>
        /// <param name="db"></param>
        public DiskJockey(Model db)
        {
            context = db;
            currentSong = null;
        }

        /// <summary>
        /// Plays the next song in the database
        /// </summary>
        public void Play()
        {
            if (currentSong == null)
            { 
                PickNextRandomSong();
            }
            else
            {
                PickNextSong();
            }
            DisplaySong();
        }

        public void Pause()
        {
            throw new System.NotImplementedException();
        }

        public void Stop()
        {
            throw new System.NotImplementedException();
        }

        public void Skip()
        {
            throw new System.NotImplementedException();
        }

        public void ThumbUp()
        {
            throw new System.NotImplementedException();
        }

        public void ThumbDown()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Displays the current song and waits for it to finish
        /// </summary>
        private void DisplaySong()
        {
            Console.WriteLine($"Now playing: {currentSong?.TrackName} by {currentSong?.Artist}");
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var timeActual = currentSong?.Time ?? new TimeOnly();
            var time = timeActual.ToTimeSpan().TotalSeconds;
            bool update = false;
            for (int second = 0; second < time; second++)
            {
                TimeSpan ts = stopWatch.Elapsed;
                var percent = (int)((second / time) * 100);
                Thread.Sleep(1000);
                ConsoleUtilities.SetProgressBar(percent, ts, timeActual, update);
                update = true;
            }
            stopWatch.Stop();
        }

        /// <summary>
        /// Picks a random song from the database
        /// </summary>
        private void PickNextRandomSong()
        {
            Random rand = new Random();
            int toSkip = rand.Next(0, context.Songs.Count());

            currentSong = context.Songs.Skip(toSkip).Take(1).First();
        }

        /// <summary>
        /// Picks the next song in the database
        /// </summary>
        private void PickNextSong()
        {
            int toSkip = (currentSong?.Id ?? 0) + 1;
            currentSong = context.Songs.Skip(toSkip).Take(1).FirstOrDefault();
        }
    }
}