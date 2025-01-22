using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public const int SECTIONS_AMOUNT_WIDTH = 6;
    public Section[,] Sections { get; private set; }
    public static HashSet<Road> IntersectingRoadsStatic { get; private set; } = new HashSet<Road>();
    public static HashSet<Road> NotIntersectingRoadsStatic { get; private set; } = new HashSet<Road>();

    private float SectionSize;

    private Vector2 TopRightCorner;
    private Vector2 BottomLeftCorner;

    public void Init(Transform BottomCorner, Transform TopCorner)
    {
        TopRightCorner = TopCorner.position;
        BottomLeftCorner = BottomCorner.position;

        SectionSize = (TopRightCorner.x - BottomLeftCorner.x) / SECTIONS_AMOUNT_WIDTH;

        int SectionAmountVertical = Mathf.CeilToInt((TopRightCorner.y - BottomLeftCorner.y) / SectionSize);

        Sections = new Section[SECTIONS_AMOUNT_WIDTH, SectionAmountVertical];
        for (int i = 0; i < SECTIONS_AMOUNT_WIDTH; i++)
            for (int j = 0; j < SectionAmountVertical; j++)
                Sections[i, j] = new Section(new List<Dot>(), new List<Road>());
    }

    public void AssignDotsAndRoads(IEnumerable<Dot> Dots, IEnumerable<Road> Roads)
    {
        foreach (Dot Dot in Dots)
            AddDot(Dot);
        foreach (Road Road in Roads)
            AddRoadToSections(Road);
    }

    public void AddRoadToSections(Road Road)
    {
        foreach(var Section in CalculateRoadSectionPositions(Road))
        {
            AddRoad(Section, Road);
        }
    }

    public void RemoveRoadFromCurrent(Road Road)
    {
        foreach (var Section in CalculateRoadSectionPositions(Road))
        {
            RemoveRoad(Section, Road);
        }
    }

    public void AddDot(Dot Dot)
    {
        (int X, int Y) = CalculateSectionPosition(Dot.Position);
        Sections[X, Y].AddDot(Dot); 
    }

    public void RemoveDotFromCurrent(Dot Dot)
    {
        (int X, int Y) = CalculateSectionPosition(Dot.Position);
        Sections[X, Y].RemoveDot(Dot);
    }

    private void AddRoad((int, int) Section, Road Road)
    {
        Sections[Section.Item1, Section.Item2].AddRoad(Road);
    }

    private void RemoveRoad((int, int) Section, Road Road)
    {
        Sections[Section.Item1, Section.Item2].RemoveRoad(Road);
    }

    private List<(int, int)> CalculateRoadSectionPositions(Road Road)
    {
        HashSet<(int, int)> Result = new HashSet<(int, int)>();

        Dot FirstDot = Road.DotFirst;
        Dot SecondDot = Road.DotSecond;

        if (FirstDot.Position.x > TopRightCorner.x || FirstDot.Position.x < BottomLeftCorner.x ||
            FirstDot.Position.y > TopRightCorner.y || FirstDot.Position.y < BottomLeftCorner.y) return null;

        if (SecondDot.Position.x > TopRightCorner.x || SecondDot.Position.x < BottomLeftCorner.x ||
            SecondDot.Position.y > TopRightCorner.y || SecondDot.Position.y < BottomLeftCorner.y) return null;

        Vector2 EndPosition = SecondDot.Position;

        float distance = Vector2.Distance(FirstDot.Position, SecondDot.Position);
        float step = (SectionSize / distance) * 0.5f;
        for (float t = 0; t <= 1; t += step)
        {
            Vector2 pos = Vector2.Lerp(FirstDot.Position, SecondDot.Position, t);
            Result.Add(CalculateSectionPosition(pos));
        }
        Result.Add(CalculateSectionPosition(EndPosition));

        List<(int, int)> Res = new List<(int, int)>();
        Res.AddRange(Result);
        return Res;
    }

    private (int, int) CalculateSectionPosition(Vector2 Position)
    {
        if (Position.x > TopRightCorner.x || Position.x < BottomLeftCorner.x ||
            Position.y > TopRightCorner.y || Position.y < BottomLeftCorner.y) return (0, 0);

        Vector2 RelativePosition = Position;
        RelativePosition.x -= BottomLeftCorner.x;
        RelativePosition.y -= BottomLeftCorner.y;

        int PositionByX = (int)(RelativePosition.x / SectionSize);
        int PositionByY = (int)(RelativePosition.y / SectionSize);

        return (PositionByX, PositionByY);
    }
    public bool ShowDebug;
    private void Update()
    {
        if (ShowDebug)
        {
            string res = "";
            int SectionAmountVertical = Mathf.CeilToInt((TopRightCorner.y - BottomLeftCorner.y) / SectionSize);

            for (int i = 0; i < SECTIONS_AMOUNT_WIDTH; i++)
            {
                for(int j = 0; j < SectionAmountVertical; j++)
                {
                    foreach (Road Road in Sections[i, j].Roads)
                        res += "In " + i + ":" + j + " sections there's " + Road.gameObject.name + "\n";
                }
            }

            ShowDebug = false;
            Debug.Log(res);
        }
    }

    /*private void Update()
    {
        for(int i = 0; i < SECTIONS_AMOUNT_WIDTH; i++)
        {
            Debug.DrawRay(BottomLeftCorner + Vector2.right * i * SectionSize, Vector2.up * 100, Color.black);
        }
        int SectionAmountVertical = Mathf.CeilToInt((TopRightCorner.y - BottomLeftCorner.y) / SectionSize);
        for (int i = 0; i < SectionAmountVertical; i++)
        {
            Debug.DrawRay(BottomLeftCorner + Vector2.up * i * SectionSize, Vector2.right * 100, Color.black);
        }

        foreach (Road Road in Section.IntersectingRoadsStatic)
            Debug.DrawLine(Road.DotFirst.Position, Road.DotSecond.Position, Color.red);

    }*/
}


public class Section
{
    public List<Dot> Dots { get; private set; }
    public List<Road> Roads { get; private set; }
    public HashSet<Road> IntersectingRoads { get; private set; } = new HashSet<Road>();
    public Section(List<Dot> dots, List<Road> roads)
    {
        Dots = dots;
        Roads = roads;
        RecalculateRoadIntersections();
    }

    public void AddDot(Dot dot)
    {
        if (!Dots.Contains(dot))
        {
            Dots.Add(dot);
        }
    }

    public void RemoveDot(Dot dot)
    {
        if (Dots.Contains(dot))
        {
            Dots.Remove(dot);
        }
        else
        {
            Debug.LogWarning("No Dot For Removal");
        }
    }

    public void AddRoad(Road road)
    {
        if (!Roads.Contains(road))
        {
            Roads.Add(road);
            RecalculateRoadIntersections();
        }
    }

    public void RemoveRoad(Road road)
    {
        if (Roads.Contains(road))
        {
            Roads.Remove(road);
            RecalculateRoadIntersections();
        }
        else
        {
            Debug.LogWarning("No Road For Removal");
        }
    }

    public void RecalculateRoadIntersections()
    {
        foreach(Road Road in IntersectingRoads)
        {
            Map.IntersectingRoadsStatic.Remove(Road);
            Map.NotIntersectingRoadsStatic.Add(Road);
        }

        IntersectingRoads.Clear();

        HashSet<Road> roadsThatIntersect = RoadIntersectorHelper.DoAnyRoadsIntersect(Roads);
        foreach (Road rd in roadsThatIntersect)
        {
            IntersectingRoads.Add(rd);

            Map.IntersectingRoadsStatic.Add(rd);
            Map.NotIntersectingRoadsStatic.Remove(rd);
        }
    }
}