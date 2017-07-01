using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class Loading : MonoBehaviour {

    public AudioSource loadingMusic;

//    public Transform OptionsPanel;
//    public Text BaseText;
//    public Text BotText;
//    public Text EventText;
//    public Text RoomText;
//    public Text hpText;
//    public Text attText;
//    public Text defText;
//
//    public Button AddBaseButton;
//    public Button ReduceBaseButton;
//    public Button AddBotButton;
//    public Button ReduceBotButton;
//    public Button AddEventButton;
//    public Button ReduceEventButton;
//    public Button AddRoomButton;
//    public Button ReduceRoomButton;
//    private int hp;
//    private int att;
//    private int def;

    public GameManager _gameManager;

    private bool isOptionOn;
    private int camp; //0秩序1真理2自然3混乱4死亡5神秘
    private int baseNum;
    private int botNum;
    private int eventNum;
    private int roomNum;

    //初始化状态************************************************

    void Start(){

        loadingMusic.loop = true;
        loadingMusic.Play();

//        isOptionOn = true;
//        OptionsPanel.localPosition = new Vector3(-200,0, 0);
//
//        camp = 0;
//
//        baseNum = 5;
//        BaseText.text = "5×5";
//        AddBaseButton.interactable = true;
//        ReduceBaseButton.interactable = false;
//
//        botNum = 2;
//        BotText.text = botNum.ToString();
//        AddBotButton.interactable = true;
//        ReduceBotButton.interactable = true;
//
//        eventNum = 5;
//        EventText.text = eventNum.ToString();
//        AddEventButton.interactable = true;
//        ReduceEventButton.interactable = true;
//
//        roomNum = 8;
//        RoomText.text = roomNum.ToString();
//        AddRoomButton.interactable = true;
//        ReduceRoomButton.interactable = true;
//        hp = 100;
//        att = 50;
//        def = 20;
    }
        


    //Loading界面按钮******************************************

    public void StartGame(){
        loadingMusic.Stop();
        this.gameObject.transform.DOLocalMoveX(2000, 0.5f);
        _gameManager.gameObject.transform.DOLocalMoveX(0, 0.5f);
        _gameManager.StartGame();
    }

//    public void OnOptions(){
//        isOptionOn = (!isOptionOn);
//        if (isOptionOn)
//        {
//            OptionsPanel.DOLocalMoveY(1000f, 0.5f);
//        }
//        else
//        {
//            OptionsPanel.DOLocalMoveY(0, 0.5f);
//        }
//
//    }

    public void ExitGame(){
        Application.Quit();
    }
        
    public void GetBackLoading(){
        loadingMusic.Play();
        this.gameObject.transform.DOLocalMoveX(0, 0.5f);
        _gameManager.gameObject.transform.DOLocalMoveX(-2000, 0.5f);
    }

    //设置面板数据**********************************************

//    public void CampChange(int campId){
//        camp = campId;
//    }
//
//    public void AddBase(){
//        if (baseNum < 10)
//            baseNum++;
//        BaseText.text = baseNum + "×" + baseNum;
//        if (baseNum >= 10)
//            AddBaseButton.interactable = false;
//        ReduceBaseButton.interactable = true;
//    }
//
//    public void ReduceBase(){
//        if (baseNum > 5)
//            baseNum--;
//        BaseText.text = baseNum + "×" + baseNum;
//        if (baseNum <= 5)
//            ReduceBaseButton.interactable = false;
//        AddBaseButton.interactable = true;
//    }
//
//    public void AddBot(){
//        if (botNum < eventNum-1)
//            botNum++;
//        BotText.text = botNum.ToString();
//        if (botNum >= eventNum-2)
//            AddBotButton.interactable = false;
//        ReduceBotButton.interactable = true;
//    }
//
//    public void ReduceBot(){
//        if (botNum > 1)
//            botNum--;
//        BotText.text = botNum.ToString();
//        if (botNum <= 1)
//            ReduceBotButton.interactable = false;
//        AddBotButton.interactable = true;
//    }
//
//    public void AddEvent(){
//        if (eventNum < roomNum)
//            eventNum++;
//        EventText.text = eventNum.ToString();
//
//        if (!AddBotButton.interactable)
//            AddBotButton.interactable = true;
//
//        if (eventNum >= roomNum)
//            AddEventButton.interactable = false;
//        ReduceEventButton.interactable = true;
//    }
//
//    public void ReduceEvent(){
//        if (eventNum > 1)
//            eventNum--;
//        EventText.text = eventNum.ToString();
//
//        if (botNum >= eventNum-2)
//        {
//            botNum = eventNum-2;
//            BotText.text = botNum.ToString();
//            AddBotButton.interactable = false;
//        }
//
//        if (eventNum <= 1)
//            ReduceEventButton.interactable = false;
//        AddEventButton.interactable = true;
//    }
//
//    public void AddRoom(){
//        if (roomNum < baseNum*baseNum)
//            roomNum++;
//
//        if (!AddEventButton.interactable)
//            AddEventButton.interactable = true;
//
//        RoomText.text = roomNum.ToString();
//        if (roomNum >= baseNum*baseNum)
//            AddRoomButton.interactable = false;
//        ReduceRoomButton.interactable = true;
//    }
//
//    public void ReduceRoom(){
//        if (roomNum > 2)
//            roomNum--;
//        RoomText.text = roomNum.ToString();
//
//        if (eventNum >= roomNum)
//        {
//            eventNum = roomNum;
//            EventText.text = eventNum.ToString();
//            AddEventButton.interactable = false;
//
//            if (botNum >= eventNum-2)
//            {
//                botNum = eventNum-2;
//                BotText.text = botNum.ToString();
//                AddBotButton.interactable = false;
//            }
//        }
//
//        if (roomNum <= 2)
//            ReduceRoomButton.interactable = false;
//        AddRoomButton.interactable = true;
//    }
//
//    public void ChangeHp(){
//        hp = int.Parse(hpText.text);
//    }
//
//    public void ChangeAtt(){
//        att = int.Parse(attText.text);
//    }
//
//    public void ChangeDef(){
//        def = int.Parse(defText.text);
//    }
}
