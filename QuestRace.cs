using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestRace : Quest {

    public Planet raceEnd;
    public float raceTime;
    public float raceStart;
    public bool racing = false;

    // public GameObject finishPointer;
    // public GameObject compass;

    public override void Update()
    {
        base.Update();

        if (Ball.instance.planetClass == raceEnd)
        {
            EndRace();
        }

        if (racing)
        {
            //finishPointer.transform.position = raceEnd.transform.position + (Vector3.up * 8f);
            //compass.transform.position = Ball.instance.transform.position + Vector3.Normalize(raceEnd.transform.position - Ball.instance.transform.position) * 3f;
        }

        //compass.SetActive(racing);
    }

    public void StartRace()
    {
        raceStart = Time.time;
        racing = true;
    }

    public void EndRace()
    {
        if (Time.time - raceStart < raceTime)
        {
            racing = false;
            questComplete = true;
            questCheck = true;
        }
    }
}