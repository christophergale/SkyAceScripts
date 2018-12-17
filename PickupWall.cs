using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupWall : MonoBehaviour {

    public int pickupsRequired;
    Text pickupsRequiredText;
	
	// Update is called once per frame
	void Update () {
        pickupsRequiredText = GetComponentInChildren<Text>();
        if (pickupsRequired - GameManager.instance.pickups > 0)
        {
            pickupsRequiredText.text = (pickupsRequired - GameManager.instance.pickups).ToString() + " pickups required";
        } else if (pickupsRequired - GameManager.instance.pickups <= 0)
        {
            Debug.Log("0 required");
            pickupsRequiredText.text = "0 pickups required";
        }
    }
}
