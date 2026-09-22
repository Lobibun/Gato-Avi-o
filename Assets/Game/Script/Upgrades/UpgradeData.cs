using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "Scriptable Objects/UpgradeData")]
public class UpgradeData : ScriptableObject
{
    [Header("Visual Information")]
    public string upgradeName;
    public Sprite Icon;
    [TextArea] public string description;

    [Header("Upgrade Effects")]
    public UpgradeType type;

    [Tooltip("Arraste o WeaponData aqui APENAS se o tipo acima for 'Weapon'")]
    public WeaponData weaponData;

    [Header("Passive Settings")]
    public int maxLevel = 5;
}

public enum UpgradeType
{
    Weapon,
    Armor,
    ProjectileSpeed,
    ProjectileQuantity,
    CoodownReduction,
    ProjectileSize,
    MovingSpeed,
    HealthIncrease,
    DamageIncrease,
    Duration,
    Magnetism,
    Growth,
    Greed,
    Luck,
    Reroll,
    Recovery
}
