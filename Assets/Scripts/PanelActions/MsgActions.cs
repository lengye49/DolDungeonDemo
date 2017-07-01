using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MsgActions : MonoBehaviour {

    public Text Info;

    public void CallInMsg(string str){
        this.gameObject.SetActive(true);
        this.gameObject.transform.localPosition = Vector3.zero;
        Info.text = str;
    }

    public void CallOutMsg(){
        this.gameObject.SetActive(false);
    }
}
