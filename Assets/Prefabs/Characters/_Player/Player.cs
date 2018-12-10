using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Actor
{


    new void Start ()
    {
        base.Start();
        
		AnimManager.animInt.Add ("state", 0);
		AnimManager.animFloat.Add ("dir", 0f);
        AnimManager.animFloat.Add("locomotionModifier", 0f);

        godManager.Player = this;

        Inventory.isPlayer = true;
	}

    private void Update()
    {
        //input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        input = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
            input.x = -1;
        if (Input.GetKey(KeyCode.D))
            input.x = 1;

        if (hasControl)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                controller.Jump(input);

            if (Input.GetKeyDown(KeyCode.LeftAlt))
                controller.Dash();

            //controller.Move(input * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {

        if (hasControl)
        {
            controller.Move(input * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
