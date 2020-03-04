using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activation_levier : MonoBehaviour {

    public bool pressed = false;
    public float max = 3f;
    public GameObject updoor;
    private Transform uptransform;
    private float starting;
    private bool fliped = false;
    private bool trigered = false;

    void Start()
    {
        uptransform = updoor.GetComponent<Transform>();
        starting = uptransform.position.y;
    }

    void Update()
    {
        if (trigered == true && Input.GetKeyDown(KeyCode.E)) {
            if (pressed == false)
                pressed = true;
            else
                pressed = false;
        }

        if (uptransform.position.y < starting + max && pressed == true)
            {
                uptransform.transform.Translate(new Vector2(0, 1) * 1 * Time.deltaTime);
                if (fliped == false)
                {
                    fliped = true;
                    this.transform.localScale = new Vector3(-1, 1, 1);
                }
            }

        if (uptransform.position.y > starting && pressed == false)
        {
            uptransform.transform.Translate(new Vector2(0, 1) * -1 * Time.deltaTime);
            if (fliped == true)
            {
                fliped = false;
                this.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            trigered = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            trigered = false;
    }
}
