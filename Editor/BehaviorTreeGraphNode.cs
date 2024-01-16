using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine.UIElements;
using Sickbow.BehaviorTrees;
using System.Reflection;

public class BehaviorTreeGraphNode : Node
{
    public BehaviorTreeNode behaviorTreeNode;
    public Port inputPort;
    public Port outputPort;
    private Dictionary<string, Label> preconditionLabels = new Dictionary<string, Label>();

    public BehaviorTreeGraphNode(BehaviorTreeNode behaviorTreeNode) : base()
    {
        this.behaviorTreeNode = behaviorTreeNode;
        this.title = behaviorTreeNode.name; // Or any other relevant title

        var titleButtonContainer = this.Q("title-button-container");
        if (titleButtonContainer != null)
        {
            titleButtonContainer.RemoveFromHierarchy();
        }

        var topContainer = new VisualElement();


        inputPort = Port.Create<Edge>(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
        inputPort.portName = "("+behaviorTreeNode.GetType().Name+ ")";
        inputPort.Q<VisualElement>("connector").visible = false;
        topContainer.Add(inputPort);
        topContainer.style.alignItems = Align.Center; // Center align the input port
        topContainer.style.justifyContent = Justify.Center;
        topContainer.style.flexDirection = FlexDirection.Column; // Stack vertically

        this.Insert(0,topContainer);

        outputPort = Port.Create<Edge>(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
        outputPort.portName = "";
        outputPort.Q<VisualElement>("connector").visible = false;
        var bottomContainer = new VisualElement();
        extensionContainer.Add(bottomContainer);
        // Apply styling or additional layout adjustments if needed
        bottomContainer.style.alignItems = Align.Center;
        bottomContainer.style.justifyContent = Justify.Center;
        // Populate the foldout with preconditions
        if (behaviorTreeNode is LeafNode leafNode)
        {
            var preconditionsFoldout = new Foldout { text = "Preconditions" };
            bottomContainer.Add(preconditionsFoldout);
            foreach (var precondition in leafNode.preConditions)
            {
                var label = new Label(precondition.name);
                preconditionsFoldout.Add(label);
                preconditionLabels[precondition.name] = label;
            }
        }

        bottomContainer.Add(outputPort);

        // Add double-click event listener
        this.AddManipulator(new Clickable(OnDoubleClick));


        RefreshExpandedState(); // Refresh to update the layout
        RefreshPorts();
    }

    private void OnDoubleClick()
    {
        if (behaviorTreeNode != null)
        {
            // Focus on the associated .asset file in the Inspector
            EditorGUIUtility.PingObject(behaviorTreeNode);
            Selection.activeObject = behaviorTreeNode;
        }
    }

    public void UpdateVisual()
    {
        
        if (behaviorTreeNode.State == NodeState.Success)
        {
            AddToClassList("active"); // Add the 'active' class
        }
        else
        {
            RemoveFromClassList("active"); // Remove the 'active' class
        }

        // Update precondition label colors
        if (behaviorTreeNode is LeafNode leafNode)
        {
            foreach (var precondition in leafNode.preConditions)
            {
                if (preconditionLabels.TryGetValue(precondition.name, out var label))
                {
                    label.style.color = precondition.value ? Color.green : Color.red;
                }
            }
        }
    }
    // Add more methods and properties as needed
}
