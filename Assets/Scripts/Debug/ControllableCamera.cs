using UnityEngine;

public class ControllableCamera : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= Vector3.up;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left;
        }
    }
}
