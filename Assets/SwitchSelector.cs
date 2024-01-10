using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwitchSelector : MonoBehaviour
{
    public Selectable[] switches;
    [SerializeField] int currentSwitchIndex = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            currentSwitchIndex++;
            if (currentSwitchIndex >= switches.Length)
            {
                currentSwitchIndex = 0;
            }
            EventSystem.current.SetSelectedGameObject(switches[currentSwitchIndex].gameObject);
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
        {
            currentSwitchIndex--;
            if (currentSwitchIndex < 0)
            {
                currentSwitchIndex = switches.Length - 1;
            }
            EventSystem.current.SetSelectedGameObject(switches[currentSwitchIndex].gameObject);
        }
    }
}
