using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thing : MonoBehaviour {

    // Lista de materiales
    public List<Material> listaMateriales;

    private void Start()
    {
        // se selecciona un material aleatorio cuando se crea el objeto
        GetComponentInChildren<Renderer>().material = listaMateriales[Random.Range(0, listaMateriales.Count)];
    }
}
