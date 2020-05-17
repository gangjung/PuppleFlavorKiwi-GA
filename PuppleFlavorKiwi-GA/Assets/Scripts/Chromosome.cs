using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.InteropServices.WindowsRuntime;

using Common;

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
    const UInt32 MAX_LENGTH = ConstValues.MAX_CHROMOSOME_LENGTH;

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

    // Fitness
    public float fitness;
    public int number;

    ////////////////////////////////////////////////////////////
    public Chromosome()
    {
        genes = new Gene[MAX_LENGTH];
        for(int idx = 0; idx < MAX_LENGTH; ++idx)
        {
            genes[idx] = new Gene();
        }
    }
    public Chromosome(int number)
    {
        this.number = number;

        genes = new Gene[MAX_LENGTH];
        for (int idx = 0; idx < MAX_LENGTH; ++idx)
        {
            genes[idx] = new Gene();
        }
    }

    public Gene GetGene(int index)
    {
        if(index < 0 || MAX_LENGTH <= index)
        {
            return null;
        }

        return genes[index];
    }

    public int Length() { return genes.Length; }
}

public class Gene
{
    public GENE_MOVE move = 0;

    ////////////////////////////////////////////////////////////
    public Gene()
    {
        move = (GENE_MOVE)UnityEngine.Random.Range(0, 4);
    }

    public Vector3 GetMoveVector()
    {
        Vector3 result = new Vector3();

        switch (move)
        {
            case GENE_MOVE.UP:
                result.z += 1;
                break;

            case GENE_MOVE.DOWN:
                result.z -= 1;
                break;

            case GENE_MOVE.RIGHT:
                result.x += 1;
                break;

            case GENE_MOVE.LEFT:
                result.x -= 1;
                break;
        }

        return result;
    }
}

// 사용할 유전자 정보.
public enum GENE_MOVE
{
    UP = 0,
    DOWN,
    RIGHT,
    LEFT,
}