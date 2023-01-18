using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
   [SerializeField] private float moveSpeed = 10;
   [SerializeField] private float rotationSpeed = 100;
   [SerializeField] private float zoomAmount = 1;
   [SerializeField] private CinemachineVirtualCamera cineMachineVirtualCamera;

   private const float MinFollowYOffSet = 2f;
   private const float MaxFollowYOffSet = 12f;

   private CinemachineTransposer cinemachineTransposer;
   private Vector3 targetFollowOffset;

   public void Awake()
   {
      cinemachineTransposer = cineMachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
      targetFollowOffset = cinemachineTransposer.m_FollowOffset;
   }

   private void Update()
   {
      MoveCamera();
      
      RotationCamera();
      
      ZoomCamera();
   }

   private void MoveCamera()
   {
      var inputMoveDir = Vector3.zero;

      if (Input.GetKey(KeyCode.W))
      {
         inputMoveDir.z = +1f;
      }

      if (Input.GetKey(KeyCode.S))
      {
         inputMoveDir.z = -1f;
      }

      if (Input.GetKey(KeyCode.D))
      {
         inputMoveDir.x = +1f;
      }

      if (Input.GetKey(KeyCode.A))
      {
         inputMoveDir.x = -1f;
      }

      var camTransform = transform;
      
      var moveDir = camTransform.forward * inputMoveDir.z + camTransform.right * inputMoveDir.x;

      camTransform.position += moveDir * (moveSpeed * Time.deltaTime);
   }

   private void RotationCamera()
   {
      var inputRotation = Vector3.zero;

      if (Input.GetKey(KeyCode.Q))
      {
         inputRotation.y = +1f;
      }

      if (Input.GetKey(KeyCode.E))
      {
         inputRotation.y = -1f;
      }

      transform.eulerAngles += inputRotation * (rotationSpeed * Time.deltaTime);
   }

   private void ZoomCamera()
   {
      switch (Input.mouseScrollDelta.y)
      {
         case > 0:
            targetFollowOffset.y -= zoomAmount;
            break;
         case < 0:
            targetFollowOffset.y += zoomAmount;
            break;
      }

      targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MinFollowYOffSet, MaxFollowYOffSet);

      cinemachineTransposer.m_FollowOffset =
         Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, Time.deltaTime * 5);
   }
}
