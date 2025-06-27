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
    class ListViewClickPatch
    {
        public static void Prefix(Control __instance, EventArgs e)
        {
            if (__instance is ListView lv)
                switch (__instance.Name) {
                    case "lstvLoadItems":
                        loadItemsClickPrefix(lv, e);
                        break;
                    default:
                        break;
                }
        }

        public static void loadItemsClickPrefix(ListView __instance, EventArgs e)
        {
            TextBox tb = Utils.Find<TextBox>(__instance, "txtMaxItems", goUp: 1);
            if (Control.ModifierKeys == Keys.Alt)
            {
                tb.Text = "1";
            }
            else if (Control.ModifierKeys == Keys.Control)
            {
                tb.Text = "10";
            }
            else
                tb.Text = "0";
        }
    }

    [HarmonyPatch(typeof(Control))]
    [HarmonyPatch("OnMouseDoubleClick")]
    class ListViewDoubleClickPatch
    {
        public static void Prefix(Control __instance, EventArgs e)
        {
            if (__instance is ListView lv)
                switch (__instance.Name)
                {
                    case "lstvOrders":
                        ordersDoubleClickPrefix(lv, e);
                        break;
                    case "lstvTechnology":
                        techDoubleClickPrefix(lv, e);
                        break;
                    case "lstvResearchProjects":
                        if (Utils.ShiftPress)
                        {
                            Utils.Find<Button>(__instance, "cmdPauseResearch").PerformClick();
                        }
                        else
                        {
                            Button delButton = Utils.Find<Button>(__instance, "cmdDeleteProject");
                            if (delButton.Visible)
                                delButton.PerformClick();
                            else
                            {
                                Utils.Find<Button>(__instance, "cmdRemoveQueue").PerformClick();
                            }
                        }
                        break;
                    case "lstvGroundUnitTraining":
                        if (Utils.ShiftPress)
                            MessageBoxPatch.DisableOneBox();
                        Utils.Find<Button>(__instance, "cmdDeleteGUTask").PerformClick();
                        break;
                    case "lstvInstallations":
                        //add supply
                        if (lv.SelectedIndices.Count > 0) {
                            Utils.Find<ComboBox>(__instance, "cboSupply", goUp: 2).SelectedIndex = lv.SelectedIndices[0] - 2;
                            Utils.Find<Button>(__instance, "cmdSupply", goUp: 2).PerformClick();
                            string name = lv.SelectedItems[0].Text.Split('(')[0].Trim();
                            ComboBox demandBox = Utils.Find<ComboBox>(__instance, "cboDemand", goUp: 2);
                            demandBox.SelectedIndex = demandBox.FindString(name);
                        }
                        break;
                    case "lstvShipyardTasks":
                        Utils.Find<Button>(__instance, "cmdDeleteTask").PerformClick();
                        break;
                    case "lstvSupply":
                        //edit supply
                        Utils.Find<Button>(__instance, "cmdEditSupply", goUp: 2).PerformClick();
                        break;
                    case "lstvDemand":
                        //edit demand
                        Utils.Find<Button>(__instance, "cmdEditDemand", goUp: 2).PerformClick();
                        break;
                    default:
                        break;
                }
        }

        public static void Postfix(Control __instance, EventArgs e)
        {
            if (__instance is ListView lv)
                switch (__instance.Name)
                {
                    case "lstvOrnance":
                        Utils.RepeatDoubleClick(lv, 0);
                        break;
                }
        }

        public static void ordersDoubleClickPrefix(ListView __instance, EventArgs e)
        {
            if (Utils.ShiftPress)
                Utils.Find<Button>(__instance, "cmdRemoveAll", goUp: 1).PerformClick();
            else
                Utils.Find<Button>(__instance, "cmdRemoveLastOrder", goUp: 1).PerformClick();
        }

        public static void techDoubleClickPrefix(ListView __instance, EventArgs e)
        {
            //choose first scientist if nothing chosen
            ListView scientists = Utils.Find<ListView>(__instance, "lstvScientists");
            if ((scientists.SelectedItems.Count == 0) && (scientists.Items.Count > 2))
                scientists.SelectedIndices.Add(2);

            if (scientists.SelectedIndices.Count == 0)
                return;
            int maxLabs = int.Parse(scientists.SelectedItems[0].SubItems[3].Text);

            if (Utils.AltPress)
            {
                Utils.Find<TextBox>(__instance, "txtAssignFacilities", goUp: 1).Text = "1";
                
            }
            else if (Utils.ControlPress)
            {
                Utils.Find<TextBox>(__instance, "txtAssignFacilities", goUp: 1).Text = "10";
            }
            else if (Utils.ShiftPress)
            {
                int availableLabs = int.Parse(Utils.Find<Label>(__instance, "lblRFAvailable", goUp: 1).Text);
                Utils.Find<TextBox>(__instance, "txtAssignFacilities", goUp: 1).Text = (maxLabs < availableLabs ? maxLabs : availableLabs).ToString();
            }  
            Utils.Find<Button>(__instance, "cmdCreateResearch", goUp: 1).PerformClick();
        }
    }
}