using UnityEngine;

public class MoverManager : MonoBehaviour
{
    [Zenject.Inject] private InputHandler Input;
    [Zenject.Inject] private DotsMover Mover;
    [Zenject.Inject] private DotsFinder Finder;
    [Zenject.Inject] private DotsShiner Shiner;
    [Zenject.Inject] private CameraConverter CameraHelper;

    private Dot ChosenDot;

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        Input.OnTouchStarted += OnStartMove;
        Input.OnTouching += OnMove;
        Input.OnTouchEnded += OnEndedMoving;
    }

    private void Unsubscribe()
    {
        Input.OnTouchStarted -= OnStartMove;
        Input.OnTouching -= OnMove;
        Input.OnTouchEnded -= OnEndedMoving;
    }

    private void OnStartMove(Vector2 Position)
    {
        Vector2 WorldPosition = CameraHelper.ConvertScreenToWorldPoint(Position);

        ChosenDot = Finder.FindDot(WorldPosition);
        if (!ChosenDot) return;
        
        Mover.OnStartMoveDot(ChosenDot);
        Shiner.OnExitShining();
        Shiner.OnShine(ChosenDot);
    }

    private void OnMove(Vector2 Position)
    {
        Vector2 WorldPosition = CameraHelper.ConvertScreenToWorldPoint(Position);
        Mover.OnMoved(WorldPosition);

        if (!ChosenDot) OnShineOverlapping(WorldPosition);
    }

    private void OnEndedMoving(Vector2 Position)
    {
        Mover.OnEndMove();
        Shiner.OnExitShining();

        ChosenDot = null;
    }

    private void OnShineOverlapping(Vector2 WorldPosition)
    {
        Dot FoundDot = Finder.FindDot(WorldPosition);

        if (!FoundDot) Shiner.OnExitShining();
        else Shiner.OnShine(FoundDot);
    }
}