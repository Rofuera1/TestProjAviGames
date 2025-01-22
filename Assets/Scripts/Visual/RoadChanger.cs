using UnityEngine;

public class RoadChanger : MonoBehaviour
{
    [Zenject.Inject] private DotsMover Mover;
    private Road[] CurrentRoads;
    private bool ShouldChange;

    private void Start()
    {
        Mover.OnStartedMovingDot += OnStartChange;
        Mover.OnEndedMovingDot += OnStartChange;
    }

    public void OnStartChange(Dot Dot)
    {
        CurrentRoads = Dot.RoadsConnected;
        ShouldChange = true;

        foreach (Road CurrentRoad in CurrentRoads)
            StartCoroutine(OnMoveRoad(CurrentRoad));
    }

    public void OnEndedChange(Dot Dot)
    {
        ShouldChange = false;

        RecolorAllRoads();
    }

    private System.Collections.IEnumerator OnMoveRoad(Road CurrentRoad)
    {
        float LERP_TIME = 0.01f;
        Vector3 PrefferablePositionRef = Vector2.zero;
        Vector3 PrefferableDirectionRef = Vector2.zero;
        Transform CurrentRoadTr = CurrentRoad.transform;

        while(ShouldChange) // Можно было бы добавить условие проверки на расстояние до позиции, или обновлять положение каждый раз при изменении позиции связанного Dot'a, но 4 Update'а несколько раз в игру кажутся неплохим разменом на громоздкую архитектуру ради сомнительной оптимизации
        {
            CurrentRoad.OnChangePositionDirectionScale( Vector3.SmoothDamp(CurrentRoadTr.position, CurrentRoad.CalculateMiddlePosition(), ref PrefferablePositionRef, LERP_TIME),
                                                        Vector3.SmoothDamp(CurrentRoadTr.right, CurrentRoad.CalculateDirectionPosition(), ref PrefferableDirectionRef, LERP_TIME),
                                                        CurrentRoad.CalculateLength());

            yield return null;
        }

        CurrentRoad.OnEndChangingPositionDirectionScale();
    }

    public void RecolorAllRoads()
    {
        foreach (Road Road in Map.NotIntersectingRoadsStatic)
            Road.SetGood(true);
        foreach (Road Road in Map.IntersectingRoadsStatic)
            Road.SetGood(false);
    }
}
