using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton_Object<GameManager>
{
    /*
     * *** Todo List ***
     * (변수)
     * (함수)
     * 예제 프로젝트 생성. -> 어떻게 할 것인가?
     */

    // Generation Count
    public int CurGeneration
    {
        get { return curGeneration; }
        set { 
            curGeneration = value;

            UpdateGeneration(curGeneration);
        }
    }
    private int curGeneration = 0;

    // Simulationi Speed
    public static float SimulSpeed
    {
        get { return simulSpeed; }
        set { simulSpeed = value; }
    }
    private static float simulSpeed = 0f;

    // Unit time is period between mini steps in simulation step.
    public float UnitTime
    {
        get { return unitTime / simulSpeed; }
        set { unitTime = value; }
    }
    private float unitTime = 0.0f;

    private float localTime;

    private SIMUL_STATE gameState;
    // Step과 유전자의 갯수를 동기화 시킬 것.
    private int simulStep;
    private const UInt32 maxSimulStep = ConstValues.MAX_CHROMOSOME_LENGTH;
    private bool isSimulPause;
    private bool isMoveNext;

    private GeneticManager geneticManager;

    public Transform startPos;
    public TestObject testObject;
    public List<TestObject> objList;

    ////////////////////////////////////////////////////////////
    void Awake()
    {
        Debug.Log("GameManager-Awake");

        curGeneration = 0;
        simulSpeed = 2.0f;
        UnitTime = 1.0f;

        localTime = 0.0f;

        gameState = SIMUL_STATE.INIT;
        simulStep = 0;
        
        isSimulPause = false;
        isMoveNext = false;

        geneticManager = GeneticManager.Instance;
        geneticManager.CheckFitness = CheckFitness;

        objList = new List<TestObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("GameManager-Start");

        // 로드시 삭제 방지.
        if(transform.GetComponent<DontDestroyComponent>() == null)
        {
            gameObject.AddComponent<DontDestroyComponent>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool isUpdateStop = (gameState == SIMUL_STATE.INIT) || isSimulPause == true;
        if (isUpdateStop)
        {
            return;
        }

        switch (gameState)
        {
            case SIMUL_STATE.SIMULATION:
                DoSimulation();
                break;

            case SIMUL_STATE.CHECK_FITNESS:
                geneticManager.StartFitnessCheck();
                break;

            case SIMUL_STATE.SELECTION_AND_CROSSOVER:
                geneticManager.StartSelectionAndCrossOver();
                MoveNextStep();
                break;

            case SIMUL_STATE.MUTATION:
                geneticManager.StartMutation();
                MoveNextStep();
                break;
        }

        if (isMoveNext)
        {
            MoveNextStep_Internal();

            if (gameState == SIMUL_STATE.SIMULATION)
            {
                StartNextGeneration();
            }
        }
    }

    ////////////////////////////////////////////////////////////
    public void StartSimulation()
    {
        if (gameState != SIMUL_STATE.INIT)
            return;

        // 임시로 초기데이터 생성로직을 이곳에 둠.
        geneticManager.SetInitialGenericData();

        SpawnObjects();

        gameState = SIMUL_STATE.SIMULATION;
        RestartSimulation();
    }

    public void DoSimulation()
    {
        // Time
        localTime += Time.deltaTime;

        // Exeception
        if (localTime < UnitTime)
            return;

        localTime = 0.0f;
        simulStep++;

        if (simulStep < maxSimulStep)
        {
            foreach (TestObject obj in objList)
            {
                obj.NextStep();
            }
        }
        else
        {
            FiniishSimulation();
        }
    }

    private void FiniishSimulation()
    {
        Debug.Log("Finish Simulation");

        foreach (TestObject obj in objList)
        {
            obj.Stop();
        }

        MoveNextStep();
    }

    public void PauseSimulation()
    {
        isSimulPause = true;

        foreach (TestObject obj in objList)
        {
            obj.Stop();
        }
    }
    public void RestartSimulation()
    {
        isSimulPause = false;

        foreach (TestObject obj in objList)
        {
            obj.Play();
        }
    }

    public void ResetSimulation()
    {
        gameState = SIMUL_STATE.INIT;

        simulStep = 0;

        localTime = 0.0f;

        isSimulPause = false;
        isMoveNext = false;
    }

    /*
     * 염색체를 가진 오브젝트 생성.
     * *** 실행 시, 생성하는 것도 좋지만, 미리 생성해두자.
     */
    public void SpawnObjects()
    {
        if (objList.Count == 0)
        {
            Vector3 spawnPos = startPos.position;
            Vector3 interval = new Vector3(0, 0, 5);

            for (int idx = 0; idx < ConstValues.MAX_SPAWN_OBJECT; ++idx)
            {
                // Object Pool은... 아직 계획에 없습니다.
                TestObject obj = Instantiate<TestObject>(testObject, spawnPos, startPos.rotation, startPos);
                spawnPos += interval;

                objList.Add(obj);
            }
        }
        
        ResetObjects();

        SetChromosomeInObject();
    }

    public void ResetObjects()
    {
        foreach (TestObject obj in objList)
        {
            obj.ResetObject();
        }
    }

    public void SetChromosomeInObject()
    {
        Chromosome[] chromosomes = geneticManager.ChromosomesList;

        if (objList.Count > chromosomes.Length)
        {
            Debug.Log("Set Chromosome In Object - 생성된 Object 수보다 생성된 염색체의 수가 작습니다.");
            return;
        }

        int idx = 0;
        foreach (TestObject obj in objList)
        {
            obj.chromosome = chromosomes[idx];
            idx++;
        }
    }

    /*
     * 다음 Step으로 이동.
     */
    public void MoveNextStep()
    {
        isMoveNext = true;
    }

    private void MoveNextStep_Internal()
    {
        if (gameState == SIMUL_STATE.MUTATION)
        {
            gameState = SIMUL_STATE.SIMULATION;
        }
        else
        {
            gameState++;
        }
    }

    private void StartNextGeneration()
    {
        ++CurGeneration;

        ResetSimulation();

        StartSimulation();
    }

    public void UpdateGeneration(int InGenCnt)
    {
        Debug.Log("Update Generation");

        // Update UI
        HUDManager hudManager = HUDManager.Instance;
        if(hudManager == null)
        {
            Debug.LogError("HUDManager를 찾을 수 없습니다.");
            return;
        }

        hudManager.UpdateGeneration(InGenCnt);

        Debug.Log("세대 정보가 업데이트 되었습니다.");
    }

    public void CheckFitness(Chromosome chromosome)
    {
        float result = 0.0f;

        // 목적지는 외부요인. Object 종류마다 다르다. Object의 상황을 가져올 수 있어야 한다.

        TestObject obj = objList[chromosome.number];
        Vector3 spawnPos = obj.SpawnPos;
        float goalPosX = spawnPos.x + 15;

        result = obj.transform.position.x / goalPosX;
        
        // 명확한 계산식을 나타내기 위해 풀어서 작성.
        chromosome.fitness = (result + 1) * 100 / 2;
        //chromosome.fitness = obj.transform.position.x + 15;

        Debug.Log("Fitness Check 완료!!!!");
    }
}
