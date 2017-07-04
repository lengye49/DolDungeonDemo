using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BackpackActions : MonoBehaviour {

    public Transform ItemTips;
    public Image tipImage;
    public GiftActions _giftActions;
    public MsgActions _msg;

    private GameObject _grid;
    private ArrayList _gridPool;
    private List<GameObject> gridList;
    private List<int> backpackItemList;
    private int thisIndex;

    private GameManager _gameManager;

	void Start () {
        _grid = Resources.Load("Grid", typeof(GameObject)) as GameObject;
        backpackItemList = new List<int>();
        gridList = new List<GameObject>();
        _gridPool = new ArrayList();
        _gameManager = gameObject.GetComponentInParent<GameManager>();
        HideTips();
	}
	
    public void ResetBackpack(){
        while(gridList.Count > 0)
        {
            GameObject o = gridList[0];
            o.SetActive(false);
            _gridPool.Add(gridList[0]);
            gridList.RemoveAt(0);
            UpdateItemIndex();
        } 

        backpackItemList = new List<int>();
    }

    //添加物品
    public void AddItem(int itemId){
        if (backpackItemList.Count >= 12)
        {
            _msg.CallInMsg("背包已满！");
            return;
        }
        backpackItemList.Add(itemId);
        string itemName = LoadTxt.ItemDic[itemId].name;


        GameObject o;
        if (_gridPool.Count == 0)
        {
            o = Instantiate(_grid) as GameObject;
        }
        else
        {
            o = _gridPool[0] as GameObject;
            _gridPool.RemoveAt(0);
        }
        o.SetActive(true);
        o.transform.SetParent(this.gameObject.transform);
        o.transform.localPosition = Vector3.zero;
        o.transform.localScale = Vector3.one;
        o.name = (backpackItemList.Count - 1).ToString();
        gridList.Add(o);

        o.gameObject.GetComponentInChildren<ItemUi>().UpdateItem(itemName);
    }

    //使用物品
    public void ConsumeItem(){

        UseItem(backpackItemList[thisIndex]);
        backpackItemList.RemoveAt(thisIndex);
        GameObject o = gridList[thisIndex];
        o.SetActive(false);
        _gridPool.Add(gridList[thisIndex]);
        gridList.RemoveAt(thisIndex);
        HideTips();
        UpdateItemIndex();
    }

    void UpdateItemIndex(){
        int i = 0;
        foreach (GameObject g in gridList)
        {
            g.gameObject.name = i.ToString();
            i++;
        }
    }

    //使用物品的效果
    void UseItem(int itemId){
        switch (itemId)
        {
            case 1:
                _gameManager.AddHp((int)(_gameManager.heroHpMax * 0.2f));
                break;
            case 2:
                _gameManager.AddHp((int)(_gameManager.heroHpMax * 0.8f));
                break;
            case 3:
                GameConfigs.NextBotDontLose = true;
                break;
            case 4:
                _gameManager.GoToBoss();
                break;
            case 5:
//                _gameManager.GoToShop();
                _gameManager.heroPower += 20;
                _gameManager.UpdateShowProperty("power");
                break;
            case 6:
                int r = Random.Range(0, 10);
                if (r <= 3)
                {
                    _gameManager.heroHp += 5;
                    _gameManager.heroHpMax += 5;
                }
                else if (r <= 7)
                {
                    _gameManager.heroAtt += 2;
                }
                else
                {
                    _gameManager.heroDef += 1;
                }
                _gameManager.UpdateShowProperty();
//                _gameManager.GoToGift();
                break;
            case 7:
                GameConfigs.NextBossAttInc++;
                _gameManager.UpdateShowProperty("att");
                break;
            case 8:
                GameConfigs.NextBossDefInc++;
                _gameManager.UpdateShowProperty("def");
                break;
            case 9:
                _gameManager.ShowThisMap();
                break;
            case 10:
                GameConfigs.KillBotDirectly = true;
                break;
            case 11:
                _gameManager.ResetThisLevel();
                break;
            case 12:
                GameConfigs.KillBotItemReward = true;
                break;
            case 13:
                _giftActions.CallInGiftPanel();
                break;
            case 14:
                _gameManager.AddHp((int)(_gameManager.heroHpMax * 0.5f));
                break;
            default:
                Debug.Log("Wrong ItemId" + itemId);
                break;
        }
    }



    //物品tips操作
    public void ShowTips(int index){
        thisIndex = index;
        ItemTips.localPosition = Vector3.zero;
        ItemTips.gameObject.SetActive(true);
        SetTipDesc(LoadTxt.ItemDic[backpackItemList[index]]);
    }

    void SetTipDesc(Item it){
        Text t = ItemTips.gameObject.GetComponent<Text>();
        Text[] ts = ItemTips.gameObject.GetComponentsInChildren<Text>();

        t.text = it.desc;
        ts[1].text = it.desc;
        ts[2].text = it.name;
        tipImage.sprite = Resources.Load(it.name, typeof(Sprite)) as Sprite;
    }

    public void HideTips(){
        ItemTips.gameObject.SetActive(false);
    }
        

}
