using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("Info Visual")]
    public string weaponName;
    public Sprite icon;

    [Header("Configuração Geral")]
    public GameObject weaponPrefab;
    public Vector2 spawnPosition;

    [Header("Evolução (Níveis)")]
    public List<WeaponStats> levels; 
}

[System.Serializable]
public struct WeaponStats
{
    [TextArea] public string description; // Ex: "+1 Projétil"
    public float damage;
    public float cooldown;
    public float speed;
    public float duration;
    public int projectileCount;
    public float area; // Tamanho
}