using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EVAEnhancements
{
    public class EVAEnhancements : PartModule
    {

        [KSPField(guiActive = true, guiName = "Profession", isPersistant = true)]
        public string kerbalProfession = null;

        [KSPField(guiName = "Jetpack Power", guiFormat = "P0", guiActive = true, isPersistant = true), UI_FloatRange(minValue = 0.01f, maxValue = 1f, stepIncrement = 0.01f)]
        public float jetPackPower = 1f;

        internal float origLinPower = 0f;
        internal float origRotPower = 0f;
        internal float origPropConsumption = 0f;

        internal KerbalEVA eva = null;

        internal Settings settings = new Settings("EVAEnhancements.cfg");

        internal bool first = true;

        public override void OnStart(PartModule.StartState state)
        {
            base.OnStart(state);

            settings.Load();
            settings.Save();

            GameSettings.EVA_ROTATE_ON_MOVE = false;

            ProtoCrewMember myKerbal = this.part.protoModuleCrew.SingleOrDefault();
            this.Fields["kerbalProfession"].guiName = myKerbal.experienceTrait.Title;
            kerbalProfession = "Level " + myKerbal.experienceLevel.ToString();

            UI_FloatRange fr = (UI_FloatRange)this.Fields["jetPackPower"].uiControlFlight;
            fr.minValue = settings.minJetPackPower;
            fr.maxValue = settings.maxJetPackPower;
            fr.stepIncrement = settings.stepJetPackPower;

        }

        public override void OnUpdate()
        {

            if (this.vessel == FlightGlobals.ActiveVessel)
            {
                if (eva == null)
                {
                    eva = FlightGlobals.ActiveVessel.GetComponent<KerbalEVA>();
                }
                if (eva.JetpackDeployed)
                {
                    if (first)
                    {
                        origLinPower = eva.linPower;
                        origRotPower = eva.rotPower;
                        origPropConsumption = eva.PropellantConsumption;
                        first = false;
                    }

                    eva.linPower = origLinPower * jetPackPower;
                    eva.rotPower = origRotPower * jetPackPower;
                    eva.PropellantConsumption = origPropConsumption * jetPackPower;

                    if (Input.GetKey(settings.pitchDown))
                    {
                        EVAController.Instance.UpdateEVAFlightProperties(-1f, 0, 0);

                    }
                    if (Input.GetKey(settings.pitchUp))
                    {
                        EVAController.Instance.UpdateEVAFlightProperties(+1f, 0, 0);
                    }
                    if (Input.GetKey(settings.rollLeft))
                    {
                        EVAController.Instance.UpdateEVAFlightProperties(0, 0, -1f);
                    }
                    if (Input.GetKey(settings.rollRight))
                    {
                        EVAController.Instance.UpdateEVAFlightProperties(0, 0, +1f);
                    }
                }

            }

            base.OnUpdate();
        }

    }
}
