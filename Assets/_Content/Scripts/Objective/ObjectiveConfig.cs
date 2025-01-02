using System.Collections.Generic;
using _Content.Scripts.Enum;
using UnityEngine;

namespace _Content.Scripts.Objective
{
    [CreateAssetMenu(fileName = "New ObjectiveConfig", menuName = "Configs/ObjectiveConfig")]
    public class ObjectiveConfig : ScriptableObject
    {
        [Header("Move")]
        public int MoveCount;
        [Header("Objective")]
        public List<Objective> Objectives;
           
        public ObjectiveConfig GetRuntimeInstance() => Instantiate(this);
      
        public static bool IsObjectiveCompleted(ObjectiveConfig config)
        {
            bool result = true;
            
            foreach (var objective in config.Objectives)
            {
                if (objective.Score > 0)
                {
                    result = false;
                }
            }

            return result;
        }

        public static Objective GetObjectiveByType(ObjectiveConfig config,ColorType colorType)
        {
            foreach (var objective in config.Objectives)
            {
                if (objective.ColorType == colorType)
                {
                    return objective;
                }
            }

            return null;
        }
    }
}