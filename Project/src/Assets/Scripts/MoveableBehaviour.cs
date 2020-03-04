using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableBehaviour : MonoBehaviour
{
    /// <summary>
    /// Update
    /// </summary>
    void Update()
    {
        if (GetComponent<Rigidbody2D>().velocity.y == 0)
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        }
    }
}
