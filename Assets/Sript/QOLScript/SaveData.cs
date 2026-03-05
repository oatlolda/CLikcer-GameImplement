using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
[System.Serializable]
public class GameData
{
  
    public PlayerSaveData player;
    public PlayerUpgradeData playerUpgrade;
    public EnemyData enemy;
    public CoinData coin;
    public SceneData scenedata;
    public VolumeData volumeData;
    public List<CompanionSaveData> companions;
}
[System.Serializable]
public class CompanionSaveData
{
    public int ID;
    public bool IsSpawn;
    public float companiondamgage;
    public long CompanionUpgradeneed;
    public int upgradeCount;
    public float bonus;
    public int LevelCom;
  
}
[System.Serializable]
public class PlayerSaveData
{
    public float playerDamage;
  
}
[System.Serializable]
public class PlayerUpgradeData
{
    public long Upgradeneed;
    public int Upgradecount;
    public float Bonus;
    public int LevelPlayer;
}
[System.Serializable]
public class EnemyData
{
    public float enemyMaxHealth;
    public float BossHpData;

}
[System.Serializable]
public class CoinData
{
    public long coin;
    public long DropcoinData;
}
[System.Serializable]
public class SceneData
{

    public int CurrentScene;
    public int CurrenIndexModel;

}
[System.Serializable]
public class VolumeData
{
    public float OstVolume;
    public float SfxVolume;
}
public class SaveData : Singletron<SaveData>
{
   
    private string path;
    public override void Awake()
    {
        base.Awake();

        path = Application.persistentDataPath + "/save.json";
        Debug.Log(path);

        
    }
    private void Start()
    {
        LoadGame();
    }

    public void SaveGame()
    {
        GameData data = new GameData();


        CoinManagement coin = FindFirstObjectByType<CoinManagement>();
        if (coin != null)
        {
            data.coin = coin.GetData();
        }

        ChagneScene changescene = FindFirstObjectByType<ChagneScene>();
        if (changescene != null)
        {
            data.scenedata = changescene.GetData();
        }

        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player != null)
            data.player = player.GetData();

        EnemyController enemy = FindFirstObjectByType<EnemyController>();

        if (enemy != null)
        {
            data.enemy = enemy.GetData();
        }
        UpgradeManager upgrade = FindFirstObjectByType<UpgradeManager>();
        if (upgrade != null)
        {
            data.playerUpgrade = upgrade.GetData();
        }
        VolumeSetting volume = FindFirstObjectByType<VolumeSetting>();
        if (volume != null)
        {
            data.volumeData = volume.GetData();
        }
        data.companions = new List<CompanionSaveData>();
        foreach (var companion in Object.FindObjectsByType<Companionscript>(FindObjectsSortMode.None))
        {
            data.companions.Add(companion.GetData());
            Debug.Log("Saved Companion ID: " + companion.companionID);
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
    }
    public void LoadGame()
    {
        if (!File.Exists(path))
            return;
        string json = File.ReadAllText(path);
        GameData data = JsonUtility.FromJson<GameData>(json);

        CoinManagement coin = FindFirstObjectByType<CoinManagement>();
        if (coin != null)
            coin.LoadData(data.coin);

        ChagneScene scene = FindFirstObjectByType<ChagneScene>();
        if (scene != null)
            scene.LoadData(data.scenedata);

        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player != null)
            player.LoadData(data.player);

        EnemyController enemy = FindFirstObjectByType<EnemyController>();
        if (enemy != null)
            enemy.LoadData(data.enemy);

        UpgradeManager upgrade = FindFirstObjectByType<UpgradeManager>();
        if (upgrade != null)
        {
            upgrade.LoadData(data.playerUpgrade);
        }
        VolumeSetting volume = FindFirstObjectByType<VolumeSetting>();
        if (volume != null)
        {
            volume.LoadData(data.volumeData);
        }

        foreach (var companion in Object.FindObjectsByType<Companionscript>(FindObjectsSortMode.None))
        {
            var saved = data.companions.Find(x => x.ID == companion.companionID);
            if (saved != null)
              companion.LoadData(saved);
            Debug.Log("Loading Companion ID: " + companion.companionID);

        }

    }
    public void ClearSave()
    {
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Save file deleted!");
        }
       
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
