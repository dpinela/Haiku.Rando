using System;
using System.Linq;
using System.Reflection;
using MMDetour = MonoMod.RuntimeDetour;
using UnityEngine;
using Haiku.Rando.Topology;

namespace Haiku.Rando.Checks
{
    internal class MundoLeverReplacer : MonoBehaviour
    {
        public IRandoItem replacement;

        public static void InitHooks()
        {
            On.FactoryElevatorDoors.Start += ChangeActivationCondition;
            new MMDetour.Hook(
                typeof(FactoryElevatorDoors).GetMethod("TakeDamage", BindingFlags.Public | BindingFlags.Instance),
                ReplaceItem
            );
            On.FactoryElevator.SaveDoor += DisableAutoOpen;
        }

        private static void ChangeActivationCondition(On.FactoryElevatorDoors.orig_Start orig, FactoryElevatorDoors self)
        {
            // Let the floor open if it has been unlocked
            // whether by a vanilla lever, or by a corresponding rando
            // lever pickup.
            orig(self);

            var lr = self.GetComponent<MundoLeverReplacer>();
            if (lr == null)
            {
                return;
            }
            var collected = lr.replacement.Obtained();
            var doorOpen = (self.door02 && self.factoryElevatorScript.door02) ||
                (self.door03 && self.factoryElevatorScript.door03);
            if (collected && doorOpen)
            {
                // it was already set inactive, don't touch that
                return;
            }
            self.gameObject.SetActive(true);
            if (doorOpen)
            {
                foreach (var coll in self.groundColliders)
                {
                    coll.enabled = false;
                }
            }
            self.coll.enabled = !collected;
        }

        private static void ReplaceItem(
            Action<FactoryElevatorDoors, int, int, Vector2> orig,
            FactoryElevatorDoors self, int a, int b, Vector2 playerPos)
        {
            var lr = self.GetComponent<MundoLeverReplacer>();
            if (lr == null)
            {
                orig(self, a, b, playerPos);
                return;
            }
            SoundManager.instance.PlayOneShot(self.switchSound);
            self.coll.enabled = false;
            lr.replacement.Trigger(self);
        }

        private static void DisableAutoOpen(On.FactoryElevator.orig_SaveDoor orig, FactoryElevator self, int num)
        {
            if (!CheckManager.Instance.Randomizer.Settings.Contains(Pool.Levers))
            {
                orig(self, num);
            }
        }

        public static void ReplaceCheck(RandoCheck orig, IRandoItem replacement)
        {
            FactoryElevatorDoors door = null;
            switch (orig.CheckId)
            {
                case 23:
                    door = SceneUtils.FindObjectsOfType<FactoryElevatorDoors>().Where(d => d.door02).FirstOrDefault();
                    break;
                case 24:
                    door = SceneUtils.FindObjectsOfType<FactoryElevatorDoors>().Where(d => d.door03).FirstOrDefault();
                    break;
            }
            if (door == null)
            {
                throw new InvalidOperationException($"attempted to replace Mundo lever {orig.CheckId} that is not present");
            }
            var lr = door.gameObject.AddComponent<MundoLeverReplacer>();
            lr.replacement = replacement;
            lr.enabled = true;
        }
    }
}