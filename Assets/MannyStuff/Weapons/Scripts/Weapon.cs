using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(LineRenderer))]
public class Weapon : MonoBehaviour {

    public int damage = 10;
    public int maxAmmo = 500;
    public int maxClip = 30;
    public float range = 10f;
    public float shootRate = .2f;
    public float lineDelay = .1f;
    public Transform shotOrigin;

    // ----------------------------------------------------- //

    // Mechanics
    private int ammo = 0;
    private int clip = 0;

    private float shootTimer = 0f;
    private bool canShoot = false;

    // Components
    private Rigidbody rigid;
    private BoxCollider boxCollider;
    private LineRenderer lineRenderer;

    // ----------------------------------------------------- //

    private void GetComponentReferences()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Reset()
    {
        GetComponentReferences();

        // Extend the box collider so that it encapsulates all child objects.
        var children = GetComponentsInChildren<MeshRenderer>();
        Bounds bounds = new Bounds(transform.position, Vector3.zero);
        foreach (var rend in children)
            bounds.Encapsulate(rend.bounds);

        // Turn off line renderer
        lineRenderer.enabled = false;

        // Turn off rigidbody
        rigid.isKinematic = false;

        // Apply bounds to box collider
        boxCollider.center = bounds.center - transform.position;
        boxCollider.size = bounds.size;
    }

    private void Awake()
    {
        GetComponentReferences();
    }

    // ----------------------------------------------------- //

}
