using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public Transform movePoint;

    public LayerMask whatStopsMovement;

    public SpriteRenderer spriteRenderer;
    public Sprite spriteRight;
    public Sprite spriteLeft;
    public Sprite spriteFront;
    public Sprite spriteBack;

    private void Start()
    {
        movePoint.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        //move to the move point
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        {
            //move horizontal
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                //determine which way to face the sprite
                if (Input.GetAxisRaw("Horizontal") == 1f)
                {
                    spriteRenderer.sprite = spriteRight;
                }
                else if (Input.GetAxisRaw("Horizontal") == -1f)
                {
                    spriteRenderer.sprite = spriteLeft;
                }

                //update the move point - walls stop movement
                if (!Physics2D.OverlapCircle(movePoint.position + 
                    new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), 0.2f, whatStopsMovement))
                {
                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                }
                
            }
            //move vertical
            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                //determine which way to face the sprite
                if (Input.GetAxisRaw("Vertical") == 1f)
                {
                    spriteRenderer.sprite = spriteBack;
                }
                else if (Input.GetAxisRaw("Vertical") == -1f)
                {
                    spriteRenderer.sprite = spriteFront;
                }

                //update the move point - walls stop movement
                if (!Physics2D.OverlapCircle(movePoint.position +
                    new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), 0.2f, whatStopsMovement))
                {
                    movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                }
            }
        }
    }
}
