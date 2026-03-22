// using System.Collections.Generic;
// using UnityEngine;

// public class KitchenManager : MonoBehaviour
// {
//     public float score;
//     public Customer currentCustomer;
//     public List<string> playerIngredients = new List<string>();
    
//     int heal = GameManager.healing;
//     int stren = GameManager.strength;
//     int lk = GameManager.luck;
//     int chm = GameManager.charm;
//     int tox = GameManager.toxicity;
//     int intox = GameManager.intoxication;

//     void Start()
//     {
//         currentCustomer = CustomerManager.Instance.currentCustomer;
//     }

//     public void SubmitRecipe()
//     {
//         bool success = EvaluateRecipe();
//         ResultData.lastResult = success;
//         GameManager.Instance.SubmitRecipe();
//     }

//     public bool EvaluateRecipe()
//     {
//         // Check perfect recipe
//         if (currentCustomer.perfect_ingredients != null &&
//             IsPerfectRecipe(currentCustomer.perfect_ingredients, playerIngredients))
//         {
//             score = 100f;
//             return true;
//         }

//         // Calculate stat differences
//         float totalDiff = 0f;
//         int count = 0;

//         if (currentCustomer.healing_ideal >= 0)
//         {
//             totalDiff += Mathf.Abs(currentCustomer.healing_ideal - heal);
//             count++;
//         }

//         if (currentCustomer.strength_ideal >= 0)
//         {
//             totalDiff += Mathf.Abs(currentCustomer.strength_ideal - stren);
//             count++;
//         }

//         if (currentCustomer.luck_ideal >= 0)
//         {
//             totalDiff += Mathf.Abs(currentCustomer.luck_ideal - lk);
//             count++;
//         }

//         if (currentCustomer.charm_ideal >= 0)
//         {
//             totalDiff += Mathf.Abs(currentCustomer.charm_ideal - chm);
//             count++;
//         }

//         float averageDiff = (count > 0) ? totalDiff / count : 0f;

//         // Convert to score
//         score = Mathf.Max(0, 100 - averageDiff * 10f);

//         // Success threshold
//         return averageDiff <= 2f;
//     }

//     public bool IsPerfectRecipe(string[] requiredIngredients, List<string> playerIngredients)
//     {
//         foreach (var req in requiredIngredients)
//         {
//             bool found = false;
//             foreach (var player in playerIngredients)
//             {
//                 if (player.Trim().ToLower() == req.Trim().ToLower())
//                 {
//                     found = true;
//                     break;
//                 }
//             }
//             if (!found) return false;
//         }
//         return true;
//     }
// }