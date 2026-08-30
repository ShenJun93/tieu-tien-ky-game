using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using TieuTienKy.Input;
using UnityEngine;
using UnityEngine.TestTools;

namespace TieuTienKy.Gameplay.Tests
{
    /// <summary>
    /// Slice 010 internal pre-production gate (Gate-0), item 3: the
    /// representative Basic-attack cross-discipline feedback probe.
    ///
    /// These tests exist to answer one question the Combat Feedback Matrix
    /// (docs/production-craft/integration/TTK_COMBAT_FEEDBACK_MATRIX.md) asks
    /// and Slice 009's Human Product Gate NO exposed: do the separate
    /// disciplines that respond to one Basic attack (animation timing,
    /// gameplay truth, hit reaction, hit-stop, VFX, audio, camera) agree on
    /// the same shared moment - or does each one merely "work" in isolation?
    /// They are therefore ordering/gating tests around the running
    /// MonoBehaviour loop, not re-tests of the pure AttackSequencer math
    /// (AttackSequencerTests, EditMode) or of the Water+Lightning rule
    /// (WaterLightningReactionTests, EditMode).
    ///
    /// PlayMode rather than EditMode because every claim here depends on
    /// Update()/coroutine/Time behaviour: the anticipation-recovery rhythm,
    /// HitStop's realtime freeze-and-restore, PrimitiveBurstVFX's
    /// ParticleSystem, and CombatAudio's PlayClipAtPoint dispatch.
    ///
    /// Matrix section 5 is the load-bearing rule under most of this file:
    /// gameplay truth is the anchor, and no presentation layer may claim an
    /// outcome the simulation has not already decided is true.
    /// </summary>
    public class Slice010CombatFeedbackTests
    {
        const string BurstName = "ConductiveBurstVFX_Primitive";
        const string SwingClipResource = "Audio/BasicSwing";
        const string HitClipResource = "Audio/BasicHit";

        // Declared LIGHT-tier Basic-attack timing contract (Matrix 2/3.1),
        // asserted against the shipped serialized values rather than
        // re-declared here as an independent source of truth.
        const float DeclaredAnticipationSeconds = 0.12f;
        const float DeclaredRecoverySeconds = 0.28f;
        const float DeclaredHitStopSeconds = 0.05f;
        const float DeclaredHitStopTimeScale = 0.05f;

        // Wired camera-impulse tiers (ArenaVerticalSliceBootstrapper).
        const float DeclaredBasicCameraImpulse = 0.08f;
        const float DeclaredLoiTramCameraImpulse = 0.12f;
        const float DeclaredPlayerDamagedCameraImpulse = 0.18f;

        static readonly Color TargetBaseColor = new Color(0.15f, 0.35f, 0.9f, 1f);

        readonly List<GameObject> spawned = new List<GameObject>();

        [TearDown]
        public void TearDown()
        {
            foreach (GameObject go in spawned)
            {
                if (go != null) Object.Destroy(go);
            }

            spawned.Clear();

            foreach (ParticleSystem system in Object.FindObjectsByType<ParticleSystem>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                if (system != null && system.gameObject.name.StartsWith(BurstName)) Object.Destroy(system.gameObject);
            }

            foreach (AudioSource source in Object.FindObjectsByType<AudioSource>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                if (source != null) Object.Destroy(source.gameObject);
            }

            // HitStop mutates global Time.timeScale; a test that fails
            // mid-freeze must never leak that into the next test.
            Time.timeScale = 1f;
        }

        // ---------- construction helpers ----------

        GameObject Track(GameObject go)
        {
            spawned.Add(go);
            return go;
        }

        BasicAttack BuildAttacker(Vector3 position)
        {
            var player = Track(new GameObject("Slice010_Player"));
            player.transform.position = position;
            player.transform.rotation = Quaternion.identity; // forward = +Z

            player.AddComponent<CharacterController>();
            player.AddComponent<KnockbackReceiver>();
            player.AddComponent<Combatant>();
            var inputReader = player.AddComponent<TouchInputReader>();

            // Test-only seam (same as RepresentativeCombatSpinePlayModeTests):
            // silence the reader so only explicit TryActivate drives the
            // swing, while BasicAttack itself keeps its shipped defaults.
            inputReader.enabled = false;

            return player.AddComponent<BasicAttack>();
        }

        /// <summary>
        /// Target is built inactive-first so its child Renderer already
        /// exists when Combatant.Awake caches it - the hit-flash reaction
        /// resolves GetComponentInChildren once, at Awake. Its primitive
        /// collider is removed so the target is discovered through its
        /// CharacterController alone and never resolves twice out of one
        /// OverlapSphere.
        /// </summary>
        (Combatant combatant, Renderer renderer, KnockbackReceiver knockback) BuildTarget(Vector3 position)
        {
            var target = Track(new GameObject("Slice010_Target"));
            target.SetActive(false);
            target.transform.position = position;

            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Object.DestroyImmediate(body.GetComponent<Collider>());
            body.transform.SetParent(target.transform, worldPositionStays: false);

            var bodyRenderer = body.GetComponent<Renderer>();
            bodyRenderer.material.color = TargetBaseColor;

            target.AddComponent<CharacterController>();
            var knockback = target.AddComponent<KnockbackReceiver>();
            var combatant = target.AddComponent<Combatant>();

            target.SetActive(true);

            return (combatant, bodyRenderer, knockback);
        }

        static IEnumerator SettlePhysics()
        {
            yield return null;
            Physics.SyncTransforms();
            yield return null;
        }

        static T Serialized<T>(object instance, string fieldName)
        {
            FieldInfo field = instance.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(field, "Expected serialized field '" + fieldName + "' on " + instance.GetType().Name + ".");
            return (T)field.GetValue(instance);
        }

        static int CountLiveSourcesPlaying(AudioClip clip)
        {
            if (clip == null) return 0;

            int count = 0;
            foreach (AudioSource source in Object.FindObjectsByType<AudioSource>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                if (source != null && source.clip == clip) count++;
            }

            return count;
        }

        static int CountLiveBursts()
        {
            int count = 0;
            foreach (ParticleSystem system in Object.FindObjectsByType<ParticleSystem>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                if (system != null && system.gameObject.name.StartsWith(BurstName)) count++;
            }

            return count;
        }

        /// <summary>Pumps frames until the condition holds, bounded by a realtime deadline so a genuine regression fails loudly instead of hanging the run.</summary>
        static IEnumerator PumpUntil(System.Func<bool> condition, float timeoutSeconds, string failureMessage)
        {
            float deadline = Time.realtimeSinceStartup + timeoutSeconds;
            while (!condition())
            {
                if (Time.realtimeSinceStartup > deadline)
                {
                    Assert.Fail(failureMessage);
                }

                yield return null;
            }
        }

        // ---------- 1. timing / rhythm ----------

        [UnityTest]
        public IEnumerator BasicAttackRhythm_AnticipationImpactRecovery_MatchesDeclaredLightTierContract()
        {
            BasicAttack basicAttack = BuildAttacker(Vector3.zero);
            yield return SettlePhysics();

            // Matrix 2.2 point 1: the contract's timing numbers must come
            // from the shipped action, never from a document's guess.
            Assert.AreEqual(DeclaredAnticipationSeconds, Serialized<float>(basicAttack, "anticipationSeconds"), 0.0001f,
                "Basic's anticipation must match the declared LIGHT-tier contract; a silent retune invalidates every discipline keyed to it.");
            Assert.AreEqual(DeclaredRecoverySeconds, Serialized<float>(basicAttack, "recoverySeconds"), 0.0001f,
                "Basic's recovery must match the declared LIGHT-tier contract - UI cooldown readout and enemy AI both key off recovery, not off the VFX/SFX tail.");
            Assert.Greater(DeclaredRecoverySeconds, DeclaredAnticipationSeconds,
                "Matrix 3.2: a fast LIGHT action still front-loads commitment into follow-through, not into wind-up.");

            float startedTime = -1f;
            float impactedTime = -1f;
            float recoveredTime = -1f;

            basicAttack.AttackStarted += () => startedTime = Time.time;
            basicAttack.AttackImpacted += () => impactedTime = Time.time;
            basicAttack.AttackRecovered += () => recoveredTime = Time.time;

            float activationTime = Time.time;
            Assert.IsTrue(basicAttack.TryActivate(activationTime), "A swing must start from Idle.");
            Assert.AreEqual(activationTime, startedTime, 0.0001f,
                "Input must be acknowledged on the activation frame itself, not deferred to the impact frame.");

            Assert.IsFalse(basicAttack.TryActivate(Time.time),
                "A second activation during anticipation must be rejected - the rhythm is a committed sequence, not a spammable trigger.");

            yield return PumpUntil(() => impactedTime >= 0f, 2f, "Basic attack never reached its impact frame.");

            float anticipationElapsed = impactedTime - activationTime;
            Assert.GreaterOrEqual(anticipationElapsed, DeclaredAnticipationSeconds - 0.001f,
                "Contact must never resolve before the declared anticipation window has actually elapsed.");
            Assert.LessOrEqual(anticipationElapsed, DeclaredAnticipationSeconds + 0.1f,
                "Contact must land within a frame or two of the declared anticipation window, not drift late.");

            Assert.IsFalse(basicAttack.TryActivate(Time.time), "A third activation during recovery must still be rejected.");

            yield return PumpUntil(() => recoveredTime >= 0f, 3f, "Basic attack never completed its recovery window.");

            float totalElapsed = recoveredTime - activationTime;
            Assert.GreaterOrEqual(totalElapsed, DeclaredAnticipationSeconds + DeclaredRecoverySeconds - 0.001f,
                "Control must not return before the full anticipation + recovery contract has elapsed.");
            Assert.LessOrEqual(totalElapsed, DeclaredAnticipationSeconds + DeclaredRecoverySeconds + 0.15f,
                "Control must return promptly once recovery ends - a late release reads as input lag, not weight.");

            Assert.Less(startedTime, impactedTime, "Stage order must be start then impact.");
            Assert.Less(impactedTime, recoveredTime, "Stage order must be impact then recovery.");
            Assert.IsTrue(basicAttack.TryActivate(Time.time), "The next swing must be available once recovery has ended.");
        }

        // ---------- 2. gameplay truth + hit reaction ----------

        [UnityTest]
        public IEnumerator LandedHit_ResolvesGameplayTruthAndReaction_BeforeAnyPresentationConsumer()
        {
            BasicAttack basicAttack = BuildAttacker(Vector3.zero);
            (Combatant target, Renderer targetRenderer, KnockbackReceiver targetKnockback) = BuildTarget(new Vector3(0f, 0f, 1.2f));
            yield return SettlePhysics();

            int startHealth = target.CurrentHealth;
            Vector3 startPosition = target.transform.position;
            Assert.Greater(startHealth, 1, "Target must survive one Basic hit so this test measures a hit reaction, not a defeat.");

            var order = new List<string>();
            int healthAtPresentation = -1;
            bool knockedAtPresentation = false;
            Color colorAtPresentation = Color.clear;

            target.Damaged += (_, __) => order.Add("gameplay_truth");
            basicAttack.HitLanded += () =>
            {
                order.Add("presentation");
                healthAtPresentation = target.CurrentHealth;
                knockedAtPresentation = targetKnockback.IsBeingKnockedBack;
                colorAtPresentation = targetRenderer.material.color;
            };

            Assert.IsTrue(basicAttack.TryActivate(Time.time));
            yield return PumpUntil(() => order.Count >= 2, 2f, "A Basic attack aimed at an adjacent target never produced both a gameplay-truth and a presentation event.");

            Assert.AreEqual(new[] { "gameplay_truth", "presentation" }, order.ToArray(),
                "Matrix 5: damage/state must be resolved in code before any presentation consumer fires - presentation may never claim an outcome the simulation has not decided.");

            Assert.AreEqual(startHealth - 1, healthAtPresentation,
                "The presentation layer must observe the already-applied damage, not a pre-hit health value.");
            Assert.IsTrue(knockedAtPresentation,
                "Hit reaction (knockback) must already be applied when presentation fires, so camera/VFX read as caused by the reaction rather than preceding it.");
            Assert.AreEqual(Color.white, colorAtPresentation,
                "The target's hit flash must already be engaged at the confirmed contact moment.");
            Assert.IsFalse(target.LastReactionTriggered,
                "Outside a WaterZone this must stay a plain Lightning hit - the Conductive Burst rule is Water-gated and must not fire on a dry Basic hit.");

            yield return new WaitForSecondsRealtime(0.5f);

            Vector3 displacement = target.transform.position - startPosition;
            Assert.Greater(displacement.z, 0.05f,
                "The target must be visibly pushed along the hit direction - knockback is the readable consequence of the Basic hit, not a state flag.");
            Assert.AreEqual(TargetBaseColor, targetRenderer.material.color,
                "The hit flash must be a brief accent that restores the target's own colour, never a permanent tint.");
        }

        // ---------- 3. hit stop ----------

        [UnityTest]
        public IEnumerator LandedHit_EngagesHitStopAtDeclaredScale_ThenRestoresTimeScale()
        {
            BasicAttack basicAttack = BuildAttacker(Vector3.zero);
            BuildTarget(new Vector3(0f, 0f, 1.2f));
            yield return SettlePhysics();

            Assert.AreEqual(DeclaredHitStopSeconds, Serialized<float>(basicAttack, "hitStopSeconds"), 0.0001f,
                "Basic's hit-stop duration must match the declared LIGHT-tier contract every consuming discipline is told to expect.");
            Assert.AreEqual(DeclaredHitStopTimeScale, Serialized<float>(basicAttack, "hitStopTimeScale"), 0.0001f,
                "Basic's hit-stop freeze scale must match the declared contract.");

            Assert.AreEqual(1f, Time.timeScale, 0.0001f, "Precondition: time must be running normally before the swing.");

            float timeScaleAtPresentation = -1f;
            bool landed = false;
            basicAttack.HitLanded += () =>
            {
                timeScaleAtPresentation = Time.timeScale;
                landed = true;
            };

            Assert.IsTrue(basicAttack.TryActivate(Time.time));
            yield return PumpUntil(() => landed, 2f, "The Basic attack never landed a hit.");

            Assert.AreEqual(DeclaredHitStopTimeScale, timeScaleAtPresentation, 0.0001f,
                "Hit-stop must already be engaged on the confirmed-contact frame, so every consumer freezes on the same moment.");

            // HitStop deliberately waits in realtime, so the freeze it sets
            // cannot postpone its own release.
            yield return new WaitForSecondsRealtime(DeclaredHitStopSeconds + 0.15f);

            Assert.AreEqual(1f, Time.timeScale, 0.0001f,
                "Hit-stop must restore the original timescale - a freeze that leaks is a stall, not impact.");
        }

        // ---------- 4. VFX gating ----------

        [UnityTest]
        public IEnumerator ImpactVFX_SpawnsExactlyOneBurstOnLandedHit_SizedByTheLightningFlashCurve()
        {
            BasicAttack basicAttack = BuildAttacker(Vector3.zero);
            BuildTarget(new Vector3(0f, 0f, 1.2f));
            yield return SettlePhysics();

            Assert.AreEqual(0, CountLiveBursts(), "Precondition: no impact VFX before the swing.");

            int burstsAtPresentation = -1;
            ParticleSystem burstSystem = null;
            bool landed = false;

            basicAttack.HitLanded += () =>
            {
                burstsAtPresentation = CountLiveBursts();
                GameObject burst = GameObject.Find(BurstName);
                if (burst != null) burstSystem = burst.GetComponent<ParticleSystem>();
                landed = true;
            };

            Assert.IsTrue(basicAttack.TryActivate(Time.time));
            yield return PumpUntil(() => landed, 2f, "The Basic attack never landed a hit.");

            Assert.AreEqual(1, burstsAtPresentation,
                "A landed Basic hit must spawn exactly one impact burst at the confirmed contact moment - not zero, and not stacked duplicates per overlapping collider.");
            Assert.IsNotNull(burstSystem, "The impact burst must be a real ParticleSystem, not an empty marker object.");

            ParticleSystem.MainModule main = burstSystem.main;
            Assert.AreEqual(BlessingPresentationMath.LightningFlashLifetimeSeconds(0), main.startLifetime.constant, 0.0001f,
                "The impact flash must be sized/held by the shared stack curve at its 0-stack baseline, not by a VFX-local invented duration.");
            Assert.Greater(main.startSpeed.constant, 0f,
                "The impact flash must actually travel outward from the contact point.");
        }

        // ---------- 5. audio content and dispatch ----------

        [UnityTest]
        public IEnumerator BasicAudio_SwingAndImpactAreDistinctRealCues_DispatchedAtTheirOwnMoments()
        {
            var swingClip = Resources.Load<AudioClip>(SwingClipResource);
            var hitClip = Resources.Load<AudioClip>(HitClipResource);

            Assert.IsNotNull(swingClip, "The Basic swing cue must exist at Resources/" + SwingClipResource + ".");
            Assert.IsNotNull(hitClip, "The Basic impact cue must exist at Resources/" + HitClipResource + ".");
            Assert.AreNotSame(swingClip, hitClip, "Swing and impact must be separate cues - one shared sound cannot distinguish intent from outcome.");

            foreach (AudioClip clip in new[] { swingClip, hitClip })
            {
                Assert.Greater(clip.samples, 0, clip.name + " must contain real PCM, not an empty stub.");
                Assert.Greater(clip.length, 0f, clip.name + " must have a non-zero duration.");
                Assert.AreEqual(1, clip.channels, clip.name + " must be mono - these are positional one-shots, not stereo beds.");
                Assert.AreEqual(AudioClipLoadType.DecompressOnLoad, clip.loadType,
                    clip.name + " must be decompress-on-load so a combat one-shot never stalls at its transient.");
            }

            BasicAttack basicAttack = BuildAttacker(Vector3.zero);
            BuildTarget(new Vector3(0f, 0f, 1.2f));
            yield return SettlePhysics();

            int swingCuesAtActivation = -1;
            int hitCuesAtActivation = -1;
            int hitCuesAtImpact = -1;
            bool landed = false;

            basicAttack.AttackStarted += () =>
            {
                swingCuesAtActivation = CountLiveSourcesPlaying(swingClip);
                hitCuesAtActivation = CountLiveSourcesPlaying(hitClip);
            };
            basicAttack.HitLanded += () =>
            {
                hitCuesAtImpact = CountLiveSourcesPlaying(hitClip);
                landed = true;
            };

            Assert.IsTrue(basicAttack.TryActivate(Time.time));

            Assert.AreEqual(1, swingCuesAtActivation,
                "The swing cue must fire on activation - it is the player's audible confirmation that the input registered.");
            Assert.AreEqual(0, hitCuesAtActivation,
                "Matrix 5: the impact cue must not sound at wind-up time, which would claim a hit the simulation has not resolved.");

            yield return PumpUntil(() => landed, 2f, "The Basic attack never landed a hit.");

            Assert.AreEqual(1, hitCuesAtImpact,
                "The impact cue must fire exactly once, on the gameplay-confirmed contact moment.");
        }

        // ---------- 6. camera weight tiers ----------

        [UnityTest]
        public IEnumerator CameraImpulseTiers_BasicIsTheLightestWiredImpact_AndTiersAreStrictlyOrdered()
        {
            MethodInfo wire = typeof(ArenaVerticalSliceBootstrapper).GetMethod("WireCameraImpulse", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.IsNotNull(wire, "ArenaVerticalSliceBootstrapper must still own the camera-impulse wiring.");

            FieldInfo impulseField = typeof(PlayerFollowCamera).GetField("impulseMagnitude", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(impulseField, "PlayerFollowCamera must still track a single bounded impulse magnitude.");

            var player = Track(new GameObject("Slice010_ImpulsePlayer"));
            player.AddComponent<CharacterController>();
            player.AddComponent<KnockbackReceiver>();
            var combatant = player.AddComponent<Combatant>();
            player.AddComponent<TouchInputReader>().enabled = false;
            var basicAttack = player.AddComponent<BasicAttack>();
            var loiTram = player.AddComponent<LoiTramSkill>();

            var cameraObject = Track(new GameObject("Slice010_FollowCamera"));
            var follow = cameraObject.AddComponent<PlayerFollowCamera>();

            yield return null;

            wire.Invoke(null, new object[] { player, follow });

            float basicImpulse = MeasureWiredImpulse(follow, impulseField, () => Raise(basicAttack, "HitLanded"));
            float loiImpulse = MeasureWiredImpulse(follow, impulseField, () => Raise(loiTram, "HitLanded"));
            float damagedImpulse = MeasureWiredImpulse(follow, impulseField, () => Raise(combatant, "Damaged", 2, 5));

            Assert.AreEqual(DeclaredBasicCameraImpulse, basicImpulse, 0.0001f,
                "A landed Basic hit must consume only the declared LIGHT-tier share of the camera's attention budget.");
            Assert.AreEqual(DeclaredLoiTramCameraImpulse, loiImpulse, 0.0001f,
                "A landed signature-skill hit must consume the declared MEDIUM/committed-tier camera impulse.");
            Assert.AreEqual(DeclaredPlayerDamagedCameraImpulse, damagedImpulse, 0.0001f,
                "The player taking damage must consume the declared heaviest camera impulse.");

            Assert.Less(basicImpulse, loiImpulse,
                "Matrix 3.2: the camera must read Basic as lighter than a signature skill, or the shared weight language collapses.");
            Assert.Less(loiImpulse, damagedImpulse,
                "Matrix 3.2: the player taking damage must read heavier than the player's own signature skill landing.");
        }

        static float MeasureWiredImpulse(PlayerFollowCamera follow, FieldInfo impulseField, System.Action raise)
        {
            // Cleared between tiers because ApplyImpulse deliberately keeps
            // the stronger of an in-flight impulse and the incoming one; the
            // value under test here is each tier's own declared magnitude.
            impulseField.SetValue(follow, 0f);
            raise();
            return (float)impulseField.GetValue(follow);
        }

        static void Raise(object instance, string eventName, params object[] args)
        {
            FieldInfo field = instance.GetType().GetField(eventName, BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(field, "Expected event '" + eventName + "' on " + instance.GetType().Name + " to still be the camera-impulse trigger.");

            var handler = field.GetValue(instance) as System.Delegate;
            Assert.IsNotNull(handler, "'" + eventName + "' must have been wired to the follow camera.");
            handler.DynamicInvoke(args);
        }

        // ---------- 7. whiff negative behaviour ----------

        [UnityTest]
        public IEnumerator Whiff_ResolvesTheSwing_ButProducesNoImpactFeedbackOfAnyDiscipline()
        {
            BasicAttack basicAttack = BuildAttacker(Vector3.zero);
            yield return SettlePhysics();

            var hitClip = Resources.Load<AudioClip>(HitClipResource);
            var swingClip = Resources.Load<AudioClip>(SwingClipResource);

            bool started = false;
            bool impacted = false;
            bool hitLanded = false;
            int swingCuesAtActivation = 0;
            int burstsAtImpact = -1;
            int hitCuesAtImpact = -1;
            float timeScaleAtImpact = -1f;

            basicAttack.AttackStarted += () =>
            {
                started = true;
                swingCuesAtActivation = CountLiveSourcesPlaying(swingClip);
            };
            basicAttack.AttackImpacted += () =>
            {
                impacted = true;
                burstsAtImpact = CountLiveBursts();
                hitCuesAtImpact = CountLiveSourcesPlaying(hitClip);
                timeScaleAtImpact = Time.timeScale;
            };
            basicAttack.HitLanded += () => hitLanded = true;

            Assert.IsTrue(basicAttack.TryActivate(Time.time));
            yield return PumpUntil(() => impacted, 2f, "A whiffed swing must still resolve its impact frame.");

            Assert.IsTrue(started, "A whiffed swing must still acknowledge the input.");
            Assert.AreEqual(1, swingCuesAtActivation,
                "The swing cue belongs to the action, not to the outcome - it must sound even when nothing is hit.");

            Assert.IsFalse(hitLanded, "HitLanded must never fire without a real target.");
            Assert.AreEqual(0, burstsAtImpact, "A whiff must spawn no impact VFX.");
            Assert.AreEqual(0, hitCuesAtImpact, "A whiff must play no impact cue.");
            Assert.AreEqual(1f, timeScaleAtImpact, 0.0001f, "A whiff must never engage hit-stop - freezing on nothing is dishonest feedback.");

            yield return new WaitForSecondsRealtime(0.15f);

            Assert.AreEqual(0, CountLiveBursts(), "A whiff must still have spawned no impact VFX a few frames later.");
            Assert.AreEqual(1f, Time.timeScale, 0.0001f, "A whiff must leave the global timescale untouched.");
        }
    }
}
