using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    public GameObject wasdObject; // a Canvas-en l�v� W, A, S, D objektum (tutorialhoz)
    public Transform playerTransform; // a j�t�kos poz�ci�j�hoz
    public Vector3 offset; // offset az objektum poz�ci�j�nak finom�t�s�hoz

    void Start()
    {
        if (wasdObject != null)
        {
            wasdObject.SetActive(true); 
        }
        else
        {
            Debug.LogError("nincs W, A, S, D object hozz�adva");
        }
    }

    void Update()
    {
        if (wasdObject != null && wasdObject.activeSelf) // ellen�rzi, hogy az objektum be van-e kapcsolva
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            {
                wasdObject.SetActive(false); // kikapcsolja a W, A, S, D objektumot
            }
        }
    }
}