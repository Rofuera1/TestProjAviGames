using UnityEngine;

public class Road: MonoBehaviour
{
    [SerializeField] private RoadVisual Visual;
    public Dot DotFirst { get; private set; }
    public Dot DotSecond { get; private set; }
    public System.Action<float> ChangedScale;
    public System.Action EndedChanges;
    private float TextureWidth;

    public void Init(Dot dotFirst, Dot dotSecond)
    {
        DotFirst = dotFirst;
        DotSecond = dotSecond;

        TextureWidth = GetComponent<SpriteRenderer>().sprite.bounds.size.x;
    }

    public Vector2 CalculateMiddlePosition() => (DotFirst.Position + DotSecond.Position) * 0.5f;
    public Vector2 CalculateDirectionPosition() => (DotSecond.Position - DotFirst.Position).normalized;

    public Vector2 CalculateLength() =>  Vector2.right * Vector2.Distance(DotSecond.Position, DotFirst.Position) / TextureWidth + Vector2.up;

    public void SetGood(bool good)
    {
        if (good) Visual.SetGood();
        else Visual.SetBad();
    }

    public void OnChangePositionDirectionScale(Vector3 NewPosition, Vector3 NewRightPosition, Vector3 NewScale)
    {
        ChangedScale?.Invoke(Vector2.Distance(transform.localScale, NewScale));

        transform.position = NewPosition;
        transform.right = NewRightPosition;
        transform.localScale = NewScale;
    }

    public void OnEndChangingPositionDirectionScale()
    {
        EndedChanges?.Invoke();
    }
}
