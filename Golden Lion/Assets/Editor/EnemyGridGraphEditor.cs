using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[CustomGraphEditor(typeof(EnemyGridGraph), "Enemy Grid Graph")]
public class EnemyGridGraphEditor : GridGraphEditor
{
    // Here goes the GUI
    public void OnInspectorGUI(EnemyGridGraph target)
    {
        OnBaseInspectorGUI(target);
        //TerrainGraph graph = (TerrainGraph)target;
    }
}