/*!*******************************************************************
\file         RewardManager.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/09/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*!*******************************************************************
\class        RewardManager
\brief		  Manages UI for reward selection, and the result of each battle.
********************************************************************/
public class RewardManager : MonoBehaviour
{
    // Singleton pattern
    public static RewardManager instance;
    private RewardManager() { }

    public Text playerHealthUI; //! UI for player health.
    private int playerHealth = 30; //! Current player health.
    private int playerHealthTemp = 30; //! If player defeat, player's health will be reduced to this value.

    public GameObject rerollButton; //! Reroll button object.
    public GameObject rewardUI; //! Reward UI object.
    public DisplayTargets targetDisplayer; //! Inactivate target displayer while reward UI is activated.

    public ParticleSystem particle; //! Particle for when player wins.

    // size = 3
    public RewardSelection[] selections; //! Each new possible item.

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        rewardUI.SetActive(false);
    }

    void Start()
    {
        playerHealthUI.text = playerHealth.ToString();
    }

    /*!
     * \brief Initialize new items and set UI.
     * 
     * \param win
     *        If player wins, the number of options is one.
     *        Otherwis, two or three.
     */
    private void NewRewards(bool win)
    {
        // Set UI.
        rewardUI.SetActive(true);
        targetDisplayer.Inactivate();

        int numberOfOptions = 1;

        for (int i = 0; i < 3; ++i)
        {
            Item item = new Item();

            // If player defeated,
            if (win == false) // choose random number of options.
                numberOfOptions = Random.Range(2, 4);

            item.Initialize(numberOfOptions);
            selections[i].Initialize(item);
        }
    }

    /*!
     * \brief If player win, show rewards with particle and win sound.
     */
    public void PlayerWin()
    {
        // Generate items with only one option.
        NewRewards(true);
        rerollButton.SetActive(false);

        if (particle.isPlaying)
            particle.Stop();

        particle.Play();
        AudioManager.instance.Play("BattleWin");
    }

    /*!
     * \brief If player defeated, waits for defeat UI finishing.
     */
    public void PlayerDefeated(int numberOfEnemy)
    {
        // Computed expected value of new player health.
        playerHealthTemp = playerHealth - numberOfEnemy;
    }

    /*!
     * \brief Each time projectile UI reaches to player health UI, that will call this.
     *        Reduce player health, and checks whether it ends or player lost the game.
     */
    public void ReducePlayerHealth()
    {
        // Update UI.
        --playerHealth;
        playerHealthUI.text = playerHealth.ToString();
        AudioManager.instance.Play("PlayerHealth");

        // Check player died.
        if (playerHealth < 0) // Game ended.
            SceneManager.LoadScene("LoseScene", LoadSceneMode.Single);
        // No more projectiles remain.
        else if (playerHealth <= playerHealthTemp)
        {
            // Give rewards having 2 or 3 options with reroll button.
            NewRewards(false);
            rerollButton.SetActive(true);
            UnitManager.instance.BattleEnd();

            AudioManager.instance.Play("BattleLose");
        }
    }

    /*!
     * \brief Player chose new reward, so close the UI.
     */
    public void SelectionMade()
    {
        foreach (var selection in selections)
            selection.DestoryOldIcons();

        rewardUI.SetActive(false);
        targetDisplayer.Activate();
    }

    /*!
     * \brief Player decided to reroll.
     */
    public void Reroll()
    {
        foreach (var selection in selections)
            selection.DestoryOldIcons();

        NewRewards(false);
    }
}
