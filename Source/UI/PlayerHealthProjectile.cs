/*!*******************************************************************
\file         PlayerHealthProjectile.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         07/08/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;

/*!*******************************************************************
\class        PlayerHealthProjectile
\brief        Projectile to player health to reduce that.
********************************************************************/
public class PlayerHealthProjectile : MonoBehaviour
{
    private static Transform playerHealth = null; //! Position of player health.
    private float speed = 0.0f; //! Speed of projectile.

    private bool arrived = false; //! Is projectile arrived to player health UI?
    private float timer = 0.5f; //! Timer for after arrived to player health. Showing polishing UI.

    // Start is called before the first frame update
    void Start()
    {
        if (playerHealth == null)
            playerHealth = GameObject.Find("PlayerHealth").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = playerHealth.position - gameObject.transform.position;

        if (arrived)
        {
            timer -= Time.deltaTime;

            // UI ends.
            if (timer <= 0.0f)
            {
                RewardManager.instance.ReducePlayerHealth();
                Destroy(gameObject);
            }
            else // Show UI.
            {
                Vector3 position = playerHealth.position;
                position.x += Random.Range(-0.5f, 0.5f);
                position.y += Random.Range(-0.5f, 0.5f);

                gameObject.transform.position = position;
            }

            return;
        }

        // Close enough.
        if (move.magnitude < 1.0f)
        {
            arrived = true;
            return;
        }

        // Accelerate projectile.
        speed += Time.deltaTime;

        // Move projectile.
        gameObject.transform.position += (move.normalized * speed);
    }
}
