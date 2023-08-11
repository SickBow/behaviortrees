using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sickbow.BehaviorTrees{
[CreateAssetMenu(fileName = "Selector", menuName = "BehaviorTree/Selector")]
public class SelectorNode : CompositeNode
{
    public override bool Execute(GameObject owner)
    {
        State = NodeState.Running;

        foreach( BehaviorTreeNode node in children){
            if (node.Execute(owner) == true){
                State = NodeState.Success;
                return true;
            }
        }
        
        State = NodeState.Failure;
        return false;
    }
}
}
