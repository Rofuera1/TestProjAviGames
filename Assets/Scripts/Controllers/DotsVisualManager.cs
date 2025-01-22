using UnityEngine;

public class DotsVisualManager : MonoBehaviour
{
    [SerializeField] private Dot[] DotsOnScene; // #2/2: Можно было бы сделать через файлы сохранения, Scriptables, левелдизайн и тд, но в тестовом задании так быстрее :-)
    private System.Collections.Generic.List<Road> Roads;

    [Zenject.Inject] private DotsMover Mover;
    [Zenject.Inject] private RoadBuilder Builder;
    [Zenject.Inject] private RoadChanger Changer;
    [Zenject.Inject] private Map Map;

    private void Start()
    {
        InitAllDots();
        BuildAllRoads();

        Map.AssignDotsAndRoads(DotsOnScene, Roads);

        Changer.RecolorAllRoads();

        Mover.OnStartedMovingDot += OnStartedMovingDot;
        Mover.OnEndedMovingDot += OnEndedMovingDot;
    }

    private void InitAllDots()
    {
        foreach (Dot Dot in DotsOnScene)
            Dot.Init();
    }

    private void BuildAllRoads()
    {
        Roads = Builder.BuildRoads(DotsOnScene);
    }

    private void OnStartedMovingDot(Dot Dot)
    {
        Changer.OnStartChange(Dot);
    }

    private void OnEndedMovingDot(Dot Dot)
    {
        Changer.OnEndedChange(Dot);
    }

    public bool ShowDebug;
    private void Update()
    {
        if(ShowDebug)
        {
            string res = "";
            foreach(Dot Dot in DotsOnScene)
            {
                res += ("Dot's #" + Dot.gameObject.name + " Position " + Dot.Position + "\n");
                res += ("Connected with ");
                foreach (Dot DotConnected in Dot.DotsConnected)
                    res += ("#" + DotConnected.gameObject.name + "\n");
            }

            ShowDebug = false;
            Debug.Log(res);
        }
    }
}
