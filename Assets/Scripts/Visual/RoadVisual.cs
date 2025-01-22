using UnityEngine;

[RequireComponent(typeof (SpriteRenderer))]
public class RoadVisual : MonoBehaviour
{
    public Sprite BadRoad;
    public Sprite GoodRoad;

    private SpriteRenderer Renderer;

    private void Awake()
    {
        Renderer = GetComponent<SpriteRenderer>();
    }

    public void SetGood()
    {
        Renderer.sprite = GoodRoad;
    }

    public void SetBad()
    {
        Renderer.sprite = BadRoad;
    }
}
