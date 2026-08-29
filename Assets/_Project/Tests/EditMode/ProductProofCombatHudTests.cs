using System;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace TieuTienKy.Gameplay.Tests
{
    /// <summary>
    /// Slice 009 Task 2: ProductionCombatHudView is a serialized reference
    /// container, and ProductionHud/BlessingChoiceHud must bind to authored
    /// references instead of building Canvas/uGUI hierarchies at runtime.
    /// </summary>
    public class ProductProofCombatHudTests
    {
        readonly System.Collections.Generic.List<GameObject> spawned = new System.Collections.Generic.List<GameObject>();

        [TearDown]
        public void TearDown()
        {
            foreach (GameObject go in spawned)
            {
                if (go != null)
                {
                    UnityEngine.Object.DestroyImmediate(go);
                }
            }
            spawned.Clear();
        }

        [Test]
        public void IncompleteView_MissingReferences_IsNotComplete()
        {
            ProductionCombatHudView view = CreateView();

            Assert.IsFalse(view.IsComplete);
        }

        [Test]
        public void FullyWiredView_AllReferencesAssigned_IsComplete()
        {
            ProductionCombatHudView view = BuildFullyWiredView();

            Assert.IsTrue(view.IsComplete);
        }

        [Test]
        public void ProductionHud_Initialize_CalledTwice_Throws()
        {
            ProductionCombatHudView view = BuildFullyWiredView();
            var hudGo = new GameObject("ProductionHud");
            spawned.Add(hudGo);
            ProductionHud hud = hudGo.AddComponent<ProductionHud>();

            hud.Initialize(null, null, null, null, view);

            Assert.Throws<InvalidOperationException>(() => hud.Initialize(null, null, null, null, view));
        }

        [Test]
        public void ProductionHud_Initialize_CalledTwice_DoesNotDuplicateBasicButtonListener()
        {
            ProductionCombatHudView view = BuildFullyWiredView();
            var hudGo = new GameObject("ProductionHud");
            spawned.Add(hudGo);
            ProductionHud hud = hudGo.AddComponent<ProductionHud>();

            hud.Initialize(null, null, null, null, view);
            try
            {
                hud.Initialize(null, null, null, null, view);
            }
            catch (InvalidOperationException)
            {
                // Expected: reinitialization must fail fast rather than
                // silently accumulate a duplicate listener.
            }

            int invocations = 0;
            hud.SetActionGateway(new RecordingActionGateway(() => invocations++));
            view.BasicButton.onClick.Invoke();

            Assert.AreEqual(1, invocations);
        }

        [Test]
        public void BlessingChoiceHud_Initialize_CalledTwice_Throws()
        {
            ProductionCombatHudView view = BuildFullyWiredView();
            var hudGo = new GameObject("BlessingChoiceHud");
            spawned.Add(hudGo);
            BlessingChoiceHud hud = hudGo.AddComponent<BlessingChoiceHud>();

            hud.Initialize(view);

            Assert.Throws<InvalidOperationException>(() => hud.Initialize(view));
        }

        sealed class RecordingActionGateway : IPlayerActionGateway
        {
            readonly System.Action onBasicAttack;

            public RecordingActionGateway(System.Action onBasicAttack)
            {
                this.onBasicAttack = onBasicAttack;
            }

            public bool RequestBasicAttack()
            {
                onBasicAttack?.Invoke();
                return true;
            }

            public bool RequestLoiTram() => true;
            public bool RequestPhongBo() => true;
            public bool RequestHoThe() => true;
        }

        [Test]
        public void ProductionHud_SourceContainsNoRuntimeUiConstruction()
        {
            string source = ReadProjectSource("Presentation/ProductionHud.cs");

            StringAssert.DoesNotContain("UiBuilder.Create", source);
            StringAssert.DoesNotContain("void Build()", source);
        }

        [Test]
        public void BlessingChoiceHud_SourceContainsNoRuntimeUiConstruction()
        {
            string source = ReadProjectSource("Presentation/BlessingChoiceHud.cs");

            StringAssert.DoesNotContain("UiBuilder.Create", source);
            StringAssert.DoesNotContain("void Build()", source);
        }

        ProductionCombatHudView CreateView()
        {
            var go = new GameObject("ProductionCombatHudView");
            spawned.Add(go);
            return go.AddComponent<ProductionCombatHudView>();
        }

        ProductionCombatHudView BuildFullyWiredView()
        {
            ProductionCombatHudView view = CreateView();
            GameObject root = view.gameObject;

            var canvasGo = new GameObject("Canvas", typeof(Canvas));
            canvasGo.transform.SetParent(root.transform);
            Canvas canvas = canvasGo.GetComponent<Canvas>();

            Text hpText = CreateText("HpText", root.transform);
            Text stageText = CreateText("StageText", root.transform);
            Text blessingText = CreateText("BlessingText", root.transform);
            Text killsText = CreateText("KillsText", root.transform);
            Text timeText = CreateText("TimeText", root.transform);
            RectTransform moveBase = CreateRect("MoveBase", root.transform);
            RectTransform moveKnob = CreateRect("MoveKnob", root.transform);
            Button basicButton = CreateButton("BasicButton", root.transform);

            var skillButtons = new[]
            {
                CreateButton("SkillButton0", root.transform),
                CreateButton("SkillButton1", root.transform),
                CreateButton("SkillButton2", root.transform)
            };
            var skillLabels = new[]
            {
                CreateText("SkillLabel0", root.transform),
                CreateText("SkillLabel1", root.transform),
                CreateText("SkillLabel2", root.transform)
            };
            var skillCooldownOverlays = new[]
            {
                CreateGameObject("SkillCooldown0", root.transform),
                CreateGameObject("SkillCooldown1", root.transform),
                CreateGameObject("SkillCooldown2", root.transform)
            };

            Button pauseButton = CreateButton("PauseButton", root.transform);
            Button resumeButton = CreateButton("ResumeButton", root.transform);
            Button restartButton = CreateButton("RestartButton", root.transform);
            Button exitButton = CreateButton("ExitButton", root.transform);
            GameObject pausePanel = CreateGameObject("PausePanel", root.transform);
            GameObject bossPanel = CreateGameObject("BossPanel", root.transform);
            GameObject resultPanel = CreateGameObject("ResultPanel", root.transform);
            Image bossHpFill = CreateImage("BossHpFill", root.transform);
            Text bossHpText = CreateText("BossHpText", root.transform);
            Text bossArrivalText = CreateText("BossArrivalText", root.transform);
            Text resultTitleText = CreateText("ResultTitleText", root.transform);
            Text resultSummaryText = CreateText("ResultSummaryText", root.transform);
            Button retryButton = CreateButton("RetryButton", root.transform);
            Button menuButton = CreateButton("MenuButton", root.transform);
            GameObject blessingRoot = CreateGameObject("BlessingRoot", root.transform);
            GameObject blessingChoicePanel = CreateGameObject("BlessingChoicePanel", root.transform);
            GameObject blessingConfirmPanel = CreateGameObject("BlessingConfirmPanel", root.transform);
            var blessingButtons = new[]
            {
                CreateButton("BlessingButton0", root.transform),
                CreateButton("BlessingButton1", root.transform),
                CreateButton("BlessingButton2", root.transform)
            };
            Text blessingConfirmTitleText = CreateText("BlessingConfirmTitleText", root.transform);
            Text blessingConfirmFlavorText = CreateText("BlessingConfirmFlavorText", root.transform);

            SetField(view, "rootCanvas", canvas);
            SetField(view, "hpText", hpText);
            SetField(view, "stageText", stageText);
            SetField(view, "blessingText", blessingText);
            SetField(view, "killsText", killsText);
            SetField(view, "timeText", timeText);
            SetField(view, "moveBase", moveBase);
            SetField(view, "moveKnob", moveKnob);
            SetField(view, "basicButton", basicButton);
            SetField(view, "skillButtons", skillButtons);
            SetField(view, "skillLabels", skillLabels);
            SetField(view, "skillCooldownOverlays", skillCooldownOverlays);
            SetField(view, "pauseButton", pauseButton);
            SetField(view, "resumeButton", resumeButton);
            SetField(view, "restartButton", restartButton);
            SetField(view, "exitButton", exitButton);
            SetField(view, "pausePanel", pausePanel);
            SetField(view, "bossPanel", bossPanel);
            SetField(view, "resultPanel", resultPanel);
            SetField(view, "bossHpFill", bossHpFill);
            SetField(view, "bossHpText", bossHpText);
            SetField(view, "bossArrivalText", bossArrivalText);
            SetField(view, "resultTitleText", resultTitleText);
            SetField(view, "resultSummaryText", resultSummaryText);
            SetField(view, "retryButton", retryButton);
            SetField(view, "menuButton", menuButton);
            SetField(view, "blessingRoot", blessingRoot);
            SetField(view, "blessingChoicePanel", blessingChoicePanel);
            SetField(view, "blessingConfirmPanel", blessingConfirmPanel);
            SetField(view, "blessingButtons", blessingButtons);
            SetField(view, "blessingConfirmTitleText", blessingConfirmTitleText);
            SetField(view, "blessingConfirmFlavorText", blessingConfirmFlavorText);

            return view;
        }

        Text CreateText(string name, Transform parent)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Text));
            go.transform.SetParent(parent);
            return go.GetComponent<Text>();
        }

        RectTransform CreateRect(string name, Transform parent)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent);
            return (RectTransform)go.transform;
        }

        Button CreateButton(string name, Transform parent)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
            go.transform.SetParent(parent);
            return go.GetComponent<Button>();
        }

        Image CreateImage(string name, Transform parent)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent);
            return go.GetComponent<Image>();
        }

        GameObject CreateGameObject(string name, Transform parent)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent);
            return go;
        }

        static void SetField(object target, string fieldName, object value)
        {
            System.Reflection.FieldInfo field = target.GetType().GetField(
                fieldName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            Assert.IsNotNull(field, $"Expected private field '{fieldName}' on {target.GetType().Name}.");
            field.SetValue(target, value);
        }

        static string ReadProjectSource(string relativePathUnderProject)
        {
            string path = Path.Combine(Application.dataPath, "_Project", relativePathUnderProject);
            Assert.IsTrue(File.Exists(path), $"Expected source file at '{path}'.");
            return File.ReadAllText(path);
        }
    }
}
