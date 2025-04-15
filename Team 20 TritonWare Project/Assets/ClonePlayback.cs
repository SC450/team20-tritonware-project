using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClonePlayback : MonoBehaviour
{
    public SpriteRenderer cloneSR;
    public float playbackInterval = 0.02f;

    private List<MovementSnapshot> playbackHistory = new List<MovementSnapshot>();

    public void Initialize(List<MovementSnapshot> recordedHistory)
    {
        playbackHistory = new List<MovementSnapshot>(recordedHistory); // copy
        StartCoroutine(PlayReverse());
    }

    IEnumerator PlayReverse()
    {
        for (int i = playbackHistory.Count - 1; i >= 0; i--)
        {
            transform.position = playbackHistory[i].position;
            cloneSR.flipX = playbackHistory[i].flipX;

            yield return new WaitForSeconds(playbackInterval);
        }

        Destroy(gameObject); // optional: remove clone after playback
    }
}