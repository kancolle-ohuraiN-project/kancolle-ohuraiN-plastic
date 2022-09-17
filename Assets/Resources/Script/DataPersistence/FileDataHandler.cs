using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    //�Ƿ���м���
    private bool useEncryption = false;

    //���ܵ�key������32λ��
    private readonly string encryptionCodeWord = "Mgs.KoAd9y^O&VKFcI2_3v<NRY07&S?%";

    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }

    public GameData Load()
    {
        //ʹ��Path.Combine��˵�����в�ͬ·���ָ����Ĳ�ͬ����ϵͳ
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                //���ļ��м������л�����
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                //��ѡ�ؽ�������
                if (useEncryption)
                {
                    dataToLoad = RijndaelDecrypt(dataToLoad, encryptionCodeWord);
                }
                //��Json�е����ݷ����л���C#����
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("���Դ��ļ���������ʱ����: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        //ʹ��Path.Combine��˵�����в�ͬ·���ָ����Ĳ�ͬ����ϵͳ
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            //�����ļ���д���Ŀ¼�������Ŀ¼�����ڣ�
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            //��C#��Ϸ���ݶ������л�ΪJson
            string dataToStore = JsonUtility.ToJson(data, true);

            //��ѡ�ؼ�������
            if (useEncryption)
            {
                dataToStore = RijndaelEncrypt(dataToStore, encryptionCodeWord);
            }

            //�����л�����д���ļ�
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("���Խ����ݱ��浽�ļ�ʱ����: " + fullPath + "\n" + e);
        }
    }

    /***********************
    / Rijndael�����㷨ʹ��
    / ���ܣ�RijndaelEncrypt(dataToStore, encryptionCodeWord);
    / ���ܣ�RijndaelDecrypt(dataToLoad, encryptionCodeWord);
    ***********************/

    /// <summary>
    /// Rijndael�����㷨
    /// </summary>
    /// <param name="pString">�����ܵ�����</param>
    /// <param name="pKey">��Կ,���ȿ���Ϊ:64λ(byte[8]),128λ(byte[16]),192λ(byte[24]),256λ(byte[32])</param>
    /// <param name="iv">iv����,����Ϊ128��byte[16])</param>
    /// <returns></returns>
    private static string RijndaelEncrypt(string pString, string pKey)
    {
        //��Կ
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes(pKey);
        //��������������
        byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(pString);

        //Rijndael�����㷨
        RijndaelManaged rDel = new RijndaelManaged();
        rDel.Key = keyArray;
        rDel.Mode = CipherMode.ECB;
        rDel.Padding = PaddingMode.PKCS7;
        ICryptoTransform cTransform = rDel.CreateEncryptor();

        //���ؼ��ܺ������
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }

    /// <summary>
    /// ijndael�����㷨
    /// </summary>
    /// <param name="pString">�����ܵ�����</param>
    /// <param name="pKey">��Կ,���ȿ���Ϊ:64λ(byte[8]),128λ(byte[16]),192λ(byte[24]),256λ(byte[32])</param>
    /// <param name="iv">iv����,����Ϊ128��byte[16])</param>
    /// <returns></returns>
    private static String RijndaelDecrypt(string pString, string pKey)
    {
        //������Կ
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes(pKey);
        //��������������
        byte[] toEncryptArray = Convert.FromBase64String(pString);

        //Rijndael�����㷨
        RijndaelManaged rDel = new RijndaelManaged();
        rDel.Key = keyArray;
        rDel.Mode = CipherMode.ECB;
        rDel.Padding = PaddingMode.PKCS7;
        ICryptoTransform cTransform = rDel.CreateDecryptor();

        //���ؽ��ܺ������
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        return UTF8Encoding.UTF8.GetString(resultArray);
    }
}