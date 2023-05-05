using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{

public float songBpm;
public float secPerBeat;
public float songPosition;
public float songPositionInBeats;
public float dspSongTime;
public float firstBeatOffset = 0;
public AudioSource musicSource;

    // Start is called before the first frame update
    void Start()
    {
        // musicSource = GetComponent<AudioSource>();
        secPerBeat = 60f / songBpm;
        dspSongTime = (float)AudioSettings.dspTime;
        musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);
        songPositionInBeats = songPosition / secPerBeat;
    }
}
