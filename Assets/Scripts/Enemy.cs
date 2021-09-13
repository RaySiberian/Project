using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
   public float lookRadius = 10f;
   private Transform target;
   private NavMeshAgent navMeshAgent;

   private void Awake()
   {
      navMeshAgent = GetComponent<NavMeshAgent>();
   }

   private void Start()
   {
      target = PlayerManager.instance.player.transform;
   }

   private void Update()
   {
      float distance = Vector3.Distance(target.position, transform.position);
      if (distance <= lookRadius)
      {
         navMeshAgent.SetDestination(target.position);
         if (distance <= navMeshAgent.stoppingDistance)
         {
            //TODO реализовать атаку 
            FaceTarget();
         }
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
