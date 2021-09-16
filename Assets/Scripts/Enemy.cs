using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
   public float lookRadius = 15f;
   public float attackDistance;
   private Transform target;
   private NavMeshAgent navMeshAgent;
   private Animator animator;

   private void Awake()
   {
      navMeshAgent = GetComponent<NavMeshAgent>();
      animator = GetComponentInChildren<Animator>();
   }

   private void Start()
   {
      target = PlayerManager.instance.player.transform;
   }

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.Y))
      {
         animator.SetBool("jump",true);
      }
      float distance = Vector3.Distance(target.position, transform.position);
      if (distance <= lookRadius)
      {
         navMeshAgent.SetDestination(target.position);
         if (distance <= navMeshAgent.stoppingDistance + 0.5f)
         {
            //TODO реализовать атаку
            animator.SetTrigger("jump");
            FaceTarget();
         }
      }

      if (navMeshAgent.velocity.magnitude != 0)
      {
         animator.SetBool("Walk",true);
      }
      else
      {
         animator.SetBool("Walk",false);
      }
   }

   private void FaceTarget()
   {
      Vector3 direction = (target.position - transform.position).normalized;
      Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x,0,direction.z));
      transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation,Time.deltaTime * 5);
   }
   
   private void OnDrawGizmosSelected()
   {
      Gizmos.color = Color.red;
      Gizmos.DrawWireSphere(transform.position,lookRadius);
   }
}
