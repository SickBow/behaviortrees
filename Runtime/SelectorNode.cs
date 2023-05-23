using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sickbow.BehaviorTrees{
[CreateAssetMenu(fileName = "Selector", menuName = "BehaviorTree/Selector")]
public class SelectorNode : CompositeNode
{
    public override bool Execute(GameObject owner)
    {
        foreach( BehaviorTreeNode node in children){
            if (node.Execute(owner) == true)
                return true;
        }

        return false;
    }
}
}