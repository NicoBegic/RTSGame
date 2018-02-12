using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Unit {

	// Use this for initialization
	void Start () {
        GameControler.player.unitStack.Add(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
