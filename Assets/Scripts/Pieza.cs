using UnityEngine;
using System.Collections;

public class Pieza : MonoBehaviour {

    float caida = 0f;
    private float velocidadCaida = 1f;

    public bool rotacion = true;
    public bool limitarRotacion = false;

    public int puntuacionIndividual = 100;

    private float tiempoPuntuacionIndividual;

    public AudioClip sonidoMovimiento;
    public AudioClip sonidoRotacion;
    public AudioClip sonidoSuelo;

    private AudioSource audioSource;

    private float velocidadPulsacionContinuaVertical = 0.05f;
    private float velocidadPulsacionContinuaHorizontal = 0.1f;
    private float tiempoEsperaBotonPulsado = 0.0f;

    private float temporizadorVertical = 0;
    private float temporizadorHorizontal = 0;
    private float temporizador = 0;

    bool rotado = false;
    

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
        caida = GameObject.Find("Tablero").GetComponent<Juego>().caida;
	}
	
	// Update is called once per frame
	void Update () {
        AccionesUsuario();
        ActualizarPuntuacionIndividual();
    }

    void ActualizarPuntuacionIndividual()
    {
        if (tiempoPuntuacionIndividual < 1)
        {
            tiempoPuntuacionIndividual += Time.deltaTime;
        }
        else
        {
            tiempoPuntuacionIndividual = 0;
            puntuacionIndividual = Mathf.Max(puntuacionIndividual - 10, 0);
        }
    }

    void AccionesUsuario()
    {
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            temporizadorHorizontal = 0;
            temporizadorVertical = 0;
            temporizador = 0;
        }
        if (Input.GetKey(KeyCode.RightArrow) || FindObjectOfType<Juego>().ControlesActivados[1])
        {
            if (temporizador < tiempoEsperaBotonPulsado)
            {
                temporizador += Time.deltaTime;
                return;
            }
            if (temporizadorHorizontal < velocidadPulsacionContinuaHorizontal && (!Input.GetKeyDown(KeyCode.RightArrow)))
            {
                temporizadorHorizontal += Time.deltaTime;
                return;
            }
            temporizadorHorizontal = 0;
            transform.position += new Vector3(1, 0, 0);
            if (!PosicionValida())
                transform.position -= new Vector3(1, 0, 0);
            else {
                FindObjectOfType<Juego>().ActualizarTablero(this);
                EscucharMovimiento();
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || FindObjectOfType<Juego>().ControlesActivados[0])
        {
            if (temporizador < tiempoEsperaBotonPulsado)
            {
                temporizador += Time.deltaTime;
                return;
            }
            if (temporizadorHorizontal < velocidadPulsacionContinuaHorizontal && (!Input.GetKeyDown(KeyCode.LeftArrow)))
            {
                temporizadorHorizontal += Time.deltaTime;
                return;
            }
            temporizadorHorizontal = 0;
            transform.position += new Vector3(-1, 0, 0);
            if (!PosicionValida())
                transform.position -= new Vector3(-1, 0, 0);
            else {
                FindObjectOfType<Juego>().ActualizarTablero(this);
                EscucharMovimiento();
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || (!rotado && FindObjectOfType<Juego>().ControlesActivados[2]))
        {
            rotado = true;
            if (rotacion)
            {
                if (limitarRotacion)
                {
                    if (transform.rotation.eulerAngles.z >= 90)
                    {
                        transform.Rotate(0, 0, -90);
                    }
                    else
                    {
                        transform.Rotate(0, 0, 90);
                    }
                }
                else
                {
                    transform.Rotate(0, 0, 90);
                }
                if (!PosicionValida())
                {
                    transform.position += new Vector3(1, 0, 0);
                    if (!PosicionValida())
                    {
                        transform.position -= new Vector3(2, 0, 0);
                        if (!PosicionValida()) 
                        {
                            transform.position += new Vector3(1, 0, 0);
                            if (limitarRotacion)
                            {
                                if (transform.rotation.eulerAngles.z >= 90)
                                    transform.Rotate(0, 0, -90);
                                else
                                    transform.Rotate(0, 0, 90);
                            }
                            else
                                transform.Rotate(0, 0, -90);
                        }
                        else
                        {
                            FindObjectOfType<Juego>().ActualizarTablero(this);
                            EscucharRotacion();
                        }
                    }
                    else
                    {
                        FindObjectOfType<Juego>().ActualizarTablero(this);
                        EscucharRotacion();
                    }
                }
                else {
                    FindObjectOfType<Juego>().ActualizarTablero(this);
                    EscucharRotacion();
                }

            }
        }
        else if (Input.GetKey(KeyCode.DownArrow) || FindObjectOfType<Juego>().ControlesActivados[3] || Time.time - caida >= velocidadCaida)
        {
            if (temporizador < tiempoEsperaBotonPulsado)
            {
                temporizador += Time.deltaTime;
                return;
            }
            if (temporizadorVertical < velocidadPulsacionContinuaVertical && (!Input.GetKeyDown(KeyCode.DownArrow)))
            {
                temporizadorVertical += Time.deltaTime;
                return;
            }
            temporizadorVertical = 0;
            transform.position += new Vector3(0, -1, 0);
            caida = Time.time;
            if (!PosicionValida())
            {
                transform.position -= new Vector3(0, -1, 0);
                FindObjectOfType<Juego>().BorrarFila();
                if (FindObjectOfType<Juego>().PorEncimaDelTablero(this))
                {
                    FindObjectOfType<Juego>().GameOver();
                }
                EscucharSuelo();
                enabled = false;
                FindObjectOfType<Juego>().Generar();
                Juego.puntuacionactual += puntuacionIndividual;
            }
            else {
                FindObjectOfType<Juego>().ActualizarTablero(this);
                if (Input.GetKey(KeyCode.DownArrow))
                    EscucharMovimiento();
            }
        }
        else if (!FindObjectOfType<Juego>().ControlesActivados[2])
            rotado = false;
    }

    void EscucharMovimiento()
    {
        audioSource.PlayOneShot(sonidoMovimiento);
    }

    void EscucharRotacion()
    {
        audioSource.PlayOneShot(sonidoRotacion);
    }

    void EscucharSuelo()
    {
        audioSource.PlayOneShot(sonidoSuelo);
    }

    bool PosicionValida()
    {
        foreach(Transform mino in transform)
        {
            Vector2 posicion = FindObjectOfType<Juego>().Aproximacion(mino.position);
            if (!FindObjectOfType<Juego>().DentroDelTablero(posicion))
            {
                return false;
            }
            if (FindObjectOfType<Juego>().ObtenerTransformEnPosicion(posicion) != null && FindObjectOfType<Juego>().ObtenerTransformEnPosicion(posicion).parent != transform)
            {
                return false;
            }
        }
        return true;
    }
}
