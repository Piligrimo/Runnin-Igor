using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {
    public GameObject player;
	// Use this for initialization
	void Start () {
		
	}
	
    public void NewGame()
    {
        player.SetActive(true);
        player.GetComponent<Player>().NewGame();

    }
    public void Exit()
    {
        Application.Quit();
    }
	// Update is called once per frame
	void Update () {
		
	}
}
