using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour {

    /*
    private int currentSelection;

    
    private GameObject notification;

    void Awake()
    {
        currentSelection = 0;

        if (dialoguePanel.activeInHierarchy)
            dialoguePanel.SetActive(false);

        notification = transform.GetChild(0).gameObject;
        notification.SetActive(false);
    }

    void OnTriggerStay2D(Collider2D col)
    {
        notification.SetActive(true);
        if (col.CompareTag("Player") && Input.GetButton("A_1"))
        {
            ShowPanel();
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        notification.SetActive(false);
    }

    private void ShowPanel()
    {
        if (dialoguePanel.activeInHierarchy)
            dialoguePanel.SetActive(true);
        else
            dialoguePanel.SetActive(false);
    }

    public void NextSection()
    {
        if (currentSelection > dialogue.Length)
            currentSelection = 0;

        dialoguePanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = dialogue[currentSelection].npcText;
        dialoguePanel.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = dialogue[currentSelection].respondText;
        currentSelection += 1;
    }*/
}


