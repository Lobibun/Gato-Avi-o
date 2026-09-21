using UnityEngine;

public class LightningWeapon :  WeaponController
{
     public GameObject lightning;
     public Vector2 detectionArea;
     public LayerMask enemyLayer;
     float damageMult = 1f;

      protected override void Attack()
    {
        // 1. CHAME O PAI
        base.Attack(); 
        Vector2 cameraCenter = Camera.main.transform.position;
        // 2. DETECTAR
        Collider2D[] targets = Physics2D.OverlapBoxAll(cameraCenter, detectionArea, 0f, enemyLayer);
        // 3. VERIFICAR
        if (targets.Length == 0) return;


          if (StatusManager.instance != null)
        {
            damageMult += StatusManager.instance.GetDamageBonus(); 
        }

        for (int i=0; i<currentCount; i++)
        {
            int randomIndex = Random.Range(0, targets.Length);
            Transform targetPos = targets[randomIndex].transform;
            // 1. Cria o objeto e guarda na variável 'raioObj'
            GameObject raioObj = Instantiate(lightning, targetPos.position, Quaternion.identity);
           // 2. Pega o script que está dentro desse objeto
            Lightning lightningScript = raioObj.GetComponent<Lightning>();
           // 3. PASSA O DANO
           lightningScript.damage = currentDamage;

           if (lightningScript != null)
            {
                lightningScript.damage = currentDamage * damageMult;
            }
        }

        
    }
    void OnDrawGizmosSelected()
    {
       if (Camera.main != null)
        {
             Gizmos.DrawWireCube(Camera.main.transform.position, detectionArea);
        }
        else
        {
             Gizmos.DrawWireCube(transform.position, detectionArea);
        }
    }
}
