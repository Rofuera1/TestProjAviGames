using UnityEngine;
using System.Collections.Generic;

public class AudioSourcePool : MonoBehaviour
{
    [SerializeField] private int initialSize = 10;
    [SerializeField] private AudioSource audioSourcePrefab;

    private Queue<AudioSource> pool = new Queue<AudioSource>();

    private void Awake()
    {
        for (int i = 0; i < initialSize; i++)
        {
            AudioSource source = CreateNewAudioSource();
            pool.Enqueue(source);
        }
    }

    public AudioSource GetAudioSource()
    {
        if (pool.Count > 0)
        {
            AudioSource source = pool.Dequeue();
            source.gameObject.SetActive(true);
            return source;
        }
        else
        {
            return CreateNewAudioSource();
        }
    }
    public void ReturnAudioSource(AudioSource source)
    {
        source.Stop();
        source.clip = null;
        source.gameObject.SetActive(false);
        pool.Enqueue(source);
    }
    private AudioSource CreateNewAudioSource()
    {
        GameObject audioObj = new GameObject("PooledAudioSource");
        audioObj.transform.SetParent(transform);
        AudioSource source = audioObj.AddComponent<AudioSource>();
        audioObj.SetActive(false);
        return source;
    }
}
