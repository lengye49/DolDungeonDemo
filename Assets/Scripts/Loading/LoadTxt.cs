using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadTxt : MonoBehaviour {
    public static Dictionary<int,Gift> GiftDic;
    public static Dictionary<int,Item> ItemDic;
    public static Dictionary<int,DgEvent> DgEventDic;

    private string[][] strs;

    void Awake(){
        GiftDic = new Dictionary<int, Gift>();
        LoadGift();

        ItemDic = new Dictionary<int, Item>();
        LoadItem();

        DgEventDic = new Dictionary<int, DgEvent>();
        LoadDgEvent();
    }

    void LoadGift(){
        strs = ReadTxt.ReadText("gifts");
        for (int i = 0; i < strs.Length-1; i++)
        {
            Gift g = new Gift();
            g.id = int.Parse(ReadTxt.GetDataByRowAndCol(strs, i + 1, 0));
            g.name = ReadTxt.GetDataByRowAndCol(strs, i + 1, 1);
            g.desc = ReadTxt.GetDataByRowAndCol(strs, i + 1, 2);
            g.openReq = new List<int>();
            string s = ReadTxt.GetDataByRowAndCol(strs, i + 1, 3);
            if (s.Contains("|"))
            {
                string[] ss = s.Split('|');
                for (int j = 0; j < ss.Length; j++)
                    g.openReq.Add(int.Parse(ss[j]));
            }
            else
            {
                g.openReq.Add(int.Parse(s));
            }
                
            g.type = int.Parse(ReadTxt.GetDataByRowAndCol(strs, i + 1, 4));
            g.value = int.Parse(ReadTxt.GetDataByRowAndCol(strs, i + 1, 5));
            GiftDic.Add(g.id,g);
        }
    }
	
    void LoadItem(){
        strs = ReadTxt.ReadText("items");
        for (int i = 0; i < strs.Length-1; i++)
        {
            Item it = new Item();
            it.id = int.Parse(ReadTxt.GetDataByRowAndCol(strs, i + 1, 0));
            it.name = ReadTxt.GetDataByRowAndCol(strs, i + 1, 1);
            it.price = int.Parse(ReadTxt.GetDataByRowAndCol(strs, i + 1, 2));
            it.desc = ReadTxt.GetDataByRowAndCol(strs, i + 1, 4);
//            Debug.Log(it.name);
            ItemDic.Add(it.id, it);
        }
    }

    void LoadDgEvent(){
        strs = ReadTxt.ReadText("events");
        for (int i = 0; i < strs.Length-1; i++)
        {
            DgEvent d = new DgEvent();
            d.id = int.Parse(ReadTxt.GetDataByRowAndCol(strs, i + 1, 0));
            d.name = ReadTxt.GetDataByRowAndCol(strs, i + 1, 1);
            d.desc = ReadTxt.GetDataByRowAndCol(strs, i + 1, 2);
            d.opt1 = ReadTxt.GetDataByRowAndCol(strs, i + 1, 3);
            d.opt2 = ReadTxt.GetDataByRowAndCol(strs, i + 1, 4);
            d.opt3 = ReadTxt.GetDataByRowAndCol(strs, i + 1, 5);
            d.opt1Desc = ReadTxt.GetDataByRowAndCol(strs, i + 1, 6);
            d.opt2Desc = ReadTxt.GetDataByRowAndCol(strs, i + 1, 7);
            d.opt3Desc = ReadTxt.GetDataByRowAndCol(strs, i + 1, 8);
//            Debug.Log(d.name);
            DgEventDic.Add(d.id, d);
        }
    }
}
