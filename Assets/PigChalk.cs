using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigChalk : MonoBehaviour
{
    private void SelfDestruct()
    {
        Destroy(this.gameObject);
    }

    private void OnEnable()
    {
        Ball.OnHitTarget += SelfDestruct;
    }

    private void OnDisable()
    {
        Ball.OnHitTarget -= SelfDestruct;
    }
}
