using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class Key : MonoBehaviour
{
    private TilemapRenderer rend;
    public bool hasKey = false;
    public GameObject key;
    // Start is called before the first frame update
    void Start()
    {
        rend = key.GetComponent<TilemapRenderer>();
        rend.sortingLayerName = "Foreground";
        rend.sortingOrder = 0;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Key")) {
            hasKey = true;
            rend.sortingLayerName = "Background";
            rend.sortingOrder = -10; 
        }

        if (other.CompareTag("Door") && hasKey) {
            SceneManager.LoadScene("End");
        }
    }
}
