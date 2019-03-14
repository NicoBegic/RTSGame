using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public Vector2 Velocity;

    //Flocking-Variablen
    private Vector2 location;
    private float speed;
    private List<Unit> unitsToAvoid;

    void Start()
    {
        this.Velocity = new Vector2();
        this.unitsToAvoid = new List<Unit>();
    }
    
    public Vector2 MoveTowards(Vector2 position, Vector2 target, float speed)
    {
        this.location = new Vector2(position.x, position.y);
        this.speed = speed;

        this.Velocity += Steer(target, false);

        this.location += this.Velocity;
        return this.location;
    }


    private Vector2 Steer(Vector2 target, bool finalPoint)
    {
        Vector2 desiredVel = target - location;
        float distance = desiredVel.magnitude;

        desiredVel.Normalize();

        if (finalPoint && distance < 5f)
        {
            desiredVel *= speed * (distance / 5f);
        } else desiredVel *= speed;

        Vector2 steeringForce = desiredVel - Velocity;
        if (unitsToAvoid.Count > 0)
        {
            steeringForce += Seperate();
        }

        return steeringForce;
    }

    private Vector2 Seperate()
    {
        Vector2 steer = Vector2.zero;
        int neighborCount = unitsToAvoid.Count;

        foreach (var unit in unitsToAvoid)
        {
            float distance = Vector2.Distance(this.location, unit.Location);
            Vector2 difference = unit.Location - this.location;
            difference.Normalize();
            difference /= distance;
            steer += difference;
        }

        steer /= neighborCount;

        if (steer.magnitude > 0)
        {
            steer.Normalize();
            steer *= speed;
        }

        return -steer;
    }

    private void OnTriggerEnter(Collider other)
    {
        Unit otherU = other.GetComponent<Unit>();
        if (otherU != null && !unitsToAvoid.Contains(otherU))
        {
            unitsToAvoid.Add(otherU);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Unit otherU = other.GetComponent<Unit>();
        if (otherU != null && unitsToAvoid.Contains(otherU))
        {
            unitsToAvoid.Remove(otherU);
        }
    }
}
