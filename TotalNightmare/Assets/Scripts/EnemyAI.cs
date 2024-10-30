using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public GameObject player;
    public float speed = 5f;
    public Transform movePoint;
    private float distance;
    public float visionDistance;
    public float patrolLength;              //must be cleanly divisible by 2
    public LayerMask whatStopsMovement;

    private float absXDistance;
    private float absYDistance;

    public SpriteRenderer spriteRenderer;
    public Sprite spriteRight;
    public Sprite spriteLeft;
    public Sprite spriteFront;
    public Sprite spriteBack;

    public PlayerHealth playerHealth;
    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
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
        //TODO: add check for 'if player is not hiding' and change first check to <
        if (distance > visionDistance && distance > 0.1)
        {
            speed = 5f;

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
        else
        {
            speed = 2f;

            //move enemy
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, speed * Time.deltaTime);

            //TODO: patrol
            float patrolEndPos = patrolLength / 2;
            float patrolEndNeg = -1 * (patrolLength / 2);
            float distanceMoved = 0;

            float dist = Vector3.Distance(movePoint.position, transform.position);
            if (dist == 0)
            {
                //patrol vertically
                if (spriteRenderer.sprite == spriteRight || spriteRenderer.sprite == spriteLeft)
                {
                    //while (true)        //change to 'while hiding'
                    {
                        PatrolVertical(distanceMoved, patrolEndPos, patrolEndNeg);
                    }
                }
                //patrol horizontally
                else if (spriteRenderer.sprite == spriteFront || spriteRenderer.sprite == spriteBack)
                {
                    //while (true)        //change to 'while hiding'
                    {
                        PatrolHorizontal(distanceMoved, patrolEndPos, patrolEndNeg);
                    }
                }
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
        } else
        {
            //move left
            if (!Physics2D.OverlapCircle(movePoint.position +
                new Vector3(-1f, 0f, 0f), 0.2f, whatStopsMovement))
            {
                movePoint.position += new Vector3(-1f, 0f, 0f);
                spriteRenderer.sprite = spriteLeft;
            }
        }
    }

    /* Enemy patrols horizontally between endPos and endNeg */
    void PatrolHorizontal(float distanceMoved, float patrolEndPos, float patrolEndNeg)
    {
        while (distanceMoved < patrolEndPos)
        {
            //move right
            if (!Physics2D.OverlapCircle(movePoint.position +
                new Vector3(1f, 0f, 0f), 0.2f, whatStopsMovement))
            {
                movePoint.position += new Vector3(1f, 0f, 0f);
                spriteRenderer.sprite = spriteRight;
                distanceMoved++;
            }
            else
            {
                //obstacle prevented movement, update distanceMoved so the loop is exited
                distanceMoved = patrolEndPos;
            }
        }

        while (distanceMoved > patrolEndNeg)
        {
            //move left
            if (!Physics2D.OverlapCircle(movePoint.position +
                new Vector3(-1f, 0f, 0f), 0.2f, whatStopsMovement))
            {
                movePoint.position += new Vector3(-1f, 0f, 0f);
                spriteRenderer.sprite = spriteLeft;
                distanceMoved--;
            }
            else
            {
                //obstacle prevented movement, update distanceMoved so the loop is exited
                distanceMoved = patrolEndNeg;
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
        } else
        {
            //move down
            if (!Physics2D.OverlapCircle(movePoint.position +
                        new Vector3(0f, -1f, 0f), 0.2f, whatStopsMovement))
            {
                movePoint.position += new Vector3(0f, -1f, 0f);    
                spriteRenderer.sprite = spriteFront;
            }
        }
    }

    /* Enemy patrols vertically between endPos and endNeg */
    void PatrolVertical(float distanceMoved, float patrolEndPos, float patrolEndNeg)
    {
        while (distanceMoved < patrolEndPos)
        {
            //move up
            if (!Physics2D.OverlapCircle(movePoint.position +
                    new Vector3(0f, 1f, 0f), 0.2f, whatStopsMovement))
            {
                movePoint.position += new Vector3(0f, 1f, 0f);
                spriteRenderer.sprite = spriteBack;
                distanceMoved++;
            }
            else
            {
                //obstacle prevented movement, update distanceMoved so the loop is exited
                distanceMoved = patrolEndPos;
            }
        }
        
        while (distanceMoved > patrolEndNeg)
        {
            //move down
            if (!Physics2D.OverlapCircle(movePoint.position +
                        new Vector3(0f, -1f, 0f), 0.2f, whatStopsMovement))
            {
                movePoint.position += new Vector3(0f, -1f, 0f);
                spriteRenderer.sprite = spriteFront;
                distanceMoved--;
            }
            else
            {
                //obstacle prevented movement, update distanceMoved so the loop is exited
                distanceMoved = patrolEndNeg;
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
