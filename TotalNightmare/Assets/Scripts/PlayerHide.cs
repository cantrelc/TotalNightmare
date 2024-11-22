using UnityEngine;

public class PlayerHide : MonoBehaviour
{
    public bool isHiding = false;
    private bool canHide = false;
    private SpriteRenderer rend;
    public PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canHide && !isHiding && Input.GetKeyDown(KeyCode.F))
        {
            Physics2D.IgnoreLayerCollision(8, 9, true);
            rend.sortingLayerName = "Background";
            rend.sortingOrder = -1;
            isHiding = true;
        }
        else if (isHiding && Input.GetKeyDown(KeyCode.F))
        {
            Physics2D.IgnoreLayerCollision(8, 9, false);
            rend.sortingLayerName = "Foreground";
            rend.sortingOrder = 1;
            isHiding = false;
        }

        if (isHiding)
        {
            playerMovement.enabled = false;
        }
        else
        {
            playerMovement.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("touch");
        if (other.CompareTag("Hide")) {
            canHide = true;
            Debug.Log("Hide");
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Hide")) {
            canHide = false;
        }
    }
}
