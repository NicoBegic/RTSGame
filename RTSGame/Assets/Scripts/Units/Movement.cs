using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement {

    public Vector2 Velocity;

    //Flocking-Variablen
    private Vector2 location;
    private Vector2 target;
    private Vector2 acceleration;
    private const float maxForce = 0.03f;
    private const float maxSpeed = 2f;

    private Unit u;

    public Movement()
    {
        Velocity = new Vector2();
    }

    public Vector2 MoveTowards(List<Unit> selection, Vector2 target, Unit u)
    {
        this.target = target;
        this.Velocity = new Vector2(1, 1); //Anfangs-Bewegung in eine Richtung
        this.u = u;
        this.location = u.Location;

        Flock(target, selection);
        UpdateLocation();

        return this.location;
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
        Velocity += acceleration;

        if (Velocity.magnitude > maxSpeed)
        {
            Velocity.Normalize();
            Velocity *= maxSpeed;
        }
        location += Velocity;
        acceleration = Vector2.zero;
    }

    private Vector2 Seek(Vector2 target)
    {
        var desired = target - this.location;
        desired.Normalize();
        desired *= maxSpeed;

        var steer = desired - Velocity;
        if (steer.magnitude > maxForce)
        {
            steer.Normalize();
            steer *= maxForce;
        }
        return steer;
    }


    private Vector2 Seperate(List<Unit> selection)
    {
        //Fuer jede Einheit unterschiedlich. Sollte protected variable einer Unit sein
        float desiredSeparation = 50;
        Vector2 steer = Vector2.zero;
        int neighborCount = 0;

        foreach (var unit in selection)
        {
            float distance = Vector2.Distance(this.location, unit.Location);

            if (!this.u.Equals(unit) && distance < desiredSeparation)
            {
                Vector2 difference = unit.Location - this.location;
                difference.Normalize();
                difference /= distance;
                steer += difference;

                neighborCount++;
            } 
        }

        if (neighborCount > 0)
            steer /= neighborCount;

        if (steer.magnitude > 0)
        {
            steer.Normalize();
            steer *= maxSpeed;
            steer -= Velocity;

            if (steer.magnitude > maxForce)
            {
                steer.Normalize();
                steer *= maxForce;
            }
        }

        return -steer;
    }

    private Vector2 Cohere(List<Unit> selection)
    {
        float neighborDistance = 100;
        Vector2 sum = Vector2.zero;
        int neighborCount = 0;

        foreach (var unit in selection)
        {
            float distance = Vector2.Distance(this.location, unit.Location);
            if (!this.u.Equals(unit) && distance < neighborDistance)
            {
                sum += unit.Location;
                neighborCount++;
            }
        }

        if (neighborCount > 0)
        {
            sum /= neighborCount;
            return Seek(sum);
        }
        else
            return Vector2.zero;
    }

    private Vector2 Align(List<Unit> selection)
    {
        float neighborDistance = 100;
        Vector2 sum = Vector2.zero;
        int neighborCount = 0;

        foreach (var unit in selection)
        {
            float distance = Vector2.Distance(this.location, unit.Location);
            if (!this.u.Equals(unit) && distance < neighborDistance)
            {
                sum += unit.Movement.Velocity;
                neighborCount++;
            }
        }

        if (neighborCount > 0)
        {
            sum /= neighborCount;
            sum.Normalize();
            sum *= maxSpeed;
            Vector2 steer = sum - Velocity;

            if (steer.magnitude > maxForce)
            {
                steer.Normalize();
                steer *= maxForce;
            }
            return steer;
        }
        else
            return Vector2.zero;
    }
}
