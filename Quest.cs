using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour {

    public bool questComplete = false;
    public bool questCheck = false;
    public string questLogText;

    public virtual void Update()
    {
        if (questCheck)
        {
            GameManager.instance.CheckQuests();
            questCheck = false;
        }
    }

}
