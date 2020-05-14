using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

/*
 * 유전자 알고리즘 관련 매니저입니다.
 */
public class GeneticManager : Singleton<GeneticManager>
{
    /*
     * *** Todo List ***
     * (변수)
     * 유전체 - 대략 100개부터 해보자.
     * 
     * (함수)
     * 초기 유전자 생성 함수
     * 자손 선택 알고리즘
     * 자손 교차 알고리즘
     * 자손 변이 알고리즘
     */

    // 사용할 유전자 갯수.
    const int CHROMOSOME_CAPACITY = 100;

    //private List<Chromosome> chromosomeList;
    private Chromosome[] chromosomeList;

    public bool IsFinishInitData;

    ////////////////////////////////////////////////////////////
    private void Awake()
    {
        IsFinishInitData = false;

        //chromosomeList = new List<Chromosome>();
        //chromosomeList = new Chromosome[CHROMOSOME_CAPACITY];
    }

    ////////////////////////////////////////////////////////////
    /*
     * 초기 유전자 세팅
     */
    public void SetInitialGenericData()
    {
        //for (int i = 0; i < CHROMOSOME_CAPACITY; ++i)
        //{
        //    chromosomeList.Add(new Chromosome());
        //}
        if (chromosomeList != null)
        {
            chromosomeList = null;
            GC.Collect();
        }

        chromosomeList = new Chromosome[CHROMOSOME_CAPACITY];

        IsFinishInitData = true;

        Debug.Log("GeneticManager-Set Initial Generic Data");
    }

    /*
     * 자손 유전자 생성 알고리즘
     */
    public void StartSelectOffspring()
    {

    }

    /*
     * 유전자 교차 알고리즘
     */
    public void StartCrossOver()
    {
        
    }

    /*
     * 유전자 돌연변이 알고리즘
     */
    public void StartMutation()
    {

    }
}
