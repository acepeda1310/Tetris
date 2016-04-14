using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    public void JugarDeNuevo()
    {
        SceneManager.LoadScene("Nivel01");
    }
}
