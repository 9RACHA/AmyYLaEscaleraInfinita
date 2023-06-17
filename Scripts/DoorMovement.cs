using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMovement : MonoBehaviour
{
    // variables necesarias para el movimiento
    public Transform leftDoorClosedPoint;
    public Transform leftDoorOpenedPoint;
    public Transform rightDoorClosedPoint;
    public Transform rightDoorOpenedPoint;
    public Transform leftDoor;
    public Transform rightDoor;

    // variables que controlan cuando se abre y cierra la puerta
    private bool doorOpen = false;
    private bool doorClose = false;
    // tiempo que tarda en abrir o cerrar
    private float timeToOpenClose = 1.5f;
    // variable el Time de cuando se hizo la llamada de abrir o cerrar
    private float startTime;

    // Update is called once per frame
    void Update()
    {
        // si abre o cierra la puerta cuando corresponda
        if (doorOpen){
            // se calcula el tiempo para el smoothsptep
            float t = (Time.time - startTime) / timeToOpenClose;            
            // se mueven las puertas izquierda y derecha para abrir
            leftDoor.localPosition = new Vector3(Mathf.SmoothStep(leftDoorClosedPoint.localPosition.x, leftDoorOpenedPoint.localPosition.x, t), 0, 0);
            rightDoor.localPosition = new Vector3(Mathf.SmoothStep(rightDoorClosedPoint.localPosition.x, rightDoorOpenedPoint.localPosition.x, t), 0, 0);
        }
        else if(doorClose)
        {
            // se calcula el tiempo para el smoothsptep
            float t = (Time.time - startTime) / timeToOpenClose;
            // se mueven las puertas izquierda y derecha para cerrar
            leftDoor.localPosition = new Vector3(Mathf.SmoothStep(leftDoorOpenedPoint.localPosition.x, leftDoorClosedPoint.localPosition.x, t), 0, 0);
            rightDoor.localPosition = new Vector3(Mathf.SmoothStep(rightDoorOpenedPoint.localPosition.x, rightDoorClosedPoint.localPosition.x, t), 0, 0);
        }
    }

    public void OpenDoor()
    {
        // se pone a false que las puertas se cierren
        doorClose = false;
        // se pone a true que se abran
        doorOpen = true;
        // se guarda el tiempo de cuando se llamo a esta funcion
        startTime = Time.time;
    }

    public void CloseDoor()
    {
        // se pone a false que se abran las puertas
        doorOpen = false;
        // se pone a true que se cierren
        doorClose = true;
        // se guarda el tiempo de cuando se llamo a esta funcion
        startTime = Time.time;
    }
}
