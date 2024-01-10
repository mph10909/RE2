using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using TMPro;
//using Cinemachine;
using System;
using UnityEngine.UI;

public class dialogueManager : MonoBehaviour
{
    public static event Action<LayerMask> SetLayerMask;
    public LayerMask MaskCharacters, ResetMask;
    public Text textComponent;
    public string[] lines;
    private float textSpeed = 0.01f; 
    private float skipSpeed = 0.00000f; //
    public bool holdE = false;
    private float currentTextSpeed; //
    public bool inRange;
    private bool isOpen;
    //private TankControls tankControls;
    //  set on object that needs text, set actiontrigger script and trigger collider to object.
    private int index;
    public AudioClip audioClip;
    public AudioSource audioSource;
    //public CinemachineVirtualCamera CameraActivate;
    // Start is called before the first frame update
    void Start()
    {
        //tankControls = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<TankControls>();
        //  textSpeed = 0.03f; - if you're going to make a variable private, might aswell not call it in function, but initialize it directly, so I commentede it out, but kept the value.
        inRange = false;

        textComponent.text = string.Empty;
    }

    public int Index
    {
        get { return index; }
    }


void Update()
{
    if (lines.Length < 0) return;
    if (!inRange) return;
    if (Input.GetKeyDown(KeyCode.E))
    {
        if (!isOpen) { StartDialogue(); return; }
        if (textComponent.text == lines[index]) NextLine();
    }
}
public void StartDialogue()
    {
        if (audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
        //tankControls.dialogueOpen = true;
        PauseGame();
        index = 0;
        StartCoroutine(TypeLine());
        isOpen = true;
    }
    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            HandleTextSpeed(); // new function at the bottom.
            textComponent.text += c;
            yield return new WaitForSecondsRealtime(currentTextSpeed); // swapped out textSpeed with currentTextSpeed, because our new HandleTextSpeed method handles the speed now.
        }


    }
    public void NextLine()
    {
        // Debug.Log("im here");
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            //tankControls.dialogueOpen = false;
            StopAllCoroutines();
            index = 0;
            textComponent.text = lines[index];
            //if (CameraActivate != null) CameraActivate.Priority = 0;
            ResumeGame();
            isOpen = false;
            textComponent.text = string.Empty;
            ResumeGame();
        }

    }

    void PauseGame()
    {
       // print("Pause");
        Time.timeScale = 0;
        //if (CameraActivate != null)
        //{
        //    CameraActivate.Priority = 11;
        //    SetLayerMask?.Invoke(MaskCharacters);
        //}
    }
    void ResumeGame()
    {
        //print("resume");
        Time.timeScale = 1;
       //if (CameraActivate!=null) CameraActivate.Priority = 0;
        SetLayerMask?.Invoke(ResetMask);
    }

    // This handles the text speed. While holding down E, the currentTextSpeed variable changes to the skipSpeed variable, and if released, it will switch back to the textSpeed variable.
    private void HandleTextSpeed()
    {
        if (Input.GetKey(KeyCode.E))
        {
            holdE = true;
            currentTextSpeed = skipSpeed;
        }
        else
        {
            holdE = false;
            currentTextSpeed = textSpeed;
        }
    }
}
