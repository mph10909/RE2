using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class PageFlip : MonoBehaviour
    {
        [SerializeField]GameObject leftPage, rightPage;

        public void OnClick_LeftPage()
        {
            leftPage.SetActive(true);
        }

        public void OnClick_RightPage()
        {
            rightPage.SetActive(true);
        }
    }
}
