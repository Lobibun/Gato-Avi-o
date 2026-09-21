using UnityEngine;

public class DrillWeapon : WeaponController
{
    [Header("Configuração Visual")]
    public GameObject drillPrefab;
    
  
    public Vector3 offset = Vector3.zero; 
    public float rotationAdjustment = -90f; 

    private GameObject currentDrill;
    private DrillBit drillScript;

    protected override void Start()
    {
        base.Start();
        SpawnDrill();
    }

    protected override void Attack()
    {
        base.Attack(); 
        UpdateDrillStats();
    }

    void SpawnDrill()
    {
        if (currentDrill != null) return;

        currentDrill = Instantiate(drillPrefab, transform.position, Quaternion.identity, transform);
        
        currentDrill.transform.localPosition = offset;
        
        currentDrill.transform.localRotation = Quaternion.Euler(0, 0, rotationAdjustment);

        drillScript = currentDrill.GetComponent<DrillBit>();
        
        if(drillScript != null)
        {
            drillScript.SetInitialPosition(offset);
        }
        
        UpdateDrillStats();
    }

    void UpdateDrillStats()
    {
        if (drillScript == null) return;

        float damageMult = 1f;
        float areaMult = 1f;
        float speedMult = 1f; 

        if (StatusManager.instance != null)
        {
            damageMult += StatusManager.instance.GetDamageBonus();
            areaMult += StatusManager.instance.GetAreaBonus();
            speedMult += StatusManager.instance.GetProjectileSpeedBonus();
        }

        drillScript.Setup(
            currentDamage * damageMult,
            currentSpeed * speedMult, 
            currentArea * areaMult
        );
    }
}