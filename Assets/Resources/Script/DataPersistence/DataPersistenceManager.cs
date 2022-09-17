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
            Debug.LogError("�ڳ������ҵ����DataPersistenceManager");
        }
        instance = this;
    }

    public void NewGame()
    {
        //����ҳ�ʼ��Դ
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        //�����Ϸ�浵��ʧ�������½��浵
        if (this.gameData == null)
        {
            Debug.Log("δ�ҵ��κ����ݣ������ݳ�ʼ��ΪĬ��ֵ");
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