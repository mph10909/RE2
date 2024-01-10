using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ladder System Data")]
public class LadderSystemData : ScriptableObject
{
    public List<LadderSystem> ladderSystems = new List<LadderSystem>();
}


