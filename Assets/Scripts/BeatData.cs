using System.Collections.Generic;
using UnityEngine;

public struct SliderPair
{
    public double startTime;
    public double endTime;

    public SliderPair(double start, double end)
    {
        startTime = start;
        endTime = end;
    }
}

public class BeatData : MonoBehaviour
{
    public static List<double> beatTimes = new List<double>()
    {
        1.2, 1.7, 2.2,
        2.7, 2.95, 3.2,
        3.7, 4.2,
        4.7, 4.95, 5.2,
        5.7, 6.2,
        6.7, 6.95, 7.2,
        7.7, 8.2,
        8.7, 8.95, 9.2,

        9.7, 9.95, 10.2, 10.45,
        10.7, 10.95, 11.2, 11.45,
        11.7, 11.95, 12.2, 12.45,
        12.7, 12.95, 13.2, 13.45,
        13.7, 13.95, 14.2, 14.45,
        14.7, 14.95, 15.2, 15.45,
        15.7, 15.95, 16.2,

        // Chorus
        16.95, 17.075, 17.45, 17.7,
        // Slider
        18.9, 19, 19.1,
        19.45, 19.7, 19.95, 20.2, 20.45, 20.7,
        20.95, 21.05, 21.15, 21.45, 21.7,
        22.2, 22.3, 22.45, 22.55,
        // Slider
        23.95, 24.05, 24.15, 24.45, 24.7,
        24.95, 25.075, 25.2, 25.45, 25.7, 25.95, 26.2,
        26.45, 26.55,
        // Slider
        28.2, 28.45, 28.7, 28.8,

        29.2, 29.3, 29.45, 29.7, 29.95, 30.2, 30.45, 30.7, 30.95,
        // Slider
        32.2, 32.45, 32.7, 32.8
    };

    public static List<SliderPair> sliderTimes = new List<SliderPair>()
    {
        new SliderPair(18.2, 18.8),
        new SliderPair(22.7, 23.7),
        new SliderPair(26.95, 27.95),
        new SliderPair(31.2, 31.95),
    };

    public static void RepeatChorus()
    {
        int startIndex = beatTimes.IndexOf(16.95);
        int endIndex = beatTimes.Count - 1;
        double offset = 33.2 - 16.95;
        for (int i = startIndex; i <= endIndex; i++)
        {
            beatTimes.Add(beatTimes[i] + offset);
        }

        endIndex = sliderTimes.Count - 1;
        for (int i = 0; i <= endIndex; i++)
        {
            var pair = sliderTimes[i];
            sliderTimes.Add(new SliderPair(pair.startTime + offset, pair.endTime + offset));
        }
    }
}
