using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;
    [SerializeField] float transitionSpeed = 2f;

    private void LateUpdate()
    {
        if(target != null)
        {
            transform.position = Vector3.Lerp(transform.position, target.position + offset, transitionSpeed * Time.deltaTime);
        }
    }
}
