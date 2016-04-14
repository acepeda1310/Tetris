using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Juego : MonoBehaviour {

    public Camera camaraHorizontal;
    public Camera camaraVertical;

    public Canvas canvasHorizontal;
    public Canvas canvasVertical;
    

    public static int anchoTablero = 10;
    public static int altoTablero = 20;

    public static Transform[,] tablero = new Transform[anchoTablero, altoTablero];

    public int puntuacionPorLinea = 40;
    public int puntuacionDosLineas = 100;
    public int puntuacionTresLineas = 300;
    public int puntuacionCuatroLineas = 1200;

    public float caida = 1.0f;

    public Text puntuacion;
    public Text puntuacionVertical;

    public static int puntuacionactual=0;

    private int numeroDeFilasEstaRonda = 0;

    public GameObject[] obj;

    public AudioClip eliminarFilas;
    private AudioSource audioSource;

    private GameObject preview;
    private GameObject siguiente;

    private bool juegoEmpezado = false;

    private Vector2 posicionPreview = new Vector2(15.5f, 15);

    public int nivelActual = 0;
    private int numeroDeLineasEliminadas = 0;
    public Text nivel;
    public Text lineas;
    public Text nivelVertical;
    public Text lineasVertical;

    public EmularControles[] controlesHorizontales;
    public EmularControles[] controlesVerticales;
    private EmularControles[] controles;
    public bool[] ControlesActivados=new bool[4];

    // Use this for initialization
    void Start () {
        puntuacion.text = "0";
        puntuacionVertical.text = "0";
        puntuacionactual = 0;
        Generar();
        Generador.siguiente = Random.Range(0, obj.Length);
        //Instantiate(obj[Generador.siguiente], new Vector2(15.0f, 10.0f), Quaternion.identity);
        audioSource = GetComponent<AudioSource>();
	}

    void Update()
    {
        if (Screen.orientation == ScreenOrientation.Landscape)
        {
            camaraHorizontal.enabled = true;
            canvasHorizontal.enabled = true;
            camaraVertical.enabled = false;
            canvasVertical.enabled = false;
            posicionPreview = new Vector2(15.5f, 15);
            controles = controlesHorizontales;
        }
        else
        {
            camaraVertical.enabled = true;
            canvasVertical.enabled = true;
            camaraHorizontal.enabled = false;
            canvasHorizontal.enabled = false;
            posicionPreview = new Vector2(14f, 11);
            controles = controlesVerticales;
        }
        if (preview != null)
        {

            preview.transform.localPosition = posicionPreview;
        }
        ActualizarNivel();
        ActualizarVelocidad();
        EmuladorControles();
    }

    void EmuladorControles()
    {
        for(int i=0; i<4; i++)
        {
            ControlesActivados[i] = controles[i].pulsado;
        }
    }

    void ActualizarNivel()
    {
        nivelActual = (numeroDeLineasEliminadas / 10) + 1;
        nivel.text = nivelActual.ToString();
        nivelVertical.text = nivelActual.ToString();
        lineas.text = numeroDeLineasEliminadas.ToString();
        lineasVertical.text = numeroDeLineasEliminadas.ToString();
    }

    void ActualizarVelocidad()
    {
        caida = 1.0f * ((float)System.Math.Pow(0.8f,nivelActual));
    }

    public void Generar()
    {
        if (!juegoEmpezado)
        {
            juegoEmpezado = true;

            siguiente=(GameObject)Instantiate(obj[Random.Range(0, obj.Length)], new Vector2(5.0f, 20.0f), Quaternion.identity);
            preview=(GameObject)Instantiate(obj[Random.Range(0, obj.Length)], posicionPreview, Quaternion.identity);
            preview.GetComponent<Pieza>().enabled = false;
        }
        else
        {
            preview.transform.localPosition = new Vector2(5.0f, 20.0f);
            siguiente = preview;
            siguiente.GetComponent<Pieza>().enabled = true;
            preview = (GameObject)Instantiate(obj[Random.Range(0, obj.Length)], posicionPreview, Quaternion.identity);
            preview.GetComponent<Pieza>().enabled = false;
        }
    }

    public void ActualizarPuntuacion()
    {
        if (numeroDeFilasEstaRonda > 0)
        {
            if (numeroDeFilasEstaRonda == 1)
            {
                puntuacionactual += puntuacionPorLinea;
            }
            else if (numeroDeFilasEstaRonda == 1)
            {
                puntuacionactual += puntuacionDosLineas;
            }
            else if (numeroDeFilasEstaRonda == 1)
            {
                puntuacionactual += puntuacionTresLineas;
            }
            else if (numeroDeFilasEstaRonda == 1)
            {
                puntuacionactual += puntuacionCuatroLineas;
            }
            audioSource.PlayOneShot(eliminarFilas);
        }
        numeroDeFilasEstaRonda = 0;
        puntuacion.text = puntuacionactual.ToString();
        puntuacionVertical.text = puntuacionactual.ToString();
    }

    public bool PorEncimaDelTablero(Pieza pieza)
    {
        for(int x = 0; x < anchoTablero; x++)
        {
            foreach(Transform bloque in pieza.transform)
            {
                Vector2 pos = Aproximacion(bloque.position);
                if (pos.y > altoTablero - 1)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool FilaLlena(int y)
    {
        for(int x=0; x < anchoTablero; ++x)
        {
            if(tablero[x,y] == null)
            {
                return false;
            }
        }
        numeroDeFilasEstaRonda++;
        return true;
    }

    public void BorrarBloquesEn(int y)
    {
        numeroDeLineasEliminadas++;
        for(int x=0; x<anchoTablero; ++x)
        {
            Destroy(tablero[x, y].gameObject);
            tablero[x, y] = null;
        }
    }

    public void MoverFilaAbajo(int y)
    {
        for(int x=0; x<anchoTablero; x++)
        {
            if (tablero[x, y] != null)
            {
                tablero[x, y - 1] = tablero[x, y];
                tablero[x, y] = null;
                tablero[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    public void MoverTodasFilasAbajo(int y)
    {
        for(int i= y; i<altoTablero; ++i)
        {
            MoverFilaAbajo(i);
        }
    }

    public void BorrarFila()
    {
        for(int y=0; y<altoTablero; y++)
        {
            if (FilaLlena(y))
            {
                BorrarBloquesEn(y);
                MoverTodasFilasAbajo(y+1);
                y--;
            }
        }
        ActualizarPuntuacion();
    }

    public void ActualizarTablero(Pieza pieza)
    {
        for(int i=0; i<altoTablero; ++i)
        {
            for(int j=1; j<= anchoTablero; ++j)
            {
                if (tablero[j-1, i] != null)
                {
                    if (tablero[j-1, i].parent == pieza.transform)
                    {
                        tablero[j-1, i] = null;
                    }
                }
            }
        }
        foreach(Transform bloque in pieza.transform)
        {
            Vector2 pos = Aproximacion(bloque.position);
            if (pos.y < altoTablero)
            {
                tablero[(int)pos.x-1, (int)pos.y] = bloque;
            }
        }
    }

    public Transform ObtenerTransformEnPosicion(Vector2 pos)
    {
        if(pos.y> altoTablero - 1)
        {
            return null;
        }
        else
        {
            return tablero[(int)pos.x-1, (int)pos.y];
        }
    }

    public bool DentroDelTablero(Vector2 posicion)
    {
        return ((int)posicion.x>0&&(int)posicion.x<=anchoTablero&&(int)posicion.y>0);
    }

    public Vector2 Aproximacion(Vector2 posicion)
    {
        return new Vector2(Mathf.Round(posicion.x), Mathf.Round(posicion.y));
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}
