using UnityEngine;
using System.Collections;
// Author: Vincent Versnel
// Bullet script for a muzzle flash
// only moves an object in one direction
public class BulletScript : MonoBehaviour
{
    public int bulletSpeed = 200;
    public ElementType ele;

    void Update()
    {
        transform.Translate(Vector2.right * Time.deltaTime * bulletSpeed);
        Destroy(gameObject, 1f);
    }
}
