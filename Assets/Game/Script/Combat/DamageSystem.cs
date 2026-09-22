using UnityEngine;
public enum DamageType
{
    Normal,
    Burn,
    Poison
}

public static class DamageSystem 
{
    private static GameObject popupPrefab;
    public static void ApplyDamage(GameObject target, float baseDamage, DamageType type = DamageType.Normal)
    {
        float finalDamage = baseDamage;
        DamageType finalType = type;
        BurnEffect burn = target.GetComponent<BurnEffect>();
        if (burn != null && finalType == DamageType.Normal)
        {
            finalDamage *= burn.GetMultiplier(); 
            finalType = DamageType.Burn;
        }

        IDamageable damageable = target.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(finalDamage);
            ShowPopup(target.transform.position, finalDamage, finalType);
        }
    }

    private static void ShowPopup(Vector3 position, float damageAmount, DamageType type)
    {
        if (popupPrefab == null)
        {
            popupPrefab = Resources.Load<GameObject>("DamagePopup");
        }

        if (popupPrefab != null)
        {
            Vector3 spawnPosition = position + new Vector3(0, 0, 0);
            GameObject popup = GameObject.Instantiate(popupPrefab, spawnPosition, Quaternion.identity);
            
            FloatingText popupScript = popup.GetComponent<FloatingText>();
            if (popupScript != null)
            {
               
                Color chosenColor = Color.white; 

                switch (type)
                {
                    case DamageType.Normal:
                        chosenColor = Color.black;
                        break;
                    case DamageType.Burn:
                        chosenColor = new Color(1f, 0.5f, 0f); 
                        break;
                    case DamageType.Poison:
                        chosenColor = Color.green;
                        break;
                }
                popupScript.Setup(damageAmount.ToString("F2"), chosenColor);
            }
        }
    }
}