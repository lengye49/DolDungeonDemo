using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class Loading : MonoBehaviour {

    public AudioSource loadingMusic;
    public GameManager _gameManager;
    public Toggle[] seedsToggle;
    private int seedCount=0;
    private int seedIndex = 0;

    //初始化状态************************************************

    void Start(){
        loadingMusic.loop = true;
        loadingMusic.Play();
        seedCount = PlayerPrefs.GetInt("SeedCount", 0);
        SetSeedList();
    }


    //Loading界面按钮******************************************

    public void StartGame(){
        
        //重置配置数据
        GameConfigs.ResetGameConfigs();



        //确定初始随机数
        int r = GetOrgSeed();
        Debug.Log("OrgSeed = " + r);

        Random.seed = r;
        string str = "";
        for (int i = 0; i < GameConfigs.seeds.Length; i++)
        {
            GameConfigs.seeds[i] = Random.Range(0, 1000000);
            str += GameConfigs.seeds[i] + ",";
        }
        str = str.Substring(0, str.Length - 1);
        Debug.Log("SeedList = " + str);


        //存储随机数
        if(seedIndex==0){
            PlayerPrefs.SetInt("SeedStoreList" + seedCount, r);
            seedCount++;
            PlayerPrefs.SetInt("SeedCount", seedCount);
        }


        //音乐和界面处理
        loadingMusic.Stop();
        this.gameObject.transform.DOLocalMoveX(2000, 0.5f);
        _gameManager.gameObject.transform.DOLocalMoveX(0, 0.5f);
        _gameManager.StartGame();
    }


    int GetOrgSeed(){
        int newSeed;
        if (seedIndex == 0)
        {
            newSeed = Random.Range(0, 100000);
        }
        else
        {
            newSeed = PlayerPrefs.GetInt("SeedStoreList" + (seedIndex - 1), 0);
        }
        return newSeed;
    }


    public void ExitGame(){
        Application.Quit();
    }
        
    public void GetBackLoading(){
        SetSeedList();
        loadingMusic.Play();
        this.gameObject.transform.DOLocalMoveX(0, 0.5f);
        _gameManager.gameObject.transform.DOLocalMoveX(-2000, 0.5f);
    }

    public void SetSeed(int index){
        seedIndex = index;
    }

    void SetSeedList(){
        for (int i = 0; i < seedsToggle.Length; i++)
        {
            if (i >= seedCount)
            {
                seedsToggle[i].gameObject.SetActive(false);
            }
            else
            {
                seedsToggle[i].gameObject.SetActive(true);
                seedsToggle[i].gameObject.GetComponentInChildren<Text>().text = PlayerPrefs.GetInt("SeedStoreList" + i, 0).ToString();
            }
        }
    }
   
}
