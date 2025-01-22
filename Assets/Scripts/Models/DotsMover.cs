using System;
using System.Collections;
using UnityEngine;

public class DotsMover : MonoBehaviour
{
    public Dot CurrentDot { get; private set; }
    public Action<Dot> OnStartedMovingDot;
    public Action<Dot> OnEndedMovingDot;

    private Vector3 DesiredPosition;
    private Vector3 SmootherRef;
    private const float SMOOTH_TIME = 0.2f;

    private bool ShouldBeMoving;

    public void OnStartMoveDot(Dot Dot)
    {
        CurrentDot = Dot;

        DesiredPosition = Dot.Position;
        SmootherRef = Vector3.zero;

        ShouldBeMoving = true;
        OnStartedMovingDot?.Invoke(Dot);

        StartCoroutine(SmoothMover());
    }

    public void OnMoved(Vector2 Position)
    {
        DesiredPosition = Position;
    }

    public void OnEndMove()
    {
        ShouldBeMoving = false;

        if(CurrentDot)
            OnEndedMovingDot?.Invoke(CurrentDot);
        CurrentDot = null;
    }

    private IEnumerator SmoothMover()
    {
        Transform DotTransform = CurrentDot.transform;

        while(ShouldBeMoving)
        {
            DotTransform.position = Vector3.SmoothDamp(DotTransform.position, DesiredPosition, ref SmootherRef, SMOOTH_TIME);

            yield return null;
        }
    }
}
