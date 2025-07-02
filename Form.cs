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
    [HarmonyPatch(typeof(Form))]
    [HarmonyPatch("OnShown")]
    class FormOnShowPatch
    {
        public static Form tacticalMap;
        public static void Postfix(Form __instance, EventArgs e)
        {
            if (Control.ModifierKeys == (Keys.Control | Keys.Shift | Keys.Alt))
                MessageBox.Show($"debug form show type={__instance.GetType()} text={__instance.Text} name={__instance.Name}");
            switch (__instance.Name)
            {
                case "ClassDesign":
                    //opne wide view
                    Button btn = Utils.Find<Button>(__instance, "cmdSuperWide", goUp: 0);
                    if (btn.Text == "Wide View")
                        btn.PerformClick();
                    break;
                case "Economics":
                    //use components
                    Utils.Find<CheckBox>(__instance, "chkUseComponents", goUp: 0).Checked = true;
                    break;
                case "TacticalMap":
                    tacticalMap = __instance;
                    break;
                case "FleetWindow":
                    Utils.Find<CheckBox>(__instance, "chkIncludeCivilians").Checked = false;
                    break;

            }
        }
    }

    [HarmonyPatch(typeof(Control))]
    [HarmonyPatch("OnMouseDoubleClick")]
    class FormOnDoubleClickPatch
    {
        public static DateTime lastGalMapDoubleClick;
        public static void Prefix(Control __instance, EventArgs e)
        {
            if (__instance is Form form)
            {
                switch (__instance.Name)
                {
                    case "GalacticMap":
                        string systemName = ComboBoxChangedPatch.SystemNameCombo.Text;
                        ComboBox tacticalSystemCombo = Utils.Find<ComboBox>(FormOnShowPatch.tacticalMap, "cboSystems", goUp: 1);
                        int index = tacticalSystemCombo.FindString(systemName);
                        if (index >= 0)
                        {
                            tacticalSystemCombo.SelectedIndex = index;
                            ((Form)ComboBoxChangedPatch.SystemNameCombo.Parent).Close();
                            FormOnShowPatch.tacticalMap.Focus();
                        }
                        break;

                }
            }
        }
    }
}