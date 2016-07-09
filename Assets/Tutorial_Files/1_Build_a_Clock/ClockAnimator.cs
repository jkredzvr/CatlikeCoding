using UnityEngine;
using System.Collections;
using System;

public class ClockAnimator : MonoBehaviour
{
    //The const keyword indicates that a value will never change and needn't be variable. 
    //Its value will be computed during compilation and is directly inserted wherever it's referenced.
    private const float
        hoursToDegrees = 360f / 12f,
        minutesToDegrees = 360f / 60f,
        secondsToDegrees = 360f / 360f;

    public Transform hours, minutes, seconds;

    public bool analog;

    private void Update()
    {
        if (analog)
        {
            //time span returns fractional hours, minutes, seconds as doubles
            TimeSpan timespan = DateTime.Now.TimeOfDay;
            hours.localRotation = Quaternion.Euler(
                0f, 0f, (float)timespan.TotalHours * -hoursToDegrees);
            minutes.localRotation = Quaternion.Euler(
                0f, 0f, (float)timespan.TotalMinutes * -minutesToDegrees);
            seconds.localRotation = Quaternion.Euler(
                0f, 0f, (float)timespan.TotalSeconds * -secondsToDegrees);
        }
        else
        {
            DateTime time = DateTime.Now;
            hours.localRotation =
                Quaternion.Euler(0f, 0f, time.Hour * -hoursToDegrees);
            minutes.localRotation =
                Quaternion.Euler(0f, 0f, time.Minute * -minutesToDegrees);
            seconds.localRotation =
                Quaternion.Euler(0f, 0f, time.Second * -secondsToDegrees);
        }
    }
}
