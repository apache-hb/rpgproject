using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterAreaBehaviour : MonoBehaviour
{
    [SerializeField] private PartyInfo partyInfo;
    [SerializeField] private int randomSeed;
    [SerializeField] private List<GameObject> grassPrefabs;
    [SerializeField] private List<GameObject> rockPrefabs;
    [SerializeField] private List<GameObject> treePrefabs;
}
