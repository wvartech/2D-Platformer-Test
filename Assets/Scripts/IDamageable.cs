using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    int HEALTH { get; }
    void getHit(int dmg);
    IEnumerator die();
}
