using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollImage: MonoBehaviour
{

    [SerializeField] Image m_Image;
    [SerializeField] float scrollSpeed;

    void Start()
    {
        m_Image = this.GetComponent<Image>();
    }

    void Update()
    {
        m_Image.material.mainTextureOffset = m_Image.material.mainTextureOffset + new Vector2(Time.unscaledDeltaTime * (-scrollSpeed / 10), 0f);
    }

}
