using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Sickbow.BehaviorTrees{
[CreateAssetMenu(fileName = "TreeConditionValue", menuName = "BehaviorTree/TreeConditionValue")]
public class TreeConditionValue : ScriptableObject
{
    public bool value;
}
}
