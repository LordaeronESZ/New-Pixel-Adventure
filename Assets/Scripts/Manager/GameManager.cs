using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("玩家预制体")]
    public GameObject Player;
    [Header("虚拟摄像机")]
    public CinemachineVirtualCamera virtualCamera; // 虚拟摄像机
    public CinemachineConfiner2D confiner2D; // 摄像机限制框
    [Header("关卡1")]
    public GameObject Level1Prefab;
    public static GameManager gmInstance;
    [Header("过关UI")]
    public GameObject clearUI;
    
    private List<GameObject> checkPoints; // 检查点
    private bool isPause; // 暂停状态
    private int checkPointsIdx; // 当前检查点
    private GameObject curPlayer; // 当前玩家
    private bool isReviving; // 正在重生
    private Level Level1; // 关卡1

    private void Start()
    {
        Restart();
        if (gmInstance == null)
        {
            gmInstance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        GameObject.DontDestroyOnLoad(this);
        checkPoints = new List<GameObject>();
        isPause = false;
        checkPointsIdx = 0;
        curPlayer = null;
        isReviving = false;
    }

    private void Update()
    {
        MyDubug(); // 调试
        RevivePlayer(); // 复活玩家
        PauseCheck(); // 暂停检测
        UpdateCheckPoints(); // 更新检查点
        FollowPlayer(); // 跟随玩家
    }

    private void PauseCheck()
    {
        if (isPause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    private void UpdateCheckPoints()
    {
        if (checkPointsIdx + 1 < checkPoints.Count)
        {
            CheckPoint checkPoint = checkPoints[checkPointsIdx + 1].GetComponent<CheckPoint>();
            if (checkPoint.GetActivated() == true) // 检查点已被激活
            {
                ++checkPointsIdx;
                if (checkPointsIdx == checkPoints.Count - 1)
                {
                    LevelClear(); // 通关
                }
            }
        }
    }

    private void LevelClear()
    {
        clearUI.SetActive(true);
    }

    private void RevivePlayer()
    {
        if (curPlayer == null && !isReviving)
        {
            isReviving = true;
            Restart(); // 重新加载
            Invoke("iRevivePlayer", 1.0f);
        }
    }

    private void Restart()
    {
        if (Level1 != null)
        {
            Destroy(Level1.gameObject);
        }
        Level1 = Instantiate<GameObject>(Level1Prefab).GetComponent<Level>();
        confiner2D.m_BoundingShape2D = Level1.edge.GetComponent<PolygonCollider2D>();
        checkPoints = Level1.checkPoints;
    }

    private void iRevivePlayer()
    {
        curPlayer = Instantiate(Player, checkPoints[checkPointsIdx].transform.position, Quaternion.identity);
        isReviving = false;
    }

    public void PauseAndResume()
    {
        isPause ^= true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnToMain()
    {
        SceneManager.LoadScene("Main");
    }

    private void FollowPlayer()
    {
        if (virtualCamera.Follow == null)
        {
            virtualCamera.Follow = checkPoints[checkPointsIdx].transform;
        }
        else if (virtualCamera.Follow.tag != "Player")
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                virtualCamera.Follow = player.transform;
            }
        }
    }

    private void MyDubug()
    {
        
    }
}
