using System.Collections;
using TMPro;
using UnityEngine;

public class TypeWriterText : MonoBehaviour
{
    public float typeSpeed = 0.05f;

    public IEnumerator PlayTypingEffect(TextMeshProUGUI textComponent, string fullText)
    {
        textComponent.text = "";
        foreach (char c in fullText)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
    }
}
