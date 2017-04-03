using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelConfig : MonoBehaviour {

    public bool characterIsTalking;

    public Image avatarImage;
    public Image textBG;
    public Text characterName;
    public Text dialogue;

    private Color maskActiveColor = new Color(103.0f / 255.0f, 101.0f / 255.0f, 101.0f / 255.0f);

    public void ToggleCharacterMask()
    {
        if (characterIsTalking)
        {
            avatarImage.color = Color.white;
            textBG.color = Color.white;
        }else
        {
            avatarImage.color = maskActiveColor;
            textBG.color = maskActiveColor;
        }
    }

    public void Configure(Dialogue currentDialogue)
    {
        ToggleCharacterMask();

        avatarImage.sprite = MasterManager.atlasManager.loadSprite(currentDialogue.atlasImageName);
        characterName.text = currentDialogue.name;

        if(characterIsTalking)
        {
            StartCoroutine(AnimateText(currentDialogue.dialogueText));
        }
        else
        {
            dialogue.text = "";
        }
    }

    IEnumerator AnimateText(string dialogueText)
    {
        dialogue.text = "";

        foreach (char letter in dialogueText)
        {
            dialogue.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
