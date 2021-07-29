/*!*******************************************************************
\file         UnitPrefabs.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/28/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*!*******************************************************************
\class        UnitPrefabs
\brief		  Load all unit prefabs and store them.
********************************************************************/
public class UnitPrefabs : MonoBehaviour
{
    // Singleton pattern
    public static UnitPrefabs instance;
    private UnitPrefabs() { }

    private Dictionary<UnitData.Type, GameObject> basePrefabs = new Dictionary<UnitData.Type, GameObject>(); //! Base type unit prefabs. Total 5.
    private Dictionary<UnitData.Type, Dictionary<UnitData.Type, GameObject>> firstUpgradePrefabs = new Dictionary<UnitData.Type, Dictionary<UnitData.Type, GameObject>>(); //! First upgraded unit prefabs. Total 20.
    private Dictionary<UnitData.Type, Dictionary<UnitData.Type, Dictionary<UnitData.Type, GameObject>>> secondUpgradePrefabs = new Dictionary<UnitData.Type, Dictionary<UnitData.Type, Dictionary<UnitData.Type, GameObject>>>(); //! Second upgraded unit prefabs. Total 60.

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadBasePrefabs();
        LoadFirstUpgradePrefabs();
        LoadSecondUpgradePrefabs();
    }

    /*!
     * \brief Getter method for unit prefab.
     * 
     * \param unit
     *        Data of unit to request prefab.
     */
    public GameObject GetPrefab(UnitData unit)
    {
        if (unit.thirdType != UnitData.Type.Default)
            return secondUpgradePrefabs[unit.baseType][unit.secondType][unit.thirdType];

        if (unit.secondType != UnitData.Type.Default)
            return firstUpgradePrefabs[unit.baseType][unit.secondType];

        return basePrefabs[unit.baseType];
    }

    /*!
     * \brief Load all base type unit prefabs.
     */
    private void LoadBasePrefabs()
    {
        basePrefabs.Add(UnitData.Type.Defender, Resources.Load("Prefabs/Units/Defender/Defender", typeof(GameObject)) as GameObject);
        basePrefabs.Add(UnitData.Type.Healer, Resources.Load("Prefabs/Units/Healer/Healer", typeof(GameObject)) as GameObject);
        basePrefabs.Add(UnitData.Type.Supporter, Resources.Load("Prefabs/Units/Supporter/Supporter", typeof(GameObject)) as GameObject);
        basePrefabs.Add(UnitData.Type.Melee, Resources.Load("Prefabs/Units/Melee/Melee", typeof(GameObject)) as GameObject);
        basePrefabs.Add(UnitData.Type.Forward, Resources.Load("Prefabs/Units/Forward/Forward", typeof(GameObject)) as GameObject);
    }

    /*!
     * \brief Load all first upgraded type unit prefabs.
     */
    private void LoadFirstUpgradePrefabs()
    {
        Dictionary<UnitData.Type, GameObject> defenders = new Dictionary<UnitData.Type, GameObject>();
        defenders.Add(UnitData.Type.Healer, Resources.Load("Prefabs/Units/Defender/Paladin/Paladin", typeof(GameObject)) as GameObject);
        defenders.Add(UnitData.Type.Forward, Resources.Load("Prefabs/Units/Defender/Slark/Slark", typeof(GameObject)) as GameObject);
        defenders.Add(UnitData.Type.Supporter, Resources.Load("Prefabs/Units/Defender/Stopper/Stopper", typeof(GameObject)) as GameObject);
        defenders.Add(UnitData.Type.Melee, Resources.Load("Prefabs/Units/Defender/Tank/Tank", typeof(GameObject)) as GameObject);
        firstUpgradePrefabs.Add(UnitData.Type.Defender, defenders);

        Dictionary<UnitData.Type, GameObject> healers = new Dictionary<UnitData.Type, GameObject>();
        healers.Add(UnitData.Type.Melee, Resources.Load("Prefabs/Units/Healer/Bard/Bard", typeof(GameObject)) as GameObject);
        healers.Add(UnitData.Type.Defender, Resources.Load("Prefabs/Units/Healer/Blacksmith/Blacksmith", typeof(GameObject)) as GameObject);
        healers.Add(UnitData.Type.Forward, Resources.Load("Prefabs/Units/Healer/Priest/Priest", typeof(GameObject)) as GameObject);
        healers.Add(UnitData.Type.Supporter, Resources.Load("Prefabs/Units/Healer/Summoner/Summoner", typeof(GameObject)) as GameObject);
        firstUpgradePrefabs.Add(UnitData.Type.Healer, healers);

        Dictionary<UnitData.Type, GameObject> supporters = new Dictionary<UnitData.Type, GameObject>();
        supporters.Add(UnitData.Type.Healer, Resources.Load("Prefabs/Units/Supporter/CentreMidfielder/CentreMidfielder", typeof(GameObject)) as GameObject);
        supporters.Add(UnitData.Type.Melee, Resources.Load("Prefabs/Units/Supporter/Enganche/Enganche", typeof(GameObject)) as GameObject);
        supporters.Add(UnitData.Type.Defender, Resources.Load("Prefabs/Units/Supporter/DefensiveMidfielder/DefensiveMidfielder", typeof(GameObject)) as GameObject);
        supporters.Add(UnitData.Type.Forward, Resources.Load("Prefabs/Units/Supporter/Playmaker/Playmaker", typeof(GameObject)) as GameObject);
        firstUpgradePrefabs.Add(UnitData.Type.Supporter, supporters);

        Dictionary<UnitData.Type, GameObject> melees = new Dictionary<UnitData.Type, GameObject>();
        melees.Add(UnitData.Type.Healer, Resources.Load("Prefabs/Units/Melee/Assassin/Assassin", typeof(GameObject)) as GameObject);
        melees.Add(UnitData.Type.Defender, Resources.Load("Prefabs/Units/Melee/FullBack/FullBack", typeof(GameObject)) as GameObject);
        melees.Add(UnitData.Type.Supporter, Resources.Load("Prefabs/Units/Melee/Teleporter/Teleporter", typeof(GameObject)) as GameObject);
        melees.Add(UnitData.Type.Forward, Resources.Load("Prefabs/Units/Melee/WingForward/WingForward", typeof(GameObject)) as GameObject);
        firstUpgradePrefabs.Add(UnitData.Type.Melee, melees);

        Dictionary<UnitData.Type, GameObject> forwards = new Dictionary<UnitData.Type, GameObject>();
        forwards.Add(UnitData.Type.Healer, Resources.Load("Prefabs/Units/Forward/Poisoner/Poisoner", typeof(GameObject)) as GameObject);
        forwards.Add(UnitData.Type.Melee, Resources.Load("Prefabs/Units/Forward/Striker/Striker", typeof(GameObject)) as GameObject);
        forwards.Add(UnitData.Type.Supporter, Resources.Load("Prefabs/Units/Forward/Turret/Turret", typeof(GameObject)) as GameObject);
        forwards.Add(UnitData.Type.Defender, Resources.Load("Prefabs/Units/Forward/Wizard/Wizard", typeof(GameObject)) as GameObject);
        firstUpgradePrefabs.Add(UnitData.Type.Forward, forwards);
    }

    /*!
     * \brief Load all second upgraded type unit prefabs.
     */
    private void LoadSecondUpgradePrefabs()
    {
        LoadSecondUpgradeDefenders();
        LoadSecondUpgradeHealers();
        LoadSecondUpgradeSupporters();
        LoadSecondUpgradeMelees();
        LoadSecondUpgradeForwards();
    }

    /*!
     * \brief Load all second upgraded type unit prefabs from defender.
     */
    private void LoadSecondUpgradeDefenders()
    {
        Dictionary<UnitData.Type, Dictionary<UnitData.Type, GameObject>> defenders = new Dictionary<UnitData.Type, Dictionary<UnitData.Type, GameObject>>();

        Dictionary<UnitData.Type, GameObject> paladins = new Dictionary<UnitData.Type, GameObject>();
        paladins.Add(UnitData.Type.Supporter, Resources.Load("Prefabs/Units/Defender/Paladin/Abaddon", typeof(GameObject)) as GameObject);
        paladins.Add(UnitData.Type.Melee, Resources.Load("Prefabs/Units/Defender/Paladin/CombatMedic", typeof(GameObject)) as GameObject);
        paladins.Add(UnitData.Type.Forward, Resources.Load("Prefabs/Units/Defender/Paladin/Inquisitor", typeof(GameObject)) as GameObject);
        defenders.Add(UnitData.Type.Healer, paladins);

        Dictionary<UnitData.Type, GameObject> stoppers = new Dictionary<UnitData.Type, GameObject>();
        stoppers.Add(UnitData.Type.Healer, Resources.Load("Prefabs/Units/Defender/Stopper/Cannavaro", typeof(GameObject)) as GameObject);
        stoppers.Add(UnitData.Type.Forward, Resources.Load("Prefabs/Units/Defender/Stopper/Commander", typeof(GameObject)) as GameObject);
        stoppers.Add(UnitData.Type.Melee, Resources.Load("Prefabs/Units/Defender/Stopper/Fighter", typeof(GameObject)) as GameObject);
        defenders.Add(UnitData.Type.Supporter, stoppers);

        Dictionary<UnitData.Type, GameObject> tanks = new Dictionary<UnitData.Type, GameObject>();
        tanks.Add(UnitData.Type.Supporter, Resources.Load("Prefabs/Units/Defender/Tank/AroundTank", typeof(GameObject)) as GameObject);
        tanks.Add(UnitData.Type.Healer, Resources.Load("Prefabs/Units/Defender/Tank/CounterTank", typeof(GameObject)) as GameObject);
        tanks.Add(UnitData.Type.Forward, Resources.Load("Prefabs/Units/Defender/Tank/LongTank", typeof(GameObject)) as GameObject);
        defenders.Add(UnitData.Type.Melee, tanks);

        Dictionary<UnitData.Type, GameObject> slarks = new Dictionary<UnitData.Type, GameObject>();
        slarks.Add(UnitData.Type.Melee, Resources.Load("Prefabs/Units/Defender/Slark/PowerSlark", typeof(GameObject)) as GameObject);
        slarks.Add(UnitData.Type.Healer, Resources.Load("Prefabs/Units/Defender/Slark/ProtectionSlark", typeof(GameObject)) as GameObject);
        slarks.Add(UnitData.Type.Supporter, Resources.Load("Prefabs/Units/Defender/Slark/SpeedSlark", typeof(GameObject)) as GameObject);
        defenders.Add(UnitData.Type.Forward, slarks);

        secondUpgradePrefabs.Add(UnitData.Type.Defender, defenders);
    }

    /*!
     * \brief Load all second upgraded type unit prefabs from healer.
     */
    private void LoadSecondUpgradeHealers()
    {
        Dictionary<UnitData.Type, Dictionary<UnitData.Type, GameObject>> healers = new Dictionary<UnitData.Type, Dictionary<UnitData.Type, GameObject>>();

        Dictionary<UnitData.Type, GameObject> blacksmiths = new Dictionary<UnitData.Type, GameObject>();
        blacksmiths.Add(UnitData.Type.Supporter, Resources.Load("Prefabs/Units/Healer/Blacksmith/Armorsmith", typeof(GameObject)) as GameObject);
        blacksmiths.Add(UnitData.Type.Forward, Resources.Load("Prefabs/Units/Healer/Blacksmith/Bladesmith", typeof(GameObject)) as GameObject);
        blacksmiths.Add(UnitData.Type.Melee, Resources.Load("Prefabs/Units/Healer/Blacksmith/Shieldsmith", typeof(GameObject)) as GameObject);
        healers.Add(UnitData.Type.Defender, blacksmiths);

        Dictionary<UnitData.Type, GameObject> summoners = new Dictionary<UnitData.Type, GameObject>();
        summoners.Add(UnitData.Type.Defender, Resources.Load("Prefabs/Units/Healer/Summoner/DefenderSummoner/DefenderSummoner", typeof(GameObject)) as GameObject);
        summoners.Add(UnitData.Type.Forward, Resources.Load("Prefabs/Units/Healer/Summoner/ForwardSummoner/ForwardSummoner", typeof(GameObject)) as GameObject);
        summoners.Add(UnitData.Type.Melee, Resources.Load("Prefabs/Units/Healer/Summoner/MeleeSummoner/MeleeSummoner", typeof(GameObject)) as GameObject);
        healers.Add(UnitData.Type.Supporter, summoners);

        Dictionary<UnitData.Type, GameObject> bards = new Dictionary<UnitData.Type, GameObject>();
        bards.Add(UnitData.Type.Supporter, Resources.Load("Prefabs/Units/Healer/Bard/Dazzle", typeof(GameObject)) as GameObject);
        bards.Add(UnitData.Type.Defender, Resources.Load("Prefabs/Units/Healer/Bard/DoubleBard", typeof(GameObject)) as GameObject);
        bards.Add(UnitData.Type.Forward, Resources.Load("Prefabs/Units/Healer/Bard/TotalBard", typeof(GameObject)) as GameObject);
        healers.Add(UnitData.Type.Melee, bards);

        Dictionary<UnitData.Type, GameObject> priests = new Dictionary<UnitData.Type, GameObject>();
        priests.Add(UnitData.Type.Melee, Resources.Load("Prefabs/Units/Healer/Priest/DealingPriest", typeof(GameObject)) as GameObject);
        priests.Add(UnitData.Type.Defender, Resources.Load("Prefabs/Units/Healer/Priest/HighPriest", typeof(GameObject)) as GameObject);
        priests.Add(UnitData.Type.Supporter, Resources.Load("Prefabs/Units/Healer/Priest/HoTPriest", typeof(GameObject)) as GameObject);
        healers.Add(UnitData.Type.Forward, priests);

        secondUpgradePrefabs.Add(UnitData.Type.Healer, healers);
    }

    /*!
     * \brief Load all second upgraded type unit prefabs from supporter.
     */
    private void LoadSecondUpgradeSupporters()
    {
        Dictionary<UnitData.Type, Dictionary<UnitData.Type, GameObject>> supporters = new Dictionary<UnitData.Type, Dictionary<UnitData.Type, GameObject>>();

        Dictionary<UnitData.Type, GameObject> defensiveMidfielders = new Dictionary<UnitData.Type, GameObject>();
        defensiveMidfielders.Add(UnitData.Type.Melee, Resources.Load("Prefabs/Units/Supporter/DefensiveMidfielder/AnchorMan", typeof(GameObject)) as GameObject);
        defensiveMidfielders.Add(UnitData.Type.Forward, Resources.Load("Prefabs/Units/Supporter/DefensiveMidfielder/Halfback", typeof(GameObject)) as GameObject);
        defensiveMidfielders.Add(UnitData.Type.Healer, Resources.Load("Prefabs/Units/Supporter/DefensiveMidfielder/Regista", typeof(GameObject)) as GameObject);
        supporters.Add(UnitData.Type.Defender, defensiveMidfielders);

        Dictionary<UnitData.Type, GameObject> centreMidfielders = new Dictionary<UnitData.Type, GameObject>();
        centreMidfielders.Add(UnitData.Type.Forward, Resources.Load("Prefabs/Units/Supporter/CentreMidfielder/BallWinningMidfielder", typeof(GameObject)) as GameObject);
        centreMidfielders.Add(UnitData.Type.Defender, Resources.Load("Prefabs/Units/Supporter/CentreMidfielder/BoxToBox", typeof(GameObject)) as GameObject);
        centreMidfielders.Add(UnitData.Type.Melee, Resources.Load("Prefabs/Units/Supporter/CentreMidfielder/Mezzala", typeof(GameObject)) as GameObject);
        supporters.Add(UnitData.Type.Healer, centreMidfielders);

        Dictionary<UnitData.Type, GameObject> enganches = new Dictionary<UnitData.Type, GameObject>();
        enganches.Add(UnitData.Type.Defender, Resources.Load("Prefabs/Units/Supporter/Enganche/DoubleEnganche", typeof(GameObject)) as GameObject);
        enganches.Add(UnitData.Type.Healer, Resources.Load("Prefabs/Units/Supporter/Enganche/FlexibleEnganche", typeof(GameObject)) as GameObject);
        enganches.Add(UnitData.Type.Forward, Resources.Load("Prefabs/Units/Supporter/Enganche/TotalEnganche", typeof(GameObject)) as GameObject);
        supporters.Add(UnitData.Type.Melee, enganches);

        Dictionary<UnitData.Type, GameObject> playmakers = new Dictionary<UnitData.Type, GameObject>();
        playmakers.Add(UnitData.Type.Defender, Resources.Load("Prefabs/Units/Supporter/Playmaker/AdvancedPlaymaker", typeof(GameObject)) as GameObject);
        playmakers.Add(UnitData.Type.Healer, Resources.Load("Prefabs/Units/Supporter/Playmaker/ResurrectionPlaymaker", typeof(GameObject)) as GameObject);
        playmakers.Add(UnitData.Type.Melee, Resources.Load("Prefabs/Units/Supporter/Playmaker/ShadowStriker", typeof(GameObject)) as GameObject);
        supporters.Add(UnitData.Type.Forward, playmakers);

        secondUpgradePrefabs.Add(UnitData.Type.Supporter, supporters);
    }

    /*!
     * \brief Load all second upgraded type unit prefabs from melee.
     */
    private void LoadSecondUpgradeMelees()
    {
        Dictionary<UnitData.Type, Dictionary<UnitData.Type, GameObject>> melees = new Dictionary<UnitData.Type, Dictionary<UnitData.Type, GameObject>>();

        Dictionary<UnitData.Type, GameObject> fullBacks = new Dictionary<UnitData.Type, GameObject>();
        fullBacks.Add(UnitData.Type.Healer, Resources.Load("Prefabs/Units/Melee/FullBack/DefensiveFullBack", typeof(GameObject)) as GameObject);
        fullBacks.Add(UnitData.Type.Supporter, Resources.Load("Prefabs/Units/Melee/FullBack/InvertedWingBack", typeof(GameObject)) as GameObject);
        fullBacks.Add(UnitData.Type.Forward, Resources.Load("Prefabs/Units/Melee/FullBack/WingBack", typeof(GameObject)) as GameObject);
        melees.Add(UnitData.Type.Defender, fullBacks);

        Dictionary<UnitData.Type, GameObject> assassins = new Dictionary<UnitData.Type, GameObject>();
        assassins.Add(UnitData.Type.Forward, Resources.Load("Prefabs/Units/Melee/Assassin/CriticalAssassin", typeof(GameObject)) as GameObject);
        assassins.Add(UnitData.Type.Supporter, Resources.Load("Prefabs/Units/Melee/Assassin/HealAssassin", typeof(GameObject)) as GameObject);
        assassins.Add(UnitData.Type.Defender, Resources.Load("Prefabs/Units/Melee/Assassin/PermanentAssassin", typeof(GameObject)) as GameObject);
        melees.Add(UnitData.Type.Healer, assassins);

        Dictionary<UnitData.Type, GameObject> teleporters = new Dictionary<UnitData.Type, GameObject>();
        teleporters.Add(UnitData.Type.Forward, Resources.Load("Prefabs/Units/Melee/Teleporter/AroundTeleporter", typeof(GameObject)) as GameObject);
        teleporters.Add(UnitData.Type.Healer, Resources.Load("Prefabs/Units/Melee/Teleporter/DoubleHealTeleporter", typeof(GameObject)) as GameObject);
        teleporters.Add(UnitData.Type.Defender, Resources.Load("Prefabs/Units/Melee/Teleporter/ShieldTeleporter", typeof(GameObject)) as GameObject);
        melees.Add(UnitData.Type.Supporter, teleporters);

        Dictionary<UnitData.Type, GameObject> wingForwards = new Dictionary<UnitData.Type, GameObject>();
        wingForwards.Add(UnitData.Type.Supporter, Resources.Load("Prefabs/Units/Melee/WingForward/InsideForward", typeof(GameObject)) as GameObject);
        wingForwards.Add(UnitData.Type.Defender, Resources.Load("Prefabs/Units/Melee/WingForward/WideTargetMan", typeof(GameObject)) as GameObject);
        wingForwards.Add(UnitData.Type.Healer, Resources.Load("Prefabs/Units/Melee/WingForward/Winger", typeof(GameObject)) as GameObject);
        melees.Add(UnitData.Type.Forward, wingForwards);

        secondUpgradePrefabs.Add(UnitData.Type.Melee, melees);
    }

    /*!
     * \brief Load all second upgraded type unit prefabs from forward.
     */
    private void LoadSecondUpgradeForwards()
    {
        Dictionary<UnitData.Type, Dictionary<UnitData.Type, GameObject>> forwards = new Dictionary<UnitData.Type, Dictionary<UnitData.Type, GameObject>>();

        Dictionary<UnitData.Type, GameObject> wizards = new Dictionary<UnitData.Type, GameObject>();
        wizards.Add(UnitData.Type.Healer, Resources.Load("Prefabs/Units/Forward/Wizard/ChainWizard", typeof(GameObject)) as GameObject);
        wizards.Add(UnitData.Type.Melee, Resources.Load("Prefabs/Units/Forward/Wizard/PenetrationWizard", typeof(GameObject)) as GameObject);
        wizards.Add(UnitData.Type.Supporter, Resources.Load("Prefabs/Units/Forward/Wizard/WizardOfTyphoon", typeof(GameObject)) as GameObject);
        forwards.Add(UnitData.Type.Defender, wizards);

        Dictionary<UnitData.Type, GameObject> poisoners = new Dictionary<UnitData.Type, GameObject>();
        poisoners.Add(UnitData.Type.Melee, Resources.Load("Prefabs/Units/Forward/Poisoner/AttackPoisoner", typeof(GameObject)) as GameObject);
        poisoners.Add(UnitData.Type.Defender, Resources.Load("Prefabs/Units/Forward/Poisoner/HealPoisoner", typeof(GameObject)) as GameObject);
        poisoners.Add(UnitData.Type.Supporter, Resources.Load("Prefabs/Units/Forward/Poisoner/MovePoisoner", typeof(GameObject)) as GameObject);
        forwards.Add(UnitData.Type.Healer, poisoners);

        Dictionary<UnitData.Type, GameObject> turrets = new Dictionary<UnitData.Type, GameObject>();
        turrets.Add(UnitData.Type.Melee, Resources.Load("Prefabs/Units/Forward/Turret/AdvancedTurret", typeof(GameObject)) as GameObject);
        turrets.Add(UnitData.Type.Healer, Resources.Load("Prefabs/Units/Forward/Turret/MultipleTurret", typeof(GameObject)) as GameObject);
        turrets.Add(UnitData.Type.Defender, Resources.Load("Prefabs/Units/Forward/Turret/TrueTurret", typeof(GameObject)) as GameObject);
        forwards.Add(UnitData.Type.Supporter, turrets);

        Dictionary<UnitData.Type, GameObject> strikers = new Dictionary<UnitData.Type, GameObject>();
        strikers.Add(UnitData.Type.Defender, Resources.Load("Prefabs/Units/Forward/Striker/Poacher", typeof(GameObject)) as GameObject);
        strikers.Add(UnitData.Type.Supporter, Resources.Load("Prefabs/Units/Forward/Striker/TargetMan", typeof(GameObject)) as GameObject);
        strikers.Add(UnitData.Type.Healer, Resources.Load("Prefabs/Units/Forward/Striker/Trequartista", typeof(GameObject)) as GameObject);
        forwards.Add(UnitData.Type.Melee, strikers);

        secondUpgradePrefabs.Add(UnitData.Type.Forward, forwards);
    }
}
