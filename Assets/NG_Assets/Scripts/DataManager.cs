using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NG.Data
{

    public class DataManager : MonoBehaviour
    {
        public static string UNIT_POSITION_KEY = "UnitPosition";

        public static Vector3 GetUnitPosition(string unitID, Vector3 defaultPosition)
        {
            Vector3 position = Vector3.zero;

            position.x = PlayerPrefs.GetFloat(UNIT_POSITION_KEY + "_X_" + unitID, defaultPosition.x);
            position.y = PlayerPrefs.GetFloat(UNIT_POSITION_KEY + "_Y_" + unitID, defaultPosition.y);
            position.z = PlayerPrefs.GetFloat(UNIT_POSITION_KEY + "_Z_" + unitID, defaultPosition.z);

            return position;
        }

        public static void SetUnitPosition(string unitID, Vector3 position)
        {
            PlayerPrefs.SetFloat(UNIT_POSITION_KEY + "_X_" + unitID, position.x);
            PlayerPrefs.SetFloat(UNIT_POSITION_KEY + "_Y_" + unitID, position.y);
            PlayerPrefs.SetFloat(UNIT_POSITION_KEY + "_Z_" + unitID, position.z);
        }
    }
}
