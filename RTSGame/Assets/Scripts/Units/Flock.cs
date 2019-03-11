using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock {

    private bool move;
    private List<Unit> boids;
    private Vector2 targetPos;

    public void Move()
    {
        if (move)
        {
            foreach (Unit unit in boids)
            {
                move = unit.MoveTowards(boids, targetPos);
            }
        }
    }

    public void RunTowards(List<Unit> boids, Vector2 targetPos)
    {
        this.boids = boids;
        this.targetPos = targetPos;
        move = true;
    }
}
