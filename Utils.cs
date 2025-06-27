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
    class Utils
    {
        public static T Find<T>(Control ctrl, string name, int goUp = 1) where T : Control
        {
            if (goUp > 0)
            {
                for (int i=0; i<goUp; i++)
                    if (ctrl.Parent != null)
                        ctrl = ctrl.Parent;
                goUp = 0;
            }
            Control[] matches = ctrl.Controls.Find(name, true);
            if (matches.Length > 0)
            {
                if (goUp != 0)
                    MessageBox.Show($"goUp wrong name={name} goUp:{goUp}");
                if (matches[0] is T typedControl)
                    return typedControl;
                return null;
            }
            if (ctrl.Parent != null)
                return Find<T>(ctrl.Parent, name, goUp-1);
            Debug.Assert(false, $"couldnt find control {typeof(T)} {name}");
            return null;
        }

        private readonly static MethodInfo DoubleClickMethod = typeof(Control).GetMethod("OnDoubleClick", BindingFlags.Instance | BindingFlags.NonPublic);
        public static void PerformDoubleClick(Control control)
        {
            DoubleClickMethod.Invoke(control, new object[] { EventArgs.Empty });
        }

        static private bool BlockPatch = false;
        static public void RepeatDoubleClick(Control control, int repeat, Func<bool> breakCondition = null)
        {
            if (BlockPatch)
                return;
            BlockPatch = true;
            if (repeat == 0) {
                if (ShiftPress)
                    repeat = 99;
                else if (ControlPress)
                    repeat = 9;
            }
            for (int i = 0; i < repeat; i++)
            {
                if (breakCondition != null && breakCondition())
                    break;
                Utils.PerformDoubleClick(control);
            }
            BlockPatch = false;
        }

        public static bool ShiftPress => (Control.ModifierKeys == Keys.Shift);
        public static bool ControlPress => (Control.ModifierKeys == Keys.Control);
        public static bool AltPress => (Control.ModifierKeys == Keys.Alt);
    }
}