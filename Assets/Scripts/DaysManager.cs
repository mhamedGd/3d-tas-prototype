using System;
using System.Collections.Generic;
using AndoomiUtils;
using FischlWorks_FogWar;
using UnityEngine;

public class DaysManager : MonoBehaviour
{
    [SerializeField] float dayLength;
    [SerializeField] float nightLength;

    [SerializeField] Color dayColor;
    [SerializeField] Color nightColor;
    [SerializeField] float dayAlpha;
    [SerializeField] float nightAlpha;

    DaysCounter daysCounter = new();
    CountdownTimer dayTimer;
    CountdownTimer nightTimer;

    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject enemyPrefabFirstDay;
    [SerializeField] int enemySpawns;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] Transform startDaySpawnPoint;
    [SerializeField] Queue<GameObject> spawnedEnemies = new();

    [SerializeField] Transform mapCenter;

    [SerializeField] Transform player;
    List<csFogWar.FogRevealer> playerRevealer = new();

    csFogWar[] allFogs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dayTimer = new(dayLength);
        nightTimer = new(nightLength);

        dayTimer.OnTimerStart += () =>
        {
            daysCounter.PassDay();
            void SpawnEnemy(GameObject prefab, Transform spawnPoint, float randomDistance)
            {
                spawnedEnemies.Enqueue(Instantiate(prefab, spawnPoint.position + UnityEngine.Random.insideUnitCircle.Vec2ToVec3Z() * randomDistance, spawnPoint.rotation));
            }
            var allFogs = FindObjectsByType<csFogWar>(FindObjectsSortMode.None);

            foreach (var f in allFogs)
            {
                f.SetFogColor(dayColor);

                f.SetFogAlpha(dayAlpha);
            }
            for(int i = 0; i < enemySpawns; i++)
            {
                var spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
                SpawnEnemy(enemyPrefab, spawnPoint, 16);
            }
            if (daysCounter.IsFirstDay())
            {
                SpawnEnemy(enemyPrefabFirstDay, startDaySpawnPoint, 2);
                print(daysCounter.number);
            }
        };
        nightTimer.OnTimerStart += () =>
        {
            var allFogs = FindObjectsByType<csFogWar>(FindObjectsSortMode.None);
            foreach (var f in allFogs)
            {
                f.SetFogColor(nightColor);
                f.SetFogAlpha(nightAlpha);

                while(spawnedEnemies.Count > 0)
                {
                    var ene = spawnedEnemies.Dequeue();
                    if (ene == null) continue;
                    ene.GetComponent<Enemy>().SetTargetPosition((ene.transform.position-mapCenter.position).normalized * 20);
                    Destroy(ene, 5);
                }
            }
        };

        dayTimer.OnTimerStop += () =>
        {
            nightTimer.Start();
        };

        nightTimer.OnTimerStop += () =>
        {
            dayTimer.Start();
            foreach(var r in playerRevealer) r.SetSightRange(originalPlayerSight);

        };

        nightTimer.OnTimerTick += (dt) => {
            oneSecond -= dt;
            if(oneSecond < 0) {
                oneSecond = 2;
                foreach(var r in playerRevealer) {
                    if(r._SightRange > 0 && !IsPlayerInLight()) r.SetSightRange(r._SightRange-1);
                }
            }
        };

        allFogs = FindObjectsByType<csFogWar>(FindObjectsSortMode.None);
        for(int i = 0; i < allFogs.Length;i++) {
            foreach(var r in allFogs[i]._FogRevealers) {
                if(r._RevealerTransform == player) {
                    playerRevealer.Add(r);
                    break;
                }
            }
        }
        originalPlayerSight = playerRevealer[0]._SightRange;

        dayTimer.Start();


    }

    float oneSecond = 2;
    int originalPlayerSight;

    // Update is called once per frame
    void Update()
    {
        dayTimer.Tick();
        nightTimer.Tick();

    }

    bool IsPlayerInLight()
    {
        bool result = false;
        foreach(var f in allFogs) {
            foreach(var r in f._FogRevealers) {
                if(r._RevealerTransform == player) continue;
                var d = Vector3.Distance(player.position, r._RevealerTransform.position);
                if(d <= r._SightRange){
                    result = true;
                    goto BREAKINGLOOP;
                }
            }
        }
        BREAKINGLOOP:
        return result;
    }

    public void NewDay() {
        nightTimer.Stop();
    }
}

[Serializable]
class DaysCounter {
    public int number;

    public int PassDay()
    {
        number += 1;
        return number;
    }

    public bool IsFirstDay() => number == 1;
}