using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Doors : MonoBehaviour
{
    [SerializeField] private AudioSource Enter;
    [SerializeField] private AudioSource Exit;
    [SerializeField] private Image image;
    [SerializeField] private Color currentColor;
    [SerializeField] private Color targetColor;
    [SerializeField] private Color startColor;

    private IEnumerator FadeExit()
    {
        currentColor = image.color;
        for (float alpha = 1f; alpha >= 0; alpha -= 0.5f)
        {
            currentColor.a = alpha;
            image.color = currentColor;
            yield return new WaitForSeconds(0.2f);
        }
    }
    private IEnumerator FadeEnter()
    {
        currentColor = image.color;
        for (float alpha = 0f; alpha >= 0; alpha -= 0.5f)
        {
            currentColor.a = alpha;
            image.color = currentColor;
            yield return new WaitForSeconds(0.2f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Enter.Play();
        StartCoroutine(FadeEnter());
        Exit.Stop();
    }
    private void OnTriggerExit(Collider other)
    {
        Exit.Play();
        StartCoroutine(FadeExit());
        Enter.Stop();
    }
}
