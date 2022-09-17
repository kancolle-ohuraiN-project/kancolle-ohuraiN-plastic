using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    private GameData gameData;
    public static DataPersistenceManager instance { get; private set; }

    private void Start()
    {
        LoadGame();
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("在场景中找到多个DataPersistenceManager");
        }
        instance = this;
    }

    public void NewGame()
    {
        //给玩家初始资源
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        //如果游戏存档丢失，触发新建存档
        if (this.gameData == null)
        {
            Debug.Log("未找到任何数据，将数据初始化为默认值");
            NewGame();
        }
    }

    public void SaveGame()
    {
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}