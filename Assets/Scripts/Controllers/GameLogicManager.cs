using UnityEngine;

public class GameLogicManager : MonoBehaviour
{
    [Zenject.Inject] private DotsMover Mover;
    [Zenject.Inject] private Map Map;

    public System.Action OnCompletedLevel;

    private void Awake()
    {
        Mover.OnStartedMovingDot += RemoveDotFromMap;
        Mover.OnEndedMovingDot += CheckMovedDot;
    }

    private void RemoveDotFromMap(Dot Dot)
    {
        Map.RemoveDotFromCurrent(Dot);
        foreach (Road Road in Dot.RoadsConnected)
            Map.RemoveRoadFromCurrent(Road);
    }

    private void CheckMovedDot(Dot Dot)
    {
        Map.AddDot(Dot);
        foreach (Road Road in Dot.RoadsConnected)
            Map.AddRoadToSections(Road);

        
    }
}
