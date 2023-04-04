using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingBtn : MonoBehaviour
{
    public GameObject resumeBtn;
    public GameObject quitBtn;

    public void ShowSetting()
    {
        if (resumeBtn.activeSelf == false)
        {
            resumeBtn.SetActive(true);
            quitBtn.SetActive(true);
        }
        else
        {
            resumeBtn.SetActive(false);
            quitBtn.SetActive(false);
        }
    }
}
