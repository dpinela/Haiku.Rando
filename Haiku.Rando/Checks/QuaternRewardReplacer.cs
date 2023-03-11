using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Haiku.Rando.Topology;

namespace Haiku.Rando.Checks
{
    internal class QuaternRewardReplacer : MonoBehaviour
    {
        private RandoCheck chipReplacement;
        private RandoCheck capsule1Replacement;
        private RandoCheck capsule2Replacement;

        public static void InitHooks()
        {
            On.e29Portal.GiveRewardsGradually += ReplaceRewards;
        }

        private static IEnumerator ReplaceRewards(On.e29Portal.orig_GiveRewardsGradually orig, e29Portal self, int startCount)
        {
            var replacer = self.GetComponent<QuaternRewardReplacer>();
            var result = orig(self, startCount);
            if (!result.MoveNext())
            {
                yield break;
            }
            var running = false;
            do
            {
                yield return result.Current;
                running = result.MoveNext();
                // lastPowercellCount is set to i+1 on each iteration of the
                // original loop
                var i = GameManager.instance.lastPowercellCount - 1;
                var rewardObj = self.rewardObjects[i];
                if (rewardObj.name.Contains("_Chip"))
                {
                    var r = replacer?.chipReplacement;
                    if (r != null)
                    {
                        rewardObj.SetActive(!CheckManager.AlreadyGotCheck(r));
                    }
                }
                else if (rewardObj.name.Contains("_Health fragment 1"))
                {
                    var r = replacer?.capsule1Replacement;
                    if (r != null)
                    {
                        rewardObj.SetActive(!CheckManager.AlreadyGotCheck(r));
                    }
                }
                else if (rewardObj.name.Contains("_Health fragment 2"))
                {
                    var r = replacer?.capsule2Replacement;
                    if (r != null)
                    {
                        rewardObj.SetActive(!CheckManager.AlreadyGotCheck(r));
                    }
                }
            }
            while (running);
        }

        public static void ReplaceCheck(RandoCheck orig, RandoCheck replacement)
        {
            var portal = SceneUtils.FindObjectOfType<e29Portal>();
            if (portal == null)
            {
                throw new InvalidOperationException("attempted to replace Quatern check while e29Portal is not present");
            }
            if (!portal.gameObject.TryGetComponent<QuaternRewardReplacer>(out var replacer))
            {
                replacer = portal.gameObject.AddComponent<QuaternRewardReplacer>();
            }
            switch (orig.Alias)
            {
                case "Item[3]0":
                    replacer.capsule1Replacement = replacement;
                    break;
                case "Item[3]1":
                    replacer.capsule2Replacement = replacement;
                    break;
                case "Chip":
                    replacer.chipReplacement = replacement;
                    break;
                default:
                    throw new InvalidOperationException($"{orig.Alias} is not a Quatern check");
            }
            // This will cause e29PortalRewardChecker.CheckReward to do nothing
            // for checks that are randomized.
            var rewardObj = SceneUtils.FindObjectsOfType<e29PortalRewardChecker>()
                .First(c => c.objectSaveID == orig.SaveId);
            rewardObj.neededPowercells = 999999;
        }
    }
}