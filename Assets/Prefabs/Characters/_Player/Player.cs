using UnityEngine;
using UnityEngine.SceneManagement;
using SoulsEngine;

public class Player : Actor
{
    bool enlarge = false;
    
    protected override void Start ()
    {
        base.Start();

        GodManager.Player = this;
	}

    private void Update()
    {
        //input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        input = Vector2.zero;

        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(0, LoadSceneMode.Single);

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (hasControl)
        {
            if (Input.GetKey(KeyCode.A))
                input.x += -1;

            if (Input.GetKey(KeyCode.D))
                input.x += 1;

            if (Input.GetKeyDown(KeyCode.Space))
                Controller.Jump(input);

            if (Input.GetKeyDown(KeyCode.LeftAlt))
                Controller.Dash();

            if (Input.GetMouseButtonDown(0))
            {
                CombatController.Attack();
            }
        }
    }

    private void FixedUpdate()
    {
        if (hasControl)
        {
            Controller.Move(input * Time.deltaTime);
        }
    }

    public override void Death(float delay)
    {
        GodManager.DeathSplash();

        base.Death(delay);
    }
}
