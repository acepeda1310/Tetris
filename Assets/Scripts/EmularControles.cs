using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class EmularControles : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    public bool pulsado;

    public void OnPointerDown(PointerEventData eventData)
    {
        pulsado = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pulsado = false;
    }

/*
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}



    public void MoverDerecha()
    {
        InputSimulator.SimulateKeyPress(VirtualKeyCode.RIGHT);
        
    }

    public void MoverIzquierda()
    {

        InputSimulator.SimulateKeyPress(VirtualKeyCode.LEFT);
    }

    public void MoverArriba()
    {

        InputSimulator.SimulateKeyPress(VirtualKeyCode.UP);
    }

    public void MoverAbajo()
    {

        InputSimulator.SimulateKeyPress(VirtualKeyCode.DOWN);
    }*/
}
