using UnityEngine;
using UnityEngine.UI;

public class SliderDots : MonoBehaviour
{
    public Slider slider;
    public GameObject dotPrefab;
    public Transform dotsContainer;

    private int totalDots = 10; // Total number of dots to create

    private void Start()
    {
        //CreateDots();
    }

    public void OnSliderValueChanged()
    {
        UpdateDots();
    }

    //private void CreateDots()
    //{
    //    for (int i = 0; i < totalDots; i++)
    //    {
    //        GameObject dot = Instantiate(dotPrefab, dotsContainer);
    //        dot.SetActive(false); // Initially, all dots are disabled
    //    }
    //}

    private void UpdateDots()
    {
        float sliderValue = slider.value;
        int activeDots = Mathf.RoundToInt(sliderValue); // Calculate the number of active dots based on the slider value

        // Enable or disable dots based on the number of active dots
        for (int i = 0; i < totalDots; i++)
        {
            dotsContainer.GetChild(i).gameObject.SetActive(i < activeDots);
        }
    }
}
