using System.Linq;
using UnityEngine;

public class SpawnMonster : MonoBehaviour
{
    public GameObject objectToSpawn;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //spawn object
            objectToSpawn.SetActive(true);

            //disable spawn trigger
            GameObject trigger = GameObject.FindGameObjectsWithTag("SpawnTrigger").FirstOrDefault();
            if (trigger != null)
            {
                trigger.SetActive(false);
            }
        }
    }
}
