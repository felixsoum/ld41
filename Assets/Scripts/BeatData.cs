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
    public static double[] beatTimes = new double[]
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

        16.95, 17.075, 17.45, 17.7

        //17.05, 17.12, 17.27,  17.42, // slider - 18.05 - 18.43,
        //18.67, 19.05, 19.17, 19.27, 19.35, 20, 20.13, 20.27,
        //20.38, 20.48, 21.06, 21.18, 21.28, 21.42, 22.1,
        //22.18, 22.32, 22.38, //slider 22.45 - 23.83,
        //24, 24.18, 24.28, 24.43, 25.05, 25.15, 25.22, 25.28,
        //25.43, 26.08, 26.21, 26.3, 26.37, //slider - 26.83 - 28,

        //28.07, 28.17, 28.32, 28.37, 29.1, 29.15, 29.3, 29.43,
        //30.05, 30.2, 30.3, 30.45, //slider - 31.03 - 31.45,

    };

    public static SliderPair[] sliderTimes = new SliderPair[]
    {
        new SliderPair(20, 25)
    };
}
