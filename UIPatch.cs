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

namespace BetterAuroraUI
{
    public class UIPatch : AuroraPatch.Patch
    {
        public static Color BackColor { get; set; } = Color.Black;

        public override string Description => "Some UI improvements.";
        public override IEnumerable<string> Dependencies => new[] { "Lib" };
        
        protected override void Loaded(Harmony harmony)
        {
            // get the exe and its checksum
            var exe = AuroraExecutablePath;
            var checksum = AuroraChecksum;

            harmony.PatchAll();
        }

    }

}
