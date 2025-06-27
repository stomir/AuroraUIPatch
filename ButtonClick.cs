using System;
using System.Linq;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;

using HarmonyLib;
using Lib;
using System.Data;
using System.ComponentModel;
using System.Diagnostics;

namespace BetterAuroraUI
{
    [HarmonyPatch(typeof(Button))]
    [HarmonyPatch("OnClick")]
    class ButtonClickPatch
    {
        private static HashSet<string> repeatingButtons = new HashSet<string> { "cmdAddLab", "cmdRemoveLab" };
        private static bool disablePatches = false;
        public static bool Prefix(Button __instance, EventArgs e)
        {
            if (disablePatches)
                return true;
            switch (__instance.Name)
            {
                case "cmdCreateResearch":
                case "cmdAddToQueue":
                    return saveResearchName(__instance, e);
            }
            return true;
        }

        public static void Postfix(Button __instance, EventArgs e)
        {
            if (disablePatches)
                return;
            switch (__instance.Name)
            {
                case "cmdCreateResearch":
                    createResearchClickPostfix(__instance, e);
                    break;
                case "cmdAddToQueue":
                    addResearchToQueuePostfix(__instance, e);
                    break;
                case "cmdDisassembleAll":
                    if (Utils.ShiftPress) {
                        disablePatches = true;
                        ListView comps = Utils.Find<ListView>(__instance, "lstvPopComponents", goUp: 2);
                        while (comps.Items.Count > 0) {
                            comps.SelectedIndices.Add(0);
                            __instance.PerformClick();
                        }
                        disablePatches = false;
                    }
                    break;
                case "cmdScrapComponent":
                    if (Utils.ShiftPress)
                    {
                        disablePatches = true;
                        ListView comps = Utils.Find<ListView>(__instance, "lstvPopComponents", goUp: 2);
                        while (comps.Items.Count > 0)
                        {
                            comps.SelectedIndices.Add(0);
                            __instance.PerformClick();
                        }
                        disablePatches = false;
                    }
                    break;
            }
            if (repeatingButtons.Contains(__instance.Name))
                repeatButton(__instance, e);
        }

        public static void repeatButton(Button __instance, EventArgs e)
        {
            if (Utils.ControlPress)
            {
                disablePatches = true;
                for (int i = 0; i < 9; i++)
                {
                    if (!__instance.Enabled)
                        break;
                    __instance.PerformClick();
                }
                disablePatches = false;
            }
            else if (Utils.ShiftPress)
            {
                disablePatches = true;
                for (int i = 0; i < 99; i++)
                {
                    if (!__instance.Enabled)
                        break;
                    __instance.PerformClick();
                }
                disablePatches = false;
            }
        }

        private static string researchName;
        public static bool saveResearchName(Button __instance, EventArgs e)
        {
            ListView lv = Utils.Find<ListView>(__instance, "lstvTechnology", goUp: 2);
            if (lv.SelectedItems.Count > 0)
                researchName = lv.SelectedItems[0].Text;
            else
                researchName = "";
            return true;
        }
        public static void createResearchClickPostfix(Button __instance, EventArgs e)
        {
            if (Utils.ShiftPress)
            {
                //check assign new
                ListView res = Utils.Find<ListView>(__instance, "lstvResearchProjects", goUp: 2);
                foreach (ListViewItem item in res.Items)
                {

                    if (item.SubItems[1].Text == researchName)
                    {
                        res.SelectedIndices.Clear();
                        res.SelectedIndices.Add(item.Index);
                        break;
                    }
                }
                Utils.Find<Button>(__instance, "cmdAssignNew", goUp: 2).PerformClick();
            }
        }

        private static void selectResearchByName(ListView res, string name, string category = "", ComboBox categoryCombo = null)
        {
            if (name.EndsWith(" (N)"))
                name = name.Substring(0, name.Length - 4);
            if (category != "")
            {
                //if (categoryCombo == null)
                    categoryCombo = Utils.Find<ComboBox>(res, "cboResearchFields");
                categoryCombo.SelectedIndex = (categoryCombo.SelectedIndex == 0) ? 1 : 0;
                switch (category)
                {
                    case "BG": categoryCombo.SelectedIndex = 0; break;
                    case "CP": categoryCombo.SelectedIndex = 1; break;
                    case "DS": categoryCombo.SelectedIndex = 2; break;
                    case "EW": categoryCombo.SelectedIndex = 3; break;
                    case "GC": categoryCombo.SelectedIndex = 4; break;
                    case "LG": categoryCombo.SelectedIndex = 5; break;
                    case "MK": categoryCombo.SelectedIndex = 6; break;
                    case "PP": categoryCombo.SelectedIndex = 7; break;
                    case "SC": categoryCombo.SelectedIndex = 8; break;
                    default: Debug.Assert(false, $"wrong category {category}"); break;
                }
            }
            res.SelectedIndices.Clear();
            res.SelectedIndices.Add(res.Items.Cast<ListViewItem>().First(item => item.Text == name).Index);
        }

        public static void addResearchToQueuePostfix(Button __instance, EventArgs e)
        {
            if (Utils.ControlPress)
            {
            }
        }
    }
}