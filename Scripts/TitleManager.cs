using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;//씬매니저

public class TitleManager : MonoBehaviour
{
    public Button joinButton; //게임 시작 버튼

    //게임 시작
    public void StartGame()
    {
        SceneManager.LoadScene("My_Main");
    }
}
