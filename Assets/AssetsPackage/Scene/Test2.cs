using System.Collections;
using System.Collections.Generic;
using MenteBacata.ScivoloCharacterController;
using UnityEngine;

public class Test2 : MonoBehaviour
{
    public CharacterCapsule capsule;
    public GroundDetector groundDetector;
    public CharacterMover mover;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (capsule.TryResolveOverlap())
        {
            var arr = new Collider[5];
            capsule.CollectOverlaps(arr);
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] != null) 
                {
                    Debug.LogError(arr[i].gameObject.name);
                }
            }
        }
    }
}
