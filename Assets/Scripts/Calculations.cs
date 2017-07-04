using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Calculations  {
    
    public static float GetBotDamage(int heroPower,int botPower){
        float m = (float)heroPower;
        float b = (float)botPower;
        float r = Mathf.Sqrt(m * m - b * b) / m;
        r = 1 - r;
        r *= ((10000f - GameConfigs.BotDamageReduce) / 10000f);

        return r;
    }

    public static int GetDamage(int att,int def,int crit){
        int dam = (int)(att * GameConfigs.DefParam / (GameConfigs.DefParam + def));
        dam = (int)(dam * (Random.Range(0.9f, 1.1f)));
        int r = Random.Range(0, 10000);
        if (r < crit)
            dam = dam * GameConfigs.CritDamage / 10000;
        return dam;
    }

    public static int GetBossHp(int power,int att,int def){
        return (int)(power / (att * (1f + (float)def / GameConfigs.DefParam)));
    }

    public static int[] GetItemIdForShop(int num){
        int[] itemIds = GetItemByWeight(num,GameConfigs.HighValueInShop);
        return itemIds;
    }

    public static int[] GetBotReward(int level){
        int[] r = new int[2];
        r[0] = GameConfigs.BotCoinReward;
        r[0] += (int)(GameConfigs.BotCoinReward * (GameConfigs.RewardCoinIncRate / 10000f));

        int rr = Random.Range(0, 10000);
        if (rr < GameConfigs.BotRewardItem)
            r[1] = 0;
        else
        {
            int[] itemIds = GetItemByWeight(1, false);
            r[1] = itemIds[0];
        }

        return r;
    }

    public static int GetRandomReward(bool isHigh){
        int[] l = GetItemByWeight(1,isHigh);
        return l[0];
    }

    static int[] GetItemByWeight(int num, bool highValue){
        List<int> ids = new List<int>();
        List<int> weights = new List<int>();

        float times = 1.0f;
        foreach (int key in LoadTxt.ItemDic.Keys)
        {
            ids.Add(key);
            if (highValue)
            {
                if (LoadTxt.ItemDic[key].price >= 8)
                    times = 3.0f;
            }
            if (key == GameConfigs.GiftItemId)
            {
                times = 0.2f;
            }

            weights.Add((int)(10000f * times / (LoadTxt.ItemDic[key].price * LoadTxt.ItemDic[key].price)));
        }

        int[] itemIds = new int[num];

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

}

