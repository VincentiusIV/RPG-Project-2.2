using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSONFactory;
using System;

public class PanelManager : MonoBehaviour, IManager
{
    public ManagerState currentState { get; private set; }

    private PanelConfig rightPanel;
    private PanelConfig leftPanel;

    private NarrativeEvent currentEvent;

    private bool leftCharacterActive = true;
    private int stepIndex = 0;

    public void BootSequence()
    {
        Debug.Log(string.Format("{0} is booting up", GetType().Name));

        rightPanel = GameObject.Find("RightCharacterPanel").GetComponent<PanelConfig>();
        leftPanel = GameObject.Find("LeftCharacterPanel").GetComponent<PanelConfig>();
        currentEvent = JSONAssembly.RunJSONFactoryForScene(1);
        InitializePanels();
        Debug.Log(string.Format("{0} status = {1}", GetType().Name, currentState));
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            UpdatePanelState();
        }
    }

    private void InitializePanels()
    {
        leftPanel.characterIsTalking = true;
        rightPanel.characterIsTalking = false;
        leftCharacterActive = !leftCharacterActive;

        leftPanel.Configure(currentEvent.dialogues[stepIndex]);
        rightPanel.Configure(currentEvent.dialogues[stepIndex + 1]);

        StartCoroutine(MasterManager.animationManager.IntroAnimation());

        stepIndex++;
    }

    private void ConfigurePanels()
    {
        if (leftCharacterActive)
        {
            leftPanel.characterIsTalking = true;
            rightPanel.characterIsTalking = false;

            leftPanel.Configure(currentEvent.dialogues[stepIndex]);
            rightPanel.ToggleCharacterMask();
        }
        else
        {
            leftPanel.characterIsTalking = false;
            rightPanel.characterIsTalking = true;

            leftPanel.ToggleCharacterMask();
            rightPanel.Configure(currentEvent.dialogues[stepIndex]);
        }
    }

    void UpdatePanelState()
    {
        if(stepIndex < currentEvent.dialogues.Count)
        {
            ConfigurePanels();

            leftCharacterActive = !leftCharacterActive;
            stepIndex++;
        }
        else
        {
            StartCoroutine(MasterManager.animationManager.ExitAnimation());
        }
    }
}
