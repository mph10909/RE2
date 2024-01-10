using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;

public class TypewriterEffect : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string fullText;
    public float typingSpeed = 0.05f;
    public float scaleDownDuration = 0.2f;
    public Vector3 initialScale = new Vector3(1.2f, 1.2f, 1.2f);

    private void Start()
    {
        StartCoroutine(TypeSentence());
    }

    IEnumerator TypeSentence()
    {
        textComponent.text = "";
        foreach (char letter in fullText.ToCharArray())
        {
            textComponent.text += letter;
            textComponent.transform.localScale = initialScale;
            textComponent.transform.DOScale(Vector3.one, scaleDownDuration);
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
