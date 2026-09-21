using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    public static FloatingText popupPrefab;

    [Header("Configurações")]
    public float lifetime = 1f; 

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            renderer.sortingOrder = 999; 
        }
    }

    public void Setup(string text, Color popupColor, float sizeMultiplier = 1f)
    {
        textMesh.SetText(text);
        textMesh.color = popupColor;
        textColor = popupColor;
        disappearTimer = lifetime;

        transform.localScale = Vector3.zero;
        transform.localScale = Vector3.one * sizeMultiplier;

        transform.position += new Vector3(
            Random.Range(-0.3f, 0.3f),
            Random.Range(-0.3f, 0.3f),
            0f);
    }

    void Update()
    {
        if (transform.localScale.x < 1f)
        {
            transform.localScale += Vector3.one * Time.deltaTime * 8f; 
        }

        transform.position += new Vector3(0, 1f, 0) * Time.deltaTime;
        disappearTimer -= Time.deltaTime;

        if (disappearTimer < lifetime * 0.5f)
        {
            textColor.a -= 3f * Time.deltaTime;
            textMesh.color = textColor;
        }

        if (disappearTimer <= 0f)
        {
            Destroy(gameObject);
        }
    }

    public static void Create(string text, Vector3 position, Color color)
    {
        if (popupPrefab == null) return;

        FloatingText popup = Instantiate(popupPrefab, position, Quaternion.identity);
        popup.Setup(text, color);
    }
}