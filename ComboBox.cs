using System;
using System.Linq;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;

using HarmonyLib;
using Lib;
using System.Data;
using System.Threading;
using System.Drawing.Text;

namespace BetterAuroraUI
{

    [HarmonyPatch(typeof(ComboBox))]
    [HarmonyPatch("OnTextChanged")]
    class ComboBoxChangedPatch
    {
        static private bool disablePatch = false;
        static private DateTime lastPress;

        static private System.Threading.Timer tacticalMapTimer;
        static public ComboBox SystemNameCombo;
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
                            SystemNameCombo = __instance;
                            break;
                    }
                    break;
            }
        }
    }
}