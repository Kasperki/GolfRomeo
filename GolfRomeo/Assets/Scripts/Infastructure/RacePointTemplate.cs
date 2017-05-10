using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public struct RacePointTemplate
{
    public int[] PointOrder;
    public int FastestLap;
    public int MostOutTakes;
    public int DidNotFinish;

    public static RacePointTemplate DefaultTemplate 
    {
        get
        {
            return new RacePointTemplate()
            {
                PointOrder = new int[RaceManager.MaxPlayers] { 10, 8, 7, 5, 3, 2, 1, 0 },
                FastestLap = 1,
                DidNotFinish = 0,
                MostOutTakes = 0,
            };
        }
    }
}
