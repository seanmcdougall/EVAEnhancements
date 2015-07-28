/* Adapted from https://github.com/AlexanderDzhoganov/ksp-advanced-flybywire/blob/master/EVAController.cs under terms of MIT license
 * Original Copyright (c) 2014 Alexander Dzhoganov
 * Modifications Copyright (c) 2015 Sean McDougall
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace EVAEnhancements
{
    class EVAController
    {

        private const float EVARotationStep = 30f;
        private List<FieldInfo> vectorFields;
        private static EVAController instance;

        public static EVAController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EVAController();
                }
                return instance;
            }
        }

        public EVAController()
        {
            LoadReflectionFields();
        }

        public void UpdateEVAFlightProperties(float pitch, float roll, float power)
        {
            KerbalEVA eva = FlightGlobals.ActiveVessel.GetComponent<KerbalEVA>();
            if (!FlightGlobals.ActiveVessel.Landed && eva.JetpackDeployed)
            {
                Quaternion rotation = Quaternion.identity;
                rotation *= Quaternion.AngleAxis(eva.turnRate * pitch * EVARotationStep * Time.deltaTime * power, -eva.transform.right);
                rotation *= Quaternion.AngleAxis(0, eva.transform.up);
                rotation *= Quaternion.AngleAxis(eva.turnRate * roll * EVARotationStep * Time.deltaTime * power, -eva.transform.forward);
                
                if (rotation != Quaternion.identity)
                {
                    this.vectorFields[8].SetValue(eva, rotation * (Vector3)this.vectorFields[8].GetValue(eva));
                    this.vectorFields[13].SetValue(eva, rotation * (Vector3)this.vectorFields[13].GetValue(eva));
                }
            }
        }


        // PRIVATE METHODS //

        private void LoadReflectionFields()
        {
            List<FieldInfo> fields = new List<FieldInfo>(typeof(KerbalEVA).GetFields(
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance));
            this.vectorFields = new List<FieldInfo>(fields.Where<FieldInfo>(f => f.FieldType.Equals(typeof(Vector3))));
        }
    }
}
