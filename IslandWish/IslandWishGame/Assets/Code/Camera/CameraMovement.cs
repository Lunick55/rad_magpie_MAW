using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private float xOffset = 0, yOffset = 0, zOffset = 0;
    [SerializeField] Transform offsetSource;

    // Start is called before the first frame update
    void Awake()
    {
        //set up the offsets so the camera doesn't hug what its following
        xOffset = transform.position.x - offsetSource.position.x;
        yOffset = transform.position.y - offsetSource.position.y;
        zOffset = transform.position.z - offsetSource.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        //follow something around
        transform.position = offsetSource.position + new Vector3(xOffset, yOffset, zOffset);
    }
}
