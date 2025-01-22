using UnityEngine;

public class MapScaler : MonoBehaviour
{
    public Transform BottomLeftCorner;
    public Transform TopRightCorner;

    [Zenject.Inject] private Map Map;
    [Zenject.Inject] private CameraConverter CameraHelper;

    private void Awake()
    {
        Vector3 TopCorner = CameraHelper.ConvertScreenToWorldPoint(Vector3.right * Screen.width + Vector3.up * Screen.height);
        TopCorner.z = TopRightCorner.position.z;

        Vector3 BottomCorner = CameraHelper.ConvertScreenToWorldPoint(Vector3.zero);
        BottomCorner.z = BottomLeftCorner.position.z;

        BottomLeftCorner.position = BottomCorner;
        TopRightCorner.position = TopCorner;

        Map.Init(BottomLeftCorner, TopRightCorner);
    }
}
