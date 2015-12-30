using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;

namespace Com.LuisPedroFonseca.ProCamera2D.Platformer
{
    public class ToggleCinematics : MonoBehaviour
    {
        public ProCamera2DCinematics Cinematics;

        void OnGUI()
        {
            if (GUI.Button(new Rect(5, 5, 180, 30), (Cinematics.IsActive ? "Stop" : "Start") + " Cinematics"))
            {
                if (Cinematics.IsActive)
                    Cinematics.Stop();
                else
                    Cinematics.Play();
            }
        }
    }
}