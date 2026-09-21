using UnityEngine;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance;

    [Header("Armas Atuais")]
    public List<WeaponController> activeWeapons = new List<WeaponController>();

    [Header("Arma Inicial")]
    public WeaponData startingWeapon;

    void Awake()
    {
        instance = this;
    }
    
    void Start()
    {
        if (startingWeapon != null)
        {
            AddWeapon(startingWeapon);
        }
    }

    public void AddWeapon(WeaponData data)
    {
        WeaponController existingWeapon = activeWeapons.Find(w => w.weaponData == data);

        if (existingWeapon != null)
        {
            existingWeapon.LevelUp();
        }
        else
        {
            GameObject newWeaponObj = Instantiate(data.weaponPrefab, transform.position, Quaternion.identity);
            
            newWeaponObj.transform.SetParent(transform); 
            newWeaponObj.transform.localPosition = data.spawnPosition; 
            
            WeaponController newController = newWeaponObj.GetComponent<WeaponController>();
            
            if (newController != null)
            {
                newController.weaponData = data;
                newController.LevelUp(); 
                activeWeapons.Add(newController);
            }
        }
    }

    public bool HasWeapon(WeaponData data)
    {
        return activeWeapons.Exists(w => w.weaponData == data);
    }

    public bool IsWeaponMaxLevel(WeaponData data)
    {
        WeaponController weapon = activeWeapons.Find(w => w.weaponData == data);
        
        if (weapon == null) return false;
        return weapon.currentLevel >= (data.levels.Count - 1);
    }

    public int GetCurrentLevelIndex(WeaponData data)
{
    WeaponController existing = activeWeapons.Find(w => w.weaponData == data);
    
    if (existing != null)
    {
        return existing.currentLevel;
    }
    return -1; 
}
}