using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour
{
    [SerializeField] private GameObject _puffFX;

    private Vector3 _startPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            Instantiate(_puffFX, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
