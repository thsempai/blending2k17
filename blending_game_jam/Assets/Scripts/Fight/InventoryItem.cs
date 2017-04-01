﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour {

    [SerializeField]
    string m_name;
    //[SerializeField]
    //float m_damage;
    //[SerializeField]
    //float m_probabilityToHave;
    [SerializeField]
    List<Ennemy.category> m_vulnerableEnnemies;
    [SerializeField]
    float m_vulnerabilityEfficiency;
    [SerializeField]
    List<Ennemy.category> m_resistantEnnemies;
    [SerializeField]
    float m_resistanceEfficiency;

    //public float GetProbability ()
    //{
    //    return m_probabilityToHave;
    //}
    
    public List<Ennemy.category> GetVulnerabilities ()
    {
        return m_vulnerableEnnemies;
    }

    public void Use ()
    {
        float damage = 1;
        Ennemy ennemy = FightManager.singleton.actualEnnemy;
        if (m_vulnerableEnnemies.Contains(ennemy.monsterCategory))
        {
            damage = 2;
        }
        else if (m_resistantEnnemies.Contains(ennemy.monsterCategory))
        {
            damage = 0;
        }
        ennemy.TakeDamage(damage);
        Destroy(gameObject);
        gameObject.SetActive(false);
    }
}
