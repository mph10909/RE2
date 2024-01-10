using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

namespace ResidentEvilClone
{
    public static class EnemyEventHandler
    {
        private static Action<GameObject> OnCharacterSwapped;
        private static Action<bool, GameObject> OnPlayerDiedEvent;

        static EnemyEventHandler()
        {
            Actions.CharacterSwap += HandleCharacterSwap;
            Actions.OnPlayerKilled += HandlePlayerKilled;

            RegisterAllEnemiesInScene();
        }

        private static void HandleCharacterSwap(GameObject character)
        {
            OnCharacterSwapped?.Invoke(character);
        }

        private static void HandlePlayerKilled(bool isDead, GameObject character)
        {
            OnPlayerDiedEvent?.Invoke(isDead, character);
        }

        public static void RegisterEnemy(EnemyAnimations enemy)
        {
            OnCharacterSwapped += enemy.SetCharacter;
            OnPlayerDiedEvent += enemy.PlayerKilled;
        }

        public static void UnregisterEnemy(EnemyAnimations enemy)
        {
            OnCharacterSwapped -= enemy.SetCharacter;
            OnPlayerDiedEvent -= enemy.PlayerKilled;
        }

        public static void RegisterAllEnemiesInScene()
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                var rootObjects = scene.GetRootGameObjects();

                foreach (var rootObject in rootObjects)
                {
                    EnemyAnimations[] enemies = rootObject.GetComponentsInChildren<EnemyAnimations>(true); // true to include inactive
                    foreach (var enemy in enemies)
                    {
                        //Debug.Log(enemy.gameObject.name);
                        RegisterEnemy(enemy);
                    }
                }
            }
        }
    }
}
