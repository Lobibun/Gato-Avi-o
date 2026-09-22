using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChestManager : MonoBehaviour
{
    public static ChestManager instance;
    public UpgradeUiElement upgradeUI;
    public GameObject chestPanel;
    public GameObject Chest;

    private Queue<UpgradeData> pendingUpgrades = new Queue<UpgradeData>();

    void Awake()
{
    if (instance == null) instance = this;
    else Destroy(gameObject);
}

    public int RollChestRewardCount()
    {
        float chance1 = 90, chance3 = 9, chance5 = 1;
        chance3 += StatusManager.instance.GetLuckBonus() * 0.6f;
        chance5 += StatusManager.instance.GetLuckBonus() * 0.25f;

        float total = chance1 + chance3 + chance5;
        float roll = Random.Range(0f, total);

        float limite5 = chance5;
        float limite3 = chance5 + chance3;

        if (roll < limite5) return 5;
        else if (roll < limite3) return 3;
        else return 1;
    }

    public IEnumerator OpenChest()
    {
        pendingUpgrades.Clear();
         Time.timeScale = 0f;
        Chest.SetActive(true);
         Debug.Log("BAU ATIVADO");
        yield return new WaitForSecondsRealtime(0.9f);
         Debug.Log("DEPOIS DO WAIT");
        Chest.SetActive(false);
        Debug.Log("Tentando gerar upgrades");

        int upgradeNumber = RollChestRewardCount();

        for (int i = 0; i < upgradeNumber; i++)
        {
            UpgradeData data = UpgradeManager.instance.GetRandomValidUpgrade();

            if (data == null){
        Debug.Log("NENHUM UPGRADE VÁLIDO ENCONTRADO");}
            if (data != null)
            {
                pendingUpgrades.Enqueue(data);
            }
            
            else
            {
                GameControler.instance.AddCoins(50);
            }
        }
       Debug.Log("Quantidade na fila: " + pendingUpgrades.Count);
        ShowNextUpgrade();
    }

    public void ShowNextUpgrade()
    {
        if (pendingUpgrades.Count == 0)
        {
            chestPanel.SetActive(false);
            Time.timeScale = 1f;
            return;
        }

        UpgradeData next = pendingUpgrades.Dequeue();
        chestPanel.SetActive(true);
        upgradeUI.Setup(next);
    }
}