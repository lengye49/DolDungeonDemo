public static class GameConfigs{
    public static int[] seeds = new int[5];

    public static int GameLevel = 5;
    public static int[] BotPower = new int[]{ 500, 550, 600,650,700 };
    public static int[] BossPower = new int[]{1500,2500,4000,6500,9000};
    public static int[] BossAttParam = new int[]{ 150, 180, 210,250,300 };
    public static int[] BossDefParam = new int[]{ 50, 50, 50,50,50 };


    public static int BaseNum = 5;
    public static int BotNum = 3;
    public static int EventNum = 3;
    public static int ShopNum = 1;
    public static int RoomNum = 18;

    public static int PlayerHp = 100;
    public static int PlayerAtt = 20;
    public static int PlayerDef = 5;
    public static int PlayerPower = 1000;
    public static int BotCoinReward = 5;
    public static int BribeCost = 1;



    public static int DefParam = 20;

    //Gift
//    public static bool IsPierceDamage = false;//真实攻击                
//    public static bool InBattleShield = false;//入场护盾
//    public static bool DeadlyAttackShield = false;//致死护盾            
    public static int TreasureNum = 1;//奖励房数量                      
//    public static int CharmRate = 0;//魅惑万分比
//    public static int AfterBattleRecover = 0;//小怪战后回血
    public static int BotRewardItem = 2000;//小怪物品奖励掉落万分比
//    public static bool EscapeAllDirection = false;//可否全方向逃跑
//    public static int BotRewardCoinInc = 0;//小怪探索点掉落增加万分比
//    public static int BossRewardCoinInc = 0;//Boss探索点掉落增加万分比 
//    public static int ShopGiftWeight = 1;//商店天赋点权重倍数
//    public static int ShopDiscount = 0;//商店打折折扣

    public static int CritRate = 0;//暴击率
    public static int CritDamage = 15000;//暴击伤害
    public static int DamageReduceRate = 0;//Boss战伤害减半的几率
    public static int HpRecoverRateAfterBoss = 0;//Boss战后恢复
    public static int BotPowerReduce = 0;//小怪实力降低万分比
    public static bool IsShowBoss = false;//显示boss位置
    public static bool IsShowMap = false;//显示全地图
    public static bool OpenRoomRecover = false;//开启房间能否回复生命 
    public static int EscapeRate = 0;//逃跑概率万分比
    public static int EscapeLoss = 500;//逃跑损失生命万分比
    public static int BotDamageReduce = 0;//除boss外受到的伤害降低比率
    public static int ShopItemNum = 3;//商店物品数量
    public static bool HighValueInShop = false;//高级物品增加概率
    public static int RewardCoinIncRate = 0;//探索点奖励加成
    public static bool KillBotByCoin = false;//是否可以用探索点杀怪
    public static int CoinCostReduceRate = 0;//探索点消耗降低比例

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


    public static void ResetGameConfigs(){
        GameConfigs.seeds = new int[5];

        GameConfigs.GameLevel = 5;
        GameConfigs.BotPower = new int[]{ 500, 550, 600, 650, 700 };
        GameConfigs.BossPower = new int[]{ 1500, 2500, 4000, 6500, 9000 };
        GameConfigs.BossAttParam = new int[]{ 150, 180, 210, 250, 300 };
        GameConfigs.BossDefParam = new int[]{ 50, 50, 50, 50, 50 };


        GameConfigs.BaseNum = 5;
        GameConfigs.BotNum = 3;
        GameConfigs.EventNum = 3;
        GameConfigs.ShopNum = 1;
        GameConfigs.RoomNum = 18;

        GameConfigs.PlayerHp = 100;
        GameConfigs.PlayerAtt = 20;
        GameConfigs.PlayerDef = 5;
        GameConfigs.PlayerPower = 1000;
        GameConfigs.BotCoinReward = 5;
        GameConfigs.BribeCost = 1;



        GameConfigs.DefParam = 20;

        GameConfigs.TreasureNum = 1;//奖励房数量                      
        GameConfigs.BotRewardItem = 2000;//小怪物品奖励掉落万分比
        GameConfigs.CritRate = 0;//暴击率
        GameConfigs.CritDamage = 15000;//暴击伤害
        GameConfigs.DamageReduceRate = 0;//Boss战伤害减半的几率
        GameConfigs.HpRecoverRateAfterBoss = 0;//Boss战后恢复
        GameConfigs.BotPowerReduce = 0;//小怪实力降低万分比
        GameConfigs.IsShowBoss = false;//显示boss位置
        GameConfigs.IsShowMap = false;//显示全地图
        GameConfigs.OpenRoomRecover = false;//开启房间能否回复生命 
        GameConfigs.EscapeRate = 0;//逃跑概率万分比
        GameConfigs.EscapeLoss = 500;//逃跑损失生命万分比
        GameConfigs.BotDamageReduce = 0;//除boss外受到的伤害降低比率
        GameConfigs.ShopItemNum = 3;//商店物品数量
        GameConfigs.HighValueInShop = false;//高级物品增加概率
        GameConfigs.RewardCoinIncRate = 0;//探索点奖励加成
        GameConfigs.KillBotByCoin = false;//是否可以用探索点杀怪
        GameConfigs.CoinCostReduceRate = 0;//探索点消耗降低比例

        //Item
        GameConfigs.GiftItemId = 13;//天赋点的物品id
        GameConfigs.NextBotDontLose = false;//下次打怪胜利不消耗血量
        GameConfigs.NextBossAttInc = 0;//下次打boss攻击增强
        GameConfigs.NextBossDefInc = 0;//下次打boss防御增强
        GameConfigs.KillBotDirectly = false;//是否可使用爆裂卷轴  这个地方如果用int的话会更好
        GameConfigs.KillBotItemReward = false;//击杀小怪必定获得宝箱

        //Event
        GameConfigs.RestartAfterDeath = false;//死亡后在祭坛复活
        GameConfigs.AfterBattleItem = false;//杀死怪物获得一个宝箱
        GameConfigs.AfterBattleGift = false;//杀死怪物获得一个天赋点
    }
}
