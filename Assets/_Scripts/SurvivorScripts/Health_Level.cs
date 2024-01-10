using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health_Level : MonoBehaviour
{
    [SerializeField] Color healthColor;
    [SerializeField] GameObject[] colorBars;
    [SerializeField] GameObject[] Status;
    Sprite h_Status;
    [SerializeField] Sprite[] healthStatus;
    public int HealthAmount ;
    int HealthColorAmount ;
    bool Death;
    [SerializeField] GameObject[] playerUI;
    [SerializeField] AudioClip deathAudio;
    AudioSource m_Audio;

    void Start()
    {
        
        HealthColorAmount = HealthAmount;
        Death = false;
        m_Audio = GetComponent<AudioSource>();
        healthColor = new Color32(0,255,16,255);

    }
    void Update()
    {

        foreach(GameObject colors in colorBars) colors.GetComponent<Image>().color = healthColor;
        foreach(GameObject Stats in Status) Stats.GetComponent<Image>().sprite = h_Status;
        if(HealthAmount == 0) PlayerDeath();
        HealthDeterminer();
    }

    void HealthDeterminer()
    {
       
        if(HealthAmount <= HealthColorAmount){healthColor = new Color32(0,255,0,255); h_Status = healthStatus[0];}            //Green
        if(HealthAmount <= HealthColorAmount/1.5f){healthColor = new Color32(255,255,0,255); h_Status = healthStatus[1];}     //Yellow
        if(HealthAmount <= HealthColorAmount/4){ healthColor = new Color32(255,0,0,255); h_Status = healthStatus[2];}         //Red
    }

    void PlayerDeath()
    {
            if(!m_Audio.isPlaying && !Death) m_Audio.PlayOneShot(deathAudio);
            playerUI[0].SetActive(false);
            playerUI[1].SetActive(false);
            playerUI[2].SetActive(true);
            Time.timeScale = 0;
            Death = true;
    }
}
