using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObject : MonoBehaviour
{
    public enum STATE
    {
        MOVE,
        STOP,
    }

    public Vector3 SpawnPos
    {
        get { return spawnPos; }
    }
    private Vector3 spawnPos;

    // 나중에 Property로 바꾸자.
    public Chromosome chromosome;

    private int idxGenetic;
    private STATE state;

    ////////////////////////////////////////////////////////////
    private void Awake()
    {
        //chromosome = new Chromosome();
        chromosome = null;
        idxGenetic = 0;
        state = STATE.STOP;

        spawnPos = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (state == STATE.STOP)
            return;

        float speed = Time.deltaTime * 3.0f * GameManager.SimulSpeed;

        // 이렇게 유전자에서 백터를 가져오는게 아니라... 여기서... 하는게 더 맞는 판단일까???
        // 어디서 판단하는게 맞는걸까...?
        // 일단 enum을 여기서 사용하지 않기 위해서 이런 방식을 사용한 것임.
        Vector3 vector = chromosome.GetGene(idxGenetic).GetMoveVector();

        transform.position += vector * speed;
    }

    ////////////////////////////////////////////////////////////
    public void ResetObject()
    {
        idxGenetic = 0;
        state = STATE.STOP;
        transform.position = spawnPos;
    }
    
    public void NextStep()
    {
        ++idxGenetic;
    }

    public void Stop()
    {
        state = STATE.STOP;
    }

    public void Play()
    {
        state = STATE.MOVE;
    }
}