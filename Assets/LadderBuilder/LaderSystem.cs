using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LadderSystem
{
    public GameObject ladderPiecePrefab;
    public Vector3 boundsSize = Vector3.one;
    public Vector3 boundsCenter = Vector3.zero;
    public List<GameObject> ladderPieces = new List<GameObject>();
    public float ladderPieceDistance = 0.0f;
    public bool isDraggingSlider = false;
    public GameObject ladderParent;
}

