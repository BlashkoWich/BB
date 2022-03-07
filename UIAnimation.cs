using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BB.Helpers
{
    public static class UIAnimation
    {
        public static IEnumerator CharactersCardsMoving(
            VerticalLayoutGroup leftLayoutGroup,
            VerticalLayoutGroup rightLayoutGroup,
            int startEdgeSize,
            int endEdgeSize,
            float changingTime,
            float division,
            System.Action callback
            )
        {
            var delta = endEdgeSize - startEdgeSize;

            float timePart = changingTime / division;

            float deltaPerTime = (int)(delta / division);

            int currentEdgeSize = startEdgeSize;

            var rectLeft = leftLayoutGroup.GetComponent<RectTransform>();
            var rectRight = rightLayoutGroup.GetComponent<RectTransform>();

            while (currentEdgeSize != endEdgeSize)
            {
                leftLayoutGroup.padding.left = currentEdgeSize;
                rightLayoutGroup.padding.right = currentEdgeSize;

                currentEdgeSize += (int)deltaPerTime;
                currentEdgeSize = Mathf.Clamp(currentEdgeSize, -Screen.width, Screen.width);
                LayoutRebuilder.ForceRebuildLayoutImmediate(rectLeft);
                LayoutRebuilder.ForceRebuildLayoutImmediate(rectRight);

                yield return new WaitForSecondsRealtime(timePart);
            }

            callback?.Invoke();
        }

        public static IEnumerator TextSizeChangingAnimation(
            TMPro.TextMeshProUGUI text,
            int startFontSize,
            int endFontSize,
            float changingTime,
            float division,
            Action callback = default
            )
        {
            var delta = endFontSize - startFontSize;


            float timePart = changingTime / division;

            int deltaPerTime = (int)(delta / division);

            int currentSize = startFontSize;

            while (currentSize != endFontSize)
            {
                currentSize += deltaPerTime;
                text.fontSize = currentSize;

                yield return new WaitForSecondsRealtime(timePart);
            }

            callback?.Invoke();
        }

        public static IEnumerator ImageVisibilityChangingAnimation(
            Image image,
            float startVisibility,
            float endVisibility,
            float changingTime,
            float division,
            Action callback = default
            )
        {
            var delta = endVisibility - startVisibility;

            float timePart = changingTime / division;

            float deltaPerTime = delta / division;

            var currentVisibility = startVisibility;

            while (currentVisibility != endVisibility)
            {
                currentVisibility += deltaPerTime;
                currentVisibility = Mathf.Clamp(currentVisibility, 0, 1);
                image.color = new Color(image.color.r, image.color.g, image.color.b, currentVisibility);

                yield return new WaitForSecondsRealtime(timePart);
            }

            callback?.Invoke();
        }

        public static IEnumerator ImageRotating(
            Image image,
            float startAngle,
            float finishAngle,
            float changingTime,
            float division,
            Action callback = default)
        {
            var delta = finishAngle - startAngle;

            float timePart = changingTime / division;

            float deltaPerTime = delta / division;

            var currentAngle = startAngle;

            while (currentAngle != finishAngle)
            {
                image.rectTransform.Rotate(Vector3.forward * deltaPerTime, Space.Self);
                yield return new WaitForSecondsRealtime(timePart);
            }

            callback?.Invoke();
        }

        public static IEnumerator UIElementsShowing(
            RectTransform uiElementTransform,
            Vector3 delta,
            float changingTime,
            int division,
            Action callback = default
            )
        {
            float timePart = changingTime / division;

            Vector3 deltaPerTime = delta / division;

            var currentPosition = uiElementTransform.position;

            for (int i = 0; i < division; i++)
            {
                uiElementTransform.position += deltaPerTime;

                uiElementTransform.ForceUpdateRectTransforms();
                yield return new WaitForSecondsRealtime(timePart);
            }



            callback?.Invoke();
        }

        public static IEnumerator UICountDown(
            TMPro.TextMeshProUGUI count,
            int startCount,
            int finishCount,
            float deltaTime,
            Action callback = default
            )
        {
            var currentCount = startCount;
            count.text = currentCount.ToString();
            var delta = (int)Mathf.Sign(finishCount - currentCount);

            while (currentCount != finishCount)
            {
                currentCount += delta;
                count.text = currentCount.ToString();
                yield return new WaitForSecondsRealtime(deltaTime);
            }

            callback?.Invoke();
        }

        public static IEnumerator ShakeRectTransform(
            RectTransform rectTransform,
            Vector2 delta,
            float timeToOneShake,
            int shakesCount,
            Action callback = default
            )
        {

            for (int i = 0; i < shakesCount; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    rectTransform.position += new Vector3(delta.x * 0.1f, delta.y * 0.1f, 0);
                    yield return new WaitForEndOfFrame();
                }

                for (int j = 0; j < 5; j++)
                {
                    rectTransform.position -= new Vector3(delta.x * 0.1f, delta.y * 0.1f, 0);
                    yield return new WaitForEndOfFrame();
                }
                for (int j = 0; j < 5; j++)
                {
                    rectTransform.position -= new Vector3(delta.x * 0.1f, delta.y * 0.1f, 0);
                    yield return new WaitForEndOfFrame();
                }
                for (int j = 0; j < 5; j++)
                {
                    rectTransform.position += new Vector3(delta.x * 0.1f, delta.y * 0.1f, 0);
                    yield return new WaitForEndOfFrame();
                }
            }
        }

        public static IEnumerator SetDangerColor(
            Image colorable,
            Color dangerColor,
            float time,
            int division,
            Action callback = default
            )
        {
            var normalColor = colorable.color;

            colorable.color = dangerColor;

            var deltaR = (normalColor.r - dangerColor.r) / division;
            var deltaG = (normalColor.g - dangerColor.g) / division;
            var deltaB = (normalColor.b - dangerColor.b) / division;

            var deltaTime = time / division;

            for (int i = 0; i < division; i++)
            {
                colorable.color = new Color(colorable.color.r + deltaR, colorable.color.g + deltaG, colorable.color.b + deltaB, normalColor.a);
                yield return new WaitForSecondsRealtime(deltaTime);
            }

        }
    }
}
