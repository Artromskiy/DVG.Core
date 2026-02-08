namespace DVG.Core.Physics
{
    public readonly struct Segment
    {
        public readonly fix2 Start;
        public readonly fix2 End;
        public readonly fix2 Normal;

        public Segment(fix2 start, fix2 end, fix2 normal)
        {
            Start = start;
            End = end;
            Normal = normal;
        }
    }
}
