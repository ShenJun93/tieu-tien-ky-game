using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace TieuTienKy.Gameplay.Tests
{
    public class Slice010SpatialLoopTests
    {
        readonly List<GameObject> spawned = new List<GameObject>();

        [TearDown]
        public void TearDown()
        {
            Time.timeScale = 1f;
            foreach (GameObject go in spawned)
            {
                if (go != null)
                {
                    Object.DestroyImmediate(go);
                }
            }
            spawned.Clear();
        }

        [Test]
        public void DryLightning_DoesNotTriggerConductiveReaction()
        {            Combatant target = CreateActor("DryTarget", new Vector3(0f, 0f, 1f));

            target.TakeHit(new HitInfo(1, DamageElement.Lightning, Vector3.zero));

            Assert.IsFalse(target.LastReactionTriggered);
            Assert.AreEqual(0, target.BurstSpawnCount);
        }

        [UnityTest]
        public IEnumerator WetBasicAndLoi_BothTriggerConductiveReaction()
        {
            GameObject basicPlayer = CreatePlayer("BasicPlayer", Vector3.zero);
            BasicAttack basic = basicPlayer.AddComponent<BasicAttack>();
            Combatant basicTarget = CreateActor("WetBasicTarget", new Vector3(0f, 0f, 1.2f));
            basicTarget.SetInWaterZone(true);

            yield return null;
            Assert.IsTrue(basic.TryActivate(Time.time));
            yield return new WaitForSecondsRealtime(0.2f);
            Assert.IsTrue(basicTarget.LastReactionTriggered,
                "Wet Basic must keep the existing Water + Lightning conductive payoff.");

            GameObject loiPlayer = CreatePlayer("LoiPlayer", new Vector3(10f, 0f, 0f));
            LoiTramSkill loi = loiPlayer.AddComponent<LoiTramSkill>();
            Combatant loiTarget = CreateActor("WetLoiTarget", new Vector3(10f, 0f, 1.8f));            loiTarget.SetInWaterZone(true);

            yield return null;
            Assert.IsTrue(loi.TryActivate(Time.time));
            yield return null;
            Assert.IsTrue(loiTarget.LastReactionTriggered,
                "Wet LÃƒÂ´i must keep the existing Water + Lightning conductive payoff.");
        }

        [UnityTest]
        public IEnumerator LegacyStormControlStyle_CannotPushDryBystanderInSlice010()
        {
            GameObject player = CreatePlayer("LoiPlayer", Vector3.zero);
            LoiTramSkill loi = player.AddComponent<LoiTramSkill>();
            Combatant wetTarget = CreateActor("WetTarget", new Vector3(0f, 0f, 1.8f));
            wetTarget.SetInWaterZone(true);
            Combatant dryBystander = CreateActor("DryBystander", new Vector3(1.5f, 0f, 1.8f));
            Vector3 bystanderStart = dryBystander.transform.position;

            yield return null;
            loi.SetStormControlRuntimeEnabled(false);
            Assert.IsFalse(loi.StormControlRuntimeEnabled);
            loi.SetStormControl(ProductProofRunStyle.FromStacks(1, 0, 0));
            Assert.IsTrue(loi.TryActivate(Time.time));
            yield return new WaitForSecondsRealtime(0.25f);

            Assert.IsNull(dryBystander.LastHitElement,
                "Slice 010 defers Storm Control: a wet LÃƒÂ´i hit must not create a second bystander pulse.");
            Assert.Less(Vector3.Distance(bystanderStart, dryBystander.transform.position), 0.02f);
        }
        [UnityTest]
        public IEnumerator GaleCounter_CanMoveTargetIntoWaterBeforeLoiPayoff()
        {
            Vector3 testOrigin = new Vector3(100f, 0f, 100f);
            GameObject player = CreatePlayer("SpatialPlayer", testOrigin);
            PhongBoSkill phong = player.AddComponent<PhongBoSkill>();
            LoiTramSkill loi = player.AddComponent<LoiTramSkill>();
            Combatant target = CreateActor("SpatialTarget", testOrigin + Vector3.forward * 3.2f);
            GameObject water = CreateWaterZone(testOrigin + Vector3.forward * 4.9f, new Vector3(4f, 2f, 2f));

            yield return new WaitForFixedUpdate();
            Assert.IsFalse(target.IsInWaterZone,
                "The target must begin outside Water so Gale Counter is the spatial setup, not decoration.");

            KnockbackReceiver targetKnockback = target.GetComponent<KnockbackReceiver>();
            bool galeTriggered = false;
            phong.GaleCounterTriggered += () => galeTriggered = true;

            phong.SetGaleCounterBuildMutationActive(true);
            Assert.IsTrue(phong.TryActivate(Time.time));

            float galeDeadline = Time.realtimeSinceStartup + 1.5f;
            while (!galeTriggered && Time.realtimeSinceStartup < galeDeadline)
            {
                yield return null;
            }
            Assert.IsTrue(galeTriggered, "Gale Counter landing pulse must occur before checking the Water setup.");

            float settleDeadline = Time.realtimeSinceStartup + 1.5f;
            while (targetKnockback.IsBeingKnockedBack && Time.realtimeSinceStartup < settleDeadline)
            {
                yield return null;
            }
            Assert.IsFalse(targetKnockback.IsBeingKnockedBack, "Gale displacement must settle within the bounded test window.");
            yield return new WaitForFixedUpdate();

            Assert.IsTrue(target.IsInWaterZone,
                "Gale Counter should be able to displace a nearby target into the authored Water region.");
            Assert.IsTrue(loi.TryActivate(Time.time));
            yield return null;
            Assert.IsTrue(target.LastReactionTriggered,
                "After the spatial setup, LÃƒÂ´i must convert Water positioning into the conductive payoff.");
        }

        GameObject CreatePlayer(string name, Vector3 position)
        {
            GameObject player = CreateBody(name, position);
            player.transform.forward = Vector3.forward;
            return player;
        }
        Combatant CreateActor(string name, Vector3 position)
        {
            return CreateBody(name, position).GetComponent<Combatant>();
        }

        GameObject CreateBody(string name, Vector3 position)
        {
            var actor = new GameObject(name);
            spawned.Add(actor);
            actor.transform.position = position;

            var controller = actor.AddComponent<CharacterController>();
            controller.center = Vector3.zero;
            controller.height = 2f;
            controller.radius = 0.5f;
            actor.AddComponent<KnockbackReceiver>();
            var combatant = actor.AddComponent<Combatant>();
            combatant.ConfigureMaxHealth(8);
            return actor;
        }

        GameObject CreateWaterZone(Vector3 position, Vector3 scale)
        {
            GameObject water = GameObject.CreatePrimitive(PrimitiveType.Cube);
            spawned.Add(water);
            water.name = "Slice010_WaterZone";
            water.transform.position = position;
            water.transform.localScale = scale;
            water.GetComponent<BoxCollider>().isTrigger = true;
            water.AddComponent<WaterZone>();
            return water;
        }
    }
}
