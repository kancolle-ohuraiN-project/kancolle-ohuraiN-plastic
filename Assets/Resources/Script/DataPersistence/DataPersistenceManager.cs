using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    [SerializeField] private bool useEncryption;
    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    public static DataPersistenceManager instance { get; private set; }

    private void Start()
    {
        //Application.persistentDataPath�������ݴ���unityĬ�ϵĳ־û�Ŀ¼
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
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
        //ʹ�����ݴ��������ļ������κα��������
        this.gameData = dataHandler.Load();
        //�����Ϸ�浵��ʧ�������½��浵
        if (this.gameData == null)
        {
            Debug.Log("δ�ҵ��κ����ݣ������ݳ�ʼ��ΪĬ��ֵ");
            NewGame();
        }
        //�����ص��������͵���Ҫ�������������ű�
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        //�����ص��������͵���Ҫ�������������ű�
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(gameData);
        }
        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}