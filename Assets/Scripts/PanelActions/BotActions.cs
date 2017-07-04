using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BotActions : MonoBehaviour {

    public Button FightButton;
    public Button BribeButton;
    public Button EscapeButton;
    public Button DirectKillButton;
    public Text Info;
    public Text Desc;
    public MsgActions _msg;

    private int thisBotPower;
    private bool canEscape;
    private GameManager _gameManager;

    void Start(){
        _gameManager = gameObject.GetComponentInParent<GameManager>();
    }

    public void CallInBotMsg(int botPower){

        this.gameObject.transform.localPosition = Vector3.zero;
        this.gameObject.SetActive(true);

        //开启小怪先魅惑
//        int r = Random.Range(0, 10000);
//        if (r < GameConfigs.CharmRate)
//        {
//            _msg.CallInMsg("魅惑成功!");
//            _gameManager.ClearRoom();
//            return;
//        }

        //怪物面板
        thisBotPower = botPower;
        int powerReduce = (int)(GameConfigs.BotPowerReduce / 10000f * thisBotPower);
        thisBotPower -= powerReduce;

        Info.text="策划的影子";
        string str = "";
        str += "名称：策划的影子\n";
        str += "实力：" + thisBotPower + "\n";
        str += "预计损血：" + (int)(GameConfigs.PlayerHp * Calculations.GetBotDamage(_gameManager.heroPower, thisBotPower)) + "\n";
        str += "逃跑：消耗" + (GameConfigs.EscapeLoss/100)+"%生命值\n";
        str += "贿赂：消耗" + (int)(GameConfigs.BribeCost * (10000f - GameConfigs.CoinCostReduceRate) / 10000f) + "探索点";
        Desc.text = str;

        BribeButton.interactable = GameConfigs.KillBotByCoin;
        DirectKillButton.interactable = GameConfigs.KillBotDirectly;
        EscapeButton.interactable = (GameConfigs.EscapeRate > 0);

    }
        

    void CallOutBotMsg(){
        this.gameObject.SetActive(false);
    }

    public void DirectKill(){
        
        GameConfigs.KillBotDirectly = false;

        CallOutBotMsg();
        _gameManager.ClearRoom();
        _gameManager.KillBotReward();

    }

    public void Fight(){
        int damage = (int)(GameConfigs.PlayerHp * Calculations.GetBotDamage(_gameManager.heroPower, thisBotPower));
        Debug.Log("Damage = " + damage);
        _gameManager.KillBotReward();

        _gameManager.ReduceHp(damage);

        int recover = 0;
        if (GameConfigs.NextBotDontLose)
        {
            recover = damage;
            GameConfigs.NextBotDontLose = false;
        }
//        else
//            recover = (int)(damage * GameConfigs.AfterBattleRecover / 10000f);
        _gameManager.AddHp(recover);


        CallOutBotMsg();
        _gameManager.ClearRoom();

    }

    public void Bribe(){
        if (!GameConfigs.KillBotByCoin)
            return;
        if (_gameManager.coin < (int)(GameConfigs.BribeCost*(10000f - GameConfigs.CoinCostReduceRate) / 10000f))
            return;
        
        CallOutBotMsg();
        _gameManager.ReduceCoin((int)(GameConfigs.BribeCost * (10000f - GameConfigs.CoinCostReduceRate) / 10000f));
        _gameManager.ClearRoom();
        _gameManager.KillBotReward();
    }

    public void Escape(){

        int damage = (int)(_gameManager.heroHpMax * (GameConfigs.EscapeLoss / 10000f));
        _gameManager.ReduceHp(damage);

        int r = Random.Range(0, 10000);
        if (r < GameConfigs.EscapeRate)
        {
            _gameManager.Escape();
            CallOutBotMsg();
        }
        else
        {
            EscapeButton.gameObject.GetComponentInChildren<Text>().text = "逃跑失败";
        }
        EscapeButton.interactable = false;
    }

}
