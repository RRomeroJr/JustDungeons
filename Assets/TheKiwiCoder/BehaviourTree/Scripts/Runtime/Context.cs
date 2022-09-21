using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TheKiwiCoder
{

    // The context is a shared object every node has access to.
    // Commonly used components and subsytems should be stored here
    // It will be somewhat specfic to your game exactly what to add here.
    // Feel free to extend this class 
    public class Context
    {
        public GameObject gameObject;
        public Transform transform;
        public Animator animator;
        public Rigidbody physics;
        public NavMeshAgent agent;
        public BoxCollider2D boxCollider;
        public CharacterController characterController;
        public Dictionary<string, object> extra;
        public EnemySO stats;
        public Actor actor;
        public EnemyController controller;
        // Add other game specific systems here

        public static Context CreateFromGameObject(GameObject gameObject)
        {
            // Fetch all commonly used components
            Context context = new Context();
            context.gameObject = gameObject;
            context.transform = gameObject.transform;
            context.animator = gameObject.GetComponent<Animator>();
            context.physics = gameObject.GetComponent<Rigidbody>();
            context.agent = gameObject.GetComponent<NavMeshAgent>();
            context.boxCollider = gameObject.GetComponent<BoxCollider2D>();
            context.characterController = gameObject.GetComponent<CharacterController>();
            context.actor = gameObject.GetComponent<Actor>();
            context.controller = gameObject.GetComponent<EnemyController>();

            // Add whatever else you need here...
            return context;
        }

        public static Context CreateFromGameObject(GameObject gameObject, Dictionary<string, object> e, EnemySO s)
        {
            // Fetch all commonly used components
            Context context = new Context();
            context.gameObject = gameObject;
            context.transform = gameObject.transform;
            context.animator = gameObject.GetComponent<Animator>();
            context.physics = gameObject.GetComponent<Rigidbody>();
            context.agent = gameObject.GetComponent<NavMeshAgent>();
            context.boxCollider = gameObject.GetComponent<BoxCollider2D>();
            context.characterController = gameObject.GetComponent<CharacterController>();
            context.actor = gameObject.GetComponent<Actor>();
            context.controller = gameObject.GetComponent<EnemyController>();
            context.extra = e;
            context.stats = s;
            // Add whatever else you need here...
            return context;
        }
    }
}
