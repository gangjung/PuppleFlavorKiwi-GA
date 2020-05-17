using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Common;

public class HUDManager : Singleton_Object<HUDManager>
{
    public GameObject UI_HUD;
    public Text UI_GenerationText;

    // Start is called before the first frame update
    void Start()
    {
        if(UI_HUD == null)
        {
            /*** 이런 식으로 찾으면 안된다. 이름이 아닌, 코드로 찾을 수 있어야 한다. ***/
            UI_HUD = GameObject.Find("HUD");
            if (UI_HUD == null) return;
        }

        if(UI_GenerationText == null)
        {
            GameObject obj = GameObject.Find("Txt_Gen");
            if(obj != null)
            {
                UI_GenerationText = obj.GetComponent<Text>();
            }
            if (UI_GenerationText == null) return;
        }

        UI_GenerationText.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateGeneration(int InGenCnt)
    {
        if(InGenCnt < 0)
        {
            Debug.LogErrorFormat("Input Generation Count : %d, 현재 업데이트 되려는 Generation Count가 정상적이지 않습니다.", InGenCnt);
            return;
        }

        if(UI_GenerationText == null)
        {
            Debug.LogError("Generation을 표시할 UI를 찾지 못했습니다.");
            return;
        }

        UI_GenerationText.text = InGenCnt.ToString();
    }
}
