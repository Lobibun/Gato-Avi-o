using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;
    
    [Header("UI References")]
    public GameObject upgradePanel;
    public UpgradeUiElement[] upgradeButtons;

    [Header("Banco de Dados")]
    public List<UpgradeData> allUpgrades;

    [Header("Limites")]
    public int maxWeaponSlots = 5;
    public int maxPassiveSlots = 5;

    private Dictionary<UpgradeType, int> acquiredPassives = new Dictionary<UpgradeType, int>();

    private void Awake()
    {
        instance = this;
        upgradePanel.SetActive(false);
    }

    public void TriggerLevelUp()
    {
        Time.timeScale = 0f;
        upgradePanel.SetActive(true);

        // 1. FILTRAR: Pega apenas o que é válido pegar agora
        List<UpgradeData> validCandidates = GetValidUpgrades();

        // Se não tiver nada (tudo no máximo), fecha
        if (validCandidates.Count == 0)
        {
            ApplyUpgrade(null);
            return;
        }

        // 2. SORTEAR: Pega 3 aleatórios da lista válida
        List<UpgradeData> selectedUpgrades = new List<UpgradeData>();
        int count = Mathf.Min(3, validCandidates.Count);

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, validCandidates.Count);
            selectedUpgrades.Add(validCandidates[randomIndex]);
            validCandidates.RemoveAt(randomIndex);
        }

        // 3. MOSTRAR NA TELA
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            if (i < selectedUpgrades.Count)
            {
                upgradeButtons[i].gameObject.SetActive(true);
                upgradeButtons[i].Setup(selectedUpgrades[i]);
            }
            else
            {
                upgradeButtons[i].gameObject.SetActive(false);
            }
        }
    }

    // --- O CORAÇÃO DO SISTEMA ---
    private List<UpgradeData> GetValidUpgrades()
    {
        Debug.Log("Total upgrades no banco: " + allUpgrades.Count);
        List<UpgradeData> validList = new List<UpgradeData>();

        // Pega quantos slots estamos usando
        int currentWeaponsCount = WeaponManager.instance.activeWeapons.Count;
        int currentPassivesCount = acquiredPassives.Count;

        foreach (UpgradeData data in allUpgrades)
        {
            // --- LÓGICA DE ARMA ---
            if (data.type == UpgradeType.Weapon)
            {
                // Verifica se já temos ESSA arma específica
                // (Precisamos adicionar o método HasWeapon no WeaponManager, ver abaixo)
                bool alreadyHasWeapon = WeaponManager.instance.HasWeapon(data.weaponData);

                if (alreadyHasWeapon)
                {
                    // Se já tem, checa se não está no nível máximo
                    if (!WeaponManager.instance.IsWeaponMaxLevel(data.weaponData))
                    {
                        validList.Add(data); // Pode fazer upgrade
                    }
                }
                else
                {
                    // É arma NOVA. Temos vaga?
                    if (currentWeaponsCount < maxWeaponSlots)
                    {
                        validList.Add(data);
                    }
                }
            }
            // --- LÓGICA DE PASSIVAS (STATUS) ---
            else
            {
                // Verifica se já temos esse tipo de passiva na lista
                bool alreadyHasPassive = acquiredPassives.ContainsKey(data.type);

                if (alreadyHasPassive)
                {
                    int level = acquiredPassives[data.type];

                    if (level < data.maxLevel)
                    {
                        validList.Add(data);
                    }
                }
                else
                {
                    // É passiva NOVA. Temos vaga?
                    if (currentPassivesCount < maxPassiveSlots)
                    {
                        validList.Add(data);
                    }
                }
            }
        }

        return validList;
    }

    public void ApplyUpgrade(UpgradeData data)
    {
        if (data != null)
        {
            if (data.type == UpgradeType.Weapon)
            {
                // Manda para o gerenciador de armas
                WeaponManager.instance.AddWeapon(data.weaponData);
            }
            else
            {
                // É Passiva: Adiciona na lista de "já tenho" se for nova
                if (!acquiredPassives.ContainsKey(data.type))
                {
                    acquiredPassives[data.type] = 1;
                }
                else
                {
                    acquiredPassives[data.type]++;
                }

                StatusManager.instance.ApplyUpgradeEffect(data.type);
            }
        }
        upgradePanel.SetActive(false);
        Time.timeScale = 1f;
    }

     private List<UpgradeData> GetOwnedUpgrades()
{
    List<UpgradeData> owned = new List<UpgradeData>();

    foreach (UpgradeData data in allUpgrades)
    {
        if (data.type == UpgradeType.Weapon)
        {
            bool hasWeapon = WeaponManager.instance.HasWeapon(data.weaponData);

            if (hasWeapon && !WeaponManager.instance.IsWeaponMaxLevel(data.weaponData))
            {
                owned.Add(data);
            }
        }
        else
        {
            bool hasPassive = acquiredPassives.ContainsKey(data.type);

            if (hasPassive)
            {
                int currentLevel = acquiredPassives[data.type];

                if (currentLevel < data.maxLevel)
                {
                    owned.Add(data);
                }
            }
        }
    }

    return owned;
}

    public UpgradeData GetRandomValidUpgrade()
    {
       List<UpgradeData> valid = GetOwnedUpgrades();
       if (valid == null) Debug.Log("NENHUM UPGRADE VÁLIDO ENCONTRADO");
       if (valid.Count == 0) return null;
       int index = Random.Range(0, valid.Count);
       return valid[index];
    }
}