using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;//���Ŵ���

public class TitleManager : MonoBehaviour
{
    public Button joinButton; //���� ���� ��ư

    //���� ����
    public void StartGame()
    {
        SceneManager.LoadScene("My_Main");
    }
}
