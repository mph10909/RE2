//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Ladder : MonoBehaviour
//{
//    PlayerAction playerActions;

//    PlayerController playerController;

//    [Header("Ladder Settings")]
//    [Tooltip("Whether this is the top of the ladder.")]
//    [SerializeField] bool isTopOfLadder;

//    [Tooltip("The position the player will be when climbing on the ladder.")]
//    [SerializeField] Transform climbOnPos;

//    [Tooltip("The position the player will be when climbing off the ladder.")]
//    [SerializeField] Transform climbOffPos;

//    [Header("UI")]
//    [Tooltip("Reference to the PressToClimb GameObject.")]
//    [SerializeField] bool _displayPressToClimb;
//    [SerializeField] GameObject _pressToClimb;


//    void Start()
//    {
//        playerActions = new PlayerAction();
//    }

//    void OnTriggerEnter(Collider collider)
//    {
//        if (collider.tag == "Player")
//        {
//            print("Enter");
//            playerController = collider.GetComponent<PlayerController>();
//            if (playerController.Climbing && isTopOfLadder) { playerController.ClimbOffLadder(climbOffPos); return; }
//        }
//    }

//    void OnTriggerStay(Collider collider)
//    {
//        if (playerController.Climbing || !playerController.IsGrounded) return;
//        if(_displayPressToClimb ) _pressToClimb.SetActive(true);
//    }

//    void OnTriggerExit(Collider collider)
//    {
//        if (_displayPressToClimb) _pressToClimb.SetActive(false);
//        playerController = null;
//    }

//    void Update()
//    {
//        if (playerController == null) return;
//        if (playerController.PlayerActions.Player.Fire.ReadValue<float>() > 0 && !playerController.Climbing)
//        {
//            playerController.ClimbOnLadder(climbOnPos);
//            _pressToClimb.SetActive(false);
//            print("Climb");
//            return;
//        }

//    }

//}
