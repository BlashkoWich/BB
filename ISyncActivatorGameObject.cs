using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISyncActivatorGameObject
{
    void SwitcherGameObject(List<GameObject> gameObjects, bool activatorGo);
}
