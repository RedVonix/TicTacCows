using System.Collections.Generic;
using System.Threading.Tasks;
using TicTacCows.Logging;
using UnityEngine;

namespace TicTacCows.UI
{
    public class UIController : MonoBehaviour
    {
        // Being an example, this is a very rudimentary UI management system. For a real project, this would need to have a lot of
        // conditionals and checks such as ensuring there is always a current object for the active event system for games
        // that allow controller input.

        public List<UIDialog> allDialogsInGame = new List<UIDialog>();

        public void OpenDialog(GameValues.DialogTypes inType)
        {
            UIDialog targetDialog = allDialogsInGame.Find(s => s.myDialogType == inType);
            if(targetDialog == null)
            {
                LoggingSystem.AddLog(GameValues.LoggingTypes.Warn, "--- UIController:OpenDialog - No dialog found of type " + inType);
                return;
            }

            targetDialog.gameObject.SetActive(true);
        }

        public void CloseDialog(GameValues.DialogTypes inType)
        {
            UIDialog targetDialog = allDialogsInGame.Find(s => s.myDialogType == inType);
            if (targetDialog == null)
            {
                LoggingSystem.AddLog(GameValues.LoggingTypes.Warn, "--- UIController:CloseDialog - No dialog found of type " + inType);
                return;
            }

            targetDialog.gameObject.SetActive(false);
        }

        public void FadeOutDialog(GameValues.DialogTypes inType)
        {
            UIDialog targetDialog = allDialogsInGame.Find(s => s.myDialogType == inType);
            if (targetDialog == null)
            {
                LoggingSystem.AddLog(GameValues.LoggingTypes.Warn, "--- UIController:CloseDialog - No dialog found of type " + inType);
                return;
            }

            PerformFade(targetDialog.gameObject);
        }

        private async void PerformFade(GameObject inObj)
        {
            CanvasGroup canvasGroup = inObj.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                LoggingSystem.AddLog(GameValues.LoggingTypes.Warn, "--- UIController:PerformFade - CanvasGroup was null!");
                inObj.SetActive(false);
                return;
            }

            while(canvasGroup.alpha > 0f)
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0f, 0.25f);
                await Task.Delay(32); // 32 milliseconds being the average frame time in Unity.
            }

            inObj.SetActive(false);
            canvasGroup.alpha = 1f;
        }
    }
}