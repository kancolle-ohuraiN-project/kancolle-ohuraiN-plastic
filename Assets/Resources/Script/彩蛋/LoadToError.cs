using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadToError : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SendErrMsg.Instance.param = "99.99";  //���ò���
            SceneManager.LoadScene("error");  //��ת��Error
        }
    }
}
