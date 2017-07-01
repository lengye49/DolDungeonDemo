using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class BossActions : MonoBehaviour {

    public Text Head;
    public Text[] Logs;
    public BackpackActions _bpAction;
    public GiftActions _giftActions;
    public Button confirmButton;

    private int bossAtt;
    private int bossDef;
    private int bossHp;

    private int heroShield;
    private int heroAtt;
    private int heroDef;

    private List<string> bossNames;
    private List<string> skillNames;

    private GameManager _gameManager;

    private bool isHeroAtt;
    private string bossName;
    private int thisLevel;
    public bool rebirthOtherPlace;

    void Start(){
        _gameManager = gameObject.GetComponentInParent<GameManager>();
    }

    public void CallInBoss(int level){
        thisLevel = level;
        this.gameObject.transform.localPosition = Vector3.zero;
        this.gameObject.SetActive(true);
        rebirthOtherPlace = false;

        InitBossProperty(level);
        InitHeroProperty();
        InitFuns();
        isHeroAtt = true;

        SetInterface();
        AddLog("遭遇：" + bossName + ",Hp=" + bossHp + ",Att=" + bossAtt + ",Def=" + bossDef + "。");
        AddLog("状态：Hp=" + _gameManager.heroHp + ",Att=" + heroAtt + ",Def=" + heroDef);
        StartFight();
    }


    void InitBossProperty(int roomLevel){
        bossAtt = (int)(GameConfigs.BossAttParam[roomLevel-1] * GameConfigs.PlayerPower / 10000);
        bossDef = (int)(GameConfigs.BossDefParam[roomLevel-1] * GameConfigs.PlayerPower / 10000);
        bossHp = Calculations.GetBossHp(GameConfigs.BossPower[roomLevel-1], bossAtt, bossDef);

        if (GameConfigs.IsShowBoss)
        {
            bossHp = (int)(bossHp * 0.95f);
        }

    }

    void InitHeroProperty(){
        heroAtt = _gameManager.heroAtt;
        heroDef = _gameManager.heroDef;
        if (GameConfigs.InBattleShield)
            heroShield = (int)(heroAtt * 0.5f);

        if (GameConfigs.NextBossAttInc > 0)
        {
            heroAtt = (int)(heroAtt * (1.0f + 0.2f * GameConfigs.NextBossAttInc));
            GameConfigs.NextBossAttInc = 0;
//            _gameManager.UpdateShowProperty("att");
        }

        if (GameConfigs.NextBossDefInc > 0)
        {
            heroDef = (int)(heroDef * (1.0f + 0.2f * GameConfigs.NextBossDefInc));
            GameConfigs.NextBossDefInc = 0;
//            _gameManager.UpdateShowProperty("def");
        }
    }

    void SetInterface(){
        confirmButton.interactable = false;
        int r = Random.Range(0, bossNames.Count);
        bossName = bossNames[r];
        Head.text = bossName;
        for (int i = 0; i < Logs.Length; i++)
            Logs[i].text = "";
    }

    void StartFight(){
        StartCoroutine(Fighting());
    }

    IEnumerator Fighting(){
        yield return new WaitForSeconds(0.6f);

        int dam = 0;
        int r;
        string bossSkill;

        if (isHeroAtt)
        {   
            if (GameConfigs.IsPierceDamage)
                dam = heroAtt;
            else
                dam = Calculations.GetDamage(heroAtt, bossDef);
            bossHp -= dam;
            AddLog("你攻击" + bossName + "，它-" + dam + "Hp，余" + bossHp + "hp。");
            isHeroAtt = false;
        }
        else
        {
            r = Random.Range(0, skillNames.Count);
            bossSkill = skillNames[r];

            dam = Calculations.GetDamage(bossAtt, heroDef);
            AddLog(bossName + "释放[" + bossSkill + "],造成-" + dam + "Hp");

            int damReduce;
            if (heroShield > 0)
            {
                if (dam > heroShield)
                {
                    dam -= heroShield;
                    damReduce = heroShield;
                    heroShield = 0;
                    AddLog("护盾抵挡" + damReduce + "点，护盾破了。");
                }
                else
                {
                    dam = 0;
                    heroShield -= dam;
                    AddLog("护盾抵挡" + dam + "点，护盾剩余"+heroShield+"点。");
                }
               
            }

            if (dam > 0)
            {
                if (_gameManager.heroHp <= dam)
                {
                    AddLog("你挂了。");
                    if (GameConfigs.DeadlyAttackShield)
                    {
                        _gameManager.heroHp = 0;
                        int addHp = (int)(_gameManager.heroHpMax * 0.3f);
                        _gameManager.AddHp(addHp);
                        GameConfigs.DeadlyAttackShield = false;
                        AddLog("你使用免死盾，Hp恢复" + addHp + "。");
                    }
                    else
                    {
                        _gameManager.ReduceHp(dam);
                    }
                }
                else
                {
                    _gameManager.ReduceHp(dam);
                }
            }
            isHeroAtt = true;
        }

        if (_gameManager.heroHp > 0 && bossHp > 0 && (!rebirthOtherPlace))
            StartFight();
        else if (bossHp <= 0)
        {
            GetBossReward();
            _gameManager.KillBoss();
            confirmButton.interactable = true;
        }
        else
        {
            CallOutBoss();
        }
    }

    void GetBossReward(){
        _gameManager.UpdateShowProperty();
        AddLog("你击败了" + bossName + "。天赋点 +1。");

        int coinR = (int)(20 * (1f + GameConfigs.BossRewardCoinInc / 10000f));
        _gameManager.AddCoin(coinR);

        int itemid = Calculations.GetRandomReward(false);
        _bpAction.AddItem(itemid);
        AddLog("探索点 +" + coinR + "。" + LoadTxt.ItemDic[itemid].name + " +1。");
    }

    void CallOutBoss(){
        this.gameObject.SetActive(false);
    }

    public void Confirm(){
        CallOutBoss();
        if (thisLevel != GameConfigs.GameLevel)
            _giftActions.CallInGiftPanel();
    }

    void AddLog(string str){
        for (int i = 0; i < Logs.Length; i++)
        {
            if (i < Logs.Length - 1)
                Logs[i].text = Logs[i + 1].text;
            else
                Logs[i].text = str;
            if (Logs.Length - i-1 == 0)
            {
                Logs[0].text = str;
            }
            else
            {
                Logs[Logs.Length - i - 2].text = Logs[Logs.Length - i - 2].text;
            }
        }
    }

    void InitFuns(){
        bossNames = new List<string>();
        skillNames = new List<string>();

        skillNames.Add("夜泷漩魔劈");
        skillNames.Add("邪风佛舞");
        skillNames.Add("昇龙妖爪");
        skillNames.Add("止水龙手");
        skillNames.Add("苍羽神破");
        skillNames.Add("紫凰圣吟");
        skillNames.Add("天罡鬼指");
        skillNames.Add("白涛神劲");
        skillNames.Add("昭天妖腿");
        skillNames.Add("青炎神枪");
        skillNames.Add("明心圣劲");
        skillNames.Add("飞雪龙杀");
        skillNames.Add("紫霜皇瘴");
        skillNames.Add("玄阳鬼瘴");
        skillNames.Add("螺旋霸斩");
        skillNames.Add("照空皇爪");
        skillNames.Add("狂龙皇刀");
        skillNames.Add("无上圣斩");
        skillNames.Add("夜叉神爪");
        skillNames.Add("月溪佛咒");
        skillNames.Add("森罗皇爆");
        skillNames.Add("重生佛斧");
        skillNames.Add("红炎佛刀");
        skillNames.Add("巫罗魔拳");
        skillNames.Add("血踪神指");
        skillNames.Add("天雷霸掌");

        bossNames.Add("青焰光语鸟");
        bossNames.Add("万丈橘秀龟");
        bossNames.Add("秀睿艳文兽");
        bossNames.Add("掩日冰爭虎");
        bossNames.Add("空无幻乾狼");
        bossNames.Add("破地噬龙蚕");
        bossNames.Add("虹光焱土鳄");
        bossNames.Add("利爪灵魄鸟");
        bossNames.Add("斩月朱岩凤");
        bossNames.Add("哭魂幻锡猫");
        bossNames.Add("暴虎蓝淑驹");
        bossNames.Add("龙爪清羽鹫");
        bossNames.Add("破浪闇桦兔");
        bossNames.Add("梨花绿陨熊");
        bossNames.Add("星罗钢雨蚊");
        bossNames.Add("延阳碧破羊");
        bossNames.Add("金雕铜艳蚊");
        bossNames.Add("紫清毒草蛙");
        bossNames.Add("平纹噬宽猴");
        bossNames.Add("雁落靛女牛");
        bossNames.Add("水月蓝光蝎");
        bossNames.Add("钢翼银逍蝉");
        bossNames.Add("琉光橙琉象");
        bossNames.Add("冰魄朱冽凤");
        bossNames.Add("天泽赤油狼");
        bossNames.Add("飘渺靛鵰豺");
        bossNames.Add("灼阳毒极鹤");
        bossNames.Add("含光梵晶猫");
        bossNames.Add("逆天圣雾蜴");
        bossNames.Add("域霆橘无狐");
        bossNames.Add("炼气素月狮");
        bossNames.Add("血网红兌狐");
        bossNames.Add("极意白波豹");
        bossNames.Add("艮山黑古猿");
        bossNames.Add("问道清烈蜂");
        bossNames.Add("软皮橙瘤犀");
        bossNames.Add("碎玉圣魄熊");
        bossNames.Add("花雨梵光猫");
        bossNames.Add("蝗雨幻花豚");
        bossNames.Add("紫青光蕊莽");
    }
   
}
