using System;
using System.Linq;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;

using HarmonyLib;
using AuroraPatch;
using Lib;
using System.Data;
using System.Diagnostics;


namespace BetterAuroraUI
{
    [HarmonyPatch(typeof(MessageBox), nameof(MessageBox.Show), new Type[] { typeof(string), typeof(string), typeof(MessageBoxButtons) })]
    class MessageBoxPatch
    {
        private static bool disableOne = false;
        private static DialogResult todoResult = DialogResult.Yes;

        public static bool Prefix(string text, string caption, MessageBoxButtons buttons, ref DialogResult __result)
        {
            if (disableOne)
            {
                __result = todoResult;
                disableOne = false;
                return false; // Skip original method
            }
            return true; // Run original method
        }

        public static void DisableOneBox(DialogResult result = DialogResult.Yes)
        {
            disableOne = true;
            todoResult = result;
        }
    }
}