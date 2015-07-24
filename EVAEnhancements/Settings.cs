using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSPPluginFramework;

namespace EVAEnhancements
{
    public class Settings : ConfigNodeStorage
    {
        public Settings(String FilePath) : base(FilePath) { }

        [Persistent]
        public float minJetPackPower = 0.01f;

        [Persistent]
        public float maxJetPackPower = 1f;

        [Persistent]
        public float stepJetPackPower = 0.01f;

        [Persistent]
        public KeyCode pitchDown = KeyCode.Alpha2;

        [Persistent]
        public KeyCode pitchUp = KeyCode.X;

        [Persistent]
        public KeyCode rollLeft = KeyCode.Z;

        [Persistent]
        public KeyCode rollRight = KeyCode.C;

    }
}
