using Microsoft.EntityFrameworkCore;
using songles.Data.Enums;
using songles.Data.Models;

namespace songles.Data.DTO
{
    internal class SongDTO
    {
        private readonly Model context;

        public SongDTO(Model db)
        {
            context = db;
        }

        /// <summary>
        /// Gets a random song from the database
        /// </summary>
        /// <returns></returns>
        public async Task<Song?> GetRandomSongAsync()
        {
            var random = new Random();
            var songs = await context.Songs.ToListAsync();
            var newSong = songs[random.Next(0, songs.Count)];

            // 1 in 3 chance of skipping a song the user dislikes
            var chance = random.Next(0, 3);
            if (chance == 0 && newSong.UserPreference == UserSongPreference.ThumbDown)
            {
                newSong = songs[random.Next(0, songs.Count)];
            }

            // 1 in 5 chance of skipping a song the user likes
            chance = random.Next(0, 5);
            if (chance == 0 && newSong.UserPreference == UserSongPreference.ThumbUp)
            {
                newSong = songs[random.Next(0, songs.Count)];
            }


            return await ChangeSongStateAsync(newSong, SongState.Playing);
        }

        /// <summary>
        /// Gets the next song in the database
        /// </summary>
        /// <param name="currentSong"></param>
        /// <returns></returns>
        public async Task<Song?> GetNextSongAsync(Song currentSong)
        {
            if (currentSong != null)
            {
                var finishedSong = await ChangeSongStateAsync(currentSong, SongState.Finished);
                if (finishedSong != null)
                {
                    currentSong = finishedSong;
                }
            }

            var songs = await context.Songs.ToListAsync();
            var index = currentSong == null ? -1 : songs.IndexOf(currentSong);
            var newSong = index == songs.Count - 1 ? songs[0] : songs[index + 1];
            return await ChangeSongStateAsync(newSong, SongState.Playing);
        }

        /// <summary>
        /// Changes the state of the song
        /// </summary>
        /// <param name="currentSong"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        private async Task<Song?> ChangeSongStateAsync(Song currentSong, SongState state)
        {
            var song = await FindSongByIdAsync(currentSong.Id);
            if (song == null)
            {
                return null;
            }
            song.State = state;
            await context.SaveChangesAsync();
            return song;
        }

        /// <summary>
        /// Pauses the song temporarily
        /// </summary>
        /// <param name="currentSong"></param>
        /// <returns></returns>
        public async Task<Song?> PauseSongAsync(Song currentSong)
        {
            var song = await FindSongByIdAsync(currentSong.Id);
            if (song == null)
            {
                return null;
            }
            song.State = SongState.Paused;
            await context.SaveChangesAsync();
            return ChangeSongStateAsync(song, SongState.Paused).Result;
        }

        /// <summary>
        /// Changes the user's preference for the song
        /// </summary>
        /// <param name="currentSong"></param>
        /// <param name="pref"></param>
        /// <returns></returns>
        public async Task<Song?> ChangeSongPreferenceAsync(Song currentSong, UserSongPreference pref)
        {
            var song = await FindSongByIdAsync(currentSong.Id);
            if (song == null)
            {
                return null;
            }
            song.UserPreference = pref;
            await context.SaveChangesAsync();
            return song;
        }

        /// <summary>
        /// Find a song by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task<Song?> FindSongByIdAsync(int id)
        {
            return await context.Songs.FindAsync(id);
        }
    }
}