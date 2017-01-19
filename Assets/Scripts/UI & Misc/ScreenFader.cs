using UnityEngine;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    private Animator ani;

    void Start()
    {
        ani = GetComponent<Animator>();
    }

    public IEnumerator FadeIn()
    {
        Debug.Log("Fading in");
        ani.SetBool("Fade", true);
        yield return new WaitForSeconds(1f);
    }
    
    public IEnumerator FadeOut()
    {
        Debug.Log("Fading out");
        ani.SetBool("Fade", false);
        yield return new WaitForSeconds(1f);
    }         	
}
