using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrolling : MonoBehaviour {

    private Transform cameraTransform;
    private Transform[] layer;
    private float viewZone = 5f;
    private int leftIndex;
    private int rightIndex;
    private float lastcameraX;

    public float backgroundsize;
    public float speed;
	void Start ()
    {
        speed *= -1;
        cameraTransform = Camera.main.transform;
        layer = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            layer[i] = transform.GetChild(i);
        lastcameraX = cameraTransform.position.x;
        leftIndex = 0;
        rightIndex = layer.Length - 1;
   	}
	

	// Update is called once per frame
	void Update ()
    {
        float deltaX = cameraTransform.position.x - lastcameraX;
        transform.position += Vector3.right * (deltaX * speed);
        lastcameraX = cameraTransform.position.x;

        if (cameraTransform.position.x < (layer[leftIndex].transform.position.x + viewZone))
            scroll_left();
        if (cameraTransform.position.x > (layer[rightIndex].transform.position.x - viewZone))
            scroll_right();
    }

    private void scroll_left ()
    {
        layer[rightIndex].position = Vector3.right * (layer[leftIndex].position.x - backgroundsize);
        leftIndex = rightIndex;
        rightIndex--;
        if (rightIndex < 0)
            rightIndex = layer.Length - 1;
    }

    private void scroll_right()
    {
        layer[leftIndex].position = Vector3.right * (layer[rightIndex].position.x + backgroundsize);
        rightIndex = leftIndex;
        leftIndex++;
        if (leftIndex == layer.Length)
            leftIndex = 0;
    }
}