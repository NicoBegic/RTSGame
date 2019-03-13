﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public int CamSpeed;

    public List<Unit> unitStack;

    private List<Flock> unitGroups;
    private int groupNum;
    private Vector3 mouseStartPos;
    private bool selecting;

	void Awake() 
    {
        GameControler.player = this;
        this.selecting = false;
        this.unitStack = new List<Unit>();
        this.unitGroups = new List<Flock>();
        this.groupNum = 0;
	}
	
	void Update () 
    {
        CheckMouseState();
        Move();

        if (unitGroups.Count > 0) 
        { 
            updateUnitGroups();
        }
	}

    private void updateUnitGroups()
    {
        List<Flock> toDelete = new List<Flock>();
        foreach (var flock in unitGroups)
        {
            if (flock.getTargetPos() != Vector2.zero)
            {
                flock.Move();
                if (!flock.isMoving() && !unitGroups[groupNum-1].Equals(flock))
                    toDelete.Add(flock);
            }
        }

        foreach (var flock in toDelete)
        {
            unitGroups.Remove(flock);
        }
    }

    private void CheckMouseState()
    {
        //Wenn linker Mausbutton geklickt wurde und eine Auswahl bereits getroffen wurde
        if(Input.GetMouseButtonDown(0) && unitGroups[groupNum-1].getSelection().Count != 0)
        {
            UnselectUnits();
        }//Wenn rechter Maus-Button geklickt wurde
        else if(Input.GetMouseButtonDown(1))
        {
            CommandUnits();
        }//Wenn linker Maus-Button gedrückt gehalten wird
        else if(Input.GetMouseButton(0) && selecting == false)
        {
            mouseStartPos = Input.mousePosition;
            selecting = true;
            Debug.Log("Halten");
        }//Wenn linker Maus-Button losgelassen wird
        else if(Input.GetMouseButton(0) == false && selecting)
        {
            SelectUnits();
            selecting = false;  
        }

        //Wenn linker Maus-Button geklickt wurde
        else if (Input.GetMouseButtonDown(0) && selecting == false)
        {
            mouseStartPos = Input.mousePosition;
        }
    }

    private void UnselectUnits()
    {
        foreach (Unit unit in unitGroups[groupNum - 1].getSelection())
        {
            //HealthBar unsichtbar machen
        }
        unitGroups.RemoveAt(groupNum - 1);
        groupNum--;
    }

    private void SelectUnits()
    {
        List<Unit> unitSelection = new List<Unit>();

        foreach (Unit unit in unitStack)
        {
            if (IsWithinSelectionBounds(unit.gameObject) && !unitSelection.Contains(unit))
            {
                //Healthbar sichtbar machen
                unitSelection.Add(unit);
            }
        }

        Flock newFlock = new Flock(unitSelection);
        unitGroups.Add(newFlock);
        groupNum++;
    }

    private bool IsWithinSelectionBounds(GameObject gameObject)
    {
        if (!selecting)
            return false;

        var camera = Camera.main;
        var viewportBounds =
            Utils.GetViewportBounds(camera, mouseStartPos, Input.mousePosition);

        return viewportBounds.Contains(
            camera.WorldToViewportPoint(gameObject.transform.position));
    }

    private void CommandUnits()
    {
        if (groupNum > 0)
        {
            //Pathfinding && Flocking
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            if (hit.collider != null)
            {
                Debug.Log("hit: " + hit.point);
                unitGroups[groupNum-1].RunTowards(new Vector2(hit.point.x, hit.point.z));
            }
        }
    }

    private void Move()
    {
        transform.Translate(Input.GetAxis("Horizontal") * CamSpeed * Time.deltaTime, 0, Input.GetAxis("Vertical") * CamSpeed * Time.deltaTime);
    }

    void OnGUI() 
    {
        //Auswahl-Fenster zeichnen
        if (selecting)
        {
            var rect = Utils.GetScreenRect(mouseStartPos, Input.mousePosition);
            Utils.DrawScreenRect(rect, new Color(0.2f, 0.2f, 0.95f, 0.25f));
            Utils.DrawScreenRectBorder(rect, 1, new Color(0.8f, 0.8f, 0.95f));
        }
    }
}
