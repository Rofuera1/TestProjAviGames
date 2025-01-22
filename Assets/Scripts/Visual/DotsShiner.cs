using UnityEngine;

public class DotsShiner : MonoBehaviour
{
    private Dot OverlappedDot;

    public void OnShine(Dot Dot)
    {
        OverlappedDot = Dot;
        OverlappedDot.OnChoose();
    }

    public void OnExitShining()
    {
        OverlappedDot?.OnUnchoose();
    }
}
