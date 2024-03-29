using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sickbow.BehaviorTrees{
public class LeafNode : BehaviorTreeNode
{
    [SerializeField] public bool invertPreConditions;
    [SerializeField] public List<TreeConditionValue> preConditions;

    protected virtual void FalseActions(GameObject owner){}
    protected virtual void Actions(GameObject owner){}

    public override bool Execute(GameObject owner)
    {
        State = NodeState.Running;

        if (CheckPreConditions() == false) {
            FalseActions(owner);
            State = NodeState.Failure;
            return false;
        }
        Actions(owner);
        State = NodeState.Success;
        return true;
    }

    protected bool CheckPreConditions(){
        if (preConditions.Count == 0) return true;
        foreach (TreeConditionValue cv in preConditions){
            if (cv.value == false) return (invertPreConditions)? true : false;
        }
        return (invertPreConditions)? false : true; 
    }
}
}
