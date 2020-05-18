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
        SELECTION_AND_CROSSOVER,
        MUTATION,
    }
}