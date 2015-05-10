using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;

    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D character;
        public bool jump;
		private bool crouched;
		private bool shoot;

        private void Awake()
        {
            character = GetComponent<PlatformerCharacter2D>();
        }

        private void Update()
        {
            if (!jump) {
			if (CrossPlatformInputManager.GetButtonDown ("Jump")) {
				jump = true;
			} else {
				if (Input.GetButtonDown ("Jump"))
					jump = true;
				}
			}
			if (!crouched) {
			if(CrossPlatformInputManager.GetButtonDown("Down"))
			{
				crouched = true;
			} else {
				if (Input.GetButtonDown("Down")) {
				   
				    crouched = true;
					}
				}
			}
		}

        private void FixedUpdate()
        {
            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            // Pass all parameters to the character control script.
            jump = false;
        }
    }
