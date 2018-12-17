using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestCollectTreasure : Quest {

    public Treasure[] treasures;

    private void Start()
    {
        questLogText = "Collect nearby treasure";
    }

    public override void Update()
    {
        base.Update();

        if (!questComplete)
        {
            for (int i = 0; i < treasures.Length; i++)
            {
                if (treasures[i] != null)
                {
                    break;
                }
                else
                {
                    questComplete = true;
                    questCheck = true;
                }
            }
        }
    }
}
