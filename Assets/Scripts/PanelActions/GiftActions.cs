using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;

public class GiftActions : MonoBehaviour {

    public GameObject pDamage;
    private int thisGift;
    private int[] thisGiftList;
    private List<int> LearntGifts;
    private Dictionary<int,int> BookList;
    private GameManager _gameManager;
    private Text[] ts;
    private Button[] bs;
    private List<int> unLearntTypes;

    void Start(){
        _gameManager = this.gameObject.GetComponentInParent<GameManager>();
        this.gameObject.transform.localPosition = new Vector3(0, -3000f, 0);
        thisGift = 0;
        LearntGifts = new List<int>();
        unLearntTypes = new List<int>();
        InitUnlearntTypes();
        BookList = InitBookList();
        ts = this.gameObject.GetComponentsInChildren<Text>();
        bs = this.gameObject.GetComponentsInChildren<Button>();
    }

    void InitUnlearntTypes(){
        foreach (int key in LoadTxt.GiftDic.Keys)
        {
            if (!unLearntTypes.Contains(LoadTxt.GiftDic[key].type))
                unLearntTypes.Add(LoadTxt.GiftDic[key].type);
        }
    }

    Dictionary<int,int> InitBookList(){
        Dictionary<int,int> d = new Dictionary<int, int>();
        foreach (int key in LoadTxt.GiftDic.Keys)
        {
            //需要天赋中有一个id跟type相同
            if (!unLearntTypes.Contains(LoadTxt.GiftDic[key].type) && (!d.ContainsValue(LoadTxt.GiftDic[key].type)))
            {
                d.Add(LoadTxt.GiftDic[key].family,LoadTxt.GiftDic[key].type);//这里貌似有点问题
            }
            if (!d.ContainsKey(LoadTxt.GiftDic[key].family))
                d.Add(LoadTxt.GiftDic[key].family, 0);
        }
        return d;
    }



    //界面操作********************************************************
    public void CallInGiftPanel(){
        this.gameObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        this.gameObject.transform.localPosition = Vector3.zero;
        this.gameObject.transform.DOBlendableScaleBy(Vector3.one, 0.5f);
        SetThreeGifts();
        SetGiftPanel();
    }

    public void CallOutGiftPanel(){
        this.gameObject.transform.localPosition = new Vector3(0f,-3000f, 0f);
    }

    void SetGiftPanel(){
        
        for (int i = 0; i < thisGiftList.Length; i++)
        {
            ts[i+1].text = LoadTxt.GiftDic[thisGiftList[i]].name;
            ts[i + 1].color = GetTextColor(LoadTxt.GiftDic[thisGiftList[i]].level);
            bs[i].interactable = true;
            bs[i].gameObject.GetComponent<Image>().color = GetButtonColor(LoadTxt.GiftDic[thisGiftList[i]].type);
        }
        ChooseGift(0);
    }

    public void ChooseGift(int index){
        if (index > (thisGiftList.Length - 1))
            return;
        ts[4].text = LoadTxt.GiftDic[thisGiftList[index]].desc;
        thisGift = thisGiftList[index];

        for (int i = 0; i < thisGiftList.Length; i++)
        {
            if(i==index)
                bs[i].gameObject.GetComponent<Image>().color = Color.yellow;
            else
                bs[i].gameObject.GetComponent<Image>().color = GetButtonColor(LoadTxt.GiftDic[thisGiftList[i]].type);
        }

    }

    Color GetTextColor(int level){
        switch (level)
        {
            case 1:
                return Color.black;
            case 2:
                return Color.blue;
            case 3:
                return Color.magenta;
            case 4:
                return Color.red;
            default:
                return Color.black;
        }   
    }

    Color GetButtonColor(int type){
        switch(type)
        {
            case 1:
                return new Color(229f / 255f, 181f / 255f, 234f / 255f);
            case 2:
                return new Color(181f / 255f, 230f / 255f, 234f / 255f);
            case 3:
                return new Color(207f / 255f, 234f / 255f, 181f / 255f);
            default:
                return Color.white;

        }
    }

    void SetThreeGifts(){
        
        List<int> availableGifts = GetAvailableGifts();
        int num = Mathf.Min(3, availableGifts.Count);
        thisGiftList = new int[num];
        for (int i = 0; i < thisGiftList.Length; i++)
        {
            int r = Random.Range(0, availableGifts.Count);
            thisGiftList[i] = availableGifts[r];
            availableGifts.RemoveAt(r);
        }
    }

    List<int> GetAvailableGifts(){
        List<int> l = new List<int>();
        foreach (int key in LoadTxt.GiftDic.Keys)
        {
            if (unLearntTypes.Contains(LoadTxt.GiftDic[key].type))
                continue;
            if (LearntGifts.Contains(key))
                continue;
            int thisFamily = LoadTxt.GiftDic[key].family;
            int thisLevel = LoadTxt.GiftDic[key].level;

            if (thisLevel <= BookList[thisFamily] + 1)
                l.Add(key);
        }

        if (unLearntTypes.Count > 0)
        {
            foreach (int u in unLearntTypes)
            {
                l.Add(u);
            }
        }
        return l;
    }



    //学习天赋********************************************************
    public void Learn(){
        Debug.Log("Learn gift = " + thisGift);

        LearnGift();
        CallOutGiftPanel();
    }



    void LearnGift(){
        LearntGifts.Add(thisGift);
        switch (thisGift)
        { 
            case 110:
            case 120:
            case 130:
                _gameManager.heroAtt += 10;
                _gameManager.UpdateShowProperty("att");
                break;
            case 140:
                GameConfigs.IsPierceDamage = true;
                pDamage.SetActive(true);
                break;
            case 210:
            case 220:
            case 230:
                _gameManager.heroDef += 5;
                _gameManager.UpdateShowProperty("def");
                break;
            case 240:
                GameConfigs.InBattleShield = true;
                break;
            case 1:
                _gameManager.heroHp += 50;
                _gameManager.heroHpMax += 50;
                _gameManager.UpdateShowProperty("hp");
                _gameManager.UpdateShowProperty("hpMax");
                break;
            case 310:
            case 320:
            case 330:
                _gameManager.heroHp += 80;
                _gameManager.heroHpMax += 80;
                _gameManager.UpdateShowProperty("hp");
                _gameManager.UpdateShowProperty("hpMax");
                break;
            case 340:
                GameConfigs.DeadlyAttackShield = true;
                break;
            case 2:
                _gameManager.heroPower += 60;
                _gameManager.UpdateShowProperty("power");
                break;
            case 410:
                _gameManager.heroPower += 100;
                _gameManager.UpdateShowProperty("power");
                break;
            case 420:
                GameConfigs.IsShowBoss = true;
                _gameManager.ShowBoss();
                break;
            case 430:
                _gameManager.heroPower += 200;
                _gameManager.UpdateShowProperty("power");
                break;
            case 440:
                GameConfigs.IsShowMap = true;
                _gameManager.ShowAllMaps();
                break;
            case 421:
                GameConfigs.TreasureNum += 1;
                _gameManager.thisTreasureNum = GameConfigs.TreasureNum;
                break;
            case 441:
                GameConfigs.OpenRoomRecover = true;
                break;
            case 422:
            case 431:
                GameConfigs.CharmRate += 3000;
                break;
            case 510:
                GameConfigs.BotPowerReduce += 1500;
                break;
            case 520:
                GameConfigs.AfterBattleRecover += 3000;
                break;
            case 530:
                GameConfigs.BotRewardItem += 2000;
                break;
            case 610:
                GameConfigs.EscapeRate += 3000;
                break;
            case 620:
                GameConfigs.EscapeAllDirection = true;
                break;
            case 630:
                GameConfigs.EscapeRate += 3000;
                break;
            case 640:
                GameConfigs.EscapeLoss = 0;
                break;
            case 3:
            case 710:
                _gameManager.coin += 10;
                _gameManager.UpdateShowProperty("coin");
                break;
            case 720:
                GameConfigs.BotRewardCoinInc += 6000;
                break;
            case 730:
                GameConfigs.BossRewardCoinInc += 6000;
                break;
            case 740:
                GameConfigs.KillBotByCoin = true;
                break;
            case 721:
                GameConfigs.ShopItemNum = 5;
                break;
            case 731:
                GameConfigs.ShopGiftWeight = 3;
                break;
            case 741:
                GameConfigs.ShopDiscount = 5000;
                break;
            case 810:
            case 820:
                _gameManager.GetRandomNormalItem();
                break;
            case 830:
            case 840:
                _gameManager.GetRandomHighValueItem();
                break;
        }

        //update bookList
        if (unLearntTypes.Contains(LoadTxt.GiftDic[thisGift].type))
            unLearntTypes.Remove(LoadTxt.GiftDic[thisGift].type);
        else
        {
            if (LoadTxt.GiftDic[thisGift].level > BookList[LoadTxt.GiftDic[thisGift].family])
                BookList[LoadTxt.GiftDic[thisGift].family] = LoadTxt.GiftDic[thisGift].level;
        }
    }
}
