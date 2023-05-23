using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sickbow.BehaviorTrees{
public abstract class CompositeNode : BehaviorTreeNode
{
    [SerializeField] public List<BehaviorTreeNode> children = new List<BehaviorTreeNode>();
}
}