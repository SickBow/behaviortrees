using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using Unity.VisualScripting;
using Sickbow.BehaviorTrees;


public class BehaviorTreeViewWindow : EditorWindow
{
    [SerializeField]
    private BehaviorTreeNode m_currentTreeNode;

    [SerializeField]
    private SerializedObject m_serializedObject;
    //
    [SerializeField]
    private BehaviorTreeView m_currentView;

    public BehaviorTreeNode currentTreeNode => m_currentTreeNode;

    void OnEnable()
    {
        EditorApplication.update += UpdateBehaviorTreeVisuals;
    }

    void OnDisable()
    {
        EditorApplication.update -= UpdateBehaviorTreeVisuals;
    }

    private void UpdateBehaviorTreeVisuals()
    {
        if (Application.isPlaying && m_currentTreeNode != null)
        {
            m_currentView.UpdateNodeVisuals();
        }
    }


    [MenuItem("Window/Behavior Tree Viewer")]
    public static void ShowWindow(){
        GetWindow<BehaviorTreeViewWindow>("Behavior Tree Viewer");
    }

    public static void Open(BehaviorTreeNode target){
        BehaviorTreeViewWindow[] windows = Resources.FindObjectsOfTypeAll<BehaviorTreeViewWindow>();
        foreach(var w in windows){
            if (w.currentTreeNode == target){
                w.Focus();
                return;
            }

        }

        BehaviorTreeViewWindow window = CreateWindow<BehaviorTreeViewWindow>(typeof(BehaviorTreeViewWindow),typeof(SceneView));
        //window.titleContent = new GUIContent($"{target.name}", EditorGUIUtility.ObjectContent(null, typeof(BehaviorTreeNode)))
        window.Load(target);
    }

    public void Load(BehaviorTreeNode target){
        m_currentTreeNode = target;
        m_serializedObject = new SerializedObject(m_currentTreeNode);
        m_currentView = new BehaviorTreeView(m_serializedObject);
        rootVisualElement.Add(m_currentView);

        m_currentView.PopulateView(target);

    }

    void OnGUI(){
        GUILayout.Label("Behavior Tree View", EditorStyles.boldLabel);
    } 
}