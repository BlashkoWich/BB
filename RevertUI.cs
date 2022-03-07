using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevertUI : MonoBehaviour
{
    public void RevertGameObject()
    {
        gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
    }
}
