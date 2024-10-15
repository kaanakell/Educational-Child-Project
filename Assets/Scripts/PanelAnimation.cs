using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelAnimation : MonoBehaviour
{

    public Transform panel;

    private void OnEnable() 
    {
        //panel.localPosition = new Vector2(0, -Screen.height);
        panel.LeanMoveLocalY(0, 0.5f).setEaseOutExpo().delay = 0.1f;
    } 

    public void ClosePanel()
    {
        panel.LeanMoveLocalY(-Screen.height, 0.5f).setEaseInExpo();
    }
}
