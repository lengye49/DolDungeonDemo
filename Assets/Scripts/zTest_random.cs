using UnityEngine;
using System.Collections;

public class zTest_random : MonoBehaviour {

	// Use this for initialization
	void Start () {
        string s = "";
        Random.seed = 2;
        for (int i = 0; i < 10; i++)
        {
            s += Random.Range(0, 1000) + ",";
        }
        s = s.Substring(0, s.Length - 1);
        Debug.Log(s);

	}
	

}
