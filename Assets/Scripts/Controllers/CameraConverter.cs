using UnityEngine;

public class CameraConverter : MonoBehaviour
{
    [SerializeField]
    private Camera Camera;

    public Vector2 ConvertScreenToWorldPoint(Vector2 ScreenPoint) => Camera.ScreenToWorldPoint(ScreenPoint);
}
