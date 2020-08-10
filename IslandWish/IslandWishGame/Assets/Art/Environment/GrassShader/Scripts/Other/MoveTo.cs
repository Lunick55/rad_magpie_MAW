using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : MonoBehaviour
{
    public GameObject _Objective;
    public float _Speed;

    void Start()
    {
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Vector3 dir = Vector3.Normalize(_Objective.transform.position - transform.position);

        transform.Translate(Vector3.left * Time.deltaTime * _Speed, Space.World);
    }
}
