using System.Reflection;
using UnityEngine;
using TMPro; // Usando TextMeshPro
using UnityEngine.UI;

public class UpgradeUiElement : MonoBehaviour
{
    [Header("UI References")]
    public Image iconImage;
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    
    private UpgradeData currentData;

    public void Setup(UpgradeData data)
    {
        Debug.Log("Setup chamado com: " + data.upgradeName);
        currentData = data;
        iconImage.sprite = data.Icon;

        if (data.type == UpgradeType.Weapon)
        {
            bool hasWeapon = WeaponManager.instance.HasWeapon(data.weaponData);

            if (!hasWeapon)
            {
                nameText.text = "Novo! " + data.upgradeName;
                
                if (descriptionText != null)
                {
                    descriptionText.text = data.description; 
                }
            }
            else
            {
                int currentLvlIndex = WeaponManager.instance.GetCurrentLevelIndex(data.weaponData);
                int nextLvlIndex = currentLvlIndex + 1;

                if (nextLvlIndex < data.weaponData.levels.Count)
                {
                    nameText.text = data.upgradeName + " " + (nextLvlIndex + 1);
                    
                    if (descriptionText != null)
                    {
                        descriptionText.text = data.weaponData.levels[nextLvlIndex].description;
                    }
                }
            }
        }
        else
        {
            nameText.text = data.upgradeName;
            if (descriptionText != null)
            {
                descriptionText.text = data.description;
            }
        }
    }

    public void OnClick()
    {
        if (UpgradeManager.instance != null)
           {
             UpgradeManager.instance.ApplyUpgrade(currentData);
           }
            ChestManager.instance.ShowNextUpgrade();
    }
}