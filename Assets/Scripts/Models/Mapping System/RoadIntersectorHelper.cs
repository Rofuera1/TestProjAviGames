using System.Collections.Generic;
using UnityEngine;

public static class RoadIntersectorHelper
{
    public static HashSet<Road> DoAnyRoadsIntersect(List<Road> roads)
    {
        HashSet<Road> RoadsThatIntersect = new HashSet<Road>();
        for (int i = 0; i < roads.Count; i++)
        {
            for (int j = i + 1; j < roads.Count; j++)
            {
                bool shareAnyDot =  (roads[i].DotFirst == roads[j].DotFirst) ||
                                    (roads[i].DotFirst == roads[j].DotSecond) ||
                                    (roads[i].DotSecond == roads[j].DotFirst) ||
                                    (roads[i].DotSecond == roads[j].DotSecond);

                if (shareAnyDot) continue;

                if (doIntersect(roads[i].DotFirst.Position, roads[i].DotSecond.Position, roads[j].DotFirst.Position, roads[j].DotSecond.Position))
                {
                    RoadsThatIntersect.Add(roads[i]);
                    RoadsThatIntersect.Add(roads[j]);
                }
            }
        }

        return RoadsThatIntersect;
    }

    static bool onSegment(Vector2 p, Vector2 q, Vector2 r)
    {
        if (q.x <= Mathf.Max(p.x, r.x) && q.x >= Mathf.Min(p.x, r.x) &&
            q.y <= Mathf.Max(p.y, r.y) && q.y >= Mathf.Min(p.y, r.y))
            return true;

        return false;
    }

    static int orientation(Vector2 p, Vector2 q, Vector2 r)
    {
        float val = (q.y - p.y) * (r.x - q.x) -
                (q.x - p.x) * (r.y - q.y);

        if (val == 0) return 0;

        return (val > 0) ? 1 : 2;
    }

    static bool doIntersect(Vector2 p1, Vector2 q1, Vector2 p2, Vector2 q2)
    {
        int o1 = orientation(p1, q1, p2);
        int o2 = orientation(p1, q1, q2);
        int o3 = orientation(p2, q2, p1);
        int o4 = orientation(p2, q2, q1);

        bool isStandardIntersect = (o1 != o2 && o3 != o4)
                                   || (o1 == 0 && onSegment(p1, p2, q1))
                                   || (o2 == 0 && onSegment(p1, q2, q1))
                                   || (o3 == 0 && onSegment(p2, p1, q2))
                                   || (o4 == 0 && onSegment(p2, q1, q2));

        if (!isStandardIntersect)
            return false;

        if ((p1 == p2) || (p1 == q2) || (q1 == p2) || (q1 == q2))
        {
            return false;
        }

        return true;
    }
}
