using UnityEngine;
using System.Collections;

public class Generador : MonoBehaviour {

    public GameObject[] objetos;
    public static GameObject[] obj;
    public static int siguiente;

	// Use this for initialization
	void Start () {
        obj = objetos;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static void Generar()
    {
        Instantiate(obj[siguiente], new Vector2(5.0f,20.0f), Quaternion.identity);
        siguiente = Random.Range(0, obj.Length);
        //Instantiate(obj[siguiente], new Vector2(15.0f, 10.0f), Quaternion.identity);
    }
}
