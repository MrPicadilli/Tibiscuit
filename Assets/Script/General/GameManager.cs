using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject uiGameOver;
    public static GameManager Instance;
    public bool isGameOver = false;
    public CorridorAnimationManager corridorAnimationManager;
    public bool isInAnimation = false;
    private void Awake() {
        if(Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
    public void GameOver(){
        uiGameOver.SetActive(true);
        isGameOver = true;
    }
    public void RestartLevel(){
        uiGameOver.SetActive(false);
        LevelGen.Instance.RestartLevel();
    }

    public void StartNextLevelTransition()
    {
        if(isInAnimation){
            return;
        }
        isInAnimation = true;
        // stop control player
        TibiscuitController.Instance.StopControl();
        // start animation
        corridorAnimationManager.player = TibiscuitController.Instance.transform;
        corridorAnimationManager.StartAnimation();
        //TibiscuitController.Instance.StartControl();
    }

    public void TransitionAnimation(){
        
        // move the camera to the player
        // when both are at the same level make player run center corridor and move the camera with him
        // stop position of player and camera at the center but not the animation
        // make the environnement move like course with code
        // put the UI with tibiscuit an nana
        // at the end move the camera and the player to the start of the next level

    }
}
