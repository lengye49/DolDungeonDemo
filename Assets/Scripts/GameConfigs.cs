public static class GameConfigs{
    public static int GameLevel = 5;
    public static int[] BotPower = new int[]{ 400, 500, 550,600,650 };
    public static int[] BossPower = new int[]{25000,40000,60000,90000,130000};
    public static int[] BossAttParam = new int[]{ 800, 900, 1000,1100,1200 };
    public static int[] BossDefParam = new int[]{ 200, 250, 300,350,400 };

    public static int Camp = 0;
    public static int BaseNum = 5;
    public static int BotNum = 2;
    public static int EventNum = 3;
    public static int ShopNum = 1;
    public static int RoomNum = 20;

    public static int PlayerHp = 400;
    public static int PlayerAtt = 50;
    public static int PlayerDef = 20;
    public static int PlayerPower = 1000;
    public static int BotCoinReward = 5;
    public static int BribeCost = 15;



    public static int DefParam = 75;

    //Gift
    public static bool IsPierceDamage = false;//真实攻击                
    public static bool InBattleShield = false;//入场护盾
    public static bool DeadlyAttackShield = false;//致死护盾            
    public static bool IsShowBoss = false;//显示boss位置并降低boss实力
    public static bool IsShowMap = false;//显示全地图
    public static int TreasureNum = 1;//奖励房数量                      
    public static bool OpenRoomRecover = false;//开启房间能否回复生命     
    public static int CharmRate = 0;//魅惑万分比
    public static int BotPowerReduce = 0;//小怪实力降低万分比
    public static int AfterBattleRecover = 0;//小怪战后回血
    public static int BotRewardItem = 3000;//小怪物品奖励掉落万分比
    public static int EscapeRate = 3000;//逃跑概率万分比
    public static bool EscapeAllDirection = false;//可否全方向逃跑
    public static int EscapeLoss = 500;//逃跑损失生命万分比
    public static int BotRewardCoinInc = 0;//小怪探索点掉落增加万分比
    public static int BossRewardCoinInc = 0;//Boss探索点掉落增加万分比    ??
    public static bool KillBotByCoin = false;//是否可以用探索点杀怪
    public static int ShopItemNum = 3;//商店物品数量
    public static int ShopGiftWeight = 1;//商店天赋点权重倍数
    public static int ShopDiscount = 0;//商店打折折扣

    //Item
    public static int GiftItemId = 13;//天赋点的物品id
    public static bool NextBotDontLose = false;//下次打怪胜利不消耗血量
    public static int NextBossAttInc = 0;//下次打boss攻击增强
    public static int NextBossDefInc = 0;//下次打boss防御增强
    public static bool KillBotDirectly = false;//是否可使用爆裂卷轴  这个地方如果用int的话会更好
    public static bool KillBotItemReward = false;//击杀小怪必定获得宝箱

    //Event
    public static bool RestartAfterDeath = false;//死亡后在祭坛复活
    public static bool AfterBattleItem = false;//杀死怪物获得一个宝箱
    public static bool AfterBattleGift = false;//杀死怪物获得一个天赋点
}
