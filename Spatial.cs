using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace DVG.Core
{
    public static class Spatial
    {
        private const int _depth = 10;
        private static readonly fix _skin = new fix(1024);

        public static fix2 Solve(List<(fix2 s, fix2 e, fix2 n)> segments, fix2 from, fix2 to)
        {
            var castFrom = from;
            var endPoint = to;
            int depth = _depth;
            while (depth > 0)
            {
                if (!RayCast(segments, castFrom, endPoint, out var res) &&
                    !RayCast(segments, from, endPoint, out _))
                {
                    break;
                }

                var newCastFrom = res.intersection + res.normal * _skin;

                if (RayCast(segments, from, newCastFrom, out _))
                {
                    endPoint = castFrom;
                    break;
                }

                if (fix2.SqrDistance(newCastFrom, to) >= fix2.SqrDistance(castFrom, to))
                {
                    endPoint = castFrom;
                    break;
                }

                castFrom = newCastFrom;
                fix2 tangent = res.normal.yx;
                tangent.x = -tangent.x;
                endPoint = Projection(endPoint, castFrom, castFrom + tangent);

                depth--;
            }

            if (depth == 0)
            {
                return from;
            }
            return endPoint;
        }

        public static fix2 SolveCircleMove(List<(fix2 s, fix2 e, fix2 n)> segments, fix2 from, fix2 to, fix radius)
        {
            var castFrom = from;
            var endPoint = to;
            int depth = _depth;
            while (depth > 0)
            {
                if (!CircleCast(segments, castFrom, endPoint, radius, out var res) &&
                    !CircleCast(segments, from, endPoint, radius, out _))
                {
                    break;
                }

                var newCastFrom = res.intersection + res.normal * _skin;

                if (CircleCast(segments, from, newCastFrom, radius, out _))
                {
                    endPoint = castFrom;
                    break;
                }

                if (depth != _depth && fix2.SqrDistance(newCastFrom, to) >= fix2.SqrDistance(castFrom, to))
                {
                    endPoint = castFrom;
                    break;
                }

                castFrom = newCastFrom;
                fix2 tangent = res.normal.yx;
                tangent.x = -tangent.x;
                endPoint = Projection(endPoint, castFrom, castFrom + tangent);

                depth--;
            }

            if (depth == 0)
            {
                return from;
            }
            return endPoint;
        }

        public static fix2 SolveCircleMove((fix2 s, fix2 e, fix2 n)[] segments, fix2 from, fix2 to, fix radius)
        {
            var castFrom = from;
            var endPoint = to;
            int depth = _depth;
            while (depth > 0)
            {
                if (!CircleCast(segments, castFrom, endPoint, radius, out var res) &&
                    !CircleCast(segments, from, endPoint, radius, out _))
                {
                    break;
                }

                var newCastFrom = res.intersection + res.normal * _skin;

                if (CircleCast(segments, from, newCastFrom, radius, out _))
                {
                    endPoint = castFrom;
                    break;
                }

                if (depth != _depth && fix2.SqrDistance(newCastFrom, to) >= fix2.SqrDistance(castFrom, to))
                {
                    endPoint = castFrom;
                    break;
                }

                castFrom = newCastFrom;
                fix2 tangent = res.normal.yx;
                tangent.x = -tangent.x;
                endPoint = Projection(endPoint, castFrom, castFrom + tangent);

                depth--;
            }

            if (depth == 0)
            {
                return from;
            }
            return endPoint;
        }


        public static bool CircleCast((fix2 s, fix2 e, fix2 n)[] lines, fix2 from, fix2 to, fix radius, out (fix2 intersection, fix2 normal) result)
        {
            fix minSqrDistance = fix.MaxValue;
            result = default;
            bool found = false;
            for (int i = 0; i < lines.Length; i++)
            {
                if (!CircleIntersection(lines[i], from, to, radius, out var res))
                    continue;

                var sqrDistance = fix2.SqrDistance(from, res.intersection);
                if (sqrDistance > minSqrDistance)
                    continue;

                result = res;
                minSqrDistance = sqrDistance;
                found = true;
            }
            return found;
        }

        public static bool RayCast(List<(fix2 s, fix2 e, fix2 n)> lines, fix2 from, fix2 to, out (fix2 intersection, fix2 normal) result)
        {
            fix minSqrDistance = fix.MaxValue;
            result = default;
            bool found = false;
            for (int i = 0; i < lines.Count; i++)
            {
                if (!LineIntersection(lines[i], from, to, out var res))
                    continue;

                var sqrDistance = fix2.SqrDistance(from, res.intersection);
                if (sqrDistance > minSqrDistance)
                    continue;

                result = res;
                minSqrDistance = sqrDistance;
                found = true;
            }
            return found;
        }

        public static bool CircleCast(List<(fix2 s, fix2 e, fix2 n)> lines, fix2 from, fix2 to, fix radius, out (fix2 intersection, fix2 normal) result)
        {
            fix minSqrDistance = fix.MaxValue;
            result = default;
            bool found = false;
            for (int i = 0; i < lines.Count; i++)
            {
                if (!CircleIntersection(lines[i], from, to, radius, out var res))
                    continue;

                var sqrDistance = fix2.SqrDistance(from, res.intersection);
                if (sqrDistance > minSqrDistance)
                    continue;

                result = res;
                minSqrDistance = sqrDistance;
                found = true;
            }
            return found;
        }

        public static bool LineIntersection((fix2 s, fix2 e, fix2 n) segment, fix2 from, fix2 to, out (fix2 intersection, fix2 normal) result)
        {
            result = default;
            if (Intersects(segment.s, segment.e, from, to, out var intersection))
            {
                result = (intersection, segment.n);
                return true;
            }
            return false;
        }

        public static bool CircleIntersection((fix2 s, fix2 e, fix2 n) segment, fix2 from, fix2 to, fix radius, out (fix2 intersection, fix2 normal) result)
        {
            var offset1 = segment.n * radius;
            var newS = segment.s + offset1;
            var newE = segment.e + offset1;
            result = default;
            if (Intersects(newS, newE, from, to, out var intersection))
            {
                result = (intersection, segment.n);
                return true;
            }
            var projS = ProjectionOnSegment(segment.s, from, to);
            var projE = ProjectionOnSegment(segment.e, from, to);
            var sqrRadius = radius * radius;
            var sqrDistanceEdgeS = fix2.SqrDistance(projS, segment.s);
            var sqrDistanceEdgeE = fix2.SqrDistance(projE, segment.e);
            if (sqrDistanceEdgeS < sqrRadius || sqrDistanceEdgeE < sqrRadius)
            {
                var center = sqrDistanceEdgeS <= sqrDistanceEdgeE ? segment.s : segment.e;
                var (i1, i2) = CircleLineIntersections(from, to, center, radius);
                intersection = fix2.SqrDistance(i1, from) < fix2.SqrDistance(i2, from) ?
                    i1 : i2;
                var normal = fix2.Normalize(intersection - center);
                result = (intersection, normal);
                return true;
            }
            return false;
        }

        public static (fix2, fix2) CircleLineIntersections(fix2 p0, fix2 p1, fix2 center, fix radius)
        {
            var dx = p1.x - p0.x;
            var dy = p1.y - p0.y;
            var ddx = p0.x - center.x;
            var ddy = p0.y - center.y;
            var a = (dx * dx) + (dy * dy);
            var b = 2 * dx * ddx + 2 * dy * ddy;
            var c = ddx * ddx + ddy * ddy - radius * radius;
            var D = Maths.Sqrt(b * b - 4 * a * c);
            var d1 = -b + D;
            var d2 = -b - D;
            if (d1 == d2 && d2 == 0)
            {
                d1 = d2 = 1;
            }
            var c2 = 2 * c;
            var t1 = c2 / (d1 == 0 ? d2 : d1);
            var t2 = c2 / (d2 == 0 ? d1 : d2);
            return (
                new fix2(dx, dy) * t1 + p0,
                new fix2(dx, dy) * t2 + p0);
        }

        public static bool Intersects(fix2 a, fix2 b, fix2 c, fix2 d, out fix2 intersection)
        {
            intersection = fix2.zero;

            fix d1 = orient(c, d, a);
            fix d2 = orient(c, d, b);
            fix d3;
            fix d4;

            if (d1 * d2 < 0)
            {
                d3 = orient(a, b, c);
                d4 = orient(a, b, d);

                if (d3 * d4 < 0)
                {
                    intersection = (a * d2 - b * d1) / (d2 - d1);
                    return true;
                }
            }

            return false;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static fix cross(fix2 a, fix2 b) => a.x * b.y - a.y * b.x;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static fix orient(fix2 a, fix2 b, fix2 c) => cross(b - a, c - a);
        }

        public static fix2 Projection(fix2 point, fix2 start, fix2 end)
        {
            if (start == end)
                return start;

            fix2 AP = point - start;
            fix2 AB = end - start;

            var sqrLength = fix2.SqrLength(AB);
            if (sqrLength == 0)
                return start;

            fix dot = fix2.Dot(AP, AB) / sqrLength;

            return start + AB * dot;
        }

        public static fix2 ProjectionOnSegment(fix2 point, fix2 start, fix2 end)
        {
            if (start == end)
                return start;

            fix2 AP = point - start;
            fix2 AB = end - start;

            var sqrLength = fix2.SqrLength(AB);
            if (sqrLength == 0)
                return start;

            fix dot = fix2.Dot(AP, AB) / sqrLength;
            dot = Maths.Clamp(dot, 0, 1);

            return start + AB * dot;
        }
    }
}
