using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class ShopActions : MonoBehaviour {

    public BackpackActions _backpackActions;
    public Image[] bsImage;
    public GiftActions _giftActions;

    private Shop[] shops;
    private Button[] bs;
    private Text[] ts;
    private GameManager _gameManager;
    private int thisShopIndex;
    private ShopItem thisShopItem;
    private int thisPrice;

    void Start(){
        this.gameObject.transform.localPosition = new Vector3(0, -3000f, 0);
        ts = this.gameObject.GetComponentsInChildren<Text>();
        bs = this.gameObject.GetComponentsInChildren<Button>();
        _gameManager = gameObject.GetComponentInParent<GameManager>();
    }


    //界面操作**********************************************
    public void CallInShopPanel(int shopIndex){
        this.gameObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        this.gameObject.transform.localPosition = Vector3.zero;
        this.gameObject.transform.DOBlendableScaleBy(Vector3.one, 0.5f);

        thisShopIndex = shopIndex;
        SetThisShop(shopIndex);
    }


    public void Buy(){
        _gameManager.coin -= thisPrice;
        _gameManager.UpdateShowProperty("coin");
        thisShopItem.isSellOut = true;

        if (thisShopItem.itemId == GameConfigs.GiftItemId)
            _giftActions.CallInGiftPanel();
        else
            _backpackActions.AddItem(thisShopItem.itemId);
        
        SetThisShop(thisShopIndex);
    }

    public void CallOutShopPanel(){
        this.gameObject.transform.DOLocalMoveY(-3000, 0.5f);
    }

    //进入场景时生成所有商店
    public void InitThisLevelShops(int level){
        //生成所有商店
        shops = new Shop[GameConfigs.ShopNum];
        for (int i = 0; i < shops.Length; i++)
        {
            //生成每个商店
            shops[i]=new Shop();
            shops[i].shopItems = new ShopItem[GameConfigs.ShopItemNum];
            int[] ids = Calculations.GetItemIdForShop(level, shops[i].shopItems.Length);
            for (int j = 0; j < shops[i].shopItems.Length; j++)
            {
                shops[i].shopItems[j] = new ShopItem();
                shops[i].shopItems[j].itemId = ids[j];
                shops[i].shopItems[j].isSellOut = false;
            }
        }
    }


    void SetThisShop(int shopIndex){
        for (int i = 0; i < 5; i++)
        {
            if (shops[shopIndex].shopItems.Length <= i)
                bs[i].gameObject.SetActive(false);
            else
            {
                bs[i].gameObject.SetActive(true);
                ts[i + 1].text = LoadTxt.ItemDic[shops[shopIndex].shopItems[i].itemId].name;
                bs[i].interactable = (!shops[shopIndex].shopItems[i].isSellOut);
                bsImage[i].sprite = Resources.Load(LoadTxt.ItemDic[shops[shopIndex].shopItems[i].itemId].name, typeof(Sprite)) as Sprite;
            }
        }
        ChooseDefaultShopItem(shopIndex);
    }

    void ChooseDefaultShopItem(int shopIndex){
        //优先选择可以购买的物品，如果没有，则选择默认物品
        for (int i = 0; i < shops[shopIndex].shopItems.Length; i++)
        {
            if (!shops[shopIndex].shopItems[i].isSellOut)
            {
                ChooseShopItem(shopIndex, i);
                break;
            }
            if (i == shops[shopIndex].shopItems.Length - 1)
                ChooseShopItem(shopIndex, 0);
        }
    }

    void ChooseShopItem(int shopIndex,int itemIndex){

        thisShopItem = shops[shopIndex].shopItems[itemIndex];
        for (int i = 0; i < bs.Length; i++)
        {
            if(i==itemIndex)
                bs[i].gameObject.GetComponent<Image>().color = Color.yellow;
            else
                bs[i].gameObject.GetComponent<Image>().color = Color.white;
        }

        ts[6].text = LoadTxt.ItemDic[thisShopItem.itemId].desc;

        if (shops[shopIndex].shopItems[itemIndex].isSellOut)
        {
            bs[5].interactable = false;
            ts[7].text = "购买";
        }
        else
        {
            int price = LoadTxt.ItemDic[thisShopItem.itemId].price;
            price = (int)((10000f - GameConfigs.ShopDiscount) / 10000f * price);
            if (_gameManager.coin >= price)
                bs[5].interactable = true;
            else
                bs[5].interactable = false;
            ts[7].text = "购买(" + price +")";
            thisPrice = price;
        }
    }

    public void ChooseShopItem(int itemIndex){
        ChooseShopItem(thisShopIndex, itemIndex);
    }
}
