using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level", order = 1)]

public class LevelInfo : ScriptableObject {
    public int levelIndex;
    public int planetCount;
    public float quickTime;
}
