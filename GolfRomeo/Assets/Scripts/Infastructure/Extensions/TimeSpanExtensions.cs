using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeSpanExtensions
{
	public static string GetTimeInMinutesAndSeconds(this TimeSpan timespan)
    {
        return timespan.Minutes.ToString("00") + ":" + timespan.Seconds.ToString("00") + "." + timespan.Milliseconds.ToString("000");
    }
}
