//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace ResidentEvilClone
//{
//    public class InteractableDetection : MonoBehaviour
//    {
//        private void OnTriggerEnter(Collider other)
//        {
//                ClickableObject clickableObject = GetComponent<ClickableObject>();
//                if (clickableObject != null)
//                {
//                print("Enter");
//                clickableObject.CloseEnoughToClick = true;
//                }

//        }

//        private void OnTriggerExit(Collider other)
//        {
//                ClickableObject clickableObject = GetComponent<ClickableObject>();
//                if (clickableObject != null)
//                {
//                print("Exit");
//                    clickableObject.CloseEnoughToClick = false;
//                }
//        }
//    }
//}
