using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sickbow.BehaviorTrees{
public abstract class BehaviorTreeNode : ScriptableObject
{
    public abstract bool Execute(GameObject owner);
    public virtual BehaviorTreeNode Clone(){
        return (BehaviorTreeNode)ScriptableObject.CreateInstance(this.GetType());
    }
}
}
