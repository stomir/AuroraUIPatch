using System;
using System.Linq;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;

using HarmonyLib;
using System.Data;

namespace BetterAuroraUI
{
    [HarmonyPatch(typeof(Control))]
    [HarmonyPatch("OnClick")]
    class ControlClickPatch
    {
        public static void Postfix(Control __instance, EventArgs e)
        {
            if (Control.ModifierKeys == (Keys.Control | Keys.Shift | Keys.Alt))
            {
                MessageBox.Show($"debug click type={__instance.GetType()} text={__instance.Text} name={__instance.Name}");
            }
        }
    }
}