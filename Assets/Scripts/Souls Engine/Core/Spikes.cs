using UnityEngine;
using SoulsEngine;

public class Spikes : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag == "Player")
        {
            var p = c.gameObject.GetComponent<Actor>();
            p.hasControl = false;

            var actions = new System.Action[] { GodManager.DeathSplash };
            GodManager.RestartScene(actions, 1);
        }
    }
}
