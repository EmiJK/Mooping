using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScaleEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Vector3 normalScale = Vector3.one; // Normalaus dydžio skalė
    public Vector3 hoverScale = new Vector3(1.2f, 1.2f, 1.2f); // Padidintas dydis
    public float scaleSpeed = 0.2f; // Keitimosi greitis

    private Vector3 targetScale; // Dabartinė tikslo skalė

    void Start()
    {
        // Pradinė būklė – normalaus dydžio
        targetScale = normalScale;
        transform.localScale = normalScale;
    }

    void Update()
    {
        // Sklandus dydžio keitimas
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime / scaleSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Kai pelė užvedama – nustatome didesnę skalę
        targetScale = hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Kai pelė nutolsta – grįžtame į normalią skalę
        targetScale = normalScale;
    }
}