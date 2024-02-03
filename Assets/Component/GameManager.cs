using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    //기본 변수들
    //public int hp = 100;  -> hp는 magicCircleComponent에서 직접 컨트롤
    public int gold = 10;
    public float atk = 1f;
    public bool isGameOver = false;
    public bool isBoss = false;


    // 스태미나 관련 변수들
    public float maxStamina = 30f;
    public float Stamina = 30f;
    public float stamina_usage = 5f;
    public float stamina_RecoverSpeed = 3f;
    public float stamina_cool = 1f;


    // 마법진 관련 변수들
    public float max_Progress = 50f;
    public float Progress = 0f;
    public float Progress_speed = 1f;


    public static GameManager instance = null;
    public magicCircleComponent magicCircle;
    public GameObject Spawner;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            
        }

        else
            Destroy(this.gameObject);
        
    }

    void Update()
    {
        if (!isGameOver)
        {
            if (isBoss)
                return;
            else
            {
                Progress += (Progress_speed * Time.unscaledDeltaTime);
                UiManager.instance.ProgressBarUpdate();

                if (Progress >= max_Progress)
                {
                    isBoss = true;
                    Spawner.GetComponent<SpawnComponent>().BossSpawn();
                }
            }
        }
    }


    
    public void ChangeAtk(int data)
    {
        atk += data;
        if (atk < 0)
        {
            atk = 0;
            
        }
    }

    public void ChangeGold(int data)
    {
        gold += data;
        if (gold < 0)
        {
            gold = 0;
            
        }
    }

    public bool StaminaUse()
    {


        if (Stamina - stamina_usage >= 0)
        {
            Stamina -= stamina_usage;
            return true;
        }
        else
        {
            Debug.Log("스태미나 부족");
            return false;
        }
    }

    public void StaminaRecover()
    {

        Stamina += (stamina_RecoverSpeed*Time.unscaledDeltaTime);
        if (Stamina > maxStamina)
            Stamina = maxStamina;
    }

    public void SpawnLevelUp()
    {
        Spawner.GetComponent<SpawnComponent>().LevelUp();
        Progress = 0f;
        max_Progress += 5f;
        UiManager.instance.ProgressBarUpdate();
    }
}
