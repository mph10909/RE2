using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDripScript : MonoBehaviour
{
    [SerializeField] AudioClip waterDrip;
    [SerializeField] float minDripDelay;
    [SerializeField] float maxDripDelay;
    Animator m_Animator;
    AudioSource waterDripAudioSource;

    

    void Awake()
    {
        waterDripAudioSource = GetComponent<AudioSource>();
        m_Animator = gameObject.GetComponent<Animator>();
    }

    void Start()
    {
        //GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    void WaterDripAudio()
    {
        waterDripAudioSource.PlayOneShot(waterDrip);
    }

    private IEnumerator WaterDripPause()
    {
        m_Animator.enabled = false;
        yield return new WaitForSecondsRealtime(Random.Range(minDripDelay , maxDripDelay));
        m_Animator.enabled = true;
    }

    void OnEnable()
    {
       GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        waterDripAudioSource.volume = 1;
    }

    void OnDisable()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        waterDripAudioSource.volume = 0;
    }

    private void OnGameStateChanged(GameState newGameState)
    {
         enabled = newGameState == GameState.GamePlay;

     }

    void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }




}
