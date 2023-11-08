using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveMonentTest : MonoBehaviour
{
    // sqrMagnitude thì khi dùng hàm này thì khi ktra điều kiện thì mình nhớ phải nhân thêm số mà mình đặt điều kiện
    private float horizontalInput;
    private float verticalInput;

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float turnSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);

        print($"Vertor Manitude before normalize: {movementDirection.magnitude}"); // dòng này chỉ để ktra vật lý
        movementDirection.Normalize(); // hàm này Normalize là chuẩn hóa Vector 
        print($"Vertor Manitude after normalize: {movementDirection.magnitude}"); // dòng này chỉ để ktra vật lý

        transform.Translate(movementDirection *moveSpeed * Time.deltaTime,Space.World); // hàm này được xử dụng khi không có vật lý , Space World là cho nhân vật di chuyển trong position  

        if(movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, turnSpeed* Time.deltaTime); // Quaternion.RotateTowards hàm này dùng để xoay từ góc mặc định tới góc mình muốn xoay mà có thêm thời gian 
           // transform.forward = movementDirection;  // hướng forward là vector màu xanh 
        }
    }
}
