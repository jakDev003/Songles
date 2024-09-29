using songles.Data;

namespace songles.Controller
{
    internal class DiskJockey
    {
        private readonly Model context;

        public DiskJockey(Model db)
        {
            context = db;
        }

        public void Play()
        {
            Random rand = new Random();
            int toSkip = rand.Next(0, context.Songs.Count());

            var song = context.Songs.Skip(toSkip).Take(1).First();

            Console.WriteLine($"Now playing: {song.TrackName} by {song.Artist}");
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
    }
}
