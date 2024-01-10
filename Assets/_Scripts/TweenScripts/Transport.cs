using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Transport
{
    public static void Move(GameObject _PlayerObject, GameObject _TargetLocation, Action _onComplete = null)
    {
            var playerToMove = !_PlayerObject.GetComponent<TransportMove>() 
                ? _PlayerObject.AddComponent<TransportMove>() 
                : _PlayerObject.GetComponent<TransportMove>();
            
            playerToMove.startPosition = _PlayerObject;
            playerToMove.targetObject = _TargetLocation;  
            playerToMove.onComplete = _onComplete;
            playerToMove.enabled = true;
    }

    public static void SceneTrasition(GameObject NewScene, GameObject _SceneTrasition, Action _onComplete = null)
    {

        var  transitionScene = !NewScene.GetComponent<SceneTrasition>() 
            ? NewScene.AddComponent<SceneTrasition>() 
            : NewScene.GetComponent<SceneTrasition>();

            transitionScene.sceneTrasition = _SceneTrasition;
            transitionScene.onComplete = _onComplete;
            transitionScene.enabled = true;



    }

}