using System;
using System.Linq;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;

using HarmonyLib;
using System.Data;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BetterAuroraUI
{
    [HarmonyPatch(typeof(Control))]
    [HarmonyPatch("OnMouseDoubleClick", new Type[] { typeof(MouseEventArgs) })]
    class TreeViewPatch
    {
        public static void Postfix(Control __instance, MouseEventArgs e)
        {
            if (__instance is System.Windows.Forms.TreeView tv)
                switch (tv.Name)
                {
                    case "tvComponents":
                            Utils.RepeatDoubleClick(tv, 0);
                        break;
                    case "tvInClass":
                        if (Utils.ControlPress)
                            Utils.RepeatDoubleClick(tv, 9, () => tv.SelectedNode == null);
                        else if (Utils.ShiftPress)
                            Utils.RepeatDoubleClick(tv, 99, () => tv.SelectedNode == null);
                        break;
                }
        }

        public static string CutName(string input)
        {
            int index = input.IndexOf(' ');
            if (index == -1)
            {
                return input;
            }
            return input.Substring(index + 1);
        }
    }
}