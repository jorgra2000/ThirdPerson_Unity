using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLoop : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] playlist;
    private int currentTrackIndex = 0;

    private void Start()
    {
        if (playlist.Length == 0)
        {
            Debug.LogWarning("No hay canciones asignadas.");
            return;
        }

        PlayTrack(currentTrackIndex);
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            NextTrack();
        }
    }

    private void PlayTrack(int index)
    {
        if (playlist.Length == 0) return;

        audioSource.clip = playlist[index];
        audioSource.Play();
    }

    private void NextTrack()
    {
        currentTrackIndex = (currentTrackIndex + 1) % playlist.Length;
        PlayTrack(currentTrackIndex);
    }
}
