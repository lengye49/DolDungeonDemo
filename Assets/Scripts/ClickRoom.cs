using UnityEngine;
using System.Collections;

public class ClickRoom : MonoBehaviour {

    private GameManager _gameManager;
	
    public void ClickThisRoom(){
        int index = int.Parse(this.gameObject.name);
        _gameManager = this.gameObject.GetComponentInParent<GameManager>();
        _gameManager.GoToRoom(index);
    }
}
