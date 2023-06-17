using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private float playerSpeed = 3.5f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    private PlayerState state;
    private Animator animator;
    private Transform cameraTransform;

    // referencia al objeto que tiene el player sobre la cabeza que indica cuando agarro algo
    public GameObject LightHouse;
    // script de la puerta de entrada a las escalreas
    public DoorMovement inputDoubleDoor;
    // variable donde se guardara la thing agarrada del player
    private GameObject thing;
    // control de si tiene algo agarrado
    private bool iHaveThing = false;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        SetState(PlayerState.Idle);
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;

        if (groundedPlayer && playerVelocity.y < 0)
        {
            //playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //Transformamos el movimiento en la direccion de la cámara
        move = cameraTransform.forward * move.z + cameraTransform.right * move.x;
        move.y = 0;
        move = move.normalized;


        Vector3 displacement = move * Time.deltaTime * playerSpeed;
        controller.Move(displacement);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            //playerVelocity.y = 0f;
            Debug.Log("Salta Amy, salta");
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            SetState(PlayerState.Jump);
        }
        else if (groundedPlayer)
        {
            if (move == Vector3.zero)
            {
                SetState(PlayerState.Idle);
            }
            else
            {
                SetState(PlayerState.Run);
            }
        }

        // se comprueba si el player pulso la tecla interaccion
        if (Input.GetButtonDown("Interaction"))
        {
            // se llama al metodo que controla las interaciones
            CheckForInteraction();
        }


        playerVelocity.y += gravityValue * Time.deltaTime;

        if (playerVelocity.y < 0 && !groundedPlayer)
        {
            SetState(PlayerState.Fall);
        }

        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void CheckForInteraction()
    {

        RaycastHit hit;
        // se hace un raycast
        if (Physics.Raycast(transform.position + Vector3.up * 0.4f + transform.forward * 0.1f, transform.forward, out hit, 0.5f))
        {
            // se compureba que tiene delante un objeto con la tag shelf y el player no tenga ninguna thing agarrada
            if (hit.collider.gameObject.CompareTag("Shelf") && !iHaveThing)
            {
                // se obtiene la ultima thing de mas arriba del shelf
                thing = hit.collider.gameObject.GetComponent<Shelf>().GetThing();
                // en caso de que el shelf no tenga nada ninguna thing no se haria nada
                if (thing != null)
                {
                    // se le asigna a la thing que el padre es el player
                    thing.transform.SetParent(transform);
                    // se le asigna su nueva posicion
                    Vector3 position = new Vector3(0f, 1f, 0.3f);
                    thing.transform.localPosition = position;
                    thing.transform.localRotation = Quaternion.identity;
                    // se asgina a la lighthouse el mismo material de la thing
                    LightHouse.GetComponent<Renderer>().material = thing.GetComponentInChildren<Renderer>().material;
                    // se activa la lighthouse
                    LightHouse.SetActive(true);
                    // se marca que el player ya tiene una thing
                    iHaveThing = true;
                    // se llama al metodo que abre la puerta de entrada a las escaleras
                    inputDoubleDoor.OpenDoor();
                }
            }
            // se comprueba si tiene delante el teletransportador de materia y que tiene una thing agarrada
            if (hit.collider.gameObject.CompareTag("TeleTransporter") && iHaveThing)
            {
                // se destrulle la thing
                Destroy(thing);
                // se desactiva la lighthouse
                LightHouse.SetActive(false);
                // se marca que el player no tiene ninguna thing agarrada
                iHaveThing = false;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        // se controla si el player colisiono con el transportador de las escaleras
        if (other.gameObject.CompareTag("TraslateFloorUp"))
        {
            GetComponent<TranslateFloorUp>().Translate();
        }
        // se controla si el player colisiono con el collider para cerrar puertas
        if (other.gameObject.CompareTag("CloserDoor"))
        {
            other.gameObject.GetComponentInParent<DoorMovement>().CloseDoor();
        }
        // se controla si el player colisiono con el collider para abrirr puertas
        if (other.gameObject.CompareTag("OpenerDoor"))
        {
            other.gameObject.GetComponentInParent<DoorMovement>().OpenDoor();
        }
    }

    private void SetState(PlayerState newState)
    {
        if (state != newState)
        {
            animator.ResetTrigger("Idle");
            animator.ResetTrigger("Run");
            animator.ResetTrigger("Jump");
            animator.ResetTrigger("Fall");
            state = newState;
            animator.SetTrigger($"{newState}");
        }
    }


}

public enum PlayerState
{
    Idle,
    Run,
    Jump,
    Fall
}
