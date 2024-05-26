using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sickbow.BehaviorTrees{
public class BehaviorTreeRunner : MonoBehaviour
{
    [SerializeField] BehaviorTreeNode rootNode;
    [SerializeField] List<TreeConditionValue> treeConditionValues;
    private Dictionary<string, TreeConditionValue> _valuePairs;
    
    public BehaviorTreeNode GetRootNode() => rootNode;
    void ResetNodeState(BehaviorTreeNode node){
        if (node == null) return;

        node.State = NodeState.Idle;
        if (node is CompositeNode composite){
            foreach (var child in composite.children){
                ResetNodeState(child);
            }
        }
    }
    void InitializeDictionary(){
        _valuePairs = new Dictionary<string, TreeConditionValue>();
        foreach (TreeConditionValue cv in treeConditionValues){
            _valuePairs.Add(cv.name, cv);
        }
    }
    public bool GetConditionValue(string valueName){
        return _valuePairs[valueName].value;
    }
    public void SetConditionValue(string valueName, bool value){
        _valuePairs[valueName].value = value;
    }

    BehaviorTreeNode CloneNodes(BehaviorTreeNode root){
        if (root == null)
            return null;
        
        var newRoot = root.Clone();
        if (newRoot is LeafNode leafNode){
            leafNode.invertPreConditions = (root as LeafNode).invertPreConditions;
            leafNode.preConditions = new List<TreeConditionValue>();
            foreach (TreeConditionValue cv in (root as LeafNode).preConditions){
                var conditionMatch = treeConditionValues.Find(x => x.name == cv.name);
                if (conditionMatch != null)
                    leafNode.preConditions.Add(conditionMatch);
            }
        }

        if (newRoot is CompositeNode compositeRoot){
            compositeRoot.children.Clear();
            if ((root as CompositeNode)?.children != null)
            foreach (BehaviorTreeNode child in (root as CompositeNode).children){
                compositeRoot.children.Add(CloneNodes(child));
            }
        }
        return newRoot;
    }
    void CloneConditionValues(){
        List<TreeConditionValue> clonedValues = new List<TreeConditionValue>(treeConditionValues.Count);
        for (int i = 0;  i < treeConditionValues.Count; i++ ){
            var valueInstance = ScriptableObject.CreateInstance(treeConditionValues[i].GetType());
            valueInstance.name = treeConditionValues[i].name;
            clonedValues.Insert(i,(TreeConditionValue)valueInstance);
        }
        treeConditionValues = clonedValues;
    }
    void CloneTree(){
        CloneConditionValues();
        var newRoot = CloneNodes(rootNode);
        rootNode = newRoot;
    }

    void Awake(){
        CloneTree();
        InitializeDictionary();
    }

    void Run(){
        if (rootNode == null) return;
        ResetNodeState(rootNode);
        rootNode.Execute(gameObject);
    }
    
    void Update()
    {
        Run();
    }
}
}
