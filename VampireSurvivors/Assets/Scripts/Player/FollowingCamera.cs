using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    [SerializeField] private Transform target;

    Vector3 camOffset;

    void Start()
    {
        camOffset = transform.position - Player.Instance.transform.position;
    }

    private void FixedUpdate()
    {
        transform.position = Player.Instance.transform.position + camOffset;
    }
}
