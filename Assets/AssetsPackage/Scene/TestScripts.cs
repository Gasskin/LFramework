using UnityEngine;

public class TestScripts : MonoBehaviour
{
    public BoxCollider m_Collider;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var flag = Physics.CheckBox(transform.position, m_Collider.size/2, Quaternion.identity,LayerMask.GetMask("Ground"));
        Debug.LogError(flag);
    }
}
