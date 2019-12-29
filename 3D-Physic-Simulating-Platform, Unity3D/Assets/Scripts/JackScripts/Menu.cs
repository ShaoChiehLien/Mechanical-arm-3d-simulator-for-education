using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void MyArmButt()
    {
        SceneManager.LoadScene("MyArm");
    }
    public void ConnectWifiButt()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void CodeButt()
    {
        SceneManager.LoadScene("CodeScene");
    }
    public void HomeButt()
    {
        SceneManager.LoadScene("Menu");
    }
    public void NextTo1Butt()
    {
        SceneManager.LoadScene("Tutorial1");
    }
    public void NextTo2Butt()
    {
        SceneManager.LoadScene("Tutorial2");
    }
    public void NextTo3Butt()
    {
        SceneManager.LoadScene("Tutorial3");
    }
    public void NextTo4Butt()
    {
        SceneManager.LoadScene("Tutorial4");
    }
    public void BackTuroial()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void BackTuroial1()
    {
        SceneManager.LoadScene("Tutorial1");
    }
    public void BackTurorial2()
    {
        SceneManager.LoadScene("Tutorial2");
    }
    public void BackTurorial3()
    {
        SceneManager.LoadScene("Tutorial3");
    }
}