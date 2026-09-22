using UnityEngine;
using System.Collections.Generic;

public class DrillBit : MonoBehaviour
{
    private float baseDamage;

    [Header("Configuração")]
    private float maxDamageMultiplier = 3.0f; 
    private float timeToMaxDamage = 2.0f;     
    
    private float damageTimer;
    private float tickRate = 0.1f; 

    private Dictionary<IDamageable, float> contactTimes = new Dictionary<IDamageable, float>();

    // Variáveis para o efeito visual
    private Vector3 initialLocalPos; 
    private float vibrationAmount = 0.05f; 

    public void Setup(float dmg, float speed, float area)
    {
        this.baseDamage = dmg;
        transform.localScale = Vector3.one * area; 
        this.damageTimer = 0;
    }

    // Chamado pelo DrillWeapon para definir o centro correto
    public void SetInitialPosition(Vector3 pos)
    {
        this.initialLocalPos = pos;
    }

    void Update()
    {
        // --- EFEITO DE TREMOR ---
        // Faz a broca vibrar ao redor da posição definida no DrillWeapon
        float xShake = Random.Range(-vibrationAmount, vibrationAmount);
        float yShake = Random.Range(-vibrationAmount, vibrationAmount);
        
        transform.localPosition = initialLocalPos + new Vector3(xShake, yShake, 0);

        // --- LÓGICA DE DANO ---
        damageTimer -= Time.deltaTime;
        if (damageTimer <= 0)
        {
            ApplyDrillDamage();
            damageTimer = tickRate;
        }

        // Atualiza contadores de tempo
        List<IDamageable> currentEnemies = new List<IDamageable>(contactTimes.Keys);
        foreach (var enemy in currentEnemies)
        {
            if (enemy == null || !((MonoBehaviour)enemy).gameObject.activeSelf)
            {
                contactTimes.Remove(enemy);
                continue;
            }
            contactTimes[enemy] += Time.deltaTime;
        }
    }

    void ApplyDrillDamage()
    {
        List<IDamageable> targets = new List<IDamageable>(contactTimes.Keys);

        foreach (var enemy in targets)
        {
            if (enemy != null && contactTimes.ContainsKey(enemy))
            {
                float timeInContact = contactTimes[enemy];
                float progress = Mathf.Clamp01(timeInContact / timeToMaxDamage);
                float currentMult = Mathf.Lerp(1f, maxDamageMultiplier, progress);
                float totalDamage = baseDamage * currentMult;
                
                GameObject enemyObj = ((MonoBehaviour)enemy).gameObject;
                DamageSystem.ApplyDamage(enemyObj, totalDamage);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            IDamageable dmg = other.GetComponent<IDamageable>();
            if (dmg != null && !contactTimes.ContainsKey(dmg))
            {
                contactTimes.Add(dmg, 0f);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            IDamageable dmg = other.GetComponent<IDamageable>();
            if (dmg != null && contactTimes.ContainsKey(dmg))
            {
                contactTimes.Remove(dmg);
            }
        }
    }
}