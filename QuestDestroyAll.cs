using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDestroyAll : Quest {

    public GameObject[] targets;
    private void Start()
    {
        questLogText = "Destroy nearby enemies";
    }

    // Update is called once per frame
    public override void Update () {
        base.Update();

        if (!questComplete)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                if (targets[i] != null)
                {
                    break;
                } else
                {
                    questComplete = true;
                    questCheck = true;
                }
            }
        }
        
	}
}
