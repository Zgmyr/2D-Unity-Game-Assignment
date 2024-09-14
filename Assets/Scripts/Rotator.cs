// Author: Zachary Gmyr
// 9/14/2024
// This script specifies the behavior of the "Pickup" items, where movement of these game objects are randomized.


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{

    public float speed;
    private Vector2 direction; // will be randomized upon start

    // Start is called before the first frame update
    void Start()
    {
        SetRandomDirection();
    }

    // Update is called once per frame
    void Update()
    {
        // rotation of the pickup
        // transform.Rotate(new Vector3(0,0,45) * Time.deltaTime);

        // random movement of the pickup
        transform.Translate(direction * speed * Time.deltaTime);
    }

    // generates a random direction of magnitude '1'
    void SetRandomDirection()
    {
        // generate random x & y decimal values to create random vector
        float randomX = Random.Range(-1f,1f);
        float randomY = Random.Range(-1f,1f);

        // creating vector out of random X & Y values
        // NOTE: magnitude of this vector may be variable
        direction = new Vector2(randomX,randomY);

        // normalize direction vector to make sure speed is consistent
        // Source: https://docs.unity3d.com/ScriptReference/Vector2.Normalize.html
        direction.Normalize();
    }

    // collision changes direction
    void OnCollisionEnter2D(Collision2D other) {
        // if pickup collides with a wall or other pickup, change its direction
        if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Pickup"))
        {
            // Vector2.Reflect() "Reflects a vector off the surface defined by a normal" ~ unity documentation
            // parameters: 'inDirection' (current direction vector) and 'inNormal' (normal vector, perpendicular at point of collision)
            // Note: other.contacts is an array of contact points between the colliders involved in collision, and
            //      contacts[0] specifies the first contact point
            // Source: https://docs.unity3d.com/ScriptReference/Vector2.Reflect.html
            direction = Vector2.Reflect(direction, other.contacts[0].normal);
        }
    }


}
