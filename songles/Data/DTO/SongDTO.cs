﻿using Microsoft.EntityFrameworkCore;
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
        public async Task<Song?> GetRandomSong()
        {
            var random = new Random();
            var songs = await context.Songs.ToListAsync();
            var newSong = songs[random.Next(0, songs.Count)];
            return await ChangeSongState(newSong, SongState.Playing);
        }

        /// <summary>
        /// Gets the next song in the database
        /// </summary>
        /// <param name="currentSong"></param>
        /// <returns></returns>
        public async Task<Song?> GetNextSong(Song currentSong)
        {
            if (currentSong != null)
            {
                var finishedSong = await ChangeSongState(currentSong, SongState.Finished);
                if (finishedSong != null)
                {
                    currentSong = finishedSong;
                }
            }

            var songs = await context.Songs.ToListAsync();
            var index = currentSong == null ? -1 : songs.IndexOf(currentSong);
            var newSong = index == songs.Count - 1 ? songs[0] : songs[index + 1];
            return await ChangeSongState(newSong, SongState.Playing);
        }

        /// <summary>
        /// Changes the state of the song
        /// </summary>
        /// <param name="currentSong"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        private async Task<Song?> ChangeSongState(Song currentSong, SongState state)
        {
            var song = await FindSongById(currentSong.Id);
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
        public async Task<Song?> PauseSong(Song currentSong)
        {
            var song = await FindSongById(currentSong.Id);
            if (song == null)
            {
                return null;
            }
            song.State = SongState.Paused;
            await context.SaveChangesAsync();
            return ChangeSongState(song, SongState.Paused).Result;
        }

        /// <summary>
        /// Changes the user's preference for the song
        /// </summary>
        /// <param name="currentSong"></param>
        /// <param name="pref"></param>
        /// <returns></returns>
        public async Task<Song?> ChangeSongPreference(Song currentSong, UserSongPreference pref)
        {
            var song = await FindSongById(currentSong.Id);
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
        private async Task<Song?> FindSongById(int id)
        {
            return await context.Songs.FindAsync(id);
        }
    }
}