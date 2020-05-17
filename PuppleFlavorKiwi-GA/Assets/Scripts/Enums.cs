using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public enum SIMUL_STATE
    {
        INIT,
        SIMULATION,
        CHECK_FITNESS,
        SELECT_OFFSPRING,
        CROSSOVER,
        MUTATION,
    }
}