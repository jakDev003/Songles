namespace songles.Service
{
    internal static class ConsoleUtilities
    {
        const string _block = "c";

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
            for (var i = 0; i < 100; ++i)
            {
                if (i >= percent + 1)
                {
                    Console.Write('-');
                }
                else if (i == percent && IsEven(percent))
                {
                    Console.Write(_block.ToUpper());
                }
                else if (i == percent && !IsEven(percent))
                {
                    Console.Write(_block);
                }
                else
                {
                    Console.Write(' ');
                }
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

        /// <summary>
        /// Determines if a number is even
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool IsEven(int value)
        {
            return value % 2 == 0;
        }
    }
}
