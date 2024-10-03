using songles.Data;
using songles.Data.DTO;
using songles.Data.Enums;
using songles.Data.Models;
using System.Diagnostics;

namespace songles.Service
{
    internal class DiskJockey
    {
        private Song? currentSong;
        private readonly SongDTO songDTO;

        /// <summary>
        /// Plays songs from the database according to the user's preferences
        /// </summary>
        /// <param name="db"></param>
        public DiskJockey(Model db)
        {
            songDTO = new SongDTO(db);
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

        /// <summary>
        /// Temporarily pauses the current song
        /// </summary>
        public void Pause()
        {
            if (currentSong == null) {
                return;
            }
            currentSong = songDTO.PauseSong(currentSong).Result;
        }

        /// <summary>
        /// Stops the current song
        /// </summary>
        public void Stop()
        {
            currentSong = null;
            Console.Clear();
        }

        /// <summary>
        /// Marks the song as something the user likes
        /// </summary>
        public void ThumbUp()
        {
            if (currentSong == null)
            {
                return;
            }
            currentSong = songDTO.ChangeSongPreference(currentSong, UserSongPreference.ThumbUp).Result;
        }

        /// <summary>
        /// Marks the song as something the user dislikes
        /// </summary>
        public void ThumbDown()
        {
            if (currentSong == null)
            {
                return;
            }
            currentSong = songDTO.ChangeSongPreference(currentSong, UserSongPreference.ThumbDown).Result;
        }

        /// <summary>
        /// Displays the current song and waits for it to finish
        /// </summary>
        private void DisplaySong()
        {
            var nowPlaying = string.Empty;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var timeActual = currentSong?.Time ?? new TimeOnly();
            var time = timeActual.ToTimeSpan().TotalSeconds;
            bool update = false;
            var previousSong = currentSong;
            for (int second = 0; second < time; second++)
            {
                // User must have stopped the song
                if (currentSong == null)
                {
                    break;
                }
                // User must have changed the song
                else if (currentSong != previousSong)
                {
                    DisplaySong();
                }

                // Show progress bar and song information
                TimeSpan ts = stopWatch.Elapsed;
                var percent = (int)((second / time) * 100);
                Thread.Sleep(1000);
                Console.Clear();
                nowPlaying = $"Now playing: {currentSong?.TrackName} by {currentSong?.Artist} [ {currentSong?.UserPreference} ]";
                Console.WriteLine(nowPlaying);
                ConsoleUtilities.SetProgressBar(percent, ts, timeActual, update);
                update = true;

                // Song is finished so pick the next song
                if (percent == 100)
                {
                    PickNextSong();
                    DisplaySong();
                }
            }
            stopWatch.Stop();
        }

        /// <summary>
        /// Picks a random song from the database
        /// </summary>
        private void PickNextRandomSong()
        {
            currentSong = songDTO.GetRandomSong().Result;
        }

        /// <summary>
        /// Picks the next song in the database
        /// </summary>
        public void PickNextSong()
        {
            if (currentSong == null)
            {
                PickNextRandomSong();
                return;
            }
            currentSong = songDTO.GetNextSong(currentSong).Result;
        }
    }
}