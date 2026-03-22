using UnityEngine;

[System.Serializable]
public class Customer
{
    public string name;
    public string intro;
    public string needs;
    public string[] reaction_perfect;
    public string[] reaction_great;
    public string[] reaction_good;
    public string[] reaction_okay;
    public string[] reaction_bad;

    public int healing_min;
    public int strength_min;
    public int toxicity_max;
    public int intoxication_max;
    public int luck_min;
    public int charm_min;

    public int healing_ideal;
    public int strength_ideal;
    public int luck_ideal;
    public int charm_ideal;
    public string[] perfect_ingredients;
    public Sprite portrait;
    public string portraitName;
}

[System.Serializable]
public class CustomerList
{
    public Customer[] customers;
}