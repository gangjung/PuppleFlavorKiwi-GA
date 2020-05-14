﻿using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
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

    private SIMUL_STATE gameState;

    ////////////////////////////////////////////////////////////
    void Awake()
    {
        Debug.Log("GameManager-Awake");

        curGeneration = 0;
        simulSpeed = 1.0f;

        gameState = SIMUL_STATE.INIT;
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
        if (gameState == SIMUL_STATE.INIT)
        {
            return;
        }

        switch (gameState)
        {
            case SIMUL_STATE.START:
                break;

            case SIMUL_STATE.DOING:
                break;
        }
    }

    ////////////////////////////////////////////////////////////
    public void StartSimulation()
    {
        gameState = SIMUL_STATE.START;
    }

    /*
     * 염색체를 가진 오브젝트 생성.
     */
    public void SpawnObjects()
    {

    }

    public void TestClick()
    {
        CurGeneration++;
    }

    public void UpdateGeneration(int InGenCnt)
    {
        Debug.Log("UpdateGeneration");

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
}
