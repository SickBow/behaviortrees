using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sickbow.BehaviorTrees;
[CustomEditor(typeof(BehaviorTreeRunner))]
public class BehaviorTreeRunnerEditor : Editor
{
    public override void OnInspectorGUI(){

        base.OnInspectorGUI();

        BehaviorTreeRunner runner = (BehaviorTreeRunner)target;
        if (runner.GetRootNode() != null){
            DrawNode(runner.GetRootNode());    
        }
    }

    private void DrawNode(BehaviorTreeNode node){
        if (node == null) return;

        //determine color based on NodeState
        Color originalColor = GUI.color;

        switch(node.State){
            case NodeState.Idle :
                GUI.color = Color.gray;
                break;
            case NodeState.Running :
                GUI.color = Color.yellow;
                break;
            case NodeState.Success :
                GUI.color = Color.green;
                break;
            case NodeState.Failure :
                GUI.color = Color.red;
                break;
        }

        string nodeName = node.name;
        bool isLeaf = node is LeafNode;
        bool isComposite = node is CompositeNode;

        if (isLeaf){
            nodeName += " (Leaf)";
        }
        else if (isComposite){
            if (node is SelectorNode) nodeName += " (Selector)";
            else if (node is SequenceNode) nodeName += " (Sequence)";
            else nodeName += " (Composite)";
        }

        if (isComposite){
            CompositeNode composite = node as CompositeNode;

            if (composite != null && composite.children.Count > 0)
            {
                //foldout for composites
                bool foldout = EditorGUILayout.Foldout(true, nodeName); //true by default
                if (foldout){
                    EditorGUI.indentLevel++;
                    foreach(var child in composite.children){
                        DrawNode(child);
                    }
                    EditorGUI.indentLevel--;
                }
            }
            
        }
        else
        {
            EditorGUILayout.LabelField(nodeName);
        }

        GUI.color = originalColor;
    }
}
