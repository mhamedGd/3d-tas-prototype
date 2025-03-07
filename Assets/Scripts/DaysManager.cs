using System.Collections.Generic;
using AndoomiUtils;
using FischlWorks_FogWar;
using NUnit.Framework;
using UnityEngine;

public class DaysManager : MonoBehaviour
{
    [SerializeField] float dayLength;
    [SerializeField] float nightLength;

    [SerializeField] Color dayColor;
    [SerializeField] Color nightColor;
    [SerializeField] float dayAlpha;
    [SerializeField] float nightAlpha;

    CountdownTimer dayTimer;
    CountdownTimer nightTimer;

    [SerializeField] GameObject enemyPrefab;
    [SerializeField] int enemySpawns;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] Queue<GameObject> spawnedEnemies = new();

    [SerializeField] Transform mapCenter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dayTimer = new(dayLength);
        nightTimer = new(nightLength);

        dayTimer.OnTimerStart += () =>
        {
            var allFogs = FindObjectsByType<csFogWar>(FindObjectsSortMode.None);

            foreach (var f in allFogs)
            {
                f.SetFogColor(dayColor);

                f.SetFogAlpha(dayAlpha);
            }
            for(int i = 0; i < enemySpawns; i++)
            {
                var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                spawnedEnemies.Enqueue(Instantiate(enemyPrefab, spawnPoint.position+Random.insideUnitCircle.Vec2ToVec3Z()* 3, spawnPoint.rotation));
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
        };

        dayTimer.Start();
    }

    // Update is called once per frame
    void Update()
    {
        dayTimer.Tick();
        nightTimer.Tick();
    }
}
