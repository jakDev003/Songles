namespace songles.Service
{
    internal static class ConsoleUtilities
    {
        const char _block = '■';

        /// <summary>
        /// Returns a string of progress bars based on the current and total values
        /// </summary>
        /// <param name="total"></param>
        /// <param name="elapsedTime"></param>
        /// <param name="totalTime"></param>
        /// <param name="current"></param>

        /// <returns></returns>
        public static void SetProgressBar(int percent, TimeSpan elapsedTime, TimeOnly totalTime, bool update = false)
        {
            if (update)
                Console.Write("\r");
            Console.Write("[");
            var p = (int)((percent / 20f) + .5f);
            for (var i = 0; i < 10; ++i)
            {
                if (i >= p)
                    Console.Write(' ');
                else
                    Console.Write(_block);
            }
            Console.Write($"] ({TimeSpanToString(elapsedTime)}/{TimeOnlyToString(totalTime)})");
        }

        /// <summary>
        /// TimeOnly to string
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private static string TimeOnlyToString(TimeOnly time)
        {
            return $"{time.Hour}:{time.Minute}:{time.Second}";
        }

        /// <summary>
        /// TimeSpan to string
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private static string TimeSpanToString(TimeSpan time)
        {
            return $"{time.Hours}:{time.Minutes}:{time.Seconds}";
        }
    }
}
