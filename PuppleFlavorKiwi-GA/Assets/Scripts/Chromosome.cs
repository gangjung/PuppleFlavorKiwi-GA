using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/*
 * 유전자 관련 클래스.
 */
public class Chromosome
{
    /*
     * 가지고 있어야 하는 목록.
     * 유전자 리스트.
     */

    // 유전자 길이
    const int MAX_LENGTH = 4;

    //public List<Gene> Genes
    public Gene[] Genes
    {
        get
        {
            return genes;
        }
        set {
            if (Length() != value.Length)
                return;

            genes.CopyTo(value, 0);
        }
    }
    private Gene[] genes;   // List가 아닌 Array를 사용한 이유는, 부하가 적기 때문이다. 그리고 List나 Array나 MAX_LENGH 값만 바꿔주면 유동적으로 사용가능.

    ////////////////////////////////////////////////////////////
    public Chromosome()
    {
        genes = new Gene[MAX_LENGTH];
    }

    public int Length() { return genes.Length; }
}

public class Gene
{
    public MOVE move = 0;

    ////////////////////////////////////////////////////////////
    public Gene()
    {
        move = (MOVE)UnityEngine.Random.Range(0, 3);
    }
    
}

// 사용할 유전자 정보.
public enum MOVE
{
    UP = 0,
    DOWN,
    RIGHT,
    LEFT,
}