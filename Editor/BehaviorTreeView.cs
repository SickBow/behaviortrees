using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using Sickbow.BehaviorTrees;

public class BehaviorTreeView : GraphView
{
    private SerializedObject m_serializedObject;
    private float xSpacing = 600;
    private float ySpacing = 200;

    public BehaviorTreeView(SerializedObject serializedObject){
        m_serializedObject = serializedObject;

        StyleSheet style = AssetDatabase.LoadAssetAtPath<StyleSheet>("Packages/com.sickbow.behaviortrees/Editor/USS/BehaviorTreesEditor.uss");
        styleSheets.Add(style);

        GridBackground background = new GridBackground();
        background.name = "Grid";
        Insert(0, background);
        //Add(background);

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        this.AddManipulator(new ClickSelector());
    }

    public void PopulateView(BehaviorTreeNode rootNode)
    {
        //Debug.Log("Populating view with root node: " + rootNode.name);
        // Clear existing nodes and edges
        DeleteElements(graphElements.ToList());

        if (rootNode == null)
            return;

        // Recursive method to create and add nodes
        CreateGraphNode(rootNode);

        
        BehaviorTreeGraphNode rootGraphNode = FindGraphNode(rootNode);
        PositionNode(rootGraphNode, 0, new Vector2(0,0));
    }

    private void CreateGraphNode(BehaviorTreeNode node, BehaviorTreeGraphNode parentGraphNode = null)
    {
        //Debug.Log("Creating Graph Node with tree node: " + node.name);
        var graphNode = new BehaviorTreeGraphNode(node);
        AddElement(graphNode);

        if (parentGraphNode != null)
        {
            var edge = new Edge { input = parentGraphNode.outputPort, output = graphNode.inputPort };
            AddElement(edge);
        }

        // If it's a composite node, add its children
        if (node is CompositeNode compositeNode)
        {
            foreach (var child in compositeNode.children)
            {
                CreateGraphNode(child, graphNode);
                
            }
        }
    }

    private void PositionNode(BehaviorTreeGraphNode graphNode, int depth, Vector2 parentPosition, int siblingIndex = 0, int totalSiblings = 0)
    {
        // Calculate position based on depth and xPos
        float widthDecreaseFactor = Mathf.Pow(0.75f, depth); // 0.9 = 90%, decreasing for each depth
        float totalWidth = totalSiblings * xSpacing * widthDecreaseFactor;
        
        
        float startOffset = totalWidth / 2;

        Vector2 position = new Vector2(parentPosition.x + (siblingIndex * xSpacing * widthDecreaseFactor) - startOffset, ySpacing * depth);


        graphNode.SetPosition(new Rect(position, graphNode.GetPosition().size));

        

        if (graphNode.behaviorTreeNode is CompositeNode composite)
        {
            int childCount = composite.children.Count;
            for(int i = 0; i < childCount; i++)
            {
                var child = composite.children[i];
                var childNode = FindGraphNode(child);
                if (childNode != null)
                {
                    PositionNode(childNode, depth + 1, position, i, childCount);
                }
            }
        }
    }

    public BehaviorTreeGraphNode FindGraphNode(BehaviorTreeNode node)
    {
        foreach (var element in graphElements.ToList())
        {
            if (element is BehaviorTreeGraphNode graphNode && graphNode.behaviorTreeNode == node)
            {
                return graphNode;
            }
        }
        return null;
    }

    public void UpdateNodeVisuals()
    {
        foreach (var element in graphElements.ToList())
        {
            if (element is BehaviorTreeGraphNode graphNode)
            {
                graphNode.UpdateVisual();
                if (graphNode.behaviorTreeNode.State == NodeState.Success)
                    UnwindAndColorEdges(graphNode);
            }

            /*if (element is BehaviorTreeGraphNode node)
            {
                if (node.behaviorTreeNode.State == NodeState.Success)
                UnwindAndColorEdges(node);
            }*/

            /*if (element is Edge edge && edge.input.node is BehaviorTreeGraphNode inputGraphNode && edge.output.node is BehaviorTreeGraphNode outputGraphNode)
            {
                // Update edge color based on the active state of nodes it connects
                
                var edgeColor = outputGraphNode.behaviorTreeNode.State == NodeState.Success ? Color.green : Color.gray;
                
                edge.edgeControl.inputColor = edgeColor;
                edge.edgeControl.outputColor = edgeColor;

            }*/
        }
    }

    public void UnwindAndColorEdges(BehaviorTreeGraphNode childNode)
    {
        
        if (childNode.behaviorTreeNode.parent == null || childNode == null) return;
        var parentGraphNode = FindGraphNode(childNode.behaviorTreeNode.parent);
        if (parentGraphNode == null) return;

        foreach (var element in graphElements.ToList())
        {
            if (element is Edge edge)
            {
                if (edge.input.node == parentGraphNode && edge.output.node == childNode)
                {
                    edge.edgeControl.inputColor = Color.green;
                    edge.edgeControl.outputColor = Color.green;
                    break;
                }
            }
        }

        UnwindAndColorEdges(parentGraphNode);
    }
}
