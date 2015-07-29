using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EVAEnhancements
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class EVAEnhancementsBehaviour : MonoBehaviour
    {
        bool showDebug = false;
        public void Update()
        {
            // do stuff
            if (showDebug)
            {

            }

        }
        public void LateUpdate()
        {
            if (showDebug)
            {

            }
            // do stuff

        }
    }

    public class EVAEnhancements : PartModule
    {
        [KSPField(guiActive = true, guiName = "Profession", isPersistant = true)]
        public string kerbalProfession = null;

        [KSPField(guiName = "Jetpack", guiFormat = "P0", guiActive = true, isPersistant = true), UI_FloatRange(minValue = 0.01f, maxValue = 1f, stepIncrement = 0.01f)]
        public float jetPackPower = 1f;

        [KSPField(guiName = "AutoRotate", guiActive = true, isPersistant = true), UI_Toggle(disabledText = "No", enabledText = "Yes")]
        public bool rotateOnMove = false;

        public bool precisionControls = false;

        // Define settings file
        internal Settings settings = new Settings("EVAEnhancements.cfg");

        // Variables to keep track of original values
        internal float origLinPower = 0f;
        internal float origRotPower = 0f;
        internal float origPropConsumption = 0f;

        // Pointers to various objects
        internal KerbalEVA eva = null;
        internal GameObject navBall;
        internal NavBall ball;

        // Flags
        internal bool first = true;
        internal bool expandUI = true;

        public override void OnStart(PartModule.StartState state)
        {
            base.OnStart(state);

            // Load settings
            settings.Load();
            settings.Save();

            // Display profession and level
            ProtoCrewMember myKerbal = this.part.protoModuleCrew.SingleOrDefault();
            this.Fields["kerbalProfession"].guiName = myKerbal.experienceTrait.Title;
            kerbalProfession = "Level " + myKerbal.experienceLevel.ToString();

            // Configure the jetpack power slider
            UI_FloatRange fr = (UI_FloatRange)this.Fields["jetPackPower"].uiControlFlight;
            fr.minValue = settings.minJetPackPower;
            fr.maxValue = settings.maxJetPackPower;
            fr.stepIncrement = settings.stepJetPackPower;

            // Find the navball
            navBall = GameObject.Find("NavBall");
            ball = navBall.GetComponent<NavBall>();

        }

        public override void OnUpdate()
        {
            // Only run processing if the EVA'd Kerbal is the active vessel
            if (this.vessel == FlightGlobals.ActiveVessel)
            {
                // Toggle precision node
                // TODO: maybe replace these messages with a different indicator
                if (GameSettings.PRECISION_CTRL.GetKeyDown())
                {
                    precisionControls = !precisionControls;
                    if (precisionControls)
                    {
                        ScreenMessages.PostScreenMessage("Precision Controls: Enabled", 2f, ScreenMessageStyle.UPPER_CENTER);
                    }
                    else
                    {
                        ScreenMessages.PostScreenMessage("Precision Controls: Disabled", 2f, ScreenMessageStyle.UPPER_CENTER);
                    }
                }

                // Toggle Rotate on Move
                if (GameSettings.SAS_TOGGLE.GetKeyDown())
                {
                    rotateOnMove = !rotateOnMove;
                }

                // Set point to KerbalEVA
                if (eva == null)
                {
                    eva = FlightGlobals.ActiveVessel.GetComponent<KerbalEVA>();
                }


                if (eva.JetpackDeployed)
                {
                    GameSettings.EVA_ROTATE_ON_MOVE = rotateOnMove;

                    if (first)
                    {
                        origLinPower = eva.linPower;
                        origRotPower = eva.rotPower;
                        origPropConsumption = eva.PropellantConsumption;
                        first = false;
                    }

                    if (precisionControls)
                    {
                        eva.linPower = origLinPower * jetPackPower * settings.precisionFactor;
                        eva.rotPower = origRotPower * jetPackPower * settings.precisionFactor;
                        eva.PropellantConsumption = origPropConsumption * jetPackPower * settings.precisionFactor;
                    }
                    else
                    {
                        eva.linPower = origLinPower * jetPackPower;
                        eva.rotPower = origRotPower * jetPackPower;
                        eva.PropellantConsumption = origPropConsumption * jetPackPower;
                    }

                    if (Input.GetKey(settings.pitchDown))
                    {
                        EVAController.Instance.UpdateEVAFlightProperties(-1, 0, jetPackPower);

                    }
                    if (Input.GetKey(settings.pitchUp))
                    {
                        EVAController.Instance.UpdateEVAFlightProperties(1, 0, jetPackPower);
                    }
                    if (Input.GetKey(settings.rollLeft))
                    {
                        EVAController.Instance.UpdateEVAFlightProperties(0, -1, jetPackPower);
                    }
                    if (Input.GetKey(settings.rollRight))
                    {
                        EVAController.Instance.UpdateEVAFlightProperties(0, 1, jetPackPower);
                    }

                }

            }
            base.OnUpdate();
        }

        internal void LateUpdate()
        {
            if (this.vessel == FlightGlobals.ActiveVessel && eva != null)
            {
                // Change rotation offset so it points in the direction the Kerbal is facing
                ball.rotationOffset = new Vector3(0, 0, 0);

                if (expandUI)
                {
                    // Display navball
                    foreach (ScreenSafeUISlideTab tab in FlightEVA.fetch.EVACollapseGroups)
                    {
                        if (tab.name == "EVACollapse_navball")
                            tab.Expand();
                    }
                    expandUI = false;
                }


                if (eva.JetpackDeployed)
                {
                    // Set throttle to jetpack power
                    if (precisionControls)
                    {
                        FlightUIController.fetch.thr.setValue(jetPackPower * settings.precisionFactor);
                    }
                    else
                    {
                        FlightUIController.fetch.thr.setValue(jetPackPower);
                    }

                    // Turn on RCS light
                    FlightUIController.fetch.rcs.renderer.material.mainTexture = FlightUIController.fetch.rcs.ledColors[1];

                }
                else
                {
                    // Set throttle to 0 and turn off RCS light
                    FlightUIController.fetch.thr.setValue(0f);
                    FlightUIController.fetch.rcs.renderer.material.mainTexture = FlightUIController.fetch.rcs.ledColors[0];
                }

                // Toggle SAS light for "EVA Rotate on Move"
                if (GameSettings.EVA_ROTATE_ON_MOVE)
                {
                    FlightUIController.fetch.SAS.renderer.material.mainTexture = FlightUIController.fetch.SAS.ledColors[1];
                }
                else
                {
                    FlightUIController.fetch.SAS.renderer.material.mainTexture = FlightUIController.fetch.SAS.ledColors[0];
                }

            }

        }

    }
}
