using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Unit : MonoBehaviour{

    public Image HealthBar;
    public int health;

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

    //Flocking-Variablen
    private Vector2 target;
    private Vector2 location;
    private Vector2 velocity;
    private Vector2 acceleration;
    private float maxForce;
    private float maxSpeed;

    private Unit targetEnemy;
    private float timePassed;

    public string Name {
        get { return _name; }
        set { _name = value; }
    }

    protected void Initialize()
    {
        GameControler.player.unitStack.Add(this);
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
        HealthBar.fillAmount = health;
    }


    public bool MoveTowards(List<Unit> selection, Vector2 target)
    {
        //Wenn an target-Pos angekommen, movement anhalten
        if(this.transform.position.Equals(target)){
            return false;
        }
        this.target = target;
        this.velocity = new Vector2(); //Anfangs-Bewegung in eine Richtung
        this.location = new Vector2(this.transform.position.x, this.transform.position.z);

        Flock(target, selection);
        UpdateLocation();
        this.transform.position = this.location;
        return true;
    }

    private void Flock(Vector2 target, List<Unit> selection)
    {
        //A* implementieren
        var seperation = Seperate(selection);
        var cohesion = Cohere(selection);
        var alignment = Align(selection);

        //Gewichtung je nach Einheitentyp
        seperation *= 1.5f;
        cohesion *= 1;
        alignment *= 1;

        ApplyForce(seperation);
        ApplyForce(cohesion);
        ApplyForce(alignment);
    }

    private void ApplyForce(Vector2 force)
    {
        acceleration += force;
    }

    private void UpdateLocation()
    {
        velocity += acceleration;

        if(velocity.magnitude > maxSpeed){
            velocity.Normalize();
            velocity *= maxSpeed;
        }
        location += velocity;
        acceleration = Vector2.zero;
    }

    private Vector2 Seek(Vector2 target)
    {
        var desired = target - this.location;
        desired.Normalize();
        desired *= maxSpeed;

        var steer = desired - velocity;
        if (steer.magnitude > maxForce)
        {
            steer.Normalize();
            steer *= maxForce;
        }
        return steer;
    }

    private Vector2 Seperate(List<Unit> selection)
    {
        return Vector2.zero;
    }

    private Vector2 Cohere(List<Unit> selection)
    {
        return Vector2.zero;
    }

    private Vector2 Align(List<Unit> selection)
    {
        return Vector2.zero;
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
        if (Vector3.Distance(this.transform.position, target.transform.position) <= shootingRange)
            return true;

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
