using UnityEngine;

public class TurbineWeapon : WeaponController
{
    [Header("Configuração Visual")]
    public GameObject firePrefab;
    
    // Posição: Negativo no X para ficar atrás. 
    // Ex: Vector3(-1.5f, 0, 0)
    public Vector3 offset = new Vector3(-1.5f, 0, 0); 

    [Header("Atributos da Queimadura")]
    public float baseBurnDuration = 4f;
    public float baseVulnerability = 1.3f; // +30% de dano base

    private GameObject currentFire;
    private TurbineFire fireScript;

    protected override void Start()
    {
        base.Start();
        SpawnFire();
    }

    protected override void Attack()
    {
        base.Attack();
        UpdateFireStats();
    }

    void SpawnFire()
    {
        if (currentFire != null) return;

        currentFire = Instantiate(firePrefab, transform.position, Quaternion.identity, transform);
        currentFire.transform.localPosition = offset;
        
        currentFire.transform.localRotation = Quaternion.Euler(0, 0, 90);

        fireScript = currentFire.GetComponent<TurbineFire>();
        UpdateFireStats();
    }

    void UpdateFireStats()
    {
        if (fireScript == null) return;

        float damageMult = 1f;
        float durationMult = 1f;

        if (StatusManager.instance != null)
        {
            damageMult += StatusManager.instance.GetDamageBonus();
            durationMult += StatusManager.instance.GetDurationBonus();
        }

        // A vulnerabilidade pode ser fixa ou aumentar um pouco com o dano
        // Aqui estou fazendo ela escalar levemente com o danoMult
        float finalVulnerability = baseVulnerability + (damageMult - 1f) * 0.5f;

        fireScript.Setup(
            currentDamage * damageMult,
            baseBurnDuration * durationMult,
            finalVulnerability
        );
    }
}
