﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightManager : MonoBehaviour {

    public static FightManager singleton;

    public Enemy actualEnemy;

    //[SerializeField]
    //GameObject m_modelContainer;
    [SerializeField]
    float m_heroHPMax = 3;
    float m_heroHP;
    [SerializeField]
    GameObject m_inventoryContainer;
    [SerializeField]
    Transform m_enemyContainer;
    [SerializeField]
    Sprite lamiaSprite;
    [SerializeField]
    Sprite fairySprite;
    //[SerializeField]
    //int m_itemNbr;

    List<Enemy> m_enemyPool = new List<Enemy>();

    private void Start()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        m_heroHP = m_heroHPMax;
        GenerateEnemyPool();
    }

    public void GenerateEnemyPool ()
    {
        InventoryItem[] items = m_inventoryContainer.GetComponentsInChildren<InventoryItem>();
        Enemy model = m_enemyContainer.gameObject.GetComponentInChildren<Enemy>();
        foreach (InventoryItem item in items)
        {
            Enemy newEnemy = Instantiate(model.gameObject).GetComponent<Enemy>();
            newEnemy.transform.SetParent(m_enemyContainer, false);
            newEnemy.monsterCategory = item.GetVulnerabilities()[Random.Range(0, item.GetVulnerabilities().Count)];
            switch (newEnemy.monsterCategory)
            {
                case Enemy.category.Lamia:
                    newEnemy.gameObject.GetComponent<Image>().sprite = lamiaSprite;
                    break;
                case Enemy.category.Fairy:
                    newEnemy.gameObject.GetComponent<Image>().sprite = fairySprite;
                    break;
            }
            m_enemyPool.Add(newEnemy);
            newEnemy.gameObject.SetActive(false);
        }
        model.gameObject.SetActive(false);
    }

    public void SelectEnnemy ()
    {
        actualEnemy = m_enemyPool[Random.Range(0, m_enemyPool.Count)];
        m_enemyPool.Remove(actualEnemy);
        actualEnemy.gameObject.SetActive(true);
    }

    public void TakeDamage (float damage)
    {
        m_heroHP -= damage;
        if (m_heroHP <= 0)
        {
            Die();
        }
    }

    void Die ()
    {
        gameObject.SetActive(false);
        print("Hero dies");
    }

    //public void GenerateInventory ()
    //{
        //m_modelContainer.SetActive(true);
        //InventoryItem[] models = m_modelContainer.GetComponentsInChildren<InventoryItem>();
        //float maxProba = 0;
        //foreach (InventoryItem model in models)
        //{
        //    maxProba += model.GetProbability();
        //}
        //for (int i = 0; i < m_itemNbr; i++)
        //{
        //    SelectNewItem();
        //}
        //m_modelContainer.SetActive(false);
    //}

    //void SelectNewItem ()
    //{
    //        float result = Random.Range(0, maxProba);
    //        float actualStep = 0;
    //        GameObject newIntem;
    //        foreach (InventoryItem model in models)
    //        {
    //            actualStep += model.GetProbability();
    //            if (result <= actualStep)
    //            {
    //                newIntem = Instantiate(model.gameObject);
    //                newIntem.transform.SetParent(m_inventoryContainer, false);
    //                return;
    //            }
    //        }
    //}
}
