using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    public Transform playerTransform;
    [SerializeField] public float speed;
    [SerializeField] public float rotationSpeed;
    [SerializeField] public float radius;
    private float distance;
    private float angle;

    void Start()
    {
        
    }

    void Update()
    {
        angle += rotationSpeed * Time.deltaTime;
        radius = Vector2.Distance(transform.position, playerTransform.position);
        float yOffset = Mathf.Sin(angle * Mathf.Deg2Rad) * radius; 
        float xOffset = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
        Vector3 nextPosition = new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, transform.position.z);
         radius -= speed * Time.deltaTime;

        //distance = Vector2.Distance(transform.position, playerTransform.position);
        //transform.position = Vector2.MoveTowards(this.transform.position, playerTransform.position, speed * Time.deltaTime);

        transform.position = nextPosition;
    }
}
