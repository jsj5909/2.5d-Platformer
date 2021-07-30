using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollAnimationEvent : StateMachineBehaviour
{
    float elapsedTime = 0;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
     
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      /*     Player player = animator.GetComponentInParent<Player>();

          elapsedTime += Time.deltaTime;

          animator.GetComponentInParent<CharacterController>().enabled = false;

          Transform t = player.transform;
            t.position = new Vector3(t.position.x + Mathf.Lerp(0, 8f, (elapsedTime/100) / 1.167f),
              t.position.y,
             t.position.z);
        
         
        animator.GetComponentInParent<CharacterController>().enabled = true;
      */
      
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        elapsedTime = 0;

        Player player = animator.GetComponentInParent<Player>();
/*
        player.GetComponent<CharacterController>().enabled = false;

        player.transform.position = new Vector3(player.transform.position.x + 8, 
            player.transform.position.y, player.transform.position.z);

        player.GetComponent<CharacterController>().enabled = true;
*/
        player.FinishRoll();
        

      //  animator.GetComponentInParent<CharacterController>().enabled = false;

      //  Transform t = player.transform;
        
       // t.position += new Vector3(t.position.x + 8, t.position.y, t.position.z);
      //  animator.GetComponentInParent<CharacterController>().enabled = true;

       // animator.GetComponentInParent<Player>().FinishRoll();
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      /*   Player player = animator.GetComponentInParent<Player>();

        elapsedTime += Time.deltaTime;
        
        animator.GetComponentInParent<CharacterController>().enabled = false;

        Transform t = player.transform;
          t.position = new Vector3(t.position.x + Mathf.Lerp(0, 8f, (elapsedTime/100) / 1.167f),
            t.position.y,
       /     t.position.z);
        t.position += new Vector3(t.position.x + 8, t.position.y, t.position.z);
        animator.GetComponentInParent<CharacterController>().enabled = true;*/
    }

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
