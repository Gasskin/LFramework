using System;
using MenteBacata.ScivoloCharacterController;
using UnityEngine;

public class TestScripts : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpSpeed = 8f;
    public float gravity = -25f;
    
    public CharacterCapsule capsule;
    public GroundDetector groundDetector;
    public CharacterMover mover;
    
    private const float timeBeforeUngrounded = 0.02f;
    private const float minVerticalSpeed = -12f;
    
    private float nextUngroundedTime = -1f;

    private Collider[] overlaps = new Collider[5];
    private MoveContact[] moveContacts = CharacterMover.NewMoveContactArray;
    private int overlapCount;
    private float verticalSpeed = 0f;
    private int contactCount;

    private void Update()
    {
        var deltaTime = Time.deltaTime;
        // float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        // var movementInput = new Vector3(x, 0, 0);
        var dir = (y * transform.forward).normalized;
        var velocity = moveSpeed * dir;
        
        overlapCount = capsule.TryResolveOverlap() ? 0 : capsule.CollectOverlaps(overlaps);
        
        var groundDetected = DetectGroundAndCheckIfGrounded(out bool isGrounded, out GroundInfo groundInfo);
        
        if (isGrounded)
        {
            mover.mode = CharacterMover.Mode.Walk;
            verticalSpeed = 0f;
        }
        else
        {
            mover.mode = CharacterMover.Mode.SimpleSlide;

            BounceDownIfTouchedCeiling();

            verticalSpeed += gravity * deltaTime;

            if (verticalSpeed < minVerticalSpeed)
                verticalSpeed = minVerticalSpeed;

            velocity += verticalSpeed * transform.up;
        }
  
        mover.Move(velocity * deltaTime, groundDetected, groundInfo, overlapCount, overlaps, moveContacts, out contactCount);
    }
    
    private bool DetectGroundAndCheckIfGrounded(out bool isGrounded, out GroundInfo groundInfo)
    {
        bool groundDetected = groundDetector.DetectGround(out groundInfo);

        if (groundDetected)
        {
            if (groundInfo.isOnFloor && verticalSpeed < 0.1f)
                nextUngroundedTime = Time.time + timeBeforeUngrounded;
        }
        else
            nextUngroundedTime = -1f;

        isGrounded = Time.time < nextUngroundedTime;
        return groundDetected;
    }
    
    private void BounceDownIfTouchedCeiling()
    {
        for (int i = 0; i < contactCount; i++)
        {
            if (Vector3.Dot(moveContacts[i].normal, transform.up) < -0.7f)
            {
                verticalSpeed = -0.25f * verticalSpeed;
                break;
            }
        }
    }
}