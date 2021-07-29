/*!*******************************************************************
\file         EnemyManager.cs
\author       Hyungseob Kim
\par          email: hn02415 \@ gmail.com
\date         06/20/2021

All content © 2021 DigiPen (USA) Corporation, all rights reserved.
********************************************************************/
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/*!*******************************************************************
\class        EnemyManager
\brief		  Generates each level.
              Has information about composition and formation of enemies.
********************************************************************/
public class EnemyManager : MonoBehaviour
{
    // Singleton pattern
    public static EnemyManager instance;
    private EnemyManager() { }

    private TileManager tileManager;
    public Text stageUI; //! UI shows current stage number.

    private List<Action> levels = new List<Action>(); //! List of levels.
    private int round = 0; //! Index to levels.

    private List<Vector3Int> positions = new List<Vector3Int>(); //! List of positions of units. Must be corresponds to units list.
    private List<UnitData> units = new List<UnitData>(); //! List of data of units. Must be corresponds to positions.
    Array types = Enum.GetValues(typeof(UnitData.Type)); // Type of base classes. Being used for random class selection.

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
        tileManager = TileManager.instance;

        // Add all levels.
        levels.Add(Level1);
        levels.Add(Level2);
        levels.Add(Level3);
        levels.Add(BossLevel1);
        levels.Add(Level4);
        levels.Add(Level5);
        levels.Add(Level6);
        levels.Add(BossLevel2);
        levels.Add(Level7);
        levels.Add(Level8);
        levels.Add(Level9);
        levels.Add(BossLevel3);
    }

    /*!
     * \brief Prepare units for next level.
     * \return bool
     *         Return true, if it is the last round.
     *         Otherwise, return false.
     */
    public bool NextRound()
    {
        // Clear lists.
        positions.Clear();
        units.Clear();

        // Get level data.
        levels[round]();
        GiveStatusBonusByLevel();

        // Spawn units.
        for (int i = 0; i < units.Count; ++i)
        {
            units[i].index = i;
            SpawnUnit(units[i], tileManager.tilemap.CellToWorld(positions[i]));
        }

        // If level is not the last one,
        if (round < levels.Count - 1)
        {
            // increases index and return false.
            ++round;
            return false;
        }

        // Last stage.
        return true;
    }

    /*!
     * \brief Give bonus to status of all units according to current level.
     */
    private void GiveStatusBonusByLevel()
    {
        foreach (UnitData unitData in units)
        {
            switch (unitData.baseType)
            {
                // Melee and supporter gain extra speed.
                case UnitData.Type.Melee:
                case UnitData.Type.Supporter:
                    unitData.speed += round;
                    break;

                // Healer and defender gain extra health.
                case UnitData.Type.Healer:
                case UnitData.Type.Defender:
                    unitData.health += (round * 10.0f);
                    break;

                // Forward gains extra power.
                case UnitData.Type.Forward:
                    unitData.power += round;
                    break;
            }
        }
    }

    /*!
     * \brief Level 1 spawns 5 defenders and 5 forwards.
     */
    private void Level1()
    {
        stageUI.text = "Stage 1";

        // Set positions for defenders.
        positions.Add(new Vector3Int(0, 1, 0));

        int preset = UnityEngine.Random.Range(0, 4);
        
        if (preset == 0 || preset == 1)
        {
            positions.Add(new Vector3Int(-1, 1, 0));
            positions.Add(new Vector3Int(1, 1, 0));
        }

        if (preset != 1)
        {
            positions.Add(new Vector3Int(-2, 1, 0));
            positions.Add(new Vector3Int(2, 1, 0));
        }

        if (preset == 1 || preset == 2)
        {
            positions.Add(new Vector3Int(-3, 1, 0));
            positions.Add(new Vector3Int(3, 1, 0));
        }
        else if (preset == 3)
        {
            positions.Add(new Vector3Int(-4, 1, 0));
            positions.Add(new Vector3Int(4, 1, 0));
        }

        for (int i = 0; i < 5; ++i)
            units.Add(GetRandomData(UnitData.Type.Defender));

        // Set positions for forwards.
        positions.Add(new Vector3Int(0, 3, 0));

        preset = UnityEngine.Random.Range(0, 7);

        if (preset < 3)
        {
            positions.Add(new Vector3Int(0, 4, 0));
            positions.Add(new Vector3Int(1, 4, 0));
        }

        if (preset == 0 || preset == 3)
        {
            positions.Add(new Vector3Int(-1, 4, 0));
            positions.Add(new Vector3Int(2, 4, 0));
        }
        else if (preset == 1 || preset == 4 || preset == 5)
        {
            positions.Add(new Vector3Int(-1, 3, 0));
            positions.Add(new Vector3Int(1, 3, 0));
        }

        if (preset == 2 || preset == 4 || preset == 6)
        {
            positions.Add(new Vector3Int(-2, 3, 0));
            positions.Add(new Vector3Int(2, 3, 0));
        }

        if (preset == 3 || preset == 5 || preset == 6)
        {
            positions.Add(new Vector3Int(-3, 3, 0));
            positions.Add(new Vector3Int(3, 3, 0));
        }

        for (int i = 0; i < 5; ++i)
            units.Add(GetRandomData(UnitData.Type.Forward));
    }

    /*!
     * \brief Level 2 spawns 3 defenders, 2 forwards, and 4 melees.
     */
    private void Level2()
    {
        stageUI.text = "Stage 2";

        // Set positions for 4 melees.
        int meleePreset = UnityEngine.Random.Range(0, 3);
        switch (meleePreset)
        {
            case 0:
                positions.Add(new Vector3Int(-3, 2, 0));
                positions.Add(new Vector3Int(-2, 2, 0));
                positions.Add(new Vector3Int(3, 2, 0));
                positions.Add(new Vector3Int(4, 2, 0));
                break;

            case 1:
                positions.Add(new Vector3Int(-3, 3, 0));
                positions.Add(new Vector3Int(-2, 3, 0));
                positions.Add(new Vector3Int(2, 3, 0));
                positions.Add(new Vector3Int(3, 3, 0));
                break;

            case 2:
                positions.Add(new Vector3Int(-3, 3, 0));
                positions.Add(new Vector3Int(3, 3, 0));
                positions.Add(new Vector3Int(-2, 2, 0));
                positions.Add(new Vector3Int(3, 2, 0));
                break;
        }

        for (int i = 0; i < 4; ++i)
            units.Add(GetRandomData(UnitData.Type.Melee));

        // Set positions for 3 defenders.
        positions.Add(new Vector3Int(0, 1, 0));

        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            positions.Add(new Vector3Int(-1, 1, 0));
            positions.Add(new Vector3Int(1, 1, 0));
        }
        else
        {
            positions.Add(new Vector3Int(-1, 1, 0));
            positions.Add(new Vector3Int(1, 1, 0));
        }
        
        for (int i = 0; i < 3; ++i)
            units.Add(GetRandomData(UnitData.Type.Defender));

        // Set positions for 2 forwards.
        positions.Add(new Vector3Int(0, 3, 0));

        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            if (UnityEngine.Random.Range(0, 2) == 0)
            {
                positions.Add(new Vector3Int(0, 4, 0));
                positions.Add(new Vector3Int(1, 4, 0));
            }
            else
            {
                positions.Add(new Vector3Int(-1, 4, 0));
                positions.Add(new Vector3Int(2, 4, 0));
            }
        }
        else // Beneath line.
        {
            if (meleePreset == 1 || UnityEngine.Random.Range(0, 2) == 0)
            {
                positions.Add(new Vector3Int(-1, 3, 0));
                positions.Add(new Vector3Int(1, 3, 0));
            }
            else
            {
                positions.Add(new Vector3Int(-2, 3, 0));
                positions.Add(new Vector3Int(2, 3, 0));
            }
        }

        units.Add(GetRandomData(UnitData.Type.Forward));
        units.Add(GetRandomData(UnitData.Type.Forward));
    }

    /*!
     * \brief Level 3 spawns 3 defenders / 1 supporter / 4 forwards OR 4 defenders / 2 supporter / 2 forwards.
     *        Both spawns 2 healers commonly.
     */
    private void Level3()
    {
        stageUI.text = "Stage 3";
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            // 1 supporter
            positions.Add(new Vector3Int(0, 3, 0));
            units.Add(GetRandomData(UnitData.Type.Supporter));

            // 4 forwards
            for (int i = -1; i <= 2; ++i)
            {
                positions.Add(new Vector3Int(i, 4, 0));
                units.Add(GetRandomData(UnitData.Type.Forward));
            }

            // 3 defenders, 2 healers
            if (UnityEngine.Random.Range(0, 2) == 0)
            {
                // Narrow
                for (int i = -1; i <= 1; ++i)
                {
                    positions.Add(new Vector3Int(i, 1, 0));
                    units.Add(GetRandomData(UnitData.Type.Defender));
                }

                positions.Add(new Vector3Int(0, 2, 0));
                positions.Add(new Vector3Int(1, 2, 0));
                units.Add(GetRandomData(UnitData.Type.Healer));
                units.Add(GetRandomData(UnitData.Type.Healer));
            }
            else // Wide
            {
                for (int i = -2; i <= 2; i += 2)
                {
                    positions.Add(new Vector3Int(i, 1, 0));
                    units.Add(GetRandomData(UnitData.Type.Defender));
                }

                positions.Add(new Vector3Int(-1, 2, 0));
                positions.Add(new Vector3Int(2, 2, 0));
                units.Add(GetRandomData(UnitData.Type.Healer));
                units.Add(GetRandomData(UnitData.Type.Healer));
            }
        }
        else
        {
            // 2 supporter
            positions.Add(new Vector3Int(-1, 3, 0));
            positions.Add(new Vector3Int(1, 3, 0));
            units.Add(GetRandomData(UnitData.Type.Supporter));
            units.Add(GetRandomData(UnitData.Type.Supporter));

            // 2 Forwards
            positions.Add(new Vector3Int(0, 4, 0));
            positions.Add(new Vector3Int(1, 4, 0));
            units.Add(GetRandomData(UnitData.Type.Forward));
            units.Add(GetRandomData(UnitData.Type.Forward));

            // 4 defenders
            if (UnityEngine.Random.Range(0, 2) == 0)
            {
                // Narrow
                positions.Add(new Vector3Int(-1, 1, 0));
                positions.Add(new Vector3Int(-2, 1, 0));
                positions.Add(new Vector3Int(1, 1, 0));
                positions.Add(new Vector3Int(2, 1, 0));

                for (int i = 0; i < 4; ++i)
                    units.Add(GetRandomData(UnitData.Type.Defender));
            }
            else // Wide
            {
                for (int i = -3; i <= 3; i += 2)
                {
                    positions.Add(new Vector3Int(i, 1, 0));
                    units.Add(GetRandomData(UnitData.Type.Defender));
                }
            }

            // 2 healers
            positions.Add(new Vector3Int(0, 2, 0));
            positions.Add(new Vector3Int(1, 2, 0));
            units.Add(GetRandomData(UnitData.Type.Healer));
            units.Add(GetRandomData(UnitData.Type.Healer));
        }
    }

    /*!
     * \brief Level 4 has same structure with level 1, but 2 of them are upgraded once.
     */
    private void Level4()
    {
        // Reuse same formation.
        Level1();
        stageUI.text = "Stage 4";

        // Choose two randomly.
        List<int> indices = GetRandomInts(2, units.Count);

        // Upgrade them.
        foreach(int index in indices)
        {
            UnitData data = units[index];
            AssignOneRandomUpgrade(ref data);
            units[index] = data;
        }
    }

    /*!
     * \brief Level 5 has same structure with level 2, but 3 of them are upgraded once.
     */
    private void Level5()
    {
        // Reuse same formation.
        Level2();
        stageUI.text = "Stage 5";

        // Choose three units.
        List<int> indices = GetRandomInts(3, units.Count);

        // Upgrade them.
        foreach (int index in indices)
        {
            UnitData data = units[index];
            AssignOneRandomUpgrade(ref data);
            units[index] = data;
        }
    }

    /*!
     * \brief Level 6 has same structure with level 3, but 4 of them are upgraded once.
     */
    private void Level6()
    {
        // Reuse same formation.
        Level3();
        stageUI.text = "Stage 6";

        // Choose four units.
        List<int> indices = GetRandomInts(4, units.Count);

        // Upgrade them.
        foreach (int index in indices)
        {
            UnitData data = units[index];
            AssignOneRandomUpgrade(ref data);
            units[index] = data;
        }
    }

    /*!
     * \brief Level 7 has same structure with level 1, but 4 of them are upgraded once and 2 of them are upgraded twice.
     */
    private void Level7()
    {
        // Reuse same formation.
        Level1();
        stageUI.text = "Stage 7";

        // Choose six units.
        List<int> indices = GetRandomInts(6, units.Count);

        // Upgrade them.
        foreach (int index in indices)
        {
            UnitData data = units[index];
            AssignOneRandomUpgrade(ref data);
            units[index] = data;
        }

        // Choose two from first upgraded units.
        indices = GetRandomInts(2, indices.Count);

        // Upgrade them once more.
        foreach (int index in indices)
        {
            UnitData data = units[index];
            AssignOneRandomUpgrade(ref data);
            units[index] = data;
        }
    }

    /*!
     * \brief Level 8 has same structure with level 2, but 4 of them are upgraded once and 3 of them are upgraded twice.
     */
    private void Level8()
    {
        // Reuse same formation.
        Level2();
        stageUI.text = "Stage 8";

        // Choose seven units.
        List<int> indices = GetRandomInts(7, units.Count);

        // Upgrade them.
        foreach (int index in indices)
        {
            UnitData data = units[index];
            AssignOneRandomUpgrade(ref data);
            units[index] = data;
        }

        // Choose three from first upgraded units.
        indices = GetRandomInts(3, indices.Count);

        // Upgrade them once more.
        foreach (int index in indices)
        {
            UnitData data = units[index];
            AssignOneRandomUpgrade(ref data);
            units[index] = data;
        }
    }

    /*!
     * \brief Level 9 has same structure with level 3, but 4 of them are upgraded once and 4 of them are upgraded twice.
     */
    private void Level9()
    {
        // Reuse same formation.
        Level3();
        stageUI.text = "Stage 9";

        // Choose eight units.
        List<int> indices = GetRandomInts(8, units.Count);

        // Upgrade them.
        foreach (int index in indices)
        {
            UnitData data = units[index];
            AssignOneRandomUpgrade(ref data);
            units[index] = data;
        }

        // Choose four from first upgraded units.
        indices = GetRandomInts(4, indices.Count);

        // Upgrade them once more.
        foreach (int index in indices)
        {
            UnitData data = units[index];
            AssignOneRandomUpgrade(ref data);
            units[index] = data;
        }
    }

    /*!
     * \brief This is template for all boss levels.
     *        They have same structure with fixed positions and base class.
     */
    private void BossLevelTemplate()
    {
        positions.Add(new Vector3Int(-1, 1, 0));
        positions.Add(new Vector3Int(1, 1, 0));
        units.Add(GetRandomData(UnitData.Type.Defender));
        units.Add(GetRandomData(UnitData.Type.Defender));

        positions.Add(new Vector3Int(0, 2, 0));
        positions.Add(new Vector3Int(1, 2, 0));
        units.Add(GetRandomData(UnitData.Type.Healer));
        units.Add(GetRandomData(UnitData.Type.Healer));

        positions.Add(new Vector3Int(-1, 3, 0));
        positions.Add(new Vector3Int(1, 3, 0));
        units.Add(GetRandomData(UnitData.Type.Supporter));
        units.Add(GetRandomData(UnitData.Type.Supporter));

        positions.Add(new Vector3Int(0, 4, 0));
        positions.Add(new Vector3Int(1, 4, 0));
        units.Add(GetRandomData(UnitData.Type.Forward));
        units.Add(GetRandomData(UnitData.Type.Forward));

        positions.Add(new Vector3Int(-3, 3, 0));
        positions.Add(new Vector3Int(3, 3, 0));
        units.Add(GetRandomData(UnitData.Type.Melee));
        units.Add(GetRandomData(UnitData.Type.Melee));
    }

    /*!
     * \brief Upgrade one random unit once from boss level template, and make it as a boss.
     */
    private void BossLevel1()
    {
        // Allocate 10 units.
        BossLevelTemplate();
        stageUI.text = "Boss I";

        // Choose one randomly.
        int boss = UnityEngine.Random.Range(0, units.Count);
        UnitData bossData = new UnitData { baseType = units[boss].baseType };

        AssignOneRandomUpgrade(ref bossData);
        SetBossStatus(ref bossData);

        units[boss] = bossData;
    }

    /*!
     * \brief Upgrade 4 random units once.
     *        Upgrade one the other unit twice and make it as a boss.
     */
    private void BossLevel2()
    {
        // Reuse same formation.
        BossLevelTemplate();
        stageUI.text = "Boss II";

        // Choose five units.
        List<int> indices = GetRandomInts(5, units.Count);

        // Upgrade them.
        foreach (int index in indices)
        {
            UnitData data = units[index];
            AssignOneRandomUpgrade(ref data);
            units[index] = data;
        }

        // Choose one from first upgraded units.
        int boss = UnityEngine.Random.Range(0, units.Count);
        UnitData bossData = units[boss];

        AssignOneRandomUpgrade(ref bossData);
        SetBossStatus(ref bossData);
        units[boss] = bossData;
    }

    /*!
     * \brief Upgrade two units twice and make them as bosses.
     *        Upgrade all other units once.
     */
    private void BossLevel3()
    {
        // Reuse same formation.
        BossLevelTemplate();
        stageUI.text = "Boss III";

        // Upgrade all units.
        for (int i = 0; i < units.Count; ++i)
        {
            UnitData data = units[i];
            AssignOneRandomUpgrade(ref data);
            units[i] = data;
        }

        // Choose two units.
        List<int> indices = GetRandomInts(2, units.Count);
        foreach (int index in indices)
        {
            UnitData data = units[index];

            AssignOneRandomUpgrade(ref data);
            SetBossStatus(ref data);

            units[index] = data;
        }
    }

    /*!
     * \brief Compute value of health should be set depends on values of power and speed.
     */
    private float ComputeHealth(float power, float speed)
    {
        // Standard health * (standard power + standard speed) / ~
        return 100.0f * 40.0f / (power + speed);
    }

    /*!
     * \brief Give high status to the boss unit according to their base class.
     */
    private void SetBossStatus(ref UnitData data)
    {
        switch(data.baseType)
        {
            case UnitData.Type.Supporter:
                data.power = 10.0f;
                data.speed = 40.0f;
                data.health = 200.0f;
                break;

            case UnitData.Type.Defender:
                data.power = 20.0f;
                data.speed = 20.0f;
                data.health = 300.0f;
                break;

            case UnitData.Type.Forward:
                data.power = 40.0f;
                data.speed = 10.0f;
                data.health = 200.0f;
                break;

            case UnitData.Type.Melee:
                data.power = 10.0f;
                data.speed = 20.0f;
                data.health = 500.0f;
                break;

            case UnitData.Type.Healer:
                data.power = 25.0f;
                data.speed = 25.0f;
                data.health = 200.0f;
                break;
        }
    }

    /*!
     * \brief Upgrade given unit once.
     */
    private void AssignOneRandomUpgrade(ref UnitData data)
    {
        // No second type. Assign to second type.
        if (data.secondType == UnitData.Type.Default)
        {
            // Choose random type.
            int index = UnityEngine.Random.Range(0, 4);
            UnitData.Type type = (UnitData.Type)types.GetValue(index);

            // Same with base.
            if (data.baseType == type)
                data.secondType = (UnitData.Type)types.GetValue(4);
            else // It's fine.
                data.secondType = type;
        }
        else if (data.thirdType == UnitData.Type.Default)
        {
            // Choose random type.
            int index = UnityEngine.Random.Range(0, 3);
            UnitData.Type type = (UnitData.Type)types.GetValue(index);

            // Same with base type.
            if (data.baseType == type)
            {
                // Try with last type.
                type = (UnitData.Type)types.GetValue(4);

                // Same with second.
                if (data.secondType == type)
                    data.thirdType = (UnitData.Type)types.GetValue(3);
                else // It's fine.
                    data.thirdType = type;
            }
            else if (data.secondType == type)
            {
                // Try with last type.
                type = (UnitData.Type)types.GetValue(4);

                // Same with base.
                if (data.baseType == type)
                    data.thirdType = (UnitData.Type)types.GetValue(3);
                else // It's fine.
                    data.thirdType = type;
            }
            else // It's fine.
                data.thirdType = type;
        }
        // else fully upgraded unit.
    }

    /*!
     * \brief Choose specific numbers of random integers from given range.
     * 
     * \param numbers
     *        The number of integers to choose randomly.
     *        
     * \param last
     *        Indicates the range, which is [0, last).
     *        
     * \return List<int>
     *         Returns a list of selected random numbers.
     */
    private List<int> GetRandomInts(int numbers, int last)
    {
        List<int> seed = new List<int>();

        for (int i = 0; i < last; ++i)
            seed.Add(i);

        List<int> result = new List<int>();
        for (int i = 0; i < numbers; ++i)
        {
            int num = UnityEngine.Random.Range(0, seed.Count);
            
            result.Add(seed[num]);
            seed.RemoveAt(num);
        }

        return result;
    }

    /*!
     * \brief Spawn a unit and set properties.
     * 
     * \param unitData
     *        Data of unit to spawn.
     *        
     * \param position
     *        Position of unit to spawn.
     */
    private void SpawnUnit(UnitData unitData, Vector3 position)
    {
        // Spawn a unit on given position.
        unitData.gameObject = UnityEngine.Object.Instantiate(UnitPrefabs.instance.GetPrefab(unitData), position, Quaternion.identity);
        Status status = unitData.gameObject.GetComponent<Status>();

        // Set status of that unit.
        status.enemy = true;
        status.indexOnMemberList = unitData.index;
        
        status.power = unitData.power;
        status.speed = unitData.speed;
        status.maxHealth = unitData.health;
        
        status.useMovement = false;
        status.useAggression = false;
    }

    /*!
     * \brief Get a UnitData with random status.
     * 
     * \param type
     *        Base type of new unit.
     *        
     * \return UnitData
     *         Data of new unit with random status.
     */
    private UnitData GetRandomData(UnitData.Type type)
    {
        UnitData unitData = new UnitData {
            baseType = type,
            power = UnityEngine.Random.Range(10, 30),
            speed = UnityEngine.Random.Range(10, 30)
        };

        unitData.health = ComputeHealth(unitData.power, unitData.speed);
        return unitData;
    }

    /*!
     * \brief Getter method for units.
     */
    public UnitData GetUnitData(int index)
    {
        return units[index];
    }
}
