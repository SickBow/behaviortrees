using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sickbow.BehaviorTrees{
[CreateAssetMenu(fileName = "Sequence", menuName = "BehaviorTree/Sequence")]
public class SequenceNode : CompositeNode
{
    public override bool Execute(GameObject owner)
    {
        State = NodeState.Running;

        foreach ( BehaviorTreeNode node in children ){
            if (node.Execute(owner) == false){
                State = NodeState.Success;
                return false;
            }
        }
        
        State = NodeState.Failure;
        return true;
    }
}
}
