using UnityEngine;

namespace TheKiwiCoder
{
    public class BehaviourTreeRunner : MonoBehaviour
    {
        // The main behaviour tree asset
        public BehaviourTree tree;

        // Storage container object to hold game object subsystems
        Context context;

        // Start is called before the first frame update
        void Awake()
        {
            // Update context for behavior tree
            context = Context.CreateFromGameObject(gameObject);
            context = Context.AddEnemyContext(gameObject, context);
            tree = tree.Clone();
            tree.Bind(context);
        }

        // Update is called once per frame
        void Update()
        {
            tree.Update();
        }

        /*private void OnDrawGizmosSelected() {
            if (!tree) {
                return;
            }

            BehaviourTree.Traverse(tree.rootNode, (n) => {
                if (n.drawGizmos) {
                    n.OnDrawGizmos();
                }
            });
        }*/
    }
}
