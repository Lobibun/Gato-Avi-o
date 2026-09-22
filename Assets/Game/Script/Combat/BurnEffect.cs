using UnityEngine;
using System.Collections;

public class BurnEffect : MonoBehaviour
{
    private float duration;
    private float damageMultiplier; 
    private float timeRemaining;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    public void Activate(float time, float multiplier)
    {
        this.duration = time;
        this.timeRemaining = time;
        this.damageMultiplier = multiplier;

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            if (originalColor == default) originalColor = spriteRenderer.color;
            spriteRenderer.color = new Color(1f, 0.4f, 0f); 
        }

        StopAllCoroutines();
        StartCoroutine(BurnRoutine());
    }

    public void RefreshDuration(float newDuration, float newMultiplier)
    {
       
        timeRemaining = newDuration;

        if (newMultiplier > damageMultiplier)
        {
            damageMultiplier = newMultiplier;
        }
    }

    public float GetMultiplier()
    {
        return damageMultiplier;
    }

    IEnumerator BurnRoutine()
    {
        while (timeRemaining > 0)
        {
            yield return new WaitForSeconds(0.1f);
            timeRemaining -= 0.1f;
        }

        // Quando acaba o tempo: Restaura a cor e remove o script
        if (spriteRenderer != null) spriteRenderer.color = originalColor;
        Destroy(this);
    }
}
