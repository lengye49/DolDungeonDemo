using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Calculations  {
    
    public static float GetBotDamage(int heroPower,int botPower){
        float m = (float)heroPower;
        float b = (float)botPower;
        float r = Mathf.Sqrt(m * m - b * b) / m;
        r = 1 - r;

        return r;
    }

    public static int GetDamage(int att,int def){
        int dam = (int)(att * GameConfigs.DefParam / (GameConfigs.DefParam + def));
        dam = (int)(dam * (Random.Range(0.9f, 1.1f)));
        return dam;
    }

    public static int GetBossHp(int power,int att,int def){
        return (int)(power / (att * (1f + (float)def / GameConfigs.DefParam)));
    }

    public static int[] GetItemIdForShop(int level,int num){

        //获取id组和权重组
        int[] itemIds = new int[num];
        List<int> ids = new List<int>();
        List<int> weights = new List<int>();

        int times = 1;
        foreach (int key in LoadTxt.ItemDic.Keys)
        {
            //如果是天赋点，则根据当前天赋倍率计算权重
            if (key == GameConfigs.GiftItemId)
                times = GameConfigs.ShopGiftWeight;
            else
                times = 1;
            
            ids.Add(key);
            if (level <= 1)
                weights.Add((int)(10000f / (LoadTxt.ItemDic[key].price * LoadTxt.ItemDic[key].price)) * times);
            else if (level <= 2)
                weights.Add((int)(10000f / LoadTxt.ItemDic[key].price) * times);
            else
                weights.Add(10000 * times);
        }

        int r;
        int thisProp;
        int totalWeight;
        for (int i = 0; i < num; i++)
        {
            totalWeight = 0;
            foreach (int n in weights)
            {
                totalWeight += n;
            }
            r = Random.Range(0, totalWeight);

            thisProp = 0;
            for (int j = 0; j < weights.Count; j++)
            {
                thisProp += weights[j];
                if (r < thisProp)
                {
                    itemIds[i] = ids[j];
                    ids.RemoveAt(j);
                    weights.RemoveAt(j);
                    break;;
                }
            }

        }
        return itemIds;
    }

    public static int[] GetBotReward(int level){
        int[] r = new int[2];
        r[0] = GameConfigs.BotCoinReward;
        r[0] += (int)(GameConfigs.BotCoinReward * (GameConfigs.BotRewardCoinInc / 10000f));

        int rr = Random.Range(0, 10000);
        if (rr < GameConfigs.BotRewardItem)
            r[1] = 0;
        else
        {
            rr = Random.Range(0, LoadTxt.ItemDic.Count);
            int index = 0;
            foreach (int key in LoadTxt.ItemDic.Keys)
            {
                if (index == rr)
                {
                    r[1] = key;
                    break;
                }
                index++;
            }
        }

        return r;
    }

    public static int GetRandomReward(bool isHigh){
        List<int> l = new List<int>();
        foreach (int key in LoadTxt.ItemDic.Keys)
        {
            if ((isHigh && (LoadTxt.ItemDic[key].price>=15)) || !isHigh)
            {
                l.Add(key);
            }
        }
        int r = Random.Range(0, l.Count);
        return l[r];
    }

    public static bool GetSimulateResult(int heroHp,int heroAtt,int heroDef,int bossHp,int bossAtt,int BossDef){
        float heroP = heroHp * heroAtt * (1.0f + heroDef / 75f);
        if (GameConfigs.IsPierceDamage)
            heroP *= (1.0f + (float)BossDef / GameConfigs.DefParam);
        float bossP = bossHp * bossAtt * (1.0f + BossDef / 75f);
        return heroP > bossP;
    }
}

