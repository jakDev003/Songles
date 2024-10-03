namespace songles.Service
{
    internal class UserInput
    {
        private Task _timerTask;
        private readonly PeriodicTimer _timer;
        private readonly CancellationTokenSource _cts = new();
        private readonly DiskJockey _diskJockey;

        public UserInput(TimeSpan interval, DiskJockey diskJockey)
        {
            _timer = new PeriodicTimer(interval);
            _diskJockey = diskJockey;
        }

        public void Start()
        {
            _timerTask = DoWorkAsync();
        }

        private async Task DoWorkAsync()
        {
            try
            {
                while (await _timer.WaitForNextTickAsync(_cts.Token))
                {
                    CheckForUserInput();
                }
            }
            catch (OperationCanceledException)
            {
                // Do nothing
            }
        }

        public async Task StopAsync()
        {
            if (_timerTask is null)
            {
                return;
            }

            await _cts.CancelAsync();
            await _timerTask;
            _cts.Dispose();
        }

        private void CheckForUserInput()
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.P:
                        _diskJockey.Pause();
                        break;
                    case ConsoleKey.S:
                        _diskJockey.Stop();
                        break;
                    case ConsoleKey.UpArrow:
                        _diskJockey.ThumbUp();
                        break;
                    case ConsoleKey.DownArrow:
                        _diskJockey.ThumbDown();
                        break;
                    case ConsoleKey.R:
                        _diskJockey.Play();
                        break;
                    case ConsoleKey.N:
                        _diskJockey.PickNextSong();
                        break;
                    default:
                        _diskJockey.Stop();
                        break;
                }
            }
        }
    }
}
