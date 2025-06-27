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
    [HarmonyPatch("OnMouseDoubleClick")]
    class ListBoxDoubleClickPatch
    {
        public static void Postfix(Control __instance, EventArgs e)
        {
            if (__instance is ListBox lv)
                switch (__instance.Name)
                {
                    case "lstClassOrdnance":
                        Utils.RepeatDoubleClick(lv, 0, () => lv.SelectedIndices.Count > 0);
                        break;
                    default:
                        break;
                }
        }
    }
}