using System;
using System.Linq;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;

using HarmonyLib;
using Lib;
using System.Data;

namespace BetterAuroraUI
{

    [HarmonyPatch(typeof(ComboBox))]
    [HarmonyPatch("OnTextChanged")]
    class ComboBoxChangedPatch
    {
        static private bool disablePatch = false;
        static private DateTime lastPress;
        public static void Postfix(ComboBox __instance, EventArgs e)
        {
            if (disablePatch)
                return;
            switch (__instance.Name)
            {
                case "cboSystems":
                    switch (__instance.Parent.Name)
                    {
                        case "SystemView":
                            if (!Utils.ControlPress)
                            {
                                double timeDiff = (DateTime.Now - lastPress).TotalMilliseconds;
                                if (timeDiff < 100)
                                {
                                    disablePatch = true;
                                    string systemName = __instance.Text;
                                    ComboBox tacticalSystemCombo = Utils.Find<ComboBox>(FormOnShowPatch.tacticalMap, "cboSystems", goUp: 1);
                                    int index = tacticalSystemCombo.FindString(systemName);
                                    if (index >= 0)
                                    {
                                        tacticalSystemCombo.SelectedIndex = index;
                                        ((Form)__instance.Parent).Close();
                                        FormOnShowPatch.tacticalMap.Focus();
                                    }
                                    disablePatch = false;
                                } else
                                {
                                    lastPress = DateTime.Now;
                                }
                            }
                            break;
                    }
                    break;
            }
        }
    }
}