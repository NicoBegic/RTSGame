using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public int CamSpeed;

    public List<Unit> unitStack;
    public List<Unit> unitSelection;
    private Vector3 mouseStartPos;

    private Flock flock;
    private bool selecting;

	void Awake() 
    {
        GameControler.player = this;
        this.flock = new Flock();
        this.selecting = false;
        this.unitStack = new List<Unit>();
        this.unitSelection = new List<Unit>();
	}
	
	void Update () 
    {
        CheckMouseState();
        Move();
        flock.Move();
	}

    private void CheckMouseState()
    {
        //Wenn linker Mausbutton geklickt wurde und eine Auswahl bereits getroffen wurde
        if(Input.GetMouseButtonDown(0) && unitSelection.Count != 0)
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
        foreach (Unit unit in unitSelection)
        {
            //HealthBar unsichtbar machen
        }
        unitSelection = new List<Unit>();
    }

    private void SelectUnits()
    {
        foreach (Unit unit in unitStack)
        {
            if (IsWithinSelectionBounds(unit.gameObject) && !unitSelection.Contains(unit))
            {
                //Healthbar sichtbar machen
                unitSelection.Add(unit);
            }
        }
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
        //Pathfinding && Flocking
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        if (hit.collider != null)
        {
            Debug.Log("hit: " + hit.point);
            flock.RunTowards(unitSelection, new Vector2(hit.point.x, hit.point.z));
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
