using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorController : MonoBehaviour
{
    public ButtonMover player;
    void Start()
    {
        player =transform.parent. GetComponent<ButtonMover>();


    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)|| Input.GetKeyDown(KeyCode.W))
            player.moveUp();
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            player.moveDown();

    }
}
