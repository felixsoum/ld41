﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BeatGenerator : MonoBehaviour
{
    public float debugStartTime;
    public GameObject beatPrefab;
    public GameObject sliderPrefab;
    public AudioSource musicAudio;
    public Combo combo;
    public Text scoreText;
    public Player player;
    public GameObject fade;

    double musicStartTime;
    bool isMusingStarted;
    float secondsBeforePlaySong = 2;
    int beatCounter;
    int beatIndex;
    int sliderIndex;
    List<Beat> beats = new List<Beat>();

    Vector2 spawnDirection = Vector2.right;
    Vector2 spawnPosition = Vector2.zero;
    float spawnDistance = 1;
    float spawnAngleMax = 45;
    const float SpawnHorizontalLimit = 7;
    const float SpawnVerticalLimit = 3.5f;
    int comboCount;
    int scoreCount;
    bool isDone;

    void Start()
    {
        BeatData.RepeatChorus();

        if (debugStartTime > 0)
        {
            while (beatIndex < BeatData.beatTimes.Count &&
    BeatData.beatTimes[beatIndex] - Beat.ShrinkTime <= debugStartTime)
            {
                beatIndex++;
            }

            while (sliderIndex < BeatData.sliderTimes.Count &&
    BeatData.sliderTimes[sliderIndex].startTime - Beat.ShrinkTime <= debugStartTime)
            {
                sliderIndex++;
            }
        }

        musicAudio.time = debugStartTime;
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
            double time = AudioSettings.dspTime - musicStartTime + debugStartTime;
            while (beatIndex < BeatData.beatTimes.Count &&
                BeatData.beatTimes[beatIndex] - Beat.ShrinkTime <= time)
            {
                SpawnBeat(BeatData.beatTimes[beatIndex]);
                beatIndex++;
            }

            while (sliderIndex < BeatData.sliderTimes.Count &&
                BeatData.sliderTimes[sliderIndex].startTime - Beat.ShrinkTime <= time)
            {
                SpawnSlider(BeatData.sliderTimes[sliderIndex]);
                sliderIndex++;
            }

            for (int i = beats.Count - 1; i >= 0; i--)
            {
                Beat beat = beats[i];
                beat.UpdateTime(time);
                if (beat.IsDone)
                {
                    beats.RemoveAt(i);
                    beat.Kill(player.transform.position.x < beat.GetPosForPlayer().x);
                }
            }

            if (!isDone && beatIndex == BeatData.beatTimes.Count)
            {
                isDone = true;
                Invoke("Fade", 3f);
            }
        }
    }

    void Fade()
    {
        fade.SetActive(true);
    }

    void SpawnBeat(double timing)
    {
        Beat beat = Instantiate(beatPrefab, RandomBeatPosition(), Quaternion.identity).GetComponent<Beat>();
        beat.Timing = timing;
        beat.OnBeatDone += OnBeatDone;
        beats.Add(beat);
    }

    void SpawnSlider(SliderPair sliderPair)
    {
        Slider slider = Instantiate(sliderPrefab, RandomBeatPosition(true), Quaternion.identity).GetComponent<Slider>();
        slider.Init();
        slider.Timing = sliderPair.startTime;
        slider.TimingEnd = sliderPair.endTime;
        slider.OnBeatDone += OnBeatDone;
        slider.OnSlide += Slider_OnSlide;
        beats.Add(slider);

        var sliderEndPos = slider.GetEndPosition();
        spawnPosition.x = sliderEndPos.x;
        spawnPosition.y = sliderEndPos.y;

        spawnDirection = slider.slideDirection;
    }

    private void Slider_OnSlide(Vector3 slidePosition, Vector3 slideDirection)
    {
        player.SlideTo(slidePosition, slideDirection);
    }

    private void OnBeatDone(Beat beat, bool isSuccess)
    {
        comboCount = isSuccess ? comboCount + 1 : 0;
        combo.SetCombo(comboCount);

        scoreCount += comboCount + (int)Mathf.Pow(comboCount, 2);
        string scoreString = scoreCount.ToString();
        while (scoreString.Length < 8)
        {
            scoreString = "0" + scoreString;
        }
        scoreText.text = scoreString;

        if (isSuccess)
        {
            player.MoveTo(beat.GetPosForPlayer());
        }
    }

    Vector3 RandomBeatPosition(bool isSlider = false)
    {

        float currentSpawnDistance = spawnDistance;
        bool isFastBeat = false;

        if (!isSlider && beatIndex > 0)
        {
            double timeDiff = BeatData.beatTimes[beatIndex] - BeatData.beatTimes[beatIndex - 1];
            if (timeDiff < 0.2)
            {
                isFastBeat = true;
            }
        }
        
        if (!isFastBeat)
        {
            spawnDirection = Quaternion.Euler(0, 0, Random.Range(-spawnAngleMax, spawnAngleMax)) * spawnDirection;

        }
        else
        {
            currentSpawnDistance /= 4;
        }

        spawnPosition += spawnDirection * currentSpawnDistance;

        if (spawnPosition.x < -SpawnHorizontalLimit ||
            spawnPosition.x > SpawnHorizontalLimit ||
            spawnPosition.y < -SpawnVerticalLimit ||
            spawnPosition.y > SpawnVerticalLimit)
        {
            ResetSpawnPosition();
        }

        return new Vector3(spawnPosition.x, spawnPosition.y, 100 + beatCounter++/100f);
    }

    void ResetSpawnPosition()
    {
        spawnPosition = new Vector2(Random.Range(-SpawnHorizontalLimit, SpawnHorizontalLimit) * 0.9f, Random.Range(-SpawnVerticalLimit, SpawnVerticalLimit) * 0.9f);
        spawnDirection = Vector2.zero - spawnPosition;
        spawnDirection.Normalize();
    }

    public void Restart()
    {
        SceneManager.LoadScene("Main");
    }
}
