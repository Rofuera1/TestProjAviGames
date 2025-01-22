using UnityEngine;

public class Dot : MonoBehaviour
{
    [SerializeField] private Dot[] ConnectedDotsStart; // #1/2 ����� ���� �� ������� ����� ����� ����������, Scriptables, ����������� � ��, �� � �������� ������� ��� ������� :-)
    [SerializeField] private DotVisual Visual;
    public Dot[] DotsConnected { get; private set; }
    public Road[] RoadsConnected { get; private set; }
    public Vector2 Position => transform.position;


    public void Init()
    {
        DotsConnected = ConnectedDotsStart;
    }

    public void AssignRoads(Road[] Roads)
    {
        RoadsConnected = Roads;
    }

    public void OnChoose()
    {
        Visual.SetActive(true);
    }

    public void OnUnchoose()
    {
        Visual.SetActive(false);
    }
}