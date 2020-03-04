using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DalleActivation : MonoBehaviour
{
    bool pressed = false;
    public float max = 5f;
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
        if (uptransform.position.y > starting - max && pressed == true)
        {
            uptransform.transform.Translate(new Vector2(0, -1) * 1 * Time.deltaTime);
        }

        if (uptransform.position.y < starting && pressed == false)
        {
            uptransform.transform.Translate(new Vector2(0, -1) * -1 * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            pressed = true;
            gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("images/Sprite/Dalle_flanc_on");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            pressed = false;
            gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("images/Sprite/Dalle_flanc_off");
        }
    }
}
