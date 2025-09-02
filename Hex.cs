namespace DVG.Core
{
    public static class Hex
    {
        private static readonly fix _sqrtThree = Maths.Sqrt((fix)3);
        private static readonly fix _sqrtThreeOverTwo = _sqrtThree / 2;

        private static readonly fix _outerRadius = fix.One / 2;
        private static readonly fix _innerRadius = _outerRadius * _sqrtThreeOverTwo;

        private static readonly fix _sqrtThreeOverThree = _sqrtThree / 3;
        private static readonly fix _oneAndHalf = fix.One + fix.One / 2;
        private static readonly fix _twoOverThree = (fix)2 / 3;
        private static readonly fix _minusOneOverThree = -fix.One / 3;
        private static int2 AxialRound(fix q, fix r)
        {
            fix x = q;
            fix z = r;
            fix y = -x - z;

            int rx = (int)Maths.Round(x);
            int ry = (int)Maths.Round(y);
            int rz = (int)Maths.Round(z);

            fix dx = Maths.Abs(rx - x);
            fix dy = Maths.Abs(ry - y);
            fix dz = Maths.Abs(rz - z);

            if (dx > dy && dx > dz)
                rx = -ry - rz;
            else if (dy > dz)
                ry = -rx - rz;
            else
                rz = -rx - ry;

            return new int2(rx, rz);
        }

        public static int2 WorldToAxial(fix2 pos)
        {
            fix q = (_twoOverThree * pos.x) / _outerRadius;
            fix r = (_minusOneOverThree * pos.x + _sqrtThreeOverThree * pos.y) / _outerRadius;
            return AxialRound(q, r);
        }

        public static fix2 AxialToWorld(int2 axial)
        {
            int q = axial.x;
            int r = axial.y;

            fix x = _outerRadius * _oneAndHalf * q;
            fix y = _innerRadius * (2 * r + q);
            return new fix2(x, y);
        }

        public static int2 OffsetToAxial(int2 offset)
        {
            var parity = offset.x & 1;
            var q = offset.x;
            var r = offset.y - (offset.x - parity) / 2;
            return new int2(q, r);
        }

        public static int2 AxialToOffset(int2 axial)
        {
            var parity = axial.x & 1;
            var col = axial.x;
            var row = axial.y + (axial.x - parity) / 2;
            return new int2(col, row);
        }
    }
}
