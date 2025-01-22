using System.Collections.Generic;
using UnityEngine;

public class RoadBuilder : MonoBehaviour
{
    public Road RoadPrefab;

    public List<Road> BuildRoads(Dot[] dots)
    {
        HashSet<string> createdRoads = new HashSet<string>();
        List<Road> createdRoadObjects = new List<Road>();
        Dictionary<Dot, List<Road>> dotRoadsMap = new Dictionary<Dot, List<Road>>();

        foreach (var dot in dots)
        {
            if (!dotRoadsMap.ContainsKey(dot))
            {
                dotRoadsMap[dot] = new List<Road>();
            }

            foreach (var targetDot in dot.DotsConnected)
            {
                string roadKey = GenerateRoadKey(dot, targetDot);
                if (createdRoads.Contains(roadKey)) continue;

                Road road = CreateRoad(dot, targetDot);

                if (!dotRoadsMap.ContainsKey(targetDot))
                {
                    dotRoadsMap[targetDot] = new List<Road>();
                }

                dotRoadsMap[dot].Add(road);
                dotRoadsMap[targetDot].Add(road);

                createdRoads.Add(roadKey);
                createdRoadObjects.Add(road);
            }
        }

        foreach (var dot in dots)
        {
            dot.AssignRoads(dotRoadsMap[dot].ToArray());
        }

        return createdRoadObjects;
    }

    private string GenerateRoadKey(Dot dot1, Dot dot2)
    {
        int id1 = dot1.GetInstanceID();
        int id2 = dot2.GetInstanceID();
        return id1 < id2 ? $"{id1}-{id2}" : $"{id2}-{id1}";
    }

    private Road CreateRoad(Dot FirstDot, Dot SecondDot)
    {
        Road NewRoad = Instantiate(RoadPrefab);

        NewRoad.Init(FirstDot, SecondDot);
        NewRoad.transform.position = NewRoad.CalculateMiddlePosition();
        NewRoad.transform.right = NewRoad.CalculateDirectionPosition();
        NewRoad.transform.localScale = NewRoad.CalculateLength();

        return NewRoad;
    }
}
