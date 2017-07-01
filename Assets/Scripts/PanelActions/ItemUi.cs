using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemUi : MonoBehaviour {
	
    public void UpdateItem(string str){
        Sprite sp = Resources.Load(str, typeof(Sprite)) as Sprite;

        this.gameObject.GetComponentInChildren<Text>().text = str;
        this.gameObject.GetComponent<Image>().sprite = sp;
    }

    public void OnClickThis(){
        int index = int.Parse(this.gameObject.transform.parent.name);
        gameObject.GetComponentInParent<BackpackActions>().ShowTips(index);
    }
}
