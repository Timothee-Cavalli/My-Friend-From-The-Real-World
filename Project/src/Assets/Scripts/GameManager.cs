using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Instance of the game manager
    /// </summary>
    public static GameManager instance = null;

    public GameObject rabbear;
    public GameObject pragma;

    public GameObject pragmaGrabPlatform;

    public Transform pragmaGroundCheck;
    public Transform rabbearGroundCheck;

    Transform rabbearTransform;
    Transform pragmaTransform;
    public Camera mainCamera;

    [HideInInspector] public GameObject currentPlayer;
    [HideInInspector] public GameObject otherPlayer;

    [HideInInspector] public bool isFocusOnRabbear = true;
    [HideInInspector] public bool isLifting = false;
    [HideInInspector] public bool isLiftingMoveable = false;
    [HideInInspector] public Vector3 endGrabPosition;
    [HideInInspector] public bool grounded;
    [HideInInspector] public GameObject objectToMove = null;

    public float pragmaSpeed = 10.0f;
    public float rabbearSpeed = 5f;
    public float grabSpeed = 1.0f;

    public float pragmaJump = 200f;
    public float rabbearJump = 140f;

    float groundRadius = 0.1f;
    public LayerMask whatIsGround;

    float speed;
    float startTime;

    bool facingRight = false;

    bool facingRightRabbear = false;
    bool facingRightPragma = false;

    bool pragmaGrounded = false;
    bool rabbearGrounded = true;

    Animator rabbearAnimator;
    Animator pragmaAnimator;


    /// <summary>
    /// Awake.
    /// </summary>
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;
        }

        //If instance already exists and it's not this:
        else if (instance != this)
        {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }

        //Sets this to not be destroyed when reloading scene
        //DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Start
    /// </summary>
    void Start ()
    {
        rabbearTransform = rabbear.transform;
        pragmaTransform = pragma.transform;

        currentPlayer = rabbear;
        otherPlayer = pragma;
        mainCamera.transform.position = new Vector3(rabbearTransform.position.x, rabbearTransform.position.y + 5, -10);

        speed = rabbearSpeed;

        rabbearAnimator = rabbear.GetComponent<Animator>();
        pragmaAnimator = pragma.GetComponent<Animator>();

        Physics2D.IgnoreCollision(rabbear.gameObject.GetComponent<Collider2D>(), pragma.gameObject.GetComponent<Collider2D>(), true);
        //Physics2D.IgnoreCollision(rabbear.gameObject.GetComponent<Collider2D>(), pragmaGrabPlatform.gameObject.GetComponent<Collider2D>(), true);

        switchPlayer();
    }

    /// <summary>
    /// Update
    /// </summary>
    void Update ()
    {
		if(Input.GetKeyDown(KeyCode.T))
        {
            //StartCoroutine("cameraLerp");
            Vector3 velocity = Vector3.zero;

            switchPlayer();
        }

        // If the amima is close enough to the girl and the player have pressed 'P'
        if(isLifting)
        {
            rabbearAnimator.SetBool("isLifting", true);
            pragma.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            Vector3 endGrabPosition = new Vector3(rabbear.transform.position.x, rabbear.transform.position.y + 5.0f, 0);
            grabAndLift(endGrabPosition);
        }

        // If the amima is close enough to a moveable object and the player have pressed 'P'
        if (isLiftingMoveable)
        {
            rabbearAnimator.SetBool("isLifting", true);
            objectToMove.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            Physics2D.IgnoreCollision(rabbear.gameObject.GetComponent<Collider2D>(), objectToMove.gameObject.GetComponent<Collider2D>(), true);
            Vector3 endGrabPosition = new Vector3(rabbear.transform.position.x, rabbear.transform.position.y + 6.0f, 0);
            grabMoveable(endGrabPosition);
        }

        if (isFocusOnRabbear)
        {
            grounded = Physics2D.OverlapCircle(rabbearGroundCheck.position, groundRadius, whatIsGround);
        }
        else
        {
            grounded = Physics2D.OverlapCircle(pragmaGroundCheck.position, groundRadius, whatIsGround);
        }

        pragmaGrounded = Physics2D.OverlapCircle(pragmaGroundCheck.position, groundRadius, whatIsGround);

        //if ( (!isLifting && !(girl.GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Kinematic))
        //    && (! isLiftingMoveable  )
        if ( (!rabbearAnimator.GetBool("isLifting") && isFocusOnRabbear) || (!isFocusOnRabbear && pragma.GetComponent<Rigidbody2D>().bodyType != RigidbodyType2D.Kinematic) )
        {
            float playerSpeed = Input.GetAxisRaw("Horizontal") * Time.deltaTime * speed;
            currentPlayer.transform.Translate(playerSpeed, 0, 0);

            if (isFocusOnRabbear)
            {
                rabbearAnimator.SetFloat("Speed", Mathf.Abs(Input.GetAxisRaw("Horizontal")));
            }
            else
            {
                pragmaAnimator.SetFloat("Speed", Mathf.Abs(Input.GetAxisRaw("Horizontal")));
            }

            Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), currentPlayer.gameObject.GetComponent<Rigidbody2D>().velocity.y);

            if (direction.x > 0 && !facingRight)
                Flip();
            else if (direction.x < 0 && facingRight)
                Flip();
        }

        pragmaAnimator.SetBool("Grounded", pragmaGrounded);
    }

    IEnumerator cameraLerp()
    {
        //Vector3 startPosition ;
        //Vector3 endPosition;
        //Vector3 velocity = Vector3.zero;

        //if (currentFocusPlayer1)
        //{
        //    currentFocusPlayer1 = false;
        //    startPosition = new Vector3(player1.position.x, player1.position.y, mainCamera.transform.position.z);
        //    endPosition = new Vector3(player2.position.x, player2.position.y, mainCamera.transform.position.z);
        //}
        //else
        //{
        //    currentFocusPlayer1 = true;
        //    startPosition = new Vector3 (player2.position.x, player2.position.y, mainCamera.transform.position.z);
        //    endPosition = new Vector3(player1.position.x, player1.position.y, mainCamera.transform.position.z);
        //}

        //float journeyLength = Vector3.Distance(startPosition, endPosition);
        //float fracJourney = Time.deltaTime * speed / journeyLength;

        ////mainCamera.transform.position = Vector3.SmoothDamp(startPosition, endPosition, ref velocity, 0.3f);
        //mainCamera.transform.position = Vector3.Lerp(startPosition, endPosition, Time.deltaTime * 0.3f);

        yield return null;
    }

    /// <summary>
    /// Switch current and other player
    /// </summary>
    void switchPlayer()
    {
        Vector3 startPosition;
        Vector3 endPosition;

        if (isFocusOnRabbear)
        {
            isFocusOnRabbear = false;
            currentPlayer = pragma;
            otherPlayer = rabbear;
            speed = pragmaSpeed;

            facingRightRabbear = facingRight;
            facingRight = facingRightPragma;

            rabbearAnimator.SetFloat("Speed", 0);
        }
        else
        {
            isFocusOnRabbear = true;
            currentPlayer = rabbear;
            otherPlayer = pragma;
            speed = rabbearSpeed;

            facingRightPragma = facingRight;
            facingRight = facingRightRabbear;

            pragmaAnimator.SetFloat("Speed", 0);
        }

        startPosition = new Vector3(currentPlayer.transform.position.x, currentPlayer.transform.position.y, mainCamera.transform.position.z);
        endPosition = new Vector3(otherPlayer.transform.position.x, otherPlayer.transform.position.y + 10, mainCamera.transform.position.z);

        //float journeyLength = Vector3.Distance(startPosition, endPosition);
        //float fracJourney = Time.deltaTime * speed / journeyLength;

        //mainCamera.transform.position = Vector3.SmoothDamp(startPosition, endPosition, ref velocity, 0.3f);
        mainCamera.transform.position = Vector3.Lerp(startPosition, endPosition, Time.deltaTime * 0.3f);
        mainCamera.transform.parent = currentPlayer.transform;
    }


    /// <summary>
    /// Player2 grabs player1 and lift them over his head
    /// </summary>
    public void grabAndLift(Vector3 endPosition)
    {
        Debug.Log("Grab and lift");
        //girl.transform.position = Vector3.Lerp(girl.transform.position, new Vector3(rabbear.transform.position.x, 0, 0), 0.05f * grabSpeed);
        //if( Mathf.Abs(girl.transform.position.x - rabbear.transform.position.x) < 0.1f && isLifting )
        //{
            //girl.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            //girlGrabPlatform.SetActive(true);

            //girlGrabPlatform.transform.position = Vector3.Lerp(girlGrabPlatform.transform.position, endPosition, Time.deltaTime * grabSpeed);
            pragma.transform.position = Vector3.Lerp(pragma.transform.position, endPosition, 0.05f * grabSpeed);

            Debug.Log(Mathf.Abs(Vector3.Distance(pragma.transform.position, endPosition)));

            if ( Mathf.Abs(Vector3.Distance(pragma.transform.position, endPosition)) < 1.45f && isLifting)
            {
                Debug.Log("Stop lift");
                pragma.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                //girlGrabPlatform.SetActive(false);
                isLifting = false;
            }
        //}
    }
    
    /// <summary>
    /// When rabbear grab an object (not the girl)
    /// </summary>
    /// <param name="moveable"></param>
    void grabMoveable(Vector3 endPosition)
    {
        objectToMove.transform.position = Vector3.Lerp(objectToMove.transform.position, endPosition, 0.05f * grabSpeed);

        Debug.Log(Mathf.Abs(Vector3.Distance(objectToMove.transform.position, endPosition)));

        if (Mathf.Abs(Vector3.Distance(objectToMove.transform.position, endPosition)) < 1.45f && isLiftingMoveable)
        {
            Debug.Log("Stop lift");
            //moveable.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            //girlGrabPlatform.SetActive(false);
            isLiftingMoveable = false;
        }
    }

    /// <summary>
    /// Flip the sprite
    /// </summary>
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = GameManager.instance.currentPlayer.transform.localScale;
        theScale.x *= -1;
        GameManager.instance.currentPlayer.transform.localScale = theScale;
    }

    /// <summary>
    /// Stop lifting animation
    /// </summary>
    public void stopLifting()
    {
        rabbearAnimator.SetBool("isLifting", false);
    }

    /// <summary>
    /// Return anim state
    /// </summary>
    public bool isAnimLiftActive()
    {
        return rabbearAnimator.GetBool("isLifting");
    }
}
