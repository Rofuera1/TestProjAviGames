using UnityEngine;

public class DotVisual : MonoBehaviour
{
    public GameObject ActiveOutline;

    public void SetActive(bool Active)
    {
        ActiveOutline.SetActive(Active);
    }
}
