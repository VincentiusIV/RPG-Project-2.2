using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// Author: Vincent Versnel
// Plays through a cutscene
// in the inspector you can set the amount of cutscene frames, the view time and audio
public class Cutscene : MonoBehaviour {

    public Image img;
    public CutSceneData[] cutscenes;
    private ButtonFunctionality ui;
    private AudioSource audio;

    void Start()
    {
        ui = GameObject.FindWithTag("UI").GetComponent<ButtonFunctionality>();
        audio = GetComponent<AudioSource>();
    }

    public IEnumerator ShowCutScene()
    {
        // fade to black
        yield return StartCoroutine(ui.FadeIn());
        // turn off menu
        GameObject.Find("ButtonPanel").SetActive(false);
        img.sprite = cutscenes[0].scene;
        yield return StartCoroutine(ui.FadeOut());
        audio.clip = cutscenes[0].audio;
        audio.Play();
        yield return new WaitForSeconds(cutscenes[0].viewTime);

        for (int i = 1; i < cutscenes.Length; i++)
        {
            yield return StartCoroutine(ui.FadeIn());
            img.sprite = cutscenes[i].scene;
            yield return StartCoroutine(ui.FadeOut());
            audio.clip = cutscenes[i].audio;
            audio.Play();
            yield return new WaitForSeconds(cutscenes[i].viewTime);
        }

        yield return StartCoroutine(ui.FadeIn());
        ui.LoadScene("Game");
    }
}
[System.Serializable]
public struct CutSceneData
{
    public Sprite scene;
    public AudioClip audio;
    public string[] dialogue;
    public float viewTime;
}
