using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class GameManager : MonoBehaviour {

    public Sprite[] CampBgs;
    public AudioClip[] CampAudios;
    public Image GameBg;
    public AudioSource GameMusic;
    public GameObject RoomPrefab;
    public GameObject DirectionPrefab;
    public Transform RoomContainer;
    public Transform DirectionContainer;
    public Loading _loading;
    public Transform player;
    public Text bossInfo;
    public Text LevelInfo;

    //属性text
    public Text coinText;
    public Text giftPointText;
    public Text hpText;
    public Text hpMaxText;
    public Text attText;
    public Text defText;
    public Text powerText;

    public ShopActions _shopActions;
    public GiftActions _giftActions;
    public EventActions _eventActions;
    public BotActions _botActions;
    public BackpackActions _backpackActions;
    public MsgActions _msgActions;
    public BossActions _bossActions;

    //房间基础数据
    private int thisBaseNum;
    private int thisBotNum;
    private int thisEventNum;
    private int thisRoomNum;
    private int thisShopNum;
    [HideInInspector]
    public int thisTreasureNum;

    private int[] C;
    private int[] H;
    private Room[] Rooms;
    private List<int> allPoints;
    private List<int> allLinks;

    private ArrayList RoomPool;
    private ArrayList DirectionPool;

    private int BossPoint;
    private int InPoint;
    private int GPointPoint;//天赋点位置
    private List<int> BotPoints;
    private List<int> EventPoints;
    private List<int> ShopPoints;
    private List<int> TreasurePoints;
    private int OrgPoint;
    private int RestartPoint;

    //角色属性
    public int heroHp;
    public int heroHpMax;
    public int heroAtt;
    public int heroDef;
    public int heroPower;

    //怪物实力
    private int botPower;

    //房间层数
    private int roomLevel;

    //代币
    [HideInInspector]
    public int giftPoint;
    [HideInInspector]
    public int coin;



    void Start(){
        RoomPool = new ArrayList();
        DirectionPool = new ArrayList();
        player.localPosition = Vector3.zero;
    }

    void Update(){

    }

    public void StartGame(){

        thisBaseNum = GameConfigs.BaseNum;
        thisBotNum = GameConfigs.BotNum;
        thisEventNum = GameConfigs.EventNum;
        thisShopNum = GameConfigs.ShopNum;
        thisRoomNum = GameConfigs.RoomNum;
        thisTreasureNum = GameConfigs.TreasureNum;

        //设定背景和音效
        int camp = Random.Range(0,4);
        GameBg.sprite = CampBgs[camp];
        GameMusic.clip = CampAudios[camp];
        GameMusic.loop = true;
        GameMusic.Play();

        heroHp = GameConfigs.PlayerHp;
        heroHpMax = heroHp;
        heroAtt = GameConfigs.PlayerAtt;
        heroDef = GameConfigs.PlayerDef;
        heroPower = GameConfigs.PlayerPower;
        roomLevel = 1;

        giftPoint = 0;
        coin = 0;

        UpdateShowProperty();
        UpdateLevelInfo();

        //设置房间
        SetRoomBase();
        _giftActions.ResetGift();
        _backpackActions.ResetBackpack();
    }

    void SetRoomBase(){

        //读取随机数
        int seed = GameConfigs.seeds[roomLevel-1];
        Random.seed = seed;

        Rooms = new Room[thisBaseNum * thisBaseNum];
        for (int i = 0; i < Rooms.Length; i++)
        {
            Rooms[i] = new Room();
            Rooms[i].RoomType = 0;
        }

        botPower = GameConfigs.BotPower[roomLevel - 1];

        //重置房间池
        ResetPools();

        int t = thisBaseNum * thisBaseNum;//格子总数
        int a = (int)(1000 / thisBaseNum);//格子边长

        //1初始化集合
        C = new int[thisRoomNum];
        H = new int[thisRoomNum - 1];
        int pickedPoint = 0;

        //取随机初始点
        int BasePoint = Random.Range(0,t);
        C[0] = BasePoint;
        OrgPoint = BasePoint;
        pickedPoint++;

        //初始化Boss坐标集
        List<int> b = new List<int>();

        do
        {
            //获得C中其它点
            for (int i = 1; i < thisRoomNum; i++)
            {
                int s;
                List<int> neighbours = new List<int>();
                do
                {
                    //获得随机点S的序号
                    s = Random.Range(0, pickedPoint);

                    //找到s点的临近点(排除已存在的点)
                    neighbours = GetNeighbour(C[s]);
                    for (int j = 0; j < pickedPoint; j++)
                    {
                        if (neighbours.Contains(C[j]))
                            neighbours.Remove(C[j]);
                    }


                } while (neighbours.Count == 0);
                    
                //随机出1个作为下个点,并存入点集和关系集
                int nextPointIndex = Random.Range(0, neighbours.Count);
                C[pickedPoint] = neighbours[nextPointIndex];
                H[pickedPoint - 1] = C[s] * 100 + C[pickedPoint];

                pickedPoint++;
            }

            allPoints = new List<int>();
            for (int i = 0; i < C.Length; i++)
                allPoints.Add(C[i]);

            allLinks = new List<int>();
            for (int i = 0; i < H.Length; i++)
                allLinks.Add(H[i]);

            b = GetBossPoints();

        } while(b.Count == 0);


        //设置房间内容
        SetAllPoints(b);

        //显示房间
        for (int i = 0; i < thisBaseNum * thisBaseNum; i++)
        {
            GameObject o;
            if (i < RoomPool.Count)
            {
                o = RoomPool[i] as GameObject;
            }
            else
            {
                o = Instantiate(RoomPrefab) as GameObject;
                o.transform.SetParent(RoomContainer);
                RoomPool.Add(o);
            }
            o.SetActive(true);
            o.name = i.ToString();
            o.transform.localPosition = GetRoomPos(i);
            o.GetComponent<RectTransform>().sizeDelta = new Vector2(a, a);

            SetRoomType(i);
        }

        //显示路径
        for(int i=0;i<H.Length;i++){
            GameObject o;
            if (i < DirectionPool.Count)
            {
                o = DirectionPool[i] as GameObject;
            }
            else
            {
                o = Instantiate(DirectionPrefab) as GameObject;
                o.transform.SetParent(DirectionContainer);
                DirectionPool.Add(o);
            }
            o.SetActive(false);
            o.name = H[i].ToString();
//            Debug.Log("H" + i + " = " + H[i]);
            o.transform.localPosition = GetDirectionPos(H[i]);
            o.GetComponent<RectTransform>().sizeDelta = new Vector2(100f * 5f / thisBaseNum, 30f * 5f / thisBaseNum);
            o.transform.localRotation = Quaternion.Euler(0,0,GetAngleAxis(H[i]));
        }
        //添加随机路径
        List<int> BreakLinks = GetBreakLinks();
        int newLinksCount = 0;
        if (BreakLinks.Count > 0)
        {
            foreach(int bl in BreakLinks){
                int r = Random.Range(0, 10);
                if (r <= 4 && bl!=0)
                {
                    allLinks.Add(bl);
//                    Debug.Log("bl = " + bl);
                    GameObject o;
                    if (H.Length + newLinksCount < DirectionPool.Count)
                    {
                        o = DirectionPool[H.Length + newLinksCount] as GameObject;
                    }
                    else
                    {
                        o = Instantiate(DirectionPrefab) as GameObject;
                        o.transform.SetParent(DirectionContainer);
                        DirectionPool.Add(o);
                    }
                    o.SetActive(false);
                    o.name = bl.ToString();
                    o.transform.localPosition = GetDirectionPos(bl);
                    o.GetComponent<RectTransform>().sizeDelta = new Vector2(100f * 5f / thisBaseNum, 30f * 5f / thisBaseNum);
                    o.transform.localRotation = Quaternion.Euler(0,0,GetAngleAxis(bl));
                    o.GetComponent<Image>().color = Color.red;
                    newLinksCount++;
                }
            }
        }
        //更新房间显示
        for (int i = 0; i < Rooms.Length; i++)
            ShowRoomInterface(i);

        if (GameConfigs.IsShowBoss)
            ShowBoss();
        if (GameConfigs.IsShowMap)
            ShowAllMaps();

        _shopActions.InitThisLevelShops(roomLevel);
        _eventActions.InitThisLevelEvent();

        StandingRoom(InPoint);

    }

    List<int> GetNeighbour(int org){
        if (org == 0)
            return new List<int>{ 1, thisBaseNum };
        if (org == (thisBaseNum * thisBaseNum - 1))
            return new List<int>{ org - 1, org - thisBaseNum };
        if (org == thisBaseNum - 1)
            return new List<int>{ org - 1, org + thisBaseNum };
        if (org == (thisBaseNum * thisBaseNum - thisBaseNum))
            return new List<int>{ org + 1, org - thisBaseNum };
        if (org < thisBaseNum)
            return new List<int>{ org - 1, org + 1, org + thisBaseNum };
        if (org > (thisBaseNum * thisBaseNum - thisBaseNum))
            return new List<int>{ org - 1, org + 1, org - thisBaseNum };
        if (org % thisBaseNum == 0)
            return new List<int>{ org - thisBaseNum, org + thisBaseNum, org + 1 };
        if (org % thisBaseNum == (thisBaseNum - 1))
            return new List<int>{ org - thisBaseNum, org + thisBaseNum, org - 1 };
        return new List<int>{ org - thisBaseNum, org + thisBaseNum, org - 1, org + 1 };
    }

    Vector3 GetRoomPos(int index){
        int a = (int)(1000 / thisBaseNum);//格子边长
        int row = (int)(index / thisBaseNum) + 1;
        int column = index % thisBaseNum + 1;
        float x = (-(thisBaseNum-1) / 2f) * a + (column - 1) * a;
        float y = ((thisBaseNum-1) / 2f) * a - (row - 1) * a;

        return new Vector3(x, y, 0f);
    }

    Vector3 GetDirectionPos(int r){
        int r1 = (int)(r / 100);
        int r2 = r % 100;
        float x = (GetRoomPos(r1).x + GetRoomPos(r2).x) / 2;
        float y = (GetRoomPos(r1).y + GetRoomPos(r2).y) / 2;
        return new Vector3(x, y, 0f);
    }

    float GetAngleAxis(int r){
        int r1 = (int)(r / 100);
        int r2 = r % 100;
        if (r1 == r2 + thisBaseNum)
            return 90f;
        if (r1 == r2 - thisBaseNum)
            return -90f;
        return 0f;
    }

    void ResetPools(){
        foreach(Object o in RoomPool){
            GameObject o1 = o as GameObject;
            o1.SetActive(false);
        }
        foreach(Object o in DirectionPool){
            GameObject o1 = o as GameObject;
            o1.SetActive(false);
        }
    }

    List<int> GetBossPoints(){
        List<int> r1 = new List<int>();
        List<int> r2 = new List<int>();
        List<int> b = new List<int>();
        foreach (int al in allLinks)
        {
            r1.Add((int)(al / 100));
            r2.Add(al % 100);
        }

        for (int i = 0; i < thisBaseNum * thisBaseNum; i++)
        {
            if (r1.Contains(i) && (!r2.Contains(i)) && (i!=OrgPoint))
                b.Add(i);
            if ((!r1.Contains(i)) && r2.Contains(i) && (i!=OrgPoint))
                b.Add(i);
        }
        return b;
    }

    void SetAllPoints(List<int> b){

        List<int> thisPoints = new List<int>();
        foreach (int al in allPoints)
            thisPoints.Add(al);

        int index = Random.Range(0, b.Count);
        BossPoint = b[index];
        thisPoints.Remove(BossPoint);

        index = Random.Range(0, thisPoints.Count);
        InPoint = thisPoints[index];
        thisPoints.RemoveAt(index);

        index = Random.Range(0, thisPoints.Count);
        GPointPoint = thisPoints[index];
        thisPoints.RemoveAt(index);

        TreasurePoints = new List<int>();
        for (int i = 0; i < thisTreasureNum; i++)
        {
            index = Random.Range(0, thisPoints.Count);
            TreasurePoints.Add(thisPoints[index]);
            thisPoints.RemoveAt(index);
        }

        ShopPoints = new List<int>();
        for (int i = 0; i < thisShopNum; i++)
        {
            index = Random.Range(0, thisPoints.Count);
            ShopPoints.Add(thisPoints[index]);
            thisPoints.RemoveAt(index);
        }

        BotPoints = new List<int>();
        for (int i = 0; i < thisBotNum; i++)
        {
            index = Random.Range(0, thisPoints.Count);
            BotPoints.Add(thisPoints[index]);
            thisPoints.RemoveAt(index);
        }

        EventPoints = new List<int>();
        for (int i = 0; i < thisEventNum; i++)
        {
            index = Random.Range(0, thisPoints.Count);
            EventPoints.Add(thisPoints[index]);
            thisPoints.RemoveAt(index);
        }


    }

    void SetRoomType(int i){
        if (i == BossPoint)
        {
            Rooms[i].RoomType = 1;
            Rooms[i].RoomEventType = 1;
        }
        else if (i == InPoint)
        {
            Rooms[i].RoomType = 2;
            Rooms[i].RoomEventType = 7;
        }
        else if (i == GPointPoint)
        {
            Rooms[i].RoomType = 1;
            Rooms[i].RoomEventType = 9;
        }
        else if (BotPoints.Contains(i))
        {
            Rooms[i].RoomType = 1;
            Rooms[i].RoomEventType = 2;
        }
        else if (TreasurePoints.Contains(i))
        {
            Rooms[i].RoomType = 1;
            Rooms[i].RoomEventType = 6;
        }
        else if (EventPoints.Contains(i))
        {
            Rooms[i].RoomType = 1;
            Rooms[i].RoomEventType = 4;
        }else if(ShopPoints.Contains(i)){
            Rooms[i].RoomType = 1;
            Rooms[i].RoomEventType = 5;
        }
        else if (allPoints.Contains(i))
        {
            Rooms[i].RoomType = 1;
            Rooms[i].RoomEventType = 3;
        }
        else
        {
            Rooms[i].RoomType = 0;
            Rooms[i].RoomEventType = 0;
        }
    }


    public void ReturnToLoading(){
        GameMusic.Stop();
        _loading.GetBackLoading();
    }

    /// <summary>
    /// 获得所有相邻但不相的“断链”
    /// </summary>
    /// <returns>The break links.</returns>
    List<int> GetBreakLinks(){
        List<int> BreakLinks = new List<int>();

        for (int i = 0; i < thisBaseNum * thisBaseNum; i++)
        {
            if (allPoints.Contains(i) && i!=BossPoint)
            {
                List<int> thisNeighbours = GetNeighbour(i);
                foreach (int p in thisNeighbours)
                {
                    if (allPoints.Contains(p) && p!=BossPoint)
                    {
                        if ((!allLinks.Contains(i * 100 + p)) && (!allLinks.Contains(p * 100 + i)) && (!BreakLinks.Contains(i * 100 + p)) && (!BreakLinks.Contains(p * 100 + i)))
                        {
                            BreakLinks.Add(i * 100 + p);
//                            print("BreakLinks" + (i * 100 + p));
                        }
                    }
                }
            }
        }
        return BreakLinks;
    }

    //房间显示相关***********************************

    /// <summary>
    /// 处理当前站立房间和临位房间的显示状态
    /// </summary>
    /// <param name="index">Index.</param>
    void StandingRoom(int index){
        player.localPosition = GetRoomPos(index);
        Debug.Log("Standing in Room " + index);
        //如果不是房间点，直接退出
        if (!allPoints.Contains(index) )
            return;

        //如果不是迷雾房间，直接退出。直达类的功能，先要设置当前房间为迷雾房间，再进入。
        if (Rooms[index].RoomType != 2)
            return;

        //处理当前房间
        Rooms[index].RoomType = 3;
        Debug.Log("Change Room " + index + " roomType to 3");
        ShowRoomInterface(index);

        //开启房间恢复生命
        if (GameConfigs.OpenRoomRecover)
        {
            int aHp = (int)(heroHpMax * 0.02f);
            AddHp(aHp);
        }

//        Debug.Log("this room = " + index);
        //处理临位房间
        List<int> thisNeighbour = GetNeighbour(index);
        foreach (int i in thisNeighbour)
        {
            Debug.Log("Check neighbour = " + i);
            if (allPoints.Contains(i))
            {
                if (Rooms[i].RoomType == 1)
                {
                    if (allLinks.Contains(index * 100 + i) || allLinks.Contains(i * 100 + index))
                    {
                        Rooms[i].RoomType = 2;
                        Debug.Log("Change room " + i + " roomType to 2");
                    }
                }
                if (Rooms[i].RoomType == 2)
                {
                    int linkId = 0;
                    foreach (int al in allLinks)
                    {
                        if ((al == index * 100 + i) || (al == i * 100 + index))
                        {
                            GameObject o = DirectionPool[linkId] as GameObject;
                            o.SetActive(true);
                            ShowRoomInterface(i);
                            break;
                        }
                        linkId++;
                    }
                }
            }
        }
    }

    void ShowRoomInterface(int index){
        GameObject o = RoomPool[index] as GameObject;

        Image i = o.GetComponent<Image>();
        i.sprite = GetRoomSprite(Rooms[index].RoomType, Rooms[index].RoomEventType);

        Button b = o.GetComponent<Button>();
        b.interactable = (Rooms[index].RoomType>0);
    }

    Sprite GetRoomSprite(int roomType,int eventType){
        string spriteName = "";
        if (roomType == 0 || roomType == 1)
            spriteName = "迷雾";
        else if (roomType == 3)
        {
            switch (eventType)
            {
                case 0:
                    spriteName = "格子_已探索";
                    break;
                case 1:
                    spriteName = "Boss";
                    break;
                case 2:
                    spriteName = "小怪";
                    break;
                case 4:
                    spriteName = "事件";
                    break;
                case 5:
                    spriteName = "商店";
                    break;
                case 6:
                    spriteName = "奖励";
                    break;
                case 8:
                    spriteName = "地图";//用于出口
                    break;
                case 9:
                    spriteName = "天赋点";
                    break;
                default:
                    spriteName = "格子_已探索";
                    break;
            }
        }
        else if (roomType == 2)
        {
            switch (eventType)
            {
                case 0:
                    spriteName = "格子_未探索";
                    break;
                case 1:
                    spriteName = "Boss_d";
                    break;
                case 2:
                    spriteName = "小怪_d";
                    break;
                case 3:
                    spriteName = "格子_未探索";
                    break;
                case 4:
                    spriteName = "事件";
                    break;
                case 5:
                    spriteName = "商店_d";
                    break;
                case 6:
                    spriteName = "奖励";
                    break;
                default:
                    spriteName = "格子_未探索";
                    break;
            }
        }
        else
        {
            spriteName = "迷雾";
        }
        Sprite sp = Resources.Load(spriteName,typeof(Sprite)) as Sprite;
        return sp;
    }


    //进入探索阶段***********************************

    /// <summary>
    /// //走进某个房间
    /// </summary>
    /// <param name="index">Index.</param>
    public void GoToRoom(int index){
        Debug.Log("Going to " + index + " with roomType = " + Rooms[index].RoomType);
        if (Rooms[index].RoomType < 2 )
            return;
        
        OrgPoint = index;
        //1.更新本房间和临位房间的显示。
        StandingRoom(index);

        //2.处理事件
        switch (Rooms[index].RoomEventType)
        {
            case 1:
                _bossActions.CallInBoss(roomLevel);
                break;
            case 2:
                _botActions.CallInBotMsg(botPower);
                break;
            case 3:
                break;
            case 4:
                int i = 0;
                foreach (int key in EventPoints)
                {
                    if (key == index)
                    {
                        _eventActions.CallInEventPanel(i);
                        break;
                    }
                    i++;
                }
                break;
            case 5:
                int j = 0;
                foreach (int key in ShopPoints)
                {
                    if (key == index)
                    {
                        _shopActions.CallInShopPanel(j);
                        break;
                    }
                    j++;
                }
                break;
            case 6:
                GetRandomNormalItem();
                ClearRoom();
                break;
            case 8:
                NextLevel();
                break;
            case 9:
                _giftActions.CallInGiftPanel();
                ClearRoom();
                break;
            default:
                break;
        }



    }


    public void ClearRoom(){
        Rooms[OrgPoint].RoomType = 3;
        Rooms[OrgPoint].RoomEventType = 0;
        ShowRoomInterface(OrgPoint);
    }

    public void KillBotReward(){
        

        int[] r = Calculations.GetBotReward(roomLevel);
        string str = "探索点 +" + r[0] + "\n";
        coin += r[0];
        UpdateShowProperty("coin");

        if (GameConfigs.AfterBattleGift)
        {
            _giftActions.CallInGiftPanel();
            GameConfigs.AfterBattleGift = false;
            return;
        }
        if (GameConfigs.AfterBattleItem)
        {
            GetRandomNormalItem();
            GameConfigs.AfterBattleItem = false;
            return;
        }

        if (r[1] != 0)
        {
            if (r[1] == GameConfigs.GiftItemId)
            {
                _giftActions.CallInGiftPanel();
                return;
            }
            
            _backpackActions.AddItem(r[1]);
            str += LoadTxt.ItemDic[r[1]].name + " +1";
        }
        _msgActions.CallInMsg(str);


    }

    public void NextLevel(){
        if (roomLevel >= GameConfigs.GameLevel)
        {
            _msgActions.CallInMsg("恭喜你通关了！！");
            return;
        }

        GameConfigs.RestartAfterDeath = false;
        roomLevel++;
        UpdateLevelInfo();
        SetRoomBase();
        UpdateShowProperty();
    }


    //战斗附属面板***********************************

//    void MessagePanel(){
////        Button b = new Button();
////
////        b.onClick.RemoveAllListeners();
////        b.onClick.AddListener(delegate()
////            {
////                this.OnClickMsgBtn(b.gameObject);
////            });
//    }
//
//    public void OnClickMsgBtn(GameObject sender){
//        switch (sender.name)
//        {
//            default:
//                Debug.Log("Wrong sender");
//                break;
//        }
//    }



   //属性面板***********************************

    public void UpdateShowProperty(string str){
        switch (str)
        {
            case "coin":
                coinText.text = coin.ToString();
                break;
            case "giftPoint":
                giftPointText.text = giftPoint.ToString();
                break;
            case "hp":
                hpText.text = heroHp.ToString();
                SetHpColor();
                break;
            case "hpMax":
                hpMaxText.text = "/" + heroHpMax.ToString();
                SetHpColor();
                break;
            case "att":
                
                if (GameConfigs.NextBossAttInc > 0)
                {
                    attText.text = heroAtt + " + " + 5 * GameConfigs.NextBossAttInc;
                }else
                    attText.text = heroAtt.ToString();
                break;
            case "def":
                if (GameConfigs.NextBossDefInc > 0)
                {
                    defText.text = heroDef + " + " + 2 * GameConfigs.NextBossDefInc;
                }
                else
                    defText.text = heroDef.ToString();
                break;
            case "power":
                powerText.text = heroPower.ToString();
                break;
            default:
                Debug.Log("Wrong property name!");
                break;
        }
        SetBossSimulate();
    }

    public void UpdateShowProperty(){
        coinText.text = coin.ToString();
        giftPointText.text = giftPoint.ToString();
        hpText.text = heroHp.ToString();
        hpMaxText.text = "/" + heroHpMax.ToString();
        SetHpColor();

        if (GameConfigs.NextBossAttInc > 0)
        {
            attText.text = heroAtt + " + " + 5 * GameConfigs.NextBossAttInc;
        }else
            attText.text = heroAtt.ToString();
        
        if (GameConfigs.NextBossDefInc > 0)
        {
            defText.text = heroDef + " + " + 2 * GameConfigs.NextBossDefInc;
        }
        else
            defText.text = heroDef.ToString();
        powerText.text = heroPower.ToString();
        SetBossSimulate();
    }

    void UpdateLevelInfo(){
        LevelInfo.text = "Level : " + roomLevel + "/" + GameConfigs.GameLevel;
    }

    void SetBossSimulate(){
        string str = "";
        int bossAtt = GetBossAtt();
        int bossDef = GetBossDef();
        int bossHp = GetBossHp(bossAtt, bossDef);
        str += "Boss 血" + bossHp + ",攻" + bossAtt + ",防" + bossDef;
//        int hHp = heroHp;
//        int hAtt = heroAtt;
//        if (GameConfigs.NextBossAttInc > 0)
//        {
//            hAtt = hAtt + 5 * GameConfigs.NextBossAttInc;
//        }
//        int hDef = heroDef;
//        if (GameConfigs.NextBossDefInc > 0)
//        {
//            hDef = hDef + 2 * GameConfigs.NextBossDefInc;
//        }

        bossInfo.text = str;

    }

    int GetBossAtt(){
        return (int)(GameConfigs.BossAttParam[roomLevel-1] * GameConfigs.PlayerPower / 10000);
    }
    int GetBossDef(){
        return (int)(GameConfigs.BossDefParam[roomLevel-1] * GameConfigs.PlayerPower / 10000);
    }
    int GetBossHp(int att,int def){
        int bossHp = Calculations.GetBossHp(GameConfigs.BossPower[roomLevel - 1], att, def);
//        if (GameConfigs.IsShowBoss)
//        {
//            bossHp = (int)(bossHp * 0.95f);
//        }
        return bossHp;
    }

    void SetHpColor(){
        float f = 1.0f * heroHp / heroHpMax;
        if (f >= 0.5f)
            hpText.color = Color.green;
        else if (f >= 0.2f)
            hpText.color = Color.yellow;
        else
            hpText.color = Color.red;
    }

    public void AddHp(int value){
        heroHp += value;
        if (heroHp > heroHpMax)
            heroHp = heroHpMax;
        UpdateShowProperty("hp");
    }

    public void ReduceHp(int value){
        heroHp -= value;
        if (heroHp <= 0)
        {
            if (GameConfigs.RestartAfterDeath)
            {
                heroHp = (int)(0.5f * heroHpMax);
                _msgActions.CallInMsg("你挂了，但你的灵魂在祭坛重生。");
                _bossActions.rebirthOtherPlace = true;
                GameConfigs.RestartAfterDeath = false;
                GoToRoom(RestartPoint);
            }
            else
            {
                heroHp = 0;
                _msgActions.CallInMsg("你挂了，请自觉重新开始游戏！\n退出游戏，重新进入。");
            }
        }
        UpdateShowProperty("hp");

        //判断死亡
    }

    public void AddCoin(int value){
        coin += value;
        UpdateShowProperty("coin");
    }

    public void ReduceCoin(int value){
        coin -= value;
        UpdateShowProperty("coin");
    }

    //天赋活动***********************************

    public void ShowBoss(){
        GameObject o = RoomPool[BossPoint] as GameObject;

        Image i = o.GetComponent<Image>();
        i.sprite = GetRoomSprite(2, 1);
    }

    public void ShowAllMaps(){
        for (int i = 0; i < thisBaseNum * thisBaseNum; i++)
        {
            if (allPoints.Contains(i) && Rooms[i].RoomType == 1)
            {
                GameObject o = RoomPool[i] as GameObject;

                Image im = o.GetComponent<Image>();
                im.sprite = GetRoomSprite(2, Rooms[i].RoomEventType);
            }
        }
    }

    public void GetRandomNormalItem(){
        int r = Calculations.GetRandomReward(false);
        if (r == GameConfigs.GiftItemId)
        {
            _giftActions.CallInGiftPanel();
            return;
        }
        _backpackActions.AddItem(r);
        _msgActions.CallInMsg(LoadTxt.ItemDic[r].name + " +1");

    }

    public void GetRandomHighValueItem(){
        int r = Calculations.GetRandomReward(true);
        if (r == GameConfigs.GiftItemId)
        {
            _giftActions.CallInGiftPanel();
            return;
        }
        _backpackActions.AddItem(r);
        _msgActions.CallInMsg(LoadTxt.ItemDic[r].name + " +1");
    }

    //商店活动***********************************
    public void GoToBoss(){
        //先设置本房间为迷雾房间，再进入房间，下同。
        Rooms[BossPoint].RoomType=2;
        GoToRoom(BossPoint);
    }

    public void GoToShop(){
        Rooms[ShopPoints[0]].RoomType = 2;
        GoToRoom(ShopPoints[0]);
    }

    public void GoToGift(){
        Rooms[GPointPoint].RoomType = 2;
        GoToRoom(GPointPoint);
    }

    public void ShowThisMap(){
        ShowAllMaps();
    }

    public void ResetThisLevel(){
        SetRoomBase();
    }

    public void Escape(){
        List<int> l = GetNeighbour(OrgPoint);
        foreach (int i in l)
        {
            if (Rooms[i].RoomType != 2)
                l.Remove(i);

            if ((!allLinks.Contains(OrgPoint * 100 + i)) && (!allLinks.Contains(OrgPoint + i * 100)))
                l.Remove(i);
        }

        if (l.Count == 0)
        {
            Debug.Log("找不到逃跑的路，好忧伤！");
            return;
        }
            
        //删掉进入点
        if (l.Count > 1 && l.Contains(thisRoomNum))
            l.Remove(thisRoomNum);    

        int r = Random.Range(0, l.Count + 1);
        if (r >= l.Count)
            return;
        GoToRoom(l[r]);
    }

    //事件活动
    public void SetRestartPoint(){
        RestartPoint = OrgPoint;
    }

    //结束boss
    public void KillBoss(){
        Rooms[OrgPoint].RoomEventType = 8;
        ShowRoomInterface(OrgPoint);
    }
}
