using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {
    [SerializeField] private int fOV;
    [SerializeField] private GameObject bulletPreFab;
    [SerializeField] private GameObject bulletSpawnPoint;
    [SerializeField] private float bulletSpeed;
    private Rigidbody2D rig;

	void Start () {
        rig = GetComponent<Rigidbody2D>();
	}

    void Update() {
        
	}

    void FOV() {

    }

    void Move() {
        
    }

    void Shoot() {
        GameObject BulletClone = (GameObject)Instantiate(bulletPreFab, bulletSpawnPoint.transform.position, transform.rotation);
        BulletClone.GetComponent<Rigidbody2D>().AddForce(transform.right * bulletSpeed);
        Destroy(BulletClone, 2f);
    }
}
