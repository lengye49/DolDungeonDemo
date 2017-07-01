using UnityEngine;
using System.Collections;

public class ReadTxt 
{
    public static string[][] ReadText (string txtName) 
    {
        string[][] textArray;
        TextAsset binAsset = Resources.Load (txtName, typeof(TextAsset)) as TextAsset;
        string[] lineArray = binAsset.text.Split ("\r" [0]);//split the txt by return("/r"[0]);

        textArray = new string[lineArray.Length][];

        for (int i=0; i<lineArray.Length; i++)   {
            textArray[i] = lineArray[i].Split(','); //split the line by ','
        }

        return textArray;

    }

    public static string GetDataByRowAndCol(string[][] textArray, int nRow,int nCol)
    {
        if (textArray.Length <= 0 || nRow >= textArray.Length)
            return "";
        if (nCol >= textArray [0].Length)
            return "";

        return textArray [nRow] [nCol];
    }
        
    public static string[][] GetRequire(string req)
    {
        if (!req.Contains ("|"))
            return null;
        string[] reqs;
        if (req.Contains (";")) {
            reqs = req.Split (';');
        } else {
            reqs = new string[1];
            reqs [0] = req;
        }
        string[][] back = new string[reqs.Length][];
        for (int i=0; i<reqs.Length; i++) {
            back[i] = reqs[i].Split('|');
        }
        return back;
    }
        
}
