using UnityEngine;

public class DotsFinder : MonoBehaviour
{
    public Dot FindDot(Vector2 Position)
    {
        RaycastHit2D HitInfo = Physics2D.Raycast(Position, Vector2.zero, 0f, LayerMask.GetMask("Dots"));

        if (HitInfo)
        {
            Dot Dot = HitInfo.collider.GetComponent<Dot>();
            return Dot;
        }

        return null;
    }
}
