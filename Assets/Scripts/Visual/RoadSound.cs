using UnityEngine;

[RequireComponent(typeof(Road))]
[RequireComponent(typeof(AudioSource))]
public class RoadSound : MonoBehaviour
{
    private AudioSource Source;
    private Road RoadAttachedTo;

    private void Awake()
    {
        RoadAttachedTo = GetComponent<Road>();
        Source = GetComponent<AudioSource>();

        RoadAttachedTo.ChangedScale += OnTryPlaySound;
        RoadAttachedTo.EndedChanges += OnEndPlaySound;
    }

    private void OnTryPlaySound(float Scale)
    {
        if (Scale < 0.001f) return;
        if (Source.isPlaying) return;

        Source.Play();
    }

    private void OnEndPlaySound()
    {
        if (!Source.isPlaying) return;

        Source.Stop();
    }
}
