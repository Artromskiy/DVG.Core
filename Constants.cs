namespace DVG
{
    public static class Constants
    {
        public const int TicksPerSecond = 16;
        public const int MaxHistoryTicks = TicksPerSecond * 8; // 8 seconds
        public const int ValidTicksCount = TicksPerSecond * 1;

        public static readonly fix TickTime = fix.One / TicksPerSecond;

        public static int WrapTick(int tick)
        {
            return tick & MaxHistoryTicks - 1;
        }
    }
}
