using UnityEngine;

public class UIFloat : MonoBehaviour
{
    public float amplitude = 5f;    // distância que vai subir/baixar (em pixels)
    public float frequency = 2f;    // velocidade da oscilação

    private RectTransform rectTransform;
    private Vector3 startPos;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;
        rectTransform.anchoredPosition = new Vector2(startPos.x, newY);
    }
}

