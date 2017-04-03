using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AtlasManager : MonoBehaviour, IManager
{
    public static Sprite[] sprites;

    public ManagerState currentState { get; private set; }

    public void BootSequence()
    {
        Debug.Log(string.Format("{0} is booting up", GetType().Name));

        sprites = Resources.LoadAll<Sprite>("EventAtlas");
        currentState = ManagerState.Completed;

        Debug.Log(string.Format("{0} status = {1}", GetType().Name, currentState));
    }

    public Sprite loadSprite(string spriteName)
    {
        foreach (Sprite s in sprites)
        {
            if (s.name == spriteName)
                return s;
        }
        return null;
    }

}
