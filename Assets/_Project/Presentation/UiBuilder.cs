using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Small shared construction helpers for the production Canvas/uGUI HUDs
    /// (Task A5). Not a generic UI framework - just the handful of
    /// GameObject+RectTransform+component wiring every HUD needs, so
    /// MainMenuController/ProductionHud/BlessingChoiceHud don't each
    /// reimplement it. Every HUD still owns its own layout and state.
    /// </summary>
    public static class UiBuilder
    {
        static Font cachedFont;
        static Font DefaultFont => cachedFont != null ? cachedFont : (cachedFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"));

        public static Canvas CreateCanvas(string name, int sortOrder)
        {
            var go = new GameObject(name, typeof(RectTransform));
            var canvas = go.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = sortOrder;

            var scaler = go.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;

            go.AddComponent<GraphicRaycaster>();
            EnsureEventSystem();
            return canvas;
        }

        public static void EnsureEventSystem()
        {
            if (Object.FindFirstObjectByType<EventSystem>() != null)
            {
                return;
            }

            var go = new GameObject("EventSystem", typeof(EventSystem));
            go.AddComponent<InputSystemUIInputModule>();
        }

        public static RectTransform CreateRect(Transform parent, string name, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPosition, Vector2 sizeDelta)
        {
            var go = new GameObject(name, typeof(RectTransform));
            var rt = (RectTransform)go.transform;
            rt.SetParent(parent, false);
            rt.anchorMin = anchorMin;
            rt.anchorMax = anchorMax;
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = anchoredPosition;
            rt.sizeDelta = sizeDelta;
            return rt;
        }

        public static Text CreateText(Transform parent, string name, string content, int fontSize, TextAnchor alignment, Color color, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPosition, Vector2 sizeDelta)
        {
            RectTransform rt = CreateRect(parent, name, anchorMin, anchorMax, anchoredPosition, sizeDelta);
            var text = rt.gameObject.AddComponent<Text>();
            text.font = DefaultFont;
            text.fontSize = fontSize;
            text.fontStyle = FontStyle.Bold;
            text.alignment = alignment;
            text.color = color;
            text.text = content;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            return text;
        }

        public static Image CreatePanel(Transform parent, string name, Color color, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPosition, Vector2 sizeDelta)
        {
            RectTransform rt = CreateRect(parent, name, anchorMin, anchorMax, anchoredPosition, sizeDelta);
            var image = rt.gameObject.AddComponent<Image>();
            image.color = color;
            return image;
        }

        /// <summary>A background Image + full-fill Text label + Button, the shared shape for every clickable HUD element.</summary>
        public static Button CreateButton(Transform parent, string name, string label, int fontSize, Color backgroundColor, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPosition, Vector2 sizeDelta, out Text labelText)
        {
            Image background = CreatePanel(parent, name, backgroundColor, anchorMin, anchorMax, anchoredPosition, sizeDelta);
            Button button = background.gameObject.AddComponent<Button>();

            labelText = CreateText(background.transform, "Label", label, fontSize, TextAnchor.MiddleCenter, Color.black, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            RectTransform labelRect = labelText.rectTransform;
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;

            return button;
        }

        /// <summary>A background Image + a foreground fill Image (Type=Filled), the shared shape for cooldown overlays and HP/boss bars.</summary>
        public static Image CreateFillBar(Transform parent, string name, Color backgroundColor, Color fillColor, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPosition, Vector2 sizeDelta, out Image fillImage)
        {
            Image background = CreatePanel(parent, name, backgroundColor, anchorMin, anchorMax, anchoredPosition, sizeDelta);
            fillImage = CreatePanel(background.transform, "Fill", fillColor, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            fillImage.rectTransform.offsetMin = Vector2.zero;
            fillImage.rectTransform.offsetMax = Vector2.zero;
            fillImage.type = Image.Type.Filled;
            fillImage.fillMethod = Image.FillMethod.Horizontal;
            fillImage.fillAmount = 1f;
            return background;
        }

        public static CanvasGroup AddCanvasGroup(Component component)
        {
            var group = component.GetComponent<CanvasGroup>();
            return group != null ? group : component.gameObject.AddComponent<CanvasGroup>();
        }
    }
}
