using TieuTienKy.Gameplay;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TieuTienKy.EditorTools.Authoring
{
    /// <summary>
    /// Deterministic, repeatable editor authoring for the one production
    /// combat HUD prefab (Slice 009 Task 3). It replaces the prototype-era
    /// runtime HUD construction: composition, layout, colour roles and
    /// typography live in the authored prefab, while ProductionHud and
    /// BlessingChoiceHud keep owning only runtime state.
    ///
    /// Running BuildAndWire twice must produce the same prefab and the same
    /// single scene reference - it rebuilds the authored hierarchy from this
    /// one source of truth and overwrites the existing asset in place, so
    /// there is no accumulate-on-rerun path.
    /// </summary>
    public static class ProductProofCombatHudAuthoring
    {
        const string PrefabFolder = "Assets/_Project/Prefabs/UI";
        const string PrefabPath = PrefabFolder + "/ProductProofCombatHud.prefab";
        const string ScenePath = "Assets/_Project/Scenes/Arena_VerticalSlice_01.unity";

        // One deliberately chosen colour language, shared by every surface in
        // this HUD so the screen reads as authored game UI rather than an
        // engine overlay: ink panels, one gold accent for "cultivation"
        // emphasis, jade for the player's own primary action, and a single
        // pair of high-contrast text roles.
        static readonly Color PanelInk = new Color(0.043f, 0.055f, 0.078f, 0.78f);
        static readonly Color CardInk = new Color(0.043f, 0.055f, 0.078f, 0.95f);
        static readonly Color Backdrop = new Color(0.016f, 0.024f, 0.039f, 0.80f);
        static readonly Color Accent = new Color(0.88f, 0.74f, 0.38f, 1f);
        static readonly Color TextPrimary = new Color(0.96f, 0.97f, 0.99f, 1f);
        static readonly Color TextSecondary = new Color(0.72f, 0.76f, 0.83f, 1f);
        static readonly Color BasicAction = new Color(0.16f, 0.62f, 0.55f, 1f);
        static readonly Color BasicActionLabel = new Color(0.98f, 0.99f, 1f, 1f);

        // ProductionHud owns the skill buttons' ready/cooldown tint at
        // runtime, so the authored base colour must match its ready colour -
        // otherwise the first frame flashes a different button treatment.
        static readonly Color SkillReady = new Color(0.9f, 0.9f, 0.92f, 1f);
        static readonly Color SkillLabel = new Color(0.08f, 0.09f, 0.12f, 1f);
        static readonly Color CooldownScrim = new Color(0f, 0f, 0f, 0.55f);
        static readonly Color MenuButtonColor = new Color(0.90f, 0.90f, 0.92f, 1f);
        static readonly Color ChoiceButtonColor = new Color(0.93f, 0.92f, 0.88f, 1f);

        static Font cachedFont;
        static Font UiFont => cachedFont != null ? cachedFont : (cachedFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"));

        /// <summary>Repeatable entry point: (re)authors the HUD prefab, then assigns it to Arena_VerticalSlice_01's bootstrapper.</summary>
        [MenuItem("TieuTienKy/Authoring/Build Product Proof Combat HUD")]
        public static void BuildAndWire()
        {
            GameObject prefab = BuildHudPrefab();
            WireArenaScene(prefab);

            AssetDatabase.SaveAssets();
            StripTrailingWhitespace(PrefabPath);
            StripTrailingWhitespace(PrefabPath + ".meta");
            AssetDatabase.Refresh();
            Debug.Log($"[ProductProofCombatHudAuthoring] Authored {PrefabPath} and wired {ScenePath}.");
        }

        /// <summary>
        /// Unity's YAML writer trails a space after several empty scalar
        /// fields (m_Name:, m_Text:, userData:, assetBundleName: ...), which
        /// fails `git diff --check`. Rewriting the saved text in place keeps
        /// a rerun from reintroducing it without hand-editing generated YAML.
        /// </summary>
        static void StripTrailingWhitespace(string assetPath)
        {
            string fullPath = System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(Application.dataPath), assetPath);
            if (!System.IO.File.Exists(fullPath))
            {
                return;
            }

            string original = System.IO.File.ReadAllText(fullPath);
            string[] lines = original.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                bool hadCarriageReturn = lines[i].EndsWith("\r");
                string trimmed = lines[i].TrimEnd(' ', '\t', '\r');
                lines[i] = hadCarriageReturn ? trimmed + "\r" : trimmed;
            }

            string cleaned = string.Join("\n", lines);
            if (cleaned != original)
            {
                System.IO.File.WriteAllText(fullPath, cleaned);
            }
        }

        static GameObject BuildHudPrefab()
        {
            if (!AssetDatabase.IsValidFolder(PrefabFolder))
            {
                AssetDatabase.CreateFolder("Assets/_Project/Prefabs", "UI");
            }

            GameObject root = BuildHierarchy();
            try
            {
                // SaveAsPrefabAsset re-mints every local file id, so blindly
                // re-saving an unchanged HUD would rewrite the whole asset on
                // every run. Comparing the authored signature first keeps a
                // repeat run a true byte-level no-op.
                GameObject existing = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath);
                if (existing != null && Signature(existing.transform) == Signature(root.transform))
                {
                    return existing;
                }

                GameObject saved = PrefabUtility.SaveAsPrefabAsset(root, PrefabPath, out bool success);
                if (!success || saved == null)
                {
                    throw new System.InvalidOperationException($"Failed to save authored HUD prefab at {PrefabPath}.");
                }

                return saved;
            }
            finally
            {
                Object.DestroyImmediate(root);
            }
        }

        /// <summary>
        /// A stable text fingerprint of the authored composition: hierarchy
        /// shape, active state, component set, RectTransform layout and the
        /// graphic/text properties this tool authors. Anything this authoring
        /// code can change is covered; deleting the prefab forces a rebuild.
        /// </summary>
        static string Signature(Transform root)
        {
            var builder = new System.Text.StringBuilder();
            AppendSignature(builder, root, string.Empty);
            return builder.ToString();
        }

        static void AppendSignature(System.Text.StringBuilder builder, Transform node, string path)
        {
            string self = path + "/" + node.name;
            builder.Append(self).Append('|').Append(node.gameObject.activeSelf);

            // A Canvas drives its own RectTransform, so the saved asset never
            // reports back the values the in-memory build had. Comparing them
            // would make every run look changed.
            if (node is RectTransform rect && node.GetComponent<Canvas>() == null)
            {
                builder.Append("|rect:")
                    .Append(Fixed(rect.anchorMin)).Append(Fixed(rect.anchorMax)).Append(Fixed(rect.pivot))
                    .Append(Fixed(rect.anchoredPosition)).Append(Fixed(rect.sizeDelta));
            }

            foreach (Component component in node.GetComponents<Component>())
            {
                if (component == null || component is Transform)
                {
                    continue;
                }

                builder.Append('|').Append(component.GetType().Name);
                if (component is Graphic graphic)
                {
                    builder.Append(':').Append(ColorUtility.ToHtmlStringRGBA(graphic.color)).Append(':').Append(graphic.raycastTarget);
                }

                if (component is Image image)
                {
                    builder.Append(':').Append(image.type).Append(':').Append(image.fillMethod);
                }

                if (component is Text text)
                {
                    builder.Append(':').Append(text.text).Append(':').Append(text.fontSize).Append(':').Append(text.alignment);
                }
            }

            builder.Append('\n');

            for (int i = 0; i < node.childCount; i++)
            {
                AppendSignature(builder, node.GetChild(i), self);
            }
        }

        static string Fixed(Vector2 value) =>
            "(" + value.x.ToString("F3", System.Globalization.CultureInfo.InvariantCulture)
                + "," + value.y.ToString("F3", System.Globalization.CultureInfo.InvariantCulture) + ")";

        static GameObject BuildHierarchy()
        {
            var root = new GameObject("ProductProofCombatHud", typeof(RectTransform));
            var canvas = root.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 5;

            var scaler = root.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;

            root.AddComponent<GraphicRaycaster>();

            // The HUD prefab carries its own event plumbing so the arena
            // scene needs no runtime UI construction at all.
            var eventSystem = new GameObject("EventSystem", typeof(EventSystem));
            eventSystem.transform.SetParent(root.transform, false);
            eventSystem.AddComponent<InputSystemUIInputModule>();

            var view = root.AddComponent<ProductionCombatHudView>();
            root.AddComponent<ProductionHud>();
            root.AddComponent<BlessingChoiceHud>();

            var refs = new HudReferences { RootCanvas = canvas };

            BuildStatusLayer(NewLayer(root.transform, "StatusLayer"), refs);
            BuildControlLayer(NewLayer(root.transform, "ControlLayer"), refs);
            BuildCombatOverlayLayer(NewLayer(root.transform, "CombatOverlayLayer"), refs);
            BuildMenuLayer(NewLayer(root.transform, "MenuLayer"), refs);
            BuildBlessingSurface(root.transform, refs);

            Apply(view, refs);
            return root;
        }

        // ---------------------------------------------------------------
        // Layers
        // ---------------------------------------------------------------

        static void BuildStatusLayer(Transform layer, HudReferences refs)
        {
            RectTransform statusGroup = NewRect(layer, "PlayerStatusGroup", TopLeft, TopLeft, new Vector2(56f, -52f), new Vector2(600f, 148f));
            NewPanel(statusGroup, "Backing", PanelInk);
            Decor(NewRect(statusGroup, "AccentStripe", new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), Vector2.zero, new Vector2(8f, 148f)), Accent);

            refs.HpText = NewText(statusGroup, "HpText", "HP 5/5", 42, TextAnchor.UpperLeft, TextPrimary,
                TopLeft, TopLeft, new Vector2(30f, -14f), new Vector2(540f, 56f));
            refs.StageText = NewText(statusGroup, "StageText", "Wave 1", 32, TextAnchor.UpperLeft, TextSecondary,
                TopLeft, TopLeft, new Vector2(30f, -80f), new Vector2(540f, 48f));

            refs.BlessingText = NewText(layer, "BlessingText", string.Empty, 28, TextAnchor.UpperLeft, Accent,
                TopLeft, TopLeft, new Vector2(56f, -216f), new Vector2(1100f, 44f));

            refs.PauseButton = NewButton(layer, "PauseButton", "II", 40, MenuButtonColor, SkillLabel,
                TopRight, TopRight, new Vector2(-56f, -52f), new Vector2(120f, 120f), out _);

            RectTransform statsGroup = NewRect(layer, "RunStatsGroup", TopRight, TopRight, new Vector2(-196f, -52f), new Vector2(420f, 148f));
            NewPanel(statsGroup, "Backing", PanelInk);
            refs.KillsText = NewText(statsGroup, "KillsText", "Kills: 0", 38, TextAnchor.UpperRight, TextPrimary,
                TopRight, TopRight, new Vector2(-30f, -14f), new Vector2(360f, 56f));
            refs.TimeText = NewText(statsGroup, "TimeText", "00:00", 32, TextAnchor.UpperRight, TextSecondary,
                TopRight, TopRight, new Vector2(-30f, -80f), new Vector2(360f, 48f));
        }

        static void BuildControlLayer(Transform layer, HudReferences refs)
        {
            // Persistent move affordance: the left-thumb pad is always
            // visible so the player never has to discover that the left half
            // of the screen steers.
            refs.MoveBase = NewRect(layer, "MoveBase", BottomLeft, Center, new Vector2(250f, 250f), new Vector2(300f, 300f));
            Image moveBaseImage = refs.MoveBase.gameObject.AddComponent<Image>();
            moveBaseImage.color = new Color(1f, 1f, 1f, 0.16f);
            moveBaseImage.raycastTarget = false;

            refs.MoveKnob = NewRect(refs.MoveBase, "MoveKnob", Center, Center, Vector2.zero, new Vector2(120f, 120f));
            Image knobImage = refs.MoveKnob.gameObject.AddComponent<Image>();
            knobImage.color = new Color(1f, 1f, 1f, 0.55f);
            knobImage.raycastTarget = false;

            Text moveHint = NewText(layer, "MoveHint", "DI CHUYỂN  /  MOVE", 26, TextAnchor.MiddleCenter, new Color(1f, 1f, 1f, 0.72f),
                BottomLeft, Center, new Vector2(250f, 452f), new Vector2(460f, 44f));
            moveHint.raycastTarget = false;

            Transform cluster = NewLayer(layer, "ActionCluster");

            // Basic is the loudest, largest and lowest target in the
            // right-thumb arc: it is the action the player uses constantly,
            // and it is the only one whose colour this HUD owns outright.
            refs.BasicButton = NewButton(cluster, "BasicButton", "ĐÁNH\nBASIC", 40, BasicAction, BasicActionLabel,
                BottomRight, Center, new Vector2(-210f, 180f), new Vector2(240f, 240f), out _);

            var skillSpecs = new[]
            {
                ("SkillButton_0", "LÔI\nTRẢM", new Vector2(-225f, 450f)),
                ("SkillButton_1", "PHONG\nBỘ", new Vector2(-450f, 340f)),
                ("SkillButton_2", "HỘ\nTHỂ", new Vector2(-575f, 145f))
            };

            for (int i = 0; i < skillSpecs.Length; i++)
            {
                (string name, string label, Vector2 position) = skillSpecs[i];
                refs.SkillButtons[i] = NewButton(cluster, name, label, 34, SkillReady, SkillLabel,
                    BottomRight, Center, position, new Vector2(180f, 180f), out Text labelText);
                refs.SkillLabels[i] = labelText;

                RectTransform overlay = NewRect(refs.SkillButtons[i].transform, "CooldownOverlay", Center, Center, Vector2.zero, new Vector2(180f, 180f));
                Image scrim = overlay.gameObject.AddComponent<Image>();
                scrim.color = CooldownScrim;
                scrim.raycastTarget = false;
                overlay.gameObject.SetActive(false);
                refs.SkillCooldownOverlays[i] = overlay.gameObject;
            }
        }

        static void BuildCombatOverlayLayer(Transform layer, HudReferences refs)
        {
            RectTransform bossPanel = NewRect(layer, "BossPanel", TopCenter, TopCenter, new Vector2(0f, -40f), new Vector2(900f, 112f));
            refs.BossPanel = bossPanel.gameObject;

            Text bossLabel = NewText(bossPanel, "Label", "MINI BOSS", 30, TextAnchor.UpperCenter, Accent,
                TopCenter, TopCenter, new Vector2(0f, 0f), new Vector2(900f, 40f));
            bossLabel.raycastTarget = false;

            RectTransform bar = NewRect(bossPanel, "HpBar", BottomCenter, BottomCenter, Vector2.zero, new Vector2(900f, 56f));
            Image barBackground = bar.gameObject.AddComponent<Image>();
            barBackground.color = new Color(0.13f, 0.13f, 0.15f, 0.92f);
            barBackground.raycastTarget = false;

            RectTransform fill = NewRect(bar, "Fill", Center, Center, Vector2.zero, new Vector2(900f, 56f));
            refs.BossHpFill = fill.gameObject.AddComponent<Image>();
            refs.BossHpFill.color = Accent;
            refs.BossHpFill.type = Image.Type.Filled;
            refs.BossHpFill.fillMethod = Image.FillMethod.Horizontal;
            refs.BossHpFill.fillAmount = 1f;
            refs.BossHpFill.raycastTarget = false;

            refs.BossHpText = NewText(bar, "HpText", string.Empty, 30, TextAnchor.MiddleCenter, TextPrimary,
                Center, Center, Vector2.zero, new Vector2(900f, 56f));
            refs.BossHpText.raycastTarget = false;

            refs.BossArrivalText = NewText(layer, "BossArrivalText", "MINI BOSS", 96, TextAnchor.MiddleCenter, Accent,
                Center, Center, new Vector2(0f, 140f), new Vector2(1400f, 160f));
            refs.BossArrivalText.raycastTarget = false;
        }

        static void BuildMenuLayer(Transform layer, HudReferences refs)
        {
            Transform pausePanel = NewLayer(layer, "PausePanel");
            refs.PausePanel = pausePanel.gameObject;
            NewPanel(pausePanel, "Backdrop", Backdrop).raycastTarget = true;
            RectTransform pauseCard = NewCard(pausePanel, new Vector2(760f, 640f));

            Text pauseTitle = NewText(pauseCard, "Title", "TẠM DỪNG", 54, TextAnchor.MiddleCenter, TextPrimary,
                TopCenter, TopCenter, new Vector2(0f, -36f), new Vector2(700f, 80f));
            pauseTitle.raycastTarget = false;

            refs.ResumeButton = NewButton(pauseCard, "ResumeButton", "TIẾP TỤC", 34, MenuButtonColor, SkillLabel,
                TopCenter, TopCenter, new Vector2(0f, -180f), new Vector2(520f, 110f), out _);
            refs.RestartButton = NewButton(pauseCard, "RestartButton", "CHƠI LẠI", 34, MenuButtonColor, SkillLabel,
                TopCenter, TopCenter, new Vector2(0f, -310f), new Vector2(520f, 110f), out _);
            refs.ExitButton = NewButton(pauseCard, "ExitButton", "VỀ MENU", 34, MenuButtonColor, SkillLabel,
                TopCenter, TopCenter, new Vector2(0f, -440f), new Vector2(520f, 110f), out _);
            pausePanel.gameObject.SetActive(false);

            Transform resultPanel = NewLayer(layer, "ResultPanel");
            refs.ResultPanel = resultPanel.gameObject;
            NewPanel(resultPanel, "Backdrop", Backdrop).raycastTarget = true;
            RectTransform resultCard = NewCard(resultPanel, new Vector2(1040f, 660f));

            refs.ResultTitleText = NewText(resultCard, "Title", "VICTORY", 76, TextAnchor.MiddleCenter, Accent,
                TopCenter, TopCenter, new Vector2(0f, -44f), new Vector2(980f, 110f));
            refs.ResultTitleText.raycastTarget = false;
            refs.ResultSummaryText = NewText(resultCard, "Summary", string.Empty, 34, TextAnchor.MiddleCenter, TextPrimary,
                Center, Center, new Vector2(0f, 20f), new Vector2(960f, 200f));
            refs.ResultSummaryText.raycastTarget = false;

            refs.RetryButton = NewButton(resultCard, "RetryButton", "CHƠI LẠI", 36, MenuButtonColor, SkillLabel,
                BottomCenter, Center, new Vector2(-230f, 120f), new Vector2(420f, 120f), out _);
            refs.MenuButton = NewButton(resultCard, "MenuButton", "MENU", 36, MenuButtonColor, SkillLabel,
                BottomCenter, Center, new Vector2(230f, 120f), new Vector2(420f, 120f), out _);
            resultPanel.gameObject.SetActive(false);
        }

        /// <summary>
        /// The Cơ Duyên surface lives on the same Canvas as combat. Its
        /// outer container stays active as the authored region, while
        /// BlessingRoot is the single node BlessingChoiceHud toggles.
        /// </summary>
        static void BuildBlessingSurface(Transform parent, HudReferences refs)
        {
            Transform region = NewLayer(parent, "BlessingChoicePanel");
            Transform blessingRoot = NewLayer(region, "BlessingRoot");
            refs.BlessingRoot = blessingRoot.gameObject;

            NewPanel(blessingRoot, "Backdrop", Backdrop).raycastTarget = true;
            Text title = NewText(blessingRoot, "Title", "CHỌN CƠ DUYÊN", 62, TextAnchor.MiddleCenter, Accent,
                TopCenter, TopCenter, new Vector2(0f, -90f), new Vector2(1400f, 92f));
            title.raycastTarget = false;

            RectTransform options = NewRect(blessingRoot, "BlessingChoiceOptions", Center, Center, new Vector2(0f, -20f), new Vector2(1560f, 600f));
            refs.BlessingChoicePanel = options.gameObject;

            var choiceSpecs = new[]
            {
                ("BlessingButton_0", "LÔI KIẾM\n\nKiếm lôi mạnh hơn\nStronger lightning", -520f),
                ("BlessingButton_1", "PHONG HÀNH\n\nDi chuyển nhanh hơn\nFaster movement", 0f),
                ("BlessingButton_2", "HỘ THỂ\n\nHộ thể vững hơn\nMore max health", 520f)
            };

            for (int i = 0; i < choiceSpecs.Length; i++)
            {
                (string name, string label, float x) = choiceSpecs[i];
                refs.BlessingButtons[i] = NewButton(options, name, label, 34, ChoiceButtonColor, SkillLabel,
                    Center, Center, new Vector2(x, 0f), new Vector2(460f, 560f), out Text labelText);
                labelText.horizontalOverflow = HorizontalWrapMode.Wrap;
                labelText.verticalOverflow = VerticalWrapMode.Truncate;
            }

            RectTransform confirm = NewRect(blessingRoot, "BlessingConfirmPanel", Center, Center, Vector2.zero, new Vector2(1200f, 320f));
            refs.BlessingConfirmPanel = confirm.gameObject;
            refs.BlessingConfirmTitleText = NewText(confirm, "ConfirmTitle", string.Empty, 88, TextAnchor.MiddleCenter, Accent,
                TopCenter, TopCenter, new Vector2(0f, -20f), new Vector2(1160f, 140f));
            refs.BlessingConfirmTitleText.raycastTarget = false;
            refs.BlessingConfirmFlavorText = NewText(confirm, "ConfirmFlavor", string.Empty, 40, TextAnchor.MiddleCenter, TextPrimary,
                BottomCenter, BottomCenter, new Vector2(0f, 40f), new Vector2(1160f, 92f));
            refs.BlessingConfirmFlavorText.raycastTarget = false;
            confirm.gameObject.SetActive(false);
        }

        // ---------------------------------------------------------------
        // Scene wiring
        // ---------------------------------------------------------------

        static void WireArenaScene(GameObject hudPrefab)
        {
            Scene scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            var bootstrapper = Object.FindFirstObjectByType<ArenaVerticalSliceBootstrapper>(FindObjectsInactive.Include);
            if (bootstrapper == null)
            {
                throw new System.InvalidOperationException($"{ScenePath} has no ArenaVerticalSliceBootstrapper to wire.");
            }

            var serialized = new SerializedObject(bootstrapper);
            SerializedProperty markersProperty = serialized.FindProperty("waterZonePositionMarkers");
            var markers = new Transform[markersProperty.arraySize];
            for (int i = 0; i < markers.Length; i++)
            {
                markers[i] = (Transform)markersProperty.GetArrayElementAtIndex(i).objectReferenceValue;
            }

            bootstrapper.ConfigureAuthoring(
                (Transform)Reference(serialized, "groundTransform"),
                (Transform)Reference(serialized, "playerSpawn"),
                (WaterZone)Reference(serialized, "waterZone"),
                markers,
                (GameObject)Reference(serialized, "cultivatorProxyPrefab"),
                (GameObject)Reference(serialized, "pursuerPrefab"),
                (GameObject)Reference(serialized, "lancerPrefab"),
                (GameObject)Reference(serialized, "bossPrefab"),
                hudPrefab);

            EditorUtility.SetDirty(bootstrapper);
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
        }

        static Object Reference(SerializedObject serialized, string fieldName)
        {
            SerializedProperty property = serialized.FindProperty(fieldName);
            if (property == null)
            {
                throw new System.InvalidOperationException($"ArenaVerticalSliceBootstrapper has no serialized field '{fieldName}'.");
            }

            return property.objectReferenceValue;
        }

        // ---------------------------------------------------------------
        // Serialized view assignment
        // ---------------------------------------------------------------

        static void Apply(ProductionCombatHudView view, HudReferences refs)
        {
            var serialized = new SerializedObject(view);
            Assign(serialized, "rootCanvas", refs.RootCanvas);
            Assign(serialized, "hpText", refs.HpText);
            Assign(serialized, "stageText", refs.StageText);
            Assign(serialized, "blessingText", refs.BlessingText);
            Assign(serialized, "killsText", refs.KillsText);
            Assign(serialized, "timeText", refs.TimeText);
            Assign(serialized, "moveBase", refs.MoveBase);
            Assign(serialized, "moveKnob", refs.MoveKnob);
            Assign(serialized, "basicButton", refs.BasicButton);
            AssignArray(serialized, "skillButtons", refs.SkillButtons);
            AssignArray(serialized, "skillLabels", refs.SkillLabels);
            AssignArray(serialized, "skillCooldownOverlays", refs.SkillCooldownOverlays);
            Assign(serialized, "pauseButton", refs.PauseButton);
            Assign(serialized, "resumeButton", refs.ResumeButton);
            Assign(serialized, "restartButton", refs.RestartButton);
            Assign(serialized, "exitButton", refs.ExitButton);
            Assign(serialized, "pausePanel", refs.PausePanel);
            Assign(serialized, "bossPanel", refs.BossPanel);
            Assign(serialized, "resultPanel", refs.ResultPanel);
            Assign(serialized, "bossHpFill", refs.BossHpFill);
            Assign(serialized, "bossHpText", refs.BossHpText);
            Assign(serialized, "bossArrivalText", refs.BossArrivalText);
            Assign(serialized, "resultTitleText", refs.ResultTitleText);
            Assign(serialized, "resultSummaryText", refs.ResultSummaryText);
            Assign(serialized, "retryButton", refs.RetryButton);
            Assign(serialized, "menuButton", refs.MenuButton);
            Assign(serialized, "blessingRoot", refs.BlessingRoot);
            Assign(serialized, "blessingChoicePanel", refs.BlessingChoicePanel);
            Assign(serialized, "blessingConfirmPanel", refs.BlessingConfirmPanel);
            AssignArray(serialized, "blessingButtons", refs.BlessingButtons);
            Assign(serialized, "blessingConfirmTitleText", refs.BlessingConfirmTitleText);
            Assign(serialized, "blessingConfirmFlavorText", refs.BlessingConfirmFlavorText);
            serialized.ApplyModifiedPropertiesWithoutUndo();

            if (!view.IsComplete)
            {
                throw new System.InvalidOperationException("Authored ProductionCombatHudView is incomplete after assignment.");
            }
        }

        static void Assign(SerializedObject serialized, string fieldName, Object value)
        {
            SerializedProperty property = serialized.FindProperty(fieldName);
            if (property == null)
            {
                throw new System.InvalidOperationException($"ProductionCombatHudView has no serialized field '{fieldName}'.");
            }

            property.objectReferenceValue = value;
        }

        static void AssignArray<T>(SerializedObject serialized, string fieldName, T[] values) where T : Object
        {
            SerializedProperty property = serialized.FindProperty(fieldName);
            if (property == null)
            {
                throw new System.InvalidOperationException($"ProductionCombatHudView has no serialized field '{fieldName}'.");
            }

            property.arraySize = values.Length;
            for (int i = 0; i < values.Length; i++)
            {
                property.GetArrayElementAtIndex(i).objectReferenceValue = values[i];
            }
        }

        // ---------------------------------------------------------------
        // Layout primitives
        // ---------------------------------------------------------------

        static readonly Vector2 TopLeft = new Vector2(0f, 1f);
        static readonly Vector2 TopRight = new Vector2(1f, 1f);
        static readonly Vector2 TopCenter = new Vector2(0.5f, 1f);
        static readonly Vector2 BottomLeft = new Vector2(0f, 0f);
        static readonly Vector2 BottomRight = new Vector2(1f, 0f);
        static readonly Vector2 BottomCenter = new Vector2(0.5f, 0f);
        static readonly Vector2 Center = new Vector2(0.5f, 0.5f);

        /// <summary>A named, screen-filling grouping node - visual grouping the prefab reviewer can read, with no graphic of its own.</summary>
        static Transform NewLayer(Transform parent, string name)
        {
            var go = new GameObject(name, typeof(RectTransform));
            var rt = (RectTransform)go.transform;
            rt.SetParent(parent, false);
            StretchToParent(rt);
            return go.transform;
        }

        static RectTransform NewRect(Transform parent, string name, Vector2 anchor, Vector2 pivot, Vector2 anchoredPosition, Vector2 size)
        {
            var go = new GameObject(name, typeof(RectTransform));
            var rt = (RectTransform)go.transform;
            rt.SetParent(parent, false);
            rt.anchorMin = anchor;
            rt.anchorMax = anchor;
            rt.pivot = pivot;
            rt.anchoredPosition = anchoredPosition;
            rt.sizeDelta = size;
            return rt;
        }

        static Image NewPanel(Transform parent, string name, Color color)
        {
            var go = new GameObject(name, typeof(RectTransform));
            var rt = (RectTransform)go.transform;
            rt.SetParent(parent, false);
            StretchToParent(rt);
            var image = go.AddComponent<Image>();
            image.color = color;
            image.raycastTarget = false;
            return image;
        }

        static RectTransform NewCard(Transform parent, Vector2 size)
        {
            RectTransform card = NewRect(parent, "Card", Center, Center, Vector2.zero, size);
            NewPanel(card, "Background", CardInk);
            Decor(NewRect(card, "AccentTop", TopCenter, TopCenter, Vector2.zero, new Vector2(size.x, 8f)), Accent);
            return card;
        }

        /// <summary>A purely decorative, non-interactive block of colour.</summary>
        static void Decor(RectTransform rt, Color color)
        {
            var image = rt.gameObject.AddComponent<Image>();
            image.color = color;
            image.raycastTarget = false;
        }

        static Text NewText(Transform parent, string name, string content, int fontSize, TextAnchor alignment, Color color,
            Vector2 anchor, Vector2 pivot, Vector2 anchoredPosition, Vector2 size)
        {
            RectTransform rt = NewRect(parent, name, anchor, pivot, anchoredPosition, size);
            var text = rt.gameObject.AddComponent<Text>();
            text.font = UiFont;
            text.fontSize = fontSize;
            text.fontStyle = FontStyle.Bold;
            text.alignment = alignment;
            text.color = color;
            text.text = content;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.raycastTarget = false;
            return text;
        }

        static Button NewButton(Transform parent, string name, string label, int fontSize, Color background, Color labelColor,
            Vector2 anchor, Vector2 pivot, Vector2 anchoredPosition, Vector2 size, out Text labelText)
        {
            RectTransform rt = NewRect(parent, name, anchor, pivot, anchoredPosition, size);
            var image = rt.gameObject.AddComponent<Image>();
            image.color = background;
            image.raycastTarget = true;

            var button = rt.gameObject.AddComponent<Button>();
            button.targetGraphic = image;
            button.transition = Selectable.Transition.ColorTint;

            ColorBlock colors = button.colors;
            colors.normalColor = Color.white;
            colors.highlightedColor = Color.white;
            // A clearly darker press state is the only touch feedback a
            // thumb gets on a phone, so it is authored, not left at default.
            colors.pressedColor = new Color(0.68f, 0.74f, 0.80f, 1f);
            colors.selectedColor = Color.white;
            // Disabled tint is left neutral: ProductionHud owns the
            // ready/cooldown colour for skill buttons and must not be
            // double-dimmed into unreadability.
            colors.disabledColor = Color.white;
            colors.fadeDuration = 0.05f;
            button.colors = colors;

            labelText = NewText(rt, "Label", label, fontSize, TextAnchor.MiddleCenter, labelColor, Center, Center, Vector2.zero, size);
            return button;
        }

        static void StretchToParent(RectTransform rt)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.pivot = Center;
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = Vector2.zero;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }

        sealed class HudReferences
        {
            public Canvas RootCanvas;
            public Text HpText;
            public Text StageText;
            public Text BlessingText;
            public Text KillsText;
            public Text TimeText;
            public RectTransform MoveBase;
            public RectTransform MoveKnob;
            public Button BasicButton;
            public readonly Button[] SkillButtons = new Button[3];
            public readonly Text[] SkillLabels = new Text[3];
            public readonly GameObject[] SkillCooldownOverlays = new GameObject[3];
            public Button PauseButton;
            public Button ResumeButton;
            public Button RestartButton;
            public Button ExitButton;
            public GameObject PausePanel;
            public GameObject BossPanel;
            public GameObject ResultPanel;
            public Image BossHpFill;
            public Text BossHpText;
            public Text BossArrivalText;
            public Text ResultTitleText;
            public Text ResultSummaryText;
            public Button RetryButton;
            public Button MenuButton;
            public GameObject BlessingRoot;
            public GameObject BlessingChoicePanel;
            public GameObject BlessingConfirmPanel;
            public readonly Button[] BlessingButtons = new Button[3];
            public Text BlessingConfirmTitleText;
            public Text BlessingConfirmFlavorText;
        }
    }
}
