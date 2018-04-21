using System.Collections.Generic;
using UnityEngine;

public class BeatGenerator : MonoBehaviour
{
    public GameObject beatPrefab;
    public AudioSource musicAudio;
    double musicStartTime;
    bool isMusingStarted;
    float secondsBeforePlaySong = 1;

    int beatIndex = 0;
    List<Beat> beats = new List<Beat>();

    void Start()
    {
        Invoke("PlaySong", secondsBeforePlaySong);
    }

    void PlaySong()
    {
        isMusingStarted = true;
        musicAudio.Play();
        musicStartTime = AudioSettings.dspTime;
    }

    void Update()
    {
        if (isMusingStarted)
        {
            double time = AudioSettings.dspTime - musicStartTime;
            while (beatIndex < BeatData.beatTimes.Length &&
                BeatData.beatTimes[beatIndex] - Beat.ShrinkTime <= time)
            {
                SpawnBeat(BeatData.beatTimes[beatIndex]);
                beatIndex++;
            }

            for (int i = beats.Count - 1; i >= 0; i--)
            {
                Beat beat = beats[i];
                beat.UpdateTime(time);
                if (beat.IsDone)
                {
                    beats.RemoveAt(i);
                    beat.Kill();
                }
            }
        }
    }

    void SpawnBeat(double timing)
    {
        Beat beat = Instantiate(beatPrefab, RandomBeatPosition(), Quaternion.identity).GetComponent<Beat>();
        beat.Timing = timing;
        beats.Add(beat);
    }

    Vector3 RandomBeatPosition()
    {
        return new Vector3(Random.Range(-5, 5), Random.Range(-3, 3), 0);
    }
}
