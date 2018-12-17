using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoreText : MonoBehaviour {

    string planetLore;
    public Text loreText;

    public CameraFollow camFollow;

    // Update is called once per frame
    void Update()
    {
        planetLore = GameObject.FindGameObjectWithTag("Player").GetComponent<Ball>().planetStats.lore;
        loreText.text = planetLore;

        if (camFollow.currentZoom > 10f)
        {
            loreText.enabled = false;
        }
        else
        {
            loreText.enabled = true;
        }
    }
}
