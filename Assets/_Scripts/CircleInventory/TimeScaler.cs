using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaler : MonoBehaviour
{
    [Range(0,1)][SerializeField]float timeScaler;
    [Range(0,1)][SerializeField]float timeSetter;
    [SerializeField] GameState gameState;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameState = GameStateManager.Instance.CurrentGameState;
        timeScaler = Time.timeScale;
        //Time.timeScale = timeSetter;
    }
}
