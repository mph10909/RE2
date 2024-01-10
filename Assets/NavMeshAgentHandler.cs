using UnityEngine;
using UnityEngine.AI;

namespace ResidentEvilClone
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NavMeshAgentHandler : MonoBehaviour, IComponentSavable
    {
        private NavMeshAgent agent;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        public string GetSavableData()
        {
            // Convert position to string format "x,y,z".
            return $"{agent.transform.position.x},{agent.transform.position.y},{agent.transform.position.z}";
        }

        public void SetFromSaveData(string savedData)
        {
            // Parse the saved string "x,y,z" back to Vector3.
            string[] parts = savedData.Split(',');
            Vector3 savedPosition = new Vector3(
                float.Parse(parts[0]),
                float.Parse(parts[1]),
                float.Parse(parts[2])
            );

            // Instead of directly setting the position, use NavMeshAgent's functionality.
            agent.Warp(savedPosition);
        }
    }
}
