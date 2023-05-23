# Behavior Trees
Provides Scriptable Objects (accessible through Create Asset Menu) and MonoBehaviors to give client a way to build behavior trees for game objects

## How to use
First, use the create asset menu to navigate to BehaviorTrees This will contain all the nodes you create, as well as Selector and Sequence nodes. Then, after creating all your custom nodes (Instructions on how to do this later), arrange them in the tree hierarchy starting with a root (Selector) node.

Then, add a component using 'Add Component' to the GameObject you wish to run your BehaviorTree on and search 'BehaviorTreeRunner' and add it. Now drag your root node to the exposed 'Root Node' inspector field in BehaviorTreeRunner.

## Custom Leaf Nodes
Override LeafNode class methods 'FalseActions(GameObject : owner)' and 'Actions(GameObject : owner)' to create custom node actions.
Use [CreateAssetMenu(fileName = "node-name", menuName = "BehaviorTree/node-name Action")] attribute to create .asset nodes for use in the BehaviorTreeRunner. 
add PreConditions (TreeConditionValues) for the LeafNode.asset's file in the inspector, otherwise will evaluate to true by default.
FalseActions(GameObject : owner) are actions performed when the PreConditions evaluate to false
Actions(GameObject : owner) are performed when the PreConditions evaluate to true

## TreeConditionValues
These are created from the create asset menu and are simply scriptable objects with a single bool 'value'. Change these values at runtime by BehaviorTreeRunner.SetConditionValue(string : name, bool : value) or bool BehaviorTreeRunner.GetConditionValue(string : name). Add references to all used TreeConditionValues to the "Tree Condition Values" exposed list in BehaviorTreeRunner for everything to work properly
