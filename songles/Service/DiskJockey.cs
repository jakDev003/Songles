using songles.Data.DTO;
using songles.Data.Enums;
using songles.Data.Models;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Model = songles.Data.Model;

namespace songles.Service
{
    internal class DiscJockey
    {
        private readonly bool debug;
        private Song? currentSong;
        private readonly SongDTO songDTO;
        const string title = "Songles - The music player for the discerning listener";
        const string instructions = "P[a]use, [S]top, [L]ike, [D]islike, [P]lay, [N]ext song, [R]andom song";
        const string divider = "------------------------------------------------------------------------------------------------------------------------";

        /// <summary>
        /// Plays songs from the database according to the user's preferences
        /// </summary>
        /// <param name="db"></param>
        /// <param name="debug"></param>
        public DiscJockey(Model db, bool debug)
        {
            songDTO = new SongDTO(db);
            currentSong = null;
            this.debug = debug;
        }

        /// <summary>
        /// Plays the next song in the database
        /// </summary>
        public void Play()
        {
            // If there is no current song, pick a random song
            if (currentSong == null)
            {
                PickNextRandomSong();
                DisplaySong();
            }
            // Resume the current song
            else if (currentSong.State == SongState.Paused)
            {
                currentSong = songDTO.ChangeSongState(currentSong, SongState.Playing).Result;
            }
            // Play the next song
            else
            {
                PickNextSong();
                DisplaySong();
            }   
        }

        /// <summary>
        /// Temporarily pauses the current song
        /// </summary>
        public void Pause()
        {
            if (currentSong == null)
            {
                return;
            }
            currentSong = songDTO.PauseSongAsync(currentSong).Result;
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
            currentSong = songDTO.ChangeSongPreferenceAsync(currentSong, UserSongPreference.ThumbUp).Result;
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
            currentSong = songDTO.ChangeSongPreferenceAsync(currentSong, UserSongPreference.ThumbDown).Result;
        }

        /// <summary>
        /// Displays the current song and waits for it to finish
        /// </summary>
        private void DisplaySong()
        {
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
                // User must have resumed the song
                else if (!stopWatch.IsRunning && currentSong.State == SongState.Playing)
                {
                    stopWatch.Start();
                }
                // User must have paused the song
                else if (stopWatch.IsRunning && currentSong.State == SongState.Paused)
                {
                    stopWatch.Stop();
                }
                // User must have changed the song
                else if (currentSong != previousSong)
                {
                    DisplaySong();
                }

                // Only update the screen if the song is playing
                if (stopWatch.IsRunning)
                {
                    update = ShowSong(stopWatch, second, time, timeActual, update);
                }
                // Song is paused so decrement the second
                else
                {
                    second--;
                }
            }
            stopWatch.Stop();
        }

        /// <summary>
        /// Shows the current song and its progress
        /// </summary>
        /// <param name="stopWatch"></param>
        /// <param name="second"></param>
        /// <param name="time"></param>
        /// <param name="timeActual"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        private bool ShowSong(Stopwatch stopWatch, int second, double time, TimeOnly timeActual, bool update)
        {
            // Show progress bar and song information

            var percent = (int)((second / time) * 100);
            Thread.Sleep(1000);
            ShowInformation(title, instructions, stopWatch, timeActual, percent, update);

            // Song is finished so pick the next song
            if (percent == 100)
            {
                PickNextSong();
                DisplaySong();
            }
            return true;
        }

        /// <summary>
        /// Shows the information about the current song
        /// </summary>
        /// <param name="title"></param>
        /// <param name="instructions"></param>
        /// <param name="stopWatch"></param>
        /// <param name="timeActual"></param>
        /// <param name="percent"></param>
        /// <param name="update"></param>
        private void ShowInformation(string title, string instructions, Stopwatch stopWatch, TimeOnly timeActual, int percent, bool update)
        {
            TimeSpan ts = stopWatch.Elapsed;
            Console.SetCursorPosition(0, 0);
            var nowPlaying = $"Now playing: {currentSong?.TrackName} by {currentSong?.Artist} [ {currentSong?.UserPreference} ]";
            var status = $"Status: {currentSong?.State.ToString().PadRight(10)}";
            const int pad = 15;
            Console.WriteLine(title.PadLeft(70 + pad));
            Console.WriteLine(instructions.PadLeft(80 + pad));
            Console.WriteLine(divider);
            Console.WriteLine();
            Console.WriteLine(nowPlaying);
            Console.WriteLine(status);
            Console.WriteLine();
            Console.WriteLine(divider);
            Console.WriteLine();
            ConsoleUtilities.SetProgressBar(percent, ts, timeActual, update);
            Console.WriteLine();

            if (debug)
            {
                ShowDebugInformation(percent);
            }
        }


        /// <summary>
        /// Shows additional information about the current song
        /// </summary>
        /// <param name="percent"></param>
        private void ShowDebugInformation(int percent)
        {
            Console.WriteLine();
            Console.WriteLine(divider);
            Console.WriteLine("Debug Information:");
            Console.WriteLine();
            Console.WriteLine($"ID: {currentSong?.Id}");
            Console.WriteLine($"Trackname: {currentSong?.TrackName}");
            Console.WriteLine($"Time: {currentSong?.Time}");
            Console.WriteLine($"UserPreference: {currentSong?.UserPreference}");
            Console.WriteLine($"Album: {currentSong?.Album}");
            Console.WriteLine($"State: {currentSong?.State}");
            Console.WriteLine($"Percent: {percent}");

        }

        /// <summary>
        /// Picks a random song from the database
        /// </summary>
        public void PickNextRandomSong()
        {
            currentSong = songDTO.GetRandomSongAsync().Result;
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
            currentSong = songDTO.GetNextSongAsync(currentSong).Result;
        }
    }
}