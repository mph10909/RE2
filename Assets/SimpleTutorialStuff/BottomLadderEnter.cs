//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class BottomLadderEnter : MonoBehaviour
//{
//    PlayerAction playerActions;
//    PlayerController playerController;
//    [SerializeField] bool isTopOfLadder;
//    [SerializeField] GameObject _pressToClimb, _pressToClimbOff;
//    [SerializeField] bool inCollider;
//    [SerializeField] Transform climbOnPos, climbOffPos;

//    void Start()
//    {
//        playerActions = new PlayerAction();
//    }

//    void OnTriggerEnter(Collider collider)
//    {
//        if(collider.tag == "Player")
//        {
//            print("Enter");
//            playerController = collider.GetComponent<PlayerController>();
//            inCollider = true;
//            if (playerController.Climbing) return;
//            _pressToClimb.SetActive(true);
//        }
//    }

//    void OnTriggerExit(Collider collider)
//    {
//        inCollider = false;
//        _pressToClimb.SetActive(false);
//        playerController = null;
//    }

//    void Update()
//    {
//        if (playerController == null) return;
//        if (playerController.PlayerActions.Player.Fire.ReadValue<float>() > 0 && !playerController.Climbing)
//        {
//            playerController.ClimbOnLadder(climbOnPos);
//            print("Climb");
//            return;
//        }

//    }

//}
