using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;

public class GiftActions : MonoBehaviour {

    public GameObject pDamage;
    public BackpackActions _bpAction;

    private int thisGift;
    private int[] thisGiftList;
    private List<int> LearntGifts;
    private List<int> UnlearntGifts;
    private List<int> BookList;
    private GameManager _gameManager;
    private Text[] ts;
    private Button[] bs;
    private List<int> unLearntTypes;

    void Start(){
        _gameManager = this.gameObject.GetComponentInParent<GameManager>();
        this.gameObject.transform.localPosition = new Vector3(0, -3000f, 0);
        ResetGift();

        ts = this.gameObject.GetComponentsInChildren<Text>();
        bs = this.gameObject.GetComponentsInChildren<Button>();
    }

    //数据操作********************************************************

    public void ResetGift(){
        thisGift = 0;
        LearntGifts = new List<int>();
        UnlearntGifts = new List<int>();
        ResetUnlearntGifts();
        UpdateBookList();
    }


    /// <summary>
    /// 重置所有未学过的天赋
    /// </summary>
    void ResetUnlearntGifts(){
        foreach (int key in LoadTxt.GiftDic.Keys)
        {
            UnlearntGifts.Add(key);
        }
    }

    /// <summary>
    /// 更新可学习的天赋列表
    /// </summary>
    void UpdateBookList(){
        BookList = new List<int>();
        foreach (int key in UnlearntGifts)
        {
            //如果是初始天赋，直接加进列表
            if (LoadTxt.GiftDic[key].openReq.Contains(0))
            {
                BookList.Add(key);
                continue;
            }
            //不是初始天赋，如果上级天赋已经学了，则加进列表
            foreach (int v in LoadTxt.GiftDic[key].openReq)
            {
                if (LearntGifts.Contains(v))
                {
                    BookList.Add(key);
                    break;
                }
            }
        }
    }
        
    void SetThreeGifts(){

        List<int> availableGifts = BookList;
        int num = Mathf.Min(3, availableGifts.Count);
        thisGiftList = new int[num];
        for (int i = 0; i < thisGiftList.Length; i++)
        {
            int r = Random.Range(0, availableGifts.Count);
            thisGiftList[i] = availableGifts[r];
            availableGifts.RemoveAt(r);
        }
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
            ts[i + 1].color = GetTextColor(LoadTxt.GiftDic[thisGiftList[i]].type);
            bs[i].interactable = true;
        }
        if (thisGiftList.Length < 3)
        {
            for (int i = thisGiftList.Length; i < 3; i++)
            {
                ts[i+1].text = "";
                ts[i + 1].color = Color.white;
                bs[i].interactable = false;
            }
        }
        ChooseGift(0);
    }

    public void ChooseGift(int index){
        if (index > (thisGiftList.Length - 1))
            return;
        ts[4].text = LoadTxt.GiftDic[thisGiftList[index]].desc + "(" + LoadTxt.GiftDic[thisGiftList[index]].value + ")";
        thisGift = thisGiftList[index];

        for (int i = 0; i < thisGiftList.Length; i++)
        {
            if (i == index)
                bs[i].gameObject.GetComponent<Image>().color = Color.yellow;
            else
                bs[i].gameObject.GetComponent<Image>().color = Color.white;
        }

    }


    Color GetTextColor(int type){
        switch(type)
        {
            case 1:
                return new Color(230f / 255f, 13f / 255f, 253f / 255f);
            case 2:
                return new Color(76f / 255f, 152f / 255f, 2f / 255f);
            case 3:
                return new Color(11f / 255f, 233f / 255f, 251f / 255f);
            default:
                return Color.white;

        }
    }



    //学习天赋********************************************************
    public void Learn(){
        Debug.Log("Learn gift = " + thisGift);

        LearnGift();
        CallOutGiftPanel();
    }



    void LearnGift(){

        int value = LoadTxt.GiftDic[thisGift].value;

        switch (thisGift)
        { 
            case 1:
                _gameManager.heroAtt += value;
                _gameManager.UpdateShowProperty("att");
                _gameManager.heroDef += value;
                _gameManager.UpdateShowProperty("def");
                break;
            case 100:
            case 110:
                _gameManager.heroAtt += value;
                _gameManager.UpdateShowProperty("att");
                break;
            case 101:
            case 111:
                _gameManager.heroDef += value;
                _gameManager.UpdateShowProperty("def");
                break;
            case 112:
                _gameManager.heroHp += value;
                _gameManager.UpdateShowProperty("hp");
                _gameManager.heroHpMax += value;
                _gameManager.UpdateShowProperty("hpMax");
                break;
            case 120:
                GameConfigs.CritRate = value;
                break;
            case 121:
                GameConfigs.DamageReduceRate = value;
                break;
            case 130:
                GameConfigs.HpRecoverRateAfterBoss = value;
                break;
            case 2:
            case 200:
            case 210:
                _gameManager.heroPower += value;
                _gameManager.UpdateShowProperty("power");
                break;
            case 220:
                GameConfigs.BotPowerReduce += value;
                break;
            case 201:
                GameConfigs.IsShowBoss = true;
                break;
            case 211:
                GameConfigs.IsShowMap = true;
                break;
            case 221:
                GameConfigs.OpenRoomRecover = true;
                break;
            case 202:
            case 212:
                GameConfigs.EscapeRate += value;
                break;
            case 222:
                GameConfigs.EscapeLoss = 0;
                break;
            case 223:
                GameConfigs.BotDamageReduce += value;
                break;
            case 3:
                _gameManager.coin += value;
                _gameManager.UpdateShowProperty("coin");
                break;
            case 300:
            case 310:
            case 320:
                _bpAction.AddItem(value);
                break;
            case 301:
            case 311:
                GameConfigs.ShopItemNum += value;
                break;
            case 321:
                GameConfigs.HighValueInShop = true;
                break;
            case 302:
            case 312:
                GameConfigs.RewardCoinIncRate += value;
                break;
            case 322:
                GameConfigs.KillBotByCoin = true;
                break;
            case 323:
                GameConfigs.CoinCostReduceRate += value;
                break;
            default:
                Debug.Log("Wrong giftId = " + thisGift);
                break;
        }

        LearntGifts.Add(thisGift);
        UnlearntGifts.Remove(thisGift);
        UpdateBookList();

    }
}
