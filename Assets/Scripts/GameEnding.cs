using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEnding : MonoBehaviour
{
    CanvasGroup canvasGroup;
    bool fadeIn = false;

    void Update() {
        if (fadeIn) {
            if (canvasGroup.alpha < 1) {
                canvasGroup.alpha += Time.deltaTime;
                if (canvasGroup.alpha >= 1) {
                    fadeIn = false;
                }
            }
        }
    }
    
    public void ShowUI(CanvasGroup gameEndingCanvas) {
        canvasGroup = gameEndingCanvas;
        fadeIn = true;
    }
}
