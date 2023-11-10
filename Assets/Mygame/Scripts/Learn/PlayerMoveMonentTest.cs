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

    [SerializeField]
    private float jumpForce;
    private float yForce; // tác dụng để tính toán 

    private float originalStepOffSet;
    [SerializeField]
    private float jumpButtonGracePeriod; // biến thời gian cho nhân vật nhảy
    private float? lastGroundTime; // biến này là khoảng thời gian cuối cùng của người dùng trên mặt đất và dấu chấm hỏi là biến này có thể null cũng được 
    private float? jumpButtonPressTime; 
    


    private CharacterController charactorController;
    // Start is called before the first frame update
    void Start()
    {
        charactorController = GetComponent<CharacterController>();
        originalStepOffSet = charactorController.stepOffset; // để giá trị mặc định , để khi đang nhảy để step off set bằng 0 ( giá trị mặc định)
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal"); // 3d nên dùng GetAxis bởi vì trong thời gian từ idie tới walk r tới run 
        verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);

        print($"Vertor Manitude before normalize: {movementDirection.magnitude}"); // dòng này chỉ để ktra vật lý

        float magnitude = movementDirection.magnitude;
        magnitude = Mathf.Clamp01(magnitude); // giá trị magnitude khi có clamp nó chỉ từ 0 đến 1

        movementDirection.Normalize(); // hàm này Normalize là chuẩn hóa Vector đi xéo nhưng nó vẫn giữ tốc độ 1 

        print($"Vertor Manitude after normalize: {movementDirection.magnitude}"); // dòng này chỉ để ktra vật lý

        //transform.Translate(magnitude *moveSpeed * Time.deltaTime * movementDirection , Space.World); // hàm này được xử dụng khi không có vật lý , Space World là cho nhân vật di chuyển trong position  

        //Jump
        yForce += Physics.gravity.y * Time.deltaTime; // công thức này để tính y (như thực tế ở ngoài) công thức có tính vật lí

        if (charactorController.isGrounded)
        {
            lastGroundTime = Time.time; // dòng này để lúc nào nhân vật ở trên mặt đất thì thời gian cuối cùng vẫn liên tục
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpButtonPressTime = Time.time; // này là set thời gian nào mà mình bấm nút Space
        }


        if (Time.time - lastGroundTime <= jumpButtonGracePeriod) // dòng này để khi kiểm tra khi ra khỏi vùng khoảng 0.1s thì vẫn được nhảy
        {
            yForce = -0.5f; // set số cho nhỏ để nó chắc chắn là nhân vật nó chạm đất để nhảy 
            charactorController.stepOffset = originalStepOffSet; // cho giá trị nó về 0 ( giá trị mặc định)
            if (Time.time - jumpButtonPressTime <= jumpButtonGracePeriod)
            {
                yForce = jumpForce;
                jumpButtonPressTime = null;
                lastGroundTime = null;
            }
        }
        else
        {
            charactorController.stepOffset = 0; // là khi đang nhảy trên cầu thang thì nó sẽ kh bị đứng trên cầu thang
        }
    
        Vector3 velocity = moveSpeed * magnitude * movementDirection;
        velocity.y = yForce;

        charactorController.Move(velocity * Time.deltaTime);
        //charactorController.SimpleMove( velocity); // dòng code này để di chuyển đơn giản là dùng SimpleMove

        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, turnSpeed* Time.deltaTime); // Quaternion.RotateTowards hàm này dùng để xoay từ góc mặc định tới góc mình muốn xoay mà có thêm thời gian ,  Quaternion nó sẽ tạo ra 1 cái góc ,Time.deltaTime thời gian mỗi frame nó thực hiện 
            // transform.forward = movementDirection;  // hướng forward là vector màu xanh 
        }
    }
}
