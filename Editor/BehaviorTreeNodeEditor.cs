using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sickbow.BehaviorTrees;

[CustomEditor(typeof(BehaviorTreeNode), true)]
[CanEditMultipleObjects]
public class BehaviorTreeNodeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (!target.GetType().IsAbstract)
            if (GUILayout.Button("Open")){
                BehaviorTreeViewWindow.Open((BehaviorTreeNode)target);
            }
    }
}
