using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShareFunction
{
    //士兵圖片初始化
    static public string SoldierAtlasName(string name)
    {
        string str = name + "_" + "Stop" + "_" + "01";
        return str;
    }

    //士兵的Atlas圖像
    static public void SoliderSpriteCheck(string name, UISprite sprite)
    {
        sprite.atlas = (NGUIAtlas)Resources.Load("Atlas/" + name);
        if (sprite.atlas == null)
        {
            Debug.LogError("Atlas Name is Error,Atlas Name:" + name);
        }
    }

}
