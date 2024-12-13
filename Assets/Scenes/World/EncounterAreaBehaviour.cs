using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

using Random = Unity.Mathematics.Random;

public class EncounterAreaBehaviour : MonoBehaviour
{
    [SerializeField] private GameState.EncounterId encounter;
    [SerializeField] private BoxCollider2D area;

    [SerializeField] private float minTimeToEncounter = 5f;
    [SerializeField] private float maxTimeToEncounter = 15f;

    [Range(1f, 1000f)]
    [SerializeField] private uint rngSeed = 1;

    [SerializeField] private List<GameObject> rocks = new();
    [SerializeField] private List<GameObject> plants = new();

    private Random rng;
    private float timeToEncounter;
    private PlayerBehaviour player;

    private float2 MinBounds => new float3(area.bounds.min).xy;
    private float2 MaxBounds => new float3(area.bounds.max).xy;

    void SetupFoliage()
    {
        // randomize the position of the rocks
        rng = new Random(rngSeed);

        foreach (var rock in rocks)
        {
            rock.transform.position = new float3(rng.NextFloat2(MinBounds, MaxBounds), 0);
        }

        // randomize the position of the plants
        foreach (var plant in plants)
        {
            plant.transform.position = new float3(rng.NextFloat2(MinBounds, MaxBounds), 0);
        }
    }

    void Start()
    {
        SetupFoliage();
        timeToEncounter = rng.NextFloat(minTimeToEncounter, maxTimeToEncounter);
    }

    void Update()
    {
        if (player != null)
        {
            timeToEncounter -= Time.deltaTime;

            if (timeToEncounter <= 0f)
            {
                timeToEncounter = rng.NextFloat(minTimeToEncounter, maxTimeToEncounter);
                player.StartEncounter(encounter);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerBehaviour player))
        {
            this.player = player;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerBehaviour player))
        {
            this.player = null;
        }
    }

    void OnValidate()
    {
        SetupFoliage();
    }
}
