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
                case "SystemView":
                    //open system map with ctrl
                    //if (Utils.ControlPress)
                    //{
                    //    string systemName = Utils.Find<ComboBox>(__instance, "cboSystems", goUp: 0).Text;
                    //    MessageBox.Show($"system name {systemName}");
                    //    ComboBox tacticalSystemCombo = Utils.Find<ComboBox>(tacticalMap, "cboSystems", goUp: 0);
                    //    int index = tacticalSystemCombo.Items.IndexOf(systemName);
                    //    MessageBox.Show($"index {index}");
                    //    if (index >= 0)
                    //    {
                    //        tacticalSystemCombo.SelectedIndex = index;
                    //        __instance.Close();
                    //    }
                    //}
                    break;

            }
        }
    }
}