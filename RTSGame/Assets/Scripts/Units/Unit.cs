using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Unit : MonoBehaviour{

    public Image HealthBar;
    public int health;

    //SyncVar
    private string _name;
    //SyncVar
    private int _hitPoints;
    private int armor;

    public string Name {
        get { return _name; }
        set { _name = value; }
    }

    public int HitPoints {
        get { return _hitPoints; }
        set { _hitPoints = value; }
    }

	// Use this for initialization
	void Start () {
        //Wird nicht in Children aufgerufen!
        GameControler.player.unitStack.Add(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        handleBars();
	}

    /**
     * Berechnet den totalen Schaden eines Angriffs auf diese Unit.
     * Die Armor wehrt hierbei einen prozentualen Anteil des Schadens ab.
     * @param dmg: Schaden, ohne Berücksichtigung der Rüstung
     */
    public void takeDmg(int dmg)
    {
        int dmgPerc = dmg / 100;
        int totalDmg = dmg - (dmgPerc * this.armor);
        HitPoints -= totalDmg;
        checkDeath();
    }

    /**
     * Setzt name-Variable
     */
    public void changeName(string name)
    {
        this._name = name;
    }

    /**
     * Ueberprueft, ob die Unit gestorben ist. 
     */
    private void checkDeath()
    {
        if(HitPoints <= 0){
            //die (Multiplayer)
        }
    }

    private void handleBars()
    {
        HealthBar.fillAmount = health;
    }
}
