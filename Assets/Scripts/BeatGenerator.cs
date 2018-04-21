using System.Collections.Generic;
using UnityEngine;

public class BeatGenerator : MonoBehaviour
{
    public GameObject beatPrefab;
    public AudioSource musicAudio;
    double musicStartTime;
    bool isMusingStarted;
    float secondsBeforePlaySong = 1;
    int beatCounter;
    int beatIndex = 0;
    List<Beat> beats = new List<Beat>();
    float randomX;
    float randomY;

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
        randomX += Random.Range(-1f, 1f);
        randomY += Random.Range(-1f, 1f);
        randomX = Mathf.Clamp(randomX, -5, 5);
        randomY = Mathf.Clamp(randomY, -3, 3);
        return new Vector3(0, 0, 1 + beatCounter++/100f);
    }
}
