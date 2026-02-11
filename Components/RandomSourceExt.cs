namespace DVG.Components
{
    public static class RandomSourceExt
    {
        private static int Next(this ref RandomSource source)
        {
            source.Value ^= source.Value << 13;
            source.Value ^= source.Value >> 17;
            source.Value ^= source.Value << 5;
            return source.Value;
        }

        public static int NextRange(this ref RandomSource source, int min, int max)
        {
            var value = Next(ref source);
            int mask = value >> 31;
            value = (mask ^ value) - mask;
            return value % (max - min) + min;
        }

        public static fix NextRange(this ref RandomSource source, fix min, fix max)
        {
            return new fix(NextRange(ref source, min.raw, max.raw + 1));
        }
    }
}
