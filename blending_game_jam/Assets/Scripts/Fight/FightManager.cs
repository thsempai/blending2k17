﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FightManager : MonoBehaviour {

    public static FightManager singleton;

    public Enemy actualEnemy;

    public bool stoping = false;
    Animator anim;
//[SerializeField]
//GameObject m_modelContainer;
[SerializeField]
    float m_heroHPMax = 3;
    float m_heroHP;
    [SerializeField]
    GameObject m_UIContainer;
    [SerializeField]
    GameObject m_inventoryContainer;
    [SerializeField]
    Transform m_enemyContainer;
    [SerializeField]
    GameObject m_roomManager;
    [SerializeField]
    AudioClip heroDies;
    [SerializeField]
    AudioClip monsterDies;
    [SerializeField]
    Sprite lamiaSprite;
    [SerializeField]
    Sprite fairySprite;
    [SerializeField]
    Sprite demonSprite;
    [SerializeField]
    Sprite spiderLadySprite;
    [SerializeField]
    Sprite vampireSprite;
    [SerializeField]
    Sprite werewolfSprite;
    [SerializeField]
    Sprite minotaurSprite;
    [SerializeField]
    Sprite zombieSprite;
    [SerializeField]
    Sprite dryadSprite;
    //[SerializeField]
    //int m_itemNbr;
    AudioSource m_player;

    List<Enemy> m_enemyPool = new List<Enemy>();

    private int enemiesCount = 0;

    private void Start()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        m_player = GetComponent<AudioSource>();
        m_heroHP = m_heroHPMax;
        GenerateEnemyPool();
        m_UIContainer.SetActive(false);
    }

    Enemy.category ChoseEnemyCat (List<Enemy.category> potentialCats)
    {
        if (potentialCats.Count == 1)
        {
            return potentialCats[0];
        }
        int leastPresentCatNumber = enemiesCount;
        List<Enemy.category> minoritaries = new List<Enemy.category>();
        foreach (Enemy.category catToCheck in potentialCats)
        {
            int presentCat = 0;
            foreach (Enemy enemy in m_enemyPool)
            {
                if (enemy.monsterCategory == catToCheck)
                {
                    presentCat++;
                }
            }
            if (presentCat < leastPresentCatNumber)
            {
                leastPresentCatNumber = presentCat;
                minoritaries.Clear();
            }
            if (presentCat == leastPresentCatNumber)
            {
                minoritaries.Add(catToCheck);
            }
        }
        if (minoritaries.Count == 1)
        {
            return minoritaries[0];
        }
        return minoritaries[Random.Range(0, minoritaries.Count)];
    }

    public void GenerateEnemyPool ()
    {
        InventoryItem[] items = m_inventoryContainer.GetComponentsInChildren<InventoryItem>();
        Enemy model = m_enemyContainer.gameObject.GetComponentInChildren<Enemy>();
        foreach (InventoryItem item in items)
        {
            enemiesCount++;
            Enemy newEnemy = Instantiate(model.gameObject).GetComponent<Enemy>();
            newEnemy.transform.SetParent(m_enemyContainer, false);
            newEnemy.monsterCategory = ChoseEnemyCat(item.GetVulnerabilities());
             anim=newEnemy.gameObject.AddComponent<Animator>();
            anim.runtimeAnimatorController = Resources.Load("monster") as RuntimeAnimatorController;
            switch (newEnemy.monsterCategory)
            {
                case Enemy.category.Lamia:
                    newEnemy.gameObject.GetComponent<Image>().sprite = lamiaSprite;
                    newEnemy.gameObject.name = "Lamia";
                    break;
                case Enemy.category.Fairy:
                    newEnemy.gameObject.GetComponent<Image>().sprite = fairySprite;
                    newEnemy.gameObject.name = "Fairy";
                    break;
                case Enemy.category.Demon:
                    newEnemy.gameObject.GetComponent<Image>().sprite = demonSprite;
                    newEnemy.gameObject.name = "Demon";
                    break;
                case Enemy.category.SpiderLady:
                    newEnemy.gameObject.GetComponent<Image>().sprite = spiderLadySprite;
                    newEnemy.gameObject.name = "SpiderLady";
                    break;
                case Enemy.category.Vampire:
                    newEnemy.gameObject.GetComponent<Image>().sprite = vampireSprite;
                    newEnemy.gameObject.name = "Vampire";
                    break;
                case Enemy.category.Werewolf:
                    newEnemy.gameObject.GetComponent<Image>().sprite = werewolfSprite;
                    newEnemy.gameObject.name = "Werewolf";
                    break;
                case Enemy.category.Minotaur:
                    newEnemy.gameObject.GetComponent<Image>().sprite = minotaurSprite;
                    newEnemy.gameObject.name = "Minotaur";
                    break;
                case Enemy.category.Zombie:
                    newEnemy.gameObject.GetComponent<Image>().sprite = zombieSprite;
                    newEnemy.gameObject.name = "Zombie";
                    break;
                case Enemy.category.Ent:
                    newEnemy.gameObject.GetComponent<Image>().sprite = dryadSprite;
                    newEnemy.gameObject.name = "Dryad";
                    break;
            }
            m_enemyPool.Add(newEnemy);
            newEnemy.gameObject.SetActive(false);
        }
        model.gameObject.SetActive(false);
    }

    public void StartFight ()
    {
        if (actualEnemy == null)
        {
            m_roomManager.SetActive(false);
            m_UIContainer.SetActive(true);
            actualEnemy = m_enemyPool[Random.Range(0, m_enemyPool.Count)];
            m_enemyPool.Remove(actualEnemy);
            actualEnemy.gameObject.SetActive(true);
        
        }
    }

    public void StopFight ()
    {
        stoping = true;
        if (m_heroHP > 0)
        {
            int score = PlayerPrefs.GetInt("score");
            PlayerPrefs.SetInt("score", score + 1);
            StartCoroutine(PlayWhenPossible(monsterDies));
        }
        else
        {

            StartCoroutine(PlayWhenPossible(heroDies));
        }
    }

    public void TakeDamage (float damage)
    {
        m_heroHP -= damage;
        if (m_heroHP <= 0)
        {
            Die();
        }
    }

    public void PlayFightSound (AudioClip soundToPlay)
    {
        print(soundToPlay);
        m_player.clip = soundToPlay;
        m_player.Play();
    }

    IEnumerator PlayWhenPossible (AudioClip soundToPlay)
    {
        yield return new WaitForSeconds(1f);
        PlayFightSound(soundToPlay);
        yield return new WaitForSeconds(1f);

        if (m_heroHP <= 0)
        {
            SceneManager.LoadScene("game_over");
        }
        Destroy(actualEnemy.gameObject);
        actualEnemy.gameObject.SetActive(false);
        actualEnemy = null;
        yield return new WaitForSeconds(1f);
        stoping = false;
        m_UIContainer.SetActive(false);
        m_roomManager.SetActive(true);
    }

    void Die ()
    {
        StopFight();
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
