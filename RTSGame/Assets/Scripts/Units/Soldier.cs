using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Unit {

	// Use this for initialization
	void Start () {
        base.Initialize();
        this.Name = "Soldier";
        this.armor = 20;
        this.movementSpeed = 10;
        this.baseStrength = 5;
        this.hitPoints = 10;
        this.fireRate = 2;
        this.shootingRange = 5;
	}
	
	// Update is called once per frame
	void Update () {
        HandleUpdate();
	}
}
