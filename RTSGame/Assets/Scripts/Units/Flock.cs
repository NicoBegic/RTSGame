using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock {

    private bool move;
    private List<Unit> boids;
    private Vector2 targetPos;

    public Flock(List<Unit> boids)
    {
        this.boids = boids;
    }

    public List<Unit> getSelection()
    {
        return boids;
    }

    public Vector2 getTargetPos()
    {
        return targetPos;
    }

    public bool isMoving()
    {
        return move;
    }

    public void setTarget(Vector2 target)
    {
        this.targetPos = target;
    }

    public void Move()
    {
        for (int i = 0; i < boids.Count && move; i++)
        {
            move = boids[i].MoveTowards(boids, targetPos);
        }
    }

    public void RunTowards(Vector2 targetPos)
    {
        this.targetPos = targetPos;
        move = true;
    }
}
