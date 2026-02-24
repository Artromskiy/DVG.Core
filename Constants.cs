namespace DVG
{
    public static class Constants
    {
        public const int TicksPerSecond = 16;
        public const int HistoryTicks = TicksPerSecond * 16; // 16 seconds

        public static readonly fix TickTime = fix.One / TicksPerSecond;

        public static int WrapTick(int tick)
        {
            return tick & HistoryTicks - 1;
        }
    }
}
