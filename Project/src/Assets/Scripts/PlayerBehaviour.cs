using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public Transform groundCheck;

    bool isRabbearColliding = false;
    bool isRabbearCollidingMoveable = false;

    GameObject collidingMoveable = null;

    /// <summary>
    /// Start
    /// </summary>
    void Start ()
    {
        //grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
    }
	
    /// <summary>
    /// Start
    /// </summary>
	void Update ()
    {
        if (Mathf.Abs(Vector3.Distance(GameManager.instance.currentPlayer.transform.position, GameManager.instance.otherPlayer.transform.position)) < 3.0f)
        {
            isRabbearColliding = true;
        }
        else
        {
            isRabbearColliding = false;
        }

        // Only player1 can grab and lift, and only if the 2 players are colliding
		if(Input.GetKeyDown(KeyCode.P) && GameManager.instance.isFocusOnRabbear)
        {
            if(isRabbearColliding && !GameManager.instance.isLiftingMoveable)
            {
                GameManager.instance.isLifting = true;
            }
            else if (isRabbearCollidingMoveable && !GameManager.instance.isLifting)
            {
                GameManager.instance.isLiftingMoveable = true;
                GameManager.instance.objectToMove = collidingMoveable;
                //GameManager.instance.grabMoveable(collidingMoveable, new Vector3(transform.position.x, transform.position.y + 4.0f, 0));
            }
        }

        if (Input.GetKeyDown(KeyCode.P) && GameManager.instance.isFocusOnRabbear && GameManager.instance.isAnimLiftActive() && GameManager.instance.objectToMove != null)
        {
            Physics2D.IgnoreCollision(GameManager.instance.rabbear.gameObject.GetComponent<Collider2D>(), GameManager.instance.objectToMove.gameObject.GetComponent<Collider2D>(), false);
            GameManager.instance.objectToMove.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            GameManager.instance.objectToMove.GetComponent<Rigidbody2D>().velocity = Vector2.up * 5;
            GameManager.instance.objectToMove = null;
            GameManager.instance.stopLifting();
        }

        if ( Input.GetKeyDown(KeyCode.Space) && (GameManager.instance.grounded 
            || (GameManager.instance.pragma.GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Kinematic)) )
        {
            // if the monster try to jump
            if(GameManager.instance.isFocusOnRabbear)
            {
                //if (GameManager.instance.pragma.GetComponent<Rigidbody2D>().bodyType != RigidbodyType2D.Kinematic)
                if (!GameManager.instance.isAnimLiftActive())
                {
                    GameManager.instance.currentPlayer.GetComponent<Rigidbody2D>().AddForce(Vector2.up * GameManager.instance.rabbearJump);
                }
            }
            else
            {
                if(GameManager.instance.pragma.GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Kinematic)
                {
                    GameManager.instance.currentPlayer.GetComponent<Rigidbody2D>().velocity = Vector2.up * 9.5f;
                    GameManager.instance.pragma.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    GameManager.instance.stopLifting();
                }
                else
                {
                    GameManager.instance.currentPlayer.GetComponent<Rigidbody2D>().AddForce(Vector2.up * GameManager.instance.pragmaJump);
                }
            }
        }
    }

    /// <summary>
    /// On collision enter, other player do not move
    /// </summary>
    /// <param name="other"></param>
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Moveable")
        {
            isRabbearCollidingMoveable = true;
            collidingMoveable = other.gameObject;
            //if(GameManager.instance.isFocusOnPlayer1 && other.gameObject.name == "player2" ||
            //  !GameManager.instance.isFocusOnPlayer1 && other.gameObject.name == "player1")
            //{
            //    other.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            //}
        }

        //else if(other.gameObject.tag == "Fixer" && gameObject.name == "Pragma")
        //{
        //    gameObject.transform.parent = other.gameObject.transform;
        //}
    }

    /// <summary>
    /// On collision exit, other player is back to dynamic
    /// </summary>
    /// <param name="other"></param>
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Moveable")
        {
            isRabbearCollidingMoveable = false;
            collidingMoveable = null;
            //if (GameManager.instance.isFocusOnPlayer1 && other.gameObject.name == "player2" ||
            //  !GameManager.instance.isFocusOnPlayer1 && other.gameObject.name == "player1")
            //{
            //    other.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            //}
        }

        //else if (other.gameObject.tag == "Fixer" && gameObject.name == "Pragma")
        //{
        //    gameObject.transform.parent = null;
        //}
    }
}
