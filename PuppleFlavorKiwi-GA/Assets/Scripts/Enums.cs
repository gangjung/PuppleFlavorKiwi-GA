using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public enum SIMUL_STATE
    {
        INIT,
        STOP,
        START,
        DOING,
        SELECT_OFFSPRING,
        CROSSOVER,
        MUTATION,
    }
}