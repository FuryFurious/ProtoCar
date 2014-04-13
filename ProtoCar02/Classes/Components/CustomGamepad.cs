using SharpDX;
using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoCar
{
    /// <summary>
    /// Extends Controller to add some higher level of usability.
    /// </summary>
    class CustomGamepad : Controller
    {

        State oldState;
        State currentState;

        public CustomGamepad(UserIndex index) 
            : base(index)
        {
            
        }

        public void update()
        {
            oldState = currentState;
            currentState = this.GetState();
        }

        /// <summary>
        /// Indicates if the given flag is clicked right now (pressed this frame and not pressed last frame). Pass multiple flags with "Flag1 | Flag2".
        /// </summary>
        /// <param name="flag">Which flag to be tested. Note: you can pass multiple flags with: "Flag1 | Flag2" </param>
        /// <returns>Returns if the given flag is clicked.</returns>
        public bool isClicked(GamepadButtonFlags flag)
        {
            return !oldState.Gamepad.Buttons.HasFlag(flag) && currentState.Gamepad.Buttons.HasFlag(flag);
        }

        /// <summary>
        /// Indicates if the given flag is pressed right now (pressed this frame). Pass multiple flags with "Flag1 | Flag2".
        /// </summary>
        /// <param name="flag">Which flag to be tested. Note: you can pass multiple flags with: "Flag1 | Flag2"</param>
        /// <returns>Returns if the given flag is pressed.</returns>
        public bool isPressed(GamepadButtonFlags flag)
        {
            return currentState.Gamepad.Buttons.HasFlag(flag);
        }

        /// <summary>
        /// Indicates if the given flag is released right now (pressed last frame and not pressed last frame). Pass multiple flags with "Flag1 | Flag2".
        /// </summary>
        /// <param name="flag">Which flag to be tested. Note: you can pass multiple flags with: "Flag1 | Flag2"</param>
        /// <returns>Returns if the given flag is released.</returns>
        public bool isReleased(GamepadButtonFlags flag)
        {
            return oldState.Gamepad.Buttons.HasFlag(flag) && !currentState.Gamepad.Buttons.HasFlag(flag);
        }

        /// <summary>
        /// LeftPad mapped to [-1.0f, 1.0f] for both X- and Y-direction.
        /// </summary>
        /// <returns></returns>
        public Vector2 leftPad()
        {
            float x = (float)currentState.Gamepad.LeftThumbX / (float)short.MaxValue;
            float y = (float)currentState.Gamepad.LeftThumbY / (float)short.MaxValue;

            //Because if nothing is pressed, there is still a small direction:
            //TODO: maybe better checking:
            if (Math.Abs(x) < 0.008f)
                x = 0;

            if (Math.Abs(y) < 0.008f)
                y = 0;

         
            //negative numbers are a bit smaller than -1.0f so clamp them there.... no need for positiv numbers
            //happens because raw value varies from [-2^15, (2^15)-1] 
            if (y < -1.0f)
                y = -1;

            if (x < -1.0f)
                x = -1;

            return new Vector2(x, y); 
                                
        }

        public Vector2 rightPad()
        {
            float x = (float)currentState.Gamepad.RightThumbX / (float)short.MaxValue;
            float y = (float)currentState.Gamepad.RightThumbY / (float)short.MaxValue;

            //Because if nothing is pressed, there is still a small direction:
            //TODO: maybe better checking:
            if (Math.Abs(x) < 0.008f)
                x = 0;

            if (Math.Abs(y) < 0.008f)
                y = 0;

            if (y < -1.0f)
                y = -1;

            if (x < -1.0f)
                x = -1;

            return new Vector2(x, y);

        }

 

       

        

       
    }
}
