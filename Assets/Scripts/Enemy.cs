using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
   public float Damage;
   public float Heath;
   public float LookRadius = 15f;
   //public float attackDistance;

   private Player player;
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
      player = PlayerManager.instance.player.GetComponent<Player>();
      target = PlayerManager.instance.player.transform;
   }

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.Y))
      {
         animator.SetBool("jump",true);
      }
      float distance = Vector3.Distance(target.position, transform.position);
      if (distance <= LookRadius)
      {
         navMeshAgent.SetDestination(target.position);
         if (distance <= navMeshAgent.stoppingDistance + 0.5f)
         {
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

   public void DealDamage()
   {
      player.Health -= Damage;
      player.HealthBarFill();
   }

   public void GetDamage(float incomeDamage)
   {
      Heath -= incomeDamage;
      if (Heath <= 0)
      {
         Destroy(gameObject);
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
      Gizmos.DrawWireSphere(transform.position,LookRadius);
   }
}
