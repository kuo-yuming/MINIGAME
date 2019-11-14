using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnum
{
    public enum COUNTRY_ENUM
    {
        EMPIRE = 0,//帝國
        KINGDOM = 1,//王國
        BUSINESS = 2,//商國
        MAX,
    }

    public enum BUILDING_ENUM
    {
        BUSINESS = 0,//商業
        TECHNOLOGY = 1,//科技
        NATIONAL_DEFENSE = 2,//國防
        SOLDIER_INFIGHTIOG = 3,//近戰
        SOLDIER_DISTANT = 4,//遠戰
        SOLDIER_DEFENSE = 5,//防禦
        MAX,
    }

    public enum SOLDIER_ENUM
    {
        INFIGHTING = 0,//近戰
        DISTANT = 1,//遠攻
        MAX,
    }

    public enum PALYER_ENUM
    {
        PLAYER,//玩家
        ENEMY,//敵方
        MAX,
    }
    //顯示資料
    public enum DATE_SHOW_ENUM
    {
        BUILDING,//建築
        SOLDIER,//士兵
        MAX,
    }
    //起始位子
    public enum SEAT_ENUM
    {
        SEAT_ONE,
        SEAT_TWO,
        MAX,
    }
    //角色狀態
    public enum CHARA_MODE_ENUM
    {
        STOP,
        RAN,
        ATTACK,
        DEAD,
        MAX,
    }
}
