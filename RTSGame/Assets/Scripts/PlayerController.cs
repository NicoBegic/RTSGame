using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public int CamSpeed;

    public List<GameObject> unitStack;
    public List<GameObject> unitSelection;
    private Vector3 mouseStartPos;

    private bool selecting;

	void Awake() 
    {
        GameControler.player = this;
        this.selecting = false;
        this.unitStack = new List<GameObject>();
        this.unitSelection = new List<GameObject>();
	}
	
	void Update () 
    {
        checkMouseState();
        move();
	}

    private void checkMouseState()
    {
        //Wenn linker Mausbutton geklickt wurde und eine Auswahl bereits getroffen wurde
        if (Input.GetMouseButtonDown(0) && unitSelection.Count != 0)
        {
            unselectUnits();
        }//Wenn rechter Maus-Button geklickt wurde
        else if (Input.GetMouseButtonDown(1))
        {
            commandUnits();
        }//Wenn linker Maus-Button gedrückt gehalten wird
        else if(Input.GetMouseButton(0) && selecting == false)
        {
            mouseStartPos = Input.mousePosition;
            selecting = true;
            Debug.Log("Halten");
        }//Wenn linker Maus-Button losgelassen wird
        else if(Input.GetMouseButton(0) == false && selecting)
        {
            selectUnits();
            selecting = false;  
        }

        //Wenn linker Maus-Button geklickt wurde
        else if (Input.GetMouseButtonDown(0) && selecting == false)
        {
            mouseStartPos = Input.mousePosition;
        }
    }

    private void unselectUnits()
    {
        unitSelection = new List<GameObject>();
        foreach (GameObject unit in unitSelection)
        {
            //HealthBar unsichtbar machen
        }
    }

    private void selectUnits()
    {
        foreach (GameObject unit in unitStack)
        {
            if (isWithinSelectionBounds(unit) && !unitSelection.Contains(unit))
            {
                //Healthbar sichtbar machen
                unitSelection.Add(unit);
            }
        }
    }

    private bool isWithinSelectionBounds(GameObject gameObject)
    {
        if (!selecting)
            return false;

        var camera = Camera.main;
        var viewportBounds =
            Utils.GetViewportBounds(camera, mouseStartPos, Input.mousePosition);

        return viewportBounds.Contains(
            camera.WorldToViewportPoint(gameObject.transform.position));
    }

    private void commandUnits()
    {
        //Pathfinding
    }

    private void move()
    {
        transform.Translate(Input.GetAxis("Horizontal") * CamSpeed * Time.deltaTime, 0, Input.GetAxis("Vertical") * CamSpeed * Time.deltaTime);
    }

    void OnGUI() 
    {
        if (selecting)
        {
            var rect = Utils.GetScreenRect(mouseStartPos, Input.mousePosition);
            Utils.DrawScreenRect(rect, new Color(0.2f, 0.2f, 0.95f, 0.25f));
            Utils.DrawScreenRectBorder(rect, 1, new Color(0.8f, 0.8f, 0.95f));
        }
    }
}
