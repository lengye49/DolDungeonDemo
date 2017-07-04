using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EventActions : MonoBehaviour {

    public BackpackActions _backpackActions;
    public GiftActions _giftAction;
    public BotActions _botActions;
    public MsgActions _msgActions;

    private GameManager _gameManager;
    private Button[] bs;
    private Text[] ts;
    private int[] eventIds;
    private DgEvent thisDgEvent;
    private int dgSelected;

    void Start(){
        _gameManager = gameObject.GetComponentInParent<GameManager>();
        bs = this.gameObject.GetComponentsInChildren<Button>();
        ts = this.gameObject.GetComponentsInChildren<Text>();
    }



    public void CallInEventPanel(int eventIndex){
        this.gameObject.transform.localPosition = Vector3.zero;
        this.gameObject.SetActive(true);

        thisDgEvent = LoadTxt.DgEventDic[eventIds[eventIndex]];
        ts[0].text = thisDgEvent.name;
        ts[1].text = thisDgEvent.desc;
        ts[2].text = thisDgEvent.opt1;
        ts[3].text = thisDgEvent.opt2;
        ts[4].text = thisDgEvent.opt3;
        bs[2].interactable = (thisDgEvent.opt3 != "");

        ChooseOption(1);
    }

    public void ChooseOption(int index){
        
        if (index == 1)
            ts[5].text = thisDgEvent.opt1Desc;
        else if (index == 2)
            ts[5].text = thisDgEvent.opt2Desc;
        else
            ts[5].text = thisDgEvent.opt3Desc;

        for (int i = 1; i < 3; i++)
        {
            if (i == index)
                bs[i - 1].gameObject.GetComponent<Image>().color = Color.yellow;
            else
                bs[i - 1].gameObject.GetComponent<Image>().color = Color.white;
        }

        dgSelected = thisDgEvent.id * 10 + index;
    }

    public void InitThisLevelEvent(){
        eventIds = new int[GameConfigs.EventNum];
        for (int i = 0; i < eventIds.Length; i++)
        {
            eventIds[i] = Random.Range(0, LoadTxt.DgEventDic.Count) + 1;
        }
    }

    public void Act(){
        ConfirmEvent(dgSelected); 
    }

    void CallOutEventPanel(){
        this.gameObject.SetActive(false);
    }

    void ConfirmEvent(int index){
        int eventId = (int)(index / 10);
        int optionId = index % 10;
        int r;
        switch (eventId)
        {
            case 1:
                if (optionId == 1)
                {
                    if (_gameManager.coin < 10)
                        return;
                    _gameManager.ReduceCoin(10);
                    _gameManager.ClearRoom();
                    CallOutEventPanel();
                    _gameManager.GetRandomNormalItem();
                }
                else if (optionId == 2)
                {
                    if (_gameManager.coin < 5)
                        return;
                    _gameManager.ReduceCoin(5);
                    _gameManager.ClearRoom();
                    CallOutEventPanel();
                    _backpackActions.AddItem(12);
                }
                else if (optionId == 3)
                {
                    _gameManager.ClearRoom();
                    CallOutEventPanel();
                }
                break;
            case 2:
                if (optionId == 1)
                {
                    if (_gameManager.coin < 15)
                        return;
                    _gameManager.ReduceCoin(15);
                    _gameManager.ClearRoom();
                    CallOutEventPanel();
                    _giftAction.CallInGiftPanel();
                }
                else if (optionId == 2)
                {
                    _gameManager.ClearRoom();
                    CallOutEventPanel();
                    GameConfigs.AfterBattleGift = true;
                    _botActions.CallInBotMsg(500);
                }
                else if (optionId == 3)
                {
                    _gameManager.ClearRoom();
                    CallOutEventPanel();
                }
                break;
            case 3:
                if (optionId == 1)
                {
                    _gameManager.ClearRoom();
                    CallOutEventPanel();
                    GameConfigs.AfterBattleItem = true;
                    _botActions.CallInBotMsg(300);
                }
                else if (optionId == 2)
                {
                    _gameManager.ClearRoom();
                    CallOutEventPanel();

                    r = Random.Range(0, 10000);
                    if (r < 5000)
                    {
                        _gameManager.AddCoin(10);
                        _msgActions.CallInMsg("净化成功，探索点 +10");
                    }
                    else
                    {
                        _msgActions.CallInMsg("净化失败");
                    }
                }
                break;
            case 4:
                if (optionId == 1)
                {
                    _gameManager.ClearRoom();
                    CallOutEventPanel();

                    int damage = (int)(_gameManager.heroHpMax * 0.05f);
                    _gameManager.ReduceHp(damage);
                    _gameManager.GetRandomNormalItem();
                }
                else if (optionId == 2)
                {
                    _gameManager.ClearRoom();
                    CallOutEventPanel();
                }
                break;
            case 5:
                if (optionId == 1)
                {
                    _gameManager.ClearRoom();
                    CallOutEventPanel();
                    _backpackActions.AddItem(3);
                }
                else if (optionId == 2)
                {
                    _gameManager.ClearRoom();
                    CallOutEventPanel();
                }
                break;
            case 6:
                if (optionId == 1)
                {
                    if (_gameManager.coin < 10)
                        return;
                    _gameManager.ClearRoom();
                    CallOutEventPanel();

                    _gameManager.ReduceCoin(10);
                    r = Random.Range(12, 16);
                    _gameManager.AddCoin(r);
                    _msgActions.CallInMsg("恭喜你用10探索点获得了" + r + "探索点");
                }
                else if (optionId == 2)
                {
                    if (_gameManager.coin < 10)
                        return;
                    _gameManager.ClearRoom();
                    CallOutEventPanel();

                    _gameManager.ReduceCoin(10);
                    r = Random.Range(1, 20);
                    if (r < 10)
                        _gameManager.GetRandomNormalItem();
                    else
                    {
                        int dam = (int)(_gameManager.heroHpMax * 0.05f);
                        _gameManager.ReduceHp(dam);
                        _msgActions.CallInMsg("Hp -" + dam);
                    }
                }
                else if (optionId == 3)
                {
                    _gameManager.ClearRoom();
                    CallOutEventPanel();
                }
                break;
            case 7:
                if (optionId == 1)
                {
                    _gameManager.ClearRoom();
                    CallOutEventPanel();

                    int dam1 = (int)(GameConfigs.PlayerHp * 0.05f);
                    _gameManager.ReduceHp(dam1);
                    int att_inc = Random.Range(1, 5);
                    _gameManager.heroAtt += att_inc;
                    _gameManager.UpdateShowProperty();
                    _msgActions.CallInMsg("Hp -" + dam1 + ", Attack +" + att_inc);
                }
                else if (optionId == 2)
                {
                    _gameManager.ClearRoom();
                    CallOutEventPanel();
                }
                break;
            case 8:
                if (optionId == 1)
                {
                    if (_gameManager.coin < 32)
                        return;
                    _gameManager.ClearRoom();
                    CallOutEventPanel();

                    _gameManager.ReduceCoin(32);
                    _gameManager.SetRestartPoint();
                    GameConfigs.RestartAfterDeath = true;
                }
                else if (optionId == 2)
                {
                    _gameManager.ClearRoom();
                    CallOutEventPanel();
                }
                break;
            default:
                break;
        }
    }
}
