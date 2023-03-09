using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Debuff : MonoBehaviour
{

    public static Dictionary<string, Dictionary<GameObject, int>> debuffStacks = new Dictionary<string, Dictionary<GameObject, int>>();
    private static Dictionary<string, Dictionary<GameObject, float>> debuffTimes = new Dictionary<string, Dictionary<GameObject, float>>();
    //private static Dictionary<string, Dictionary<GameObject, Image>> debuffSprite = new Dictionary<string, Dictionary<GameObject, Image>>();

    private float debuffDuration = 15f;
    private int maxStacks = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DebuffTimer();
    }

    public void ApplyDebuff(string debuffName, GameObject enemy)
    {
        if (!debuffStacks.ContainsKey(debuffName))
        {
            debuffStacks[debuffName] = new Dictionary<GameObject, int>();
            debuffTimes[debuffName] = new Dictionary<GameObject, float>();
        }

        if (!debuffStacks[debuffName].ContainsKey(enemy))
        {
            debuffStacks[debuffName][enemy] = 0;
        }

        debuffTimes[debuffName][enemy] = Time.time;
        debuffStacks[debuffName][enemy] = Mathf.Min(debuffStacks[debuffName][enemy] + 1, maxStacks);
    }

    void DebuffTimer()
    {
        List<string> debuffsToRemove = new List<string>();

        foreach (var debuff in debuffStacks)
        {
            List<GameObject> keysToRemove = new List<GameObject>();

            foreach (var enemy in debuff.Value.Keys)
            {
                if (Time.time > debuffTimes[debuff.Key][enemy] + debuffDuration)
                {
                    keysToRemove.Add(enemy);
                }
            }

            foreach (var enemy in keysToRemove)
            {
                debuff.Value.Remove(enemy);
                debuffTimes[debuff.Key].Remove(enemy);
            }

            if (debuff.Value.Count == 0)
            {
                debuffsToRemove.Add(debuff.Key);
            }
        }

        foreach (var debuff in debuffsToRemove)
        {
            debuffStacks.Remove(debuff);
            debuffTimes.Remove(debuff);
        }
    }


    public int GetStacks(string debuffName, GameObject enemy)
    {
        if (debuffStacks.ContainsKey(debuffName) && debuffStacks[debuffName].ContainsKey(enemy))
        {
            return debuffStacks[debuffName][enemy];
        }
        else
        {
            return 0;
        }
    }

    public List<string> GetActiveDebuffs(GameObject enemy)
    {
        List<string> activeDebuffs = new List<string>();

        foreach (var debuff in debuffStacks)
        {
            if (debuff.Value.ContainsKey(enemy))
            {
                activeDebuffs.Add(debuff.Key);
            }
        }

        return activeDebuffs;
    }
}
