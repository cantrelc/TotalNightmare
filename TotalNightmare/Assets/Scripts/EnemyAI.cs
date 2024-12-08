using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public GameObject player;
    public Transform movePoint;
    public float speed = .01f;
    private float distance;
    private int direction;                  //vertical = 0; horizontal = 1
    public float patrolLength;              //must be cleanly divisible by 2
    private float patrolEndPos;
    private float patrolEndNeg;
    private bool patrolForward = true;
    public float horizontalDistanceMoved = 0.0f;
    private float verticalDistanceMoved = 0.0f;
    public LayerMask whatStopsMovement;

    private float absXDistance;
    private float absYDistance;

    public SpriteRenderer spriteRenderer;
    public Sprite spriteRight;
    public Sprite spriteLeft;
    public Sprite spriteFront;
    public Sprite spriteBack;

    public PlayerHealth playerHealth;
    public PlayerHide playerHide;
    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;

        patrolEndPos = patrolLength / 2;
        patrolEndNeg = -1 * (patrolLength / 2);
    }

    // Update is called once per frame
    void Update()
    {
        //stop chasing if player is already dead
        if (playerHealth.health <= 0)
        {
            return;
        }

        //calculate distance between player and enemy
        distance = Vector2.Distance(transform.position, player.transform.position);
        absXDistance = Mathf.Abs(transform.position.x - player.transform.position.x);
        absYDistance = Mathf.Abs(transform.position.y - player.transform.position.y);

        //move towards player
        if (!playerHide.isHiding && distance > 0.1)
        {
            speed = 4f;

            //move enemy
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, speed * Time.deltaTime);
 
            //only update move point position if enemy is at same position as move point
            float dist = Vector3.Distance(movePoint.position, transform.position);
            if(dist == 0)
            {
                if (absXDistance > absYDistance)
                {
                    MoveHorizontal();
                }
                else if (absXDistance < absYDistance)
                {
                    MoveVertical();
                }
                else
                {
                    MoveHorizontal();
                }
            }
        }
        else            //patrol
        {
            speed = 1f;

            //move enemy
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, speed * Time.deltaTime);

            //patrol horizontally if previous movement was on y axis
            if (direction == 0)
            {
                PatrolHorizontal(patrolEndPos, patrolEndNeg);
            }
            //patrol vertically if previous movement was on x axis
            else if (direction == 1)
            {
                PatrolVertical(patrolEndPos, patrolEndNeg);
            }
        }
    }

    /* Update the move point position on the x axis */
    void MoveHorizontal()
    {
        float xDistance = transform.position.x - player.transform.position.x;

        if (xDistance < 0)
        {
            //move right
            if (!Physics2D.OverlapCircle(movePoint.position +
                new Vector3(1f, 0f, 0f), 0.2f, whatStopsMovement))
            {
                movePoint.position += new Vector3(1f, 0f, 0f);
                spriteRenderer.sprite = spriteRight;
            }
            else    //cannot move right, try to move vertical
            {
                MoveVertical();
            }
        } 
        else
        {
            //move left
            if (!Physics2D.OverlapCircle(movePoint.position +
                new Vector3(-1f, 0f, 0f), 0.2f, whatStopsMovement))
            {
                movePoint.position += new Vector3(-1f, 0f, 0f);
                spriteRenderer.sprite = spriteLeft;
            }
            else    //cannot move left, try to move vertical
            {
                MoveVertical();
            }
        }

        direction = 1;      //last moved direction was horizontal
    }

    /* Enemy patrols horizontally between endPos and endNeg */
    void PatrolHorizontal(float patrolEndPos, float patrolEndNeg)
    {
        float dist = Vector3.Distance(movePoint.position, transform.position);
        if (dist == 0)
        {
            if (patrolForward)
            {
                //move right
                if (horizontalDistanceMoved < patrolEndPos && !Physics2D.OverlapCircle(movePoint.position +
                    new Vector3(1f, 0f, 0f), 0.2f, whatStopsMovement))
                {
                    movePoint.position += new Vector3(1f, 0f, 0f);
                    spriteRenderer.sprite = spriteRight;
                    horizontalDistanceMoved++;
                }
                else
                {
                    //reached end of patrol or obstacle prevented movement; reverse direction
                    patrolForward = false;
                }
            }
            else
            {
                //move left
                if (horizontalDistanceMoved > patrolEndNeg && !Physics2D.OverlapCircle(movePoint.position +
                    new Vector3(-1f, 0f, 0f), 0.2f, whatStopsMovement))
                {
                    movePoint.position += new Vector3(-1f, 0f, 0f);
                    spriteRenderer.sprite = spriteLeft;
                    horizontalDistanceMoved--;
                }
                else
                {
                    //reached end of patrol or obstacle prevented movement; reverse direction
                    patrolForward = true;
                }
            }
        }
    }

    /* Update the move point position on the y axis */
    void MoveVertical()
    {
        float yDistance = transform.position.y - player.transform.position.y;

        if (yDistance < 0)
        {
            //move up
            if (!Physics2D.OverlapCircle(movePoint.position +
                    new Vector3(0f, 1f, 0f), 0.2f, whatStopsMovement))
            {
                movePoint.position += new Vector3(0f, 1f, 0f);
                spriteRenderer.sprite = spriteBack;
            } 
            else    //cannot move up, try to move horizontal
            {
                MoveHorizontal();
            }
        } 
        else
        {
            //move down
            if (!Physics2D.OverlapCircle(movePoint.position +
                        new Vector3(0f, -1f, 0f), 0.2f, whatStopsMovement))
            {
                movePoint.position += new Vector3(0f, -1f, 0f);    
                spriteRenderer.sprite = spriteFront;
            }
            else    //cannot move down, try to move horizontal
            {
                MoveHorizontal();
            }
        }

        direction = 0;      //last moved direction was vertical
    }

    /* Enemy patrols vertically between endPos and endNeg */
    void PatrolVertical(float patrolEndPos, float patrolEndNeg)
    {
        float dist = Vector3.Distance(movePoint.position, transform.position);
        if (dist == 0)
        {
            if (patrolForward)
            {
                //move up
                if (verticalDistanceMoved < patrolEndPos && !Physics2D.OverlapCircle(movePoint.position +
                        new Vector3(0f, 1f, 0f), 0.2f, whatStopsMovement))
                {
                    movePoint.position += new Vector3(0f, 1f, 0f);
                    spriteRenderer.sprite = spriteBack;
                    verticalDistanceMoved++;
                }
                else
                {
                    //reached end of patrol or obstacle prevented movement; reverse direction
                    patrolForward = false;
                }
            }
            else
            {
                //move down
                if (verticalDistanceMoved > patrolEndNeg && !Physics2D.OverlapCircle(movePoint.position +
                            new Vector3(0f, -1f, 0f), 0.2f, whatStopsMovement))
                {
                    movePoint.position += new Vector3(0f, -1f, 0f);
                    spriteRenderer.sprite = spriteFront;
                    verticalDistanceMoved--;
                }
                else
                {
                    //reached end of patrol or obstacle prevented movement; reverse direction
                    patrolForward = true;
                }
            }
        }        
    }

    /* Check for collisions */
    private void OnCollisionEnter2D(Collision2D other)
    {
        //if other object is the player, damage (kill) the player
        if (other.gameObject.CompareTag("Player"))
        {
            playerHealth.health -= damage;
        }
    }
}
