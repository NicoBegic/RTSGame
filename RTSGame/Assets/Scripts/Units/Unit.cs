using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Unit : MonoBehaviour{

    public Image HealthBar;
    public int Health;
    public Movement Movement;

    //SyncVar
    private string _name;

    //Prozentualer Wert (Max 100/Min 0)
    protected int armor;
    protected int baseStrength;
    protected float movementSpeed;
    protected int hitPoints;
    //Zeitraum zwischen zwei Schüssen in Sec
    protected float fireRate;
    protected int shootingRange;

    private float maxForce;
    private float maxSpeed;

    private Unit targetEnemy;
    private float timePassed;

    public string Name {
        get { return _name; }
        set { _name = value; }
    }

    public Vector2 Location { 
        get { return new Vector2(this.transform.position.x, this.transform.position.z); }
        set { this.transform.position.Set(value.x, this.transform.position.y, value.y); }
    }

    protected void Initialize()
    {
        GameControler.player.unitStack.Add(this);
        this.Location = this.transform.position;
        this.Movement = new Movement();
        this.maxForce = 0.3f;
        this.maxSpeed = this.movementSpeed;
    }

    protected void HandleUpdate()
    {
        HandleBars();

        if (targetEnemy != null)
        {
            if (InRange(targetEnemy))
            {
                timePassed += Time.deltaTime;

                if (timePassed >= fireRate)
                    Shoot(targetEnemy);
            }
            else
            {
                timePassed = 0;
                //Flock(targetEnemy.transform.position);
            }
        }
    }

    /**
     * Berechnet den totalen Schaden eines Angriffs auf diese Unit.
     * Die Armor wehrt hierbei einen prozentualen Anteil des Schadens ab.
     * @param dmg: Schaden, ohne Berücksichtigung der Rüstung
     */
    public void TakeDmg(int dmg)
    {
        int dmgPerc = dmg / 100;
        int totalDmg = dmg - (dmgPerc * this.armor);
        hitPoints -= totalDmg;
        CheckDeath();
    }

    /**
     * Setzt name-Variable
     */
    public void ChangeName(string name)
    {
        this._name = name;
    }

    private void HandleBars()
    {
        HealthBar.fillAmount = Health;
    }


    public bool MoveTowards(List<Unit> selection, Vector2 target)
    {
        //Wenn an target-Pos angekommen, movement anhalten
        if(this.Location.Equals(target)){
            return false;
        }
        Debug.Log("Update Move");
        this.Location = this.Movement.MoveTowards(selection, target, this);
        return true;
    }


    public void SetTargetEnemy(Unit enemy)
    {
        timePassed = 0;
        targetEnemy = enemy;
    }

    private void Shoot(Unit enemy)
    {
        enemy.TakeDmg(baseStrength);
    }

    protected bool InRange(Unit target)
    {
        if (Vector3.Distance(this.Location, target.Location) <= shootingRange)
            return true;
        else
            return false;
    }

    /**
     * Ueberprueft, ob die Unit gestorben ist. 
     */
    private void CheckDeath()
    {
        if (hitPoints <= 0)
        {
            //die (Multiplayer)
        }
    }
}
