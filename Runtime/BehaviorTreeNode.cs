using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sickbow.BehaviorTrees{
public abstract class BehaviorTreeNode : ScriptableObject
{
    public NodeState State {get; protected set;} = NodeState.Idle;
    public abstract bool Execute(GameObject owner);
    public virtual BehaviorTreeNode Clone(){
        var clone = (BehaviorTreeNode)ScriptableObject.CreateInstance(this.GetType());
        clone.name = this.name;
        return clone;
    }
}
}
