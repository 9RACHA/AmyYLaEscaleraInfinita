using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // prefab de thing para instanciarlo
    public GameObject thingPrefab;
    // lista del shlefs donde se instanciaran las things
    public List<GameObject> shelfList;
    // cantidad de thing a instanciar
    private int cantidadThing = 20;

    // Start is called before the first frame update
    void Start()
    {
        // se llama al metodo para que instance las things
        SpawnThings();    
    }

    private void SpawnThings()
    {
        // randon donde se guardara el numero aleatorio del shelf seleccionado
        int randomShelf;
        // altura del siguiente thing
        float thingNextAltura = 0.085f;
        // vector3 con la posicion del thing
        Vector3 position = new Vector3(0f, 0.35f, 0);
        for (int i = 0; i < cantidadThing; i++)
        {
            // se hace un reset a la posicion de Y en cada vuelta ya que es una thing diferente
            position.y = 0.35f;
            // se hace un randon de los shelfs
            randomShelf = UnityEngine.Random.Range(0, shelfList.Count);
            // se instancia la thing siendo hija del shlef random y se guarda para poder modificarla
            GameObject goThing = Instantiate(thingPrefab, shelfList[randomShelf].transform);
            // modificamos la altura que tendra teniendo en cuenta cuantas things hay ya en el shelf
            position.y += (thingNextAltura * shelfList[randomShelf].GetComponent<Shelf>().GetThingListCount());
            // se le pone como posicion local la posicion calculada
            goThing.transform.localPosition = position;
            // se añade al shelf
            shelfList[randomShelf].GetComponent<Shelf>().Addthing(goThing);
        }
    }
}
