using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelf : MonoBehaviour {

    // lista donde se guardaran las things
    private List<GameObject> thingList = new List<GameObject>();

    // metodo para añadir thigns a la lista
    public void Addthing(GameObject thing)
    {
        thingList.Add(thing);
    }

    // metodo para saber cuantas things tenemos
    public float GetThingListCount()
    {
        return thingList.Count;
    }

    // metodo que nos devuelve siempre la ultima thing mientras la lista no estea vacia
    public GameObject GetThing()
    {
        if(thingList.Count > 0)
        {
            GameObject gothing = thingList[thingList.Count - 1];
            thingList.Remove(gothing);
            return gothing;
        }
        else
        {
            return null;
        }

    }
}
