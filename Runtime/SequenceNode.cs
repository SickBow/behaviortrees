using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sickbow.BehaviorTrees{
[CreateAssetMenu(fileName = "Sequence", menuName = "BehaviorTree/Sequence")]
public class SequenceNode : CompositeNode
{
    public override bool Execute(GameObject owner)
    {
        foreach ( BehaviorTreeNode node in children ){
            if (node.Execute(owner) == false)
                return false;
        }

        return true;
    }
}
}