using UnityEngine;

public class PulsingAura : AreaEffect
{
    private float damage;
    private float maxScale;
    private float growthSpeed;
    private float holdDuration;

    private bool isGrowing = true;
    private float currentHoldTime = 0f;

    public void Setup(float dmg, float targetScale, float speed, float duration)
    {
        damage = dmg;
        maxScale = targetScale;
        growthSpeed = speed;
        holdDuration = duration;

        transform.localScale = Vector3.zero;
    }

    protected override void Update()
    {
        if (isGrowing)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one * maxScale, growthSpeed * Time.deltaTime);

            if (transform.localScale.x >= maxScale)
            {
                transform.localScale = Vector3.one * maxScale;
                isGrowing = false;
            }
        }
        else
        {
            currentHoldTime += Time.deltaTime;
            if (currentHoldTime >= holdDuration)
            {
                Destroy(gameObject);
            }
        }

        base.Update();
    }

    protected override void ApplyAreaDamage()
    {
        for (int i = enemiesInside.Count - 1; i >= 0; i--)
        {
            IDamageable enemy = enemiesInside[i];

            if (enemy != null && ((MonoBehaviour)enemy).gameObject.activeSelf)
            {
                GameObject enemyObj = ((MonoBehaviour)enemy).gameObject;
                DamageSystem.ApplyDamage(enemyObj, damage);
            }
            else
            {
                enemiesInside.RemoveAt(i);
            }
        }
    }
}