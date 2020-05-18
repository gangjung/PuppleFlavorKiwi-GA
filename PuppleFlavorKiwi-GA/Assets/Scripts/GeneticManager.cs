using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public delegate void FitnessAlgorism(Chromosome chromosome);

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
    public Chromosome[] ChromosomesList
    {
        get { return chromosomeList; }
        set { chromosomeList = value; }
    }
    private Chromosome[] chromosomeList;

    public bool IsFinishInitData;

    public FitnessAlgorism CheckFitness;

    ////////////////////////////////////////////////////////////
    public GeneticManager()
    {
        IsFinishInitData = false;

        //chromosomeList = new List<Chromosome>();
        //chromosomeList = new Chromosome[CHROMOSOME_CAPACITY];
    }

    ////////////////////////////////////////////////////////////
    /*
     * 초기 유전자 세팅
     * -> 그냥 오브젝트가 만들어지면서 생성하고있는데... 이걸 이용하는 방법으로 바꿔야 할까???
     */
    public void SetInitialGenericData()
    {
        if (IsFinishInitData == true)
            return;

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
        for(int idx = 0; idx < ConstValues.MAX_SPAWN_OBJECT; ++idx)
        {
            chromosomeList[idx] = new Chromosome(idx);
        }

        IsFinishInitData = true;

        Debug.Log("GeneticManager-Set Initial Generic Data");
    }

    /*
     * 자손 생성 알고리즘.
     * 부모 유전자 선택 및 교배 알고리즘.
     */
    public void StartSelectionAndCrossOver()
    {
        // 새로운 자손 집단.
        Chromosome[] newList = new Chromosome[CHROMOSOME_CAPACITY];

        // Selection
        for(int idx = 0; idx < chromosomeList.Length; ++idx)
        {
            int first = SelectionAlgorism();
            int second = SelectionAlgorism();

            newList[idx] = CrossoverAlgorism(chromosomeList[first], chromosomeList[second]);
        }

        chromosomeList = newList;
        GC.Collect();
    }

    /*
     * 유전자 돌연변이 알고리즘
     */
    public void StartMutation()
    {
        for(int idx = 0; idx < chromosomeList.Length; ++idx)
        {
            int mutationPoint = UnityEngine.Random.Range(0, 1000);
            if(mutationPoint < 10)
            {
                MutationAlgorism(ref chromosomeList[idx]);

                Debug.Log("유전자 변이가 일어났다!~!!");
            }
        }
    }

    // 사용하지 않음.
    public void StartFitnessCheck()
    {
        float maxFitness = 0.0f;
        float minFitness = 0.0f;
        float curFitness = 0.0f;
        int k = 3;

        if(chromosomeList == null)
        {
            Debug.Log("Check Fitness - Null Reference : chromosomeList");
            return;
        }

        // 적합도 계산 - 실제
        for(int idx = 0; idx < chromosomeList.Length; ++idx)
        {
            CheckFitness(chromosomeList[idx]);

            float fitness = chromosomeList[idx].fitness;
            if (maxFitness < fitness)
            {
                maxFitness = fitness;
            }
            if(minFitness > fitness)
            {
                minFitness = fitness;
            }
        }

        // 적합도 계산 - 부모 선택 알고리즘에서 이용
        // ** 따로 계산 X **
        // 내가 Notion에 올린 룰렛 휠 선택 알고리즘의 적합도 계산 식이 이상하다... 음수가 나오는 상황ㅠㅠ. 알아서 해보자.
        //for (int idx = 0; idx < chromosomeList.Length; ++idx)
        //{
        //    curFitness = chromosomeList[idx].fitness;
            
            
        //    curFitness = (minFitness - curFitness) + (maxFitness - minFitness)
        //}
    }


    // 이 식을 사용할 수 있는 이유는, Array.Sort의 인자값으로 IComparer가 들어가야하는데, 그 IComparer의 형태와 같기 때문인걸로 보입니다.
    // 1. Sort를 사용할 Array의 데이터형을 2개 받는다.
    // 2. public 접근자다.
    // 3. int형을 반환한다.
    // 아마 위의 3가지를 다 만족하는 함수형태가 IComparer로 구현할 함수와 동일한 형태이므로 변환이 가능한 것 같습니다!!!
    public int FitnessSortAlgorism(Chromosome A, Chromosome B)
    {
        return A.fitness.CompareTo(B.fitness);
    }

    public int SelectionAlgorism()
    {
        // To Do : '룰렛 휠 선택' 알고리즘 적용.
        // 1. 적합도 재계산 -> 이건 적합도 구하는 부분에서 다시 구해보자.
        // 2. 적합도 총 합 계산
        // 3. 높은 순서대로 Sorting.
        // 4. 랜덤 숫자 선택
        // 5. 적합도에 맞는 자손 선택.
        int resultIdx = 0;
        float sumOfFitness = 0.0f;
        for (int idx = 0; idx < chromosomeList.Length; ++idx)
        {
            sumOfFitness += chromosomeList[idx].fitness;
        }

        // 람다를 사용할 순 있지만, 이유는 잘 모르겠다. 그러면뭐다? 정석을 사용한다. 람다를 사용할 수 있는 이유를 알면 좋을텐데 ㅠㅠㅠ
        //Array.Sort(chromosomeList, Comparer<Chromosome>.Create((a, b) => { return a.fitness.CompareTo(b.fitness); }));
        Array.Sort(chromosomeList, FitnessSortAlgorism);

        float point = UnityEngine.Random.Range(0, sumOfFitness);
        sumOfFitness = 0.0f;
        for (int idx = 0; idx < chromosomeList.Length; ++idx)
        {
            sumOfFitness += chromosomeList[idx].fitness;
            resultIdx = idx;

            if (point < sumOfFitness)
                break;
        }

        return resultIdx;
    }

    public Chromosome CrossoverAlgorism(in Chromosome first, in Chromosome second)
    {
        Chromosome result = new Chromosome();

        // 간단하게 'One Point Crossover' 기법을 사용해보자.
        int length = result.Genes.Length;
        int point = UnityEngine.Random.Range(0, length) / 2;
        for (int idx = 0; idx < length; ++idx)
        {
            if (idx < point)
                result.Genes[idx].CopyValue(first.Genes[idx].GetValue());
            else
                result.Genes[idx].CopyValue(second.Genes[idx].GetValue());
        }

        return result;
    }

    public void MutationAlgorism(ref Chromosome chromosome)
    {
        int point = UnityEngine.Random.Range(0, 5);
        int value = UnityEngine.Random.Range(0, 4);

        chromosome.Genes[point].move = (GENE_MOVE)value;
    }

    //public void CheckFitness(in Chromosome chromosome)
    //{
    //    if (chromosome == null)
    //        return;

    //    // 적합도를 구해서 chromosome 내부에 집어넣자.
    //}
}
