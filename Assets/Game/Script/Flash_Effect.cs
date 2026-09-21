using UnityEngine;
using System.Collections;


public class FlashEffect : MonoBehaviour
{
    [SerializeField] private Material flashMaterial;
    [SerializeField] private float duration = 0.2f;
    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;
    private Coroutine flashRoutine;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
    }

    public void Flash()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }
        flashRoutine = StartCoroutine(FlashRoutine());
    }


      private IEnumerator FlashRoutine()
    {
        // Troca para o material de flash (branco)
        spriteRenderer.material = flashMaterial;

        // Espera pela duração definida (ex: 0.2 segundos)
        yield return new WaitForSeconds(duration);

        // Retorna ao material original
        spriteRenderer.material = originalMaterial;

        // Finaliza a rotina
        flashRoutine = null;
    }
}
