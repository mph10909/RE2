using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberPad : MonoBehaviour
{
    int numberHighlighted;
    [SerializeField] GameObject safeDoor , safeItem;
    [SerializeField] Vector3 doorRotation;
    [SerializeField] SafeTrigger Safe;
    //[SerializeField] Text codeText;
    //[SerializeField] Text openCodeText;
    [SerializeField] string codeValue = "0420";
    string codeTextValue = "";
    public bool safeOpen;

    void Start()
    {
        SelectNumber(0);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) || DpadController.rightD) { ChangeNumber(+1); DpadController.rightD = false; }
        if (Input.GetKeyDown(KeyCode.A) || DpadController.leftD) { ChangeNumber(-1); DpadController.leftD = false; }
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Submit")) NumberInput();
        if (Input.GetButtonDown("Cancel")) Safe.PuzzleEnded();
    }

    private void SelectNumber(int index){
        for(int i = 0; i < transform.childCount; i++){
            transform.GetChild(i).gameObject.SetActive(i == index);
        }

    }

    public void ChangeNumber(int change){
        SoundManager.PlaySound(SoundManager.Sound.menuClick);
        numberHighlighted += change;
        if (numberHighlighted > transform.childCount-1) numberHighlighted = 0;
        if (numberHighlighted < 0) numberHighlighted = transform.childCount -1;
        SelectNumber(numberHighlighted);

    }

    private void NumberSelected(string digit){
        codeTextValue += digit;
    }

    private void NumberInput(){
        SoundManager.PlaySound(SoundManager.Sound.safeKnobTurn);
        NumberSelected(numberHighlighted.ToString());
        //codeText.text = codeTextValue;
        if (codeTextValue == codeValue)
        {
            safeOpen = true;
            SoundManager.PlaySound(SoundManager.Sound.safeOpen);
            safeItem.SetActive(true);
            //Tween.DoorOpen(safeDoor, doorRotation, 3, ()=> Safe.PuzzleEnded());
            return;    
            
            //openCodeText.text = "Safe_Open";
        }
        if (codeTextValue.Length >= codeValue.Length && !safeOpen)
        {
            codeTextValue = "";
            SoundManager.PlaySound(SoundManager.Sound.menuDelcine);
            //codeText.text = codeTextValue;
        }
    }

}
