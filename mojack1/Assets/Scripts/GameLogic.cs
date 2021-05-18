using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    //다음 레벨까지 필요한 레벨ㅇ업 통 용량
    public static  float ExpForNextLevel(int curLv)
    {
        if (curLv == 0) return 0;
        return (curLv * curLv + curLv + 3) * 3;
    }
}
