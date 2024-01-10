//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;



//public static class Tween
//{
//    public static void Move(GameObject _targetObject, Vector3 _targetPosition, float _totalDuration, Action _onComplete = null)
//    {
//            var objectToMove = !_targetObject.GetComponent<TweenMove>() 
//                ? _targetObject.AddComponent<TweenMove>() 
//                : _targetObject.GetComponent<TweenMove>();

//            objectToMove.elapseDuration = 0;
//            objectToMove.targetPosition = _targetPosition;
//            objectToMove.totalDuration = _totalDuration;
//            objectToMove.startPosition = _targetObject.transform.position;
//            objectToMove.onComplete = _onComplete;
//            objectToMove.enabled = true;
//    }
//        public static void Slerping(GameObject _targetObject, GameObject _startPosition, GameObject _targetPosition, float _totalDuration, Action _onComplete = null)
//    {
//            var objectToMove = !_targetObject.GetComponent<TweenSlerp>() 
//                ? _targetObject.AddComponent<TweenSlerp>() 
//                : _targetObject.GetComponent<TweenSlerp>();

//            objectToMove.elapseDuration = 0;
//            objectToMove.targetPosition = _targetPosition.transform;
//            objectToMove.totalDuration = _totalDuration;
//            objectToMove.startPosition = _startPosition.transform;
//            objectToMove.onComplete = _onComplete;
//            objectToMove.enabled = true;
//    }

//    public static void Fade(Image _targetImage, float _targetAlpha, float _totalDuration, Action _onComplete = null)
//    {
//            var objectToFade = !_targetImage.gameObject.GetComponent<TweenFade>() 
//                ? _targetImage.gameObject.AddComponent<TweenFade>() 
//                : _targetImage.gameObject.GetComponent<TweenFade>();

//            objectToFade.elapseDuration = 0;
//            objectToFade.targetImage = _targetImage;
//            objectToFade.startAlpha = _targetImage.color.a;
//            objectToFade.targetAlpha = _targetAlpha;
//            objectToFade.totalDuration = _totalDuration;
//            objectToFade.onComplete = _onComplete;
//            objectToFade.enabled = true;
//    }

//        public static void TextFade(Text _targetImage, float _targetAlpha, float _totalDuration, Action _onComplete = null)
//    {
//            var objectToFade = !_targetImage.gameObject.GetComponent<TweenTextFade>() 
//                ? _targetImage.gameObject.AddComponent<TweenTextFade>() 
//                : _targetImage.gameObject.GetComponent<TweenTextFade>();

//            objectToFade.elapseDuration = 0;
//            objectToFade.targetImage = _targetImage;
//            objectToFade.startAlpha = _targetImage.color.a;
//            objectToFade.targetAlpha = _targetAlpha;
//            objectToFade.totalDuration = _totalDuration;
//            objectToFade.onComplete = _onComplete;
//            objectToFade.enabled = true;
//    }

//    public static void Rotation(GameObject _startRotation, GameObject _targetRotation, float _totalDuration, Action _onComplete = null)
//    {
//        var objectToRotate = ! _startRotation.GetComponent<TweenRotate>() 
//                ?  _startRotation.AddComponent<TweenRotate>() 
//                :  _startRotation.GetComponent<TweenRotate>();
            
//            objectToRotate.elapseDuration = 0;
//            objectToRotate.targetRot = _targetRotation.transform;           
//            objectToRotate.totalDuration = _totalDuration;
//            objectToRotate.startRot = _startRotation.transform;
//            objectToRotate.onComplete = _onComplete;
//            objectToRotate.enabled = true;
//    }

//        public static void PickedUpItem(GameObject _StartObject , Quaternion _startRotation, Quaternion _targetRotation, float _totalDuration, Action _onComplete = null)
//    {
//        var objectToRotate = ! _StartObject.GetComponent<itemPickedUp>() 
//                ?  _StartObject.AddComponent<itemPickedUp>() 
//                :  _StartObject .GetComponent<itemPickedUp>();

//            objectToRotate.elapseDuration = 0;
//            objectToRotate.targetRot = _targetRotation;           
//            objectToRotate.totalDuration = _totalDuration;
//            objectToRotate.startRot = _startRotation;
//            objectToRotate.onComplete = _onComplete;
//            objectToRotate.enabled = true;
//    }
//        public static void DoorOpen(GameObject _StartObject , Vector3 _targetRotation, float _totalDuration, Action _onComplete = null)
//    {
//        var objectToRotate = ! _StartObject.GetComponent<DoorRotate>() 
//                ?  _StartObject.AddComponent<DoorRotate>() 
//                :  _StartObject.GetComponent<DoorRotate>();
            
//            objectToRotate.elapseDuration = 0;
//            objectToRotate.targetRot = _targetRotation;           
//            objectToRotate.totalDuration = _totalDuration;
//            objectToRotate.startRot = _StartObject.transform.localEulerAngles;
//            objectToRotate.onComplete = _onComplete;
//            objectToRotate.enabled = true;
//    }
//            public static void DoorClose(GameObject _StartObject , Vector3 _targetRotation, float _totalDuration, Action _onComplete = null)
//    {
//        var objectToRotate = ! _StartObject.GetComponent<DoorRotate>() 
//                ?  _StartObject.AddComponent<DoorRotate>() 
//                :  _StartObject.GetComponent<DoorRotate>();
            
//            objectToRotate.elapseDuration = 0;
//            objectToRotate.targetRot = _StartObject.transform.localEulerAngles;           
//            objectToRotate.totalDuration = _totalDuration;
//            objectToRotate.startRot = _targetRotation;
//            objectToRotate.onComplete = _onComplete;
//            objectToRotate.enabled = true;
//    }
//                public static void Scale(GameObject _StartObject ,float _targetSize, float _totalDuration, Action _onComplete = null)
//    {
//        var objectToScale = ! _StartObject.GetComponent<TweenScale>() 
//                ?  _StartObject.AddComponent<TweenScale>() 
//                :  _StartObject.GetComponent<TweenScale>();
            
//            objectToScale.elapseDuration = 0;
//            objectToScale.startScale = _StartObject.GetComponent<RectTransform>().localScale;           
//            objectToScale.totalDuration = _totalDuration;
//            objectToScale.howMuchScale = _targetSize;
//            objectToScale.onComplete = _onComplete;
//            objectToScale.enabled = true;
//    }
//}
