using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Reflection;


namespace VoxelCovvEngine
{
    /// <summary>
    /// Calls events when keyboard and mouse data is updated.
    /// </summary>
    public class InputListener
    {
        public StateMachine receiver;
        KeyboardState ckb, pkb;
        MouseState cmouse, pmouse;

        public InputListener(StateMachine receiver)
        {
            this.receiver = receiver;
            ckb = Keyboard.GetState();
            cmouse = Mouse.GetState();
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Update mouse and keyboard states and call events
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            pkb = ckb;
            pmouse = cmouse;
            ckb = Keyboard.GetState();
            cmouse = Mouse.GetState();

            CallKeyEvents();
            CallMouseEvents();
        }

        /// <summary>
        /// Calls keyboard events
        /// </summary>
        void CallKeyEvents()
        {
            List<Keys> pkeys = pkb.GetPressedKeys().ToList();
            Keys[] ckeys = ckb.GetPressedKeys();
            foreach (Keys k in ckeys)
            {
                if (!pkeys.Contains(k))
                {
                    CallEvent("KEYBOARD_PRESSED_" + k.ToString(), null);
                    CallEvent("KEYBOARD_PRESSED", k);
                }
                pkeys.Remove(k);
                CallEvent("KEYBOARD_DOWN_" + k.ToString(), null);
                CallEvent("KEYBOARD_DOWN", k);
            }
            foreach (Keys k in pkeys)
            {
                if (!ckeys.Contains(k))
                {
                    CallEvent("KEYBOARD_RELEASED_" + k.ToString(), null);
                    CallEvent("KEYBOARD_RELEASED", k);
                }
            }
        }

        /// <summary>
        /// Calls mouse events
        /// </summary>
        void CallMouseEvents()
        {
            Vector2 pmousepos = new Vector2(pmouse.X, pmouse.Y);
            Vector2 mousepos = new Vector2(cmouse.X, cmouse.Y);
            if (cmouse.LeftButton == ButtonState.Pressed && pmouse.LeftButton == ButtonState.Released)
                CallEvent("MOUSE_PRESSED_LEFT", mousepos);
            if (pmouse.LeftButton == ButtonState.Pressed && cmouse.LeftButton == ButtonState.Released)
                CallEvent("MOUSE_RELEASED_LEFT", mousepos);
            if (cmouse.LeftButton == ButtonState.Pressed)
                CallEvent("MOUSE_DOWN_LEFT", mousepos);

            if (cmouse.RightButton == ButtonState.Pressed && pmouse.RightButton == ButtonState.Released)
                CallEvent("MOUSE_PRESSED_RIGHT", mousepos);
            if (pmouse.RightButton == ButtonState.Pressed && cmouse.RightButton == ButtonState.Released)
                CallEvent("MOUSE_RELEASED_RIGHT", mousepos);
            if (cmouse.RightButton == ButtonState.Pressed)
                CallEvent("MOUSE_DOWN_RIGHT", mousepos);

            if (cmouse.MiddleButton == ButtonState.Pressed && pmouse.MiddleButton == ButtonState.Released)
                CallEvent("MOUSE_PRESSED_MIDDLE", mousepos);
            if (pmouse.MiddleButton == ButtonState.Pressed && cmouse.MiddleButton == ButtonState.Released)
                CallEvent("MOUSE_RELEASED_MIDDLE", mousepos);
            if (cmouse.RightButton == ButtonState.Pressed)
                CallEvent("MOUSE_DOWN_RIGHT", mousepos);
            if (mousepos != pmousepos)
                CallEvent("MOUSE_MOVED", new object[] { mousepos, pmousepos });

            if (cmouse.ScrollWheelValue > pmouse.ScrollWheelValue)
                CallEvent("MOUSE_SCROLL_UP", cmouse.ScrollWheelValue - pmouse.ScrollWheelValue);
            if (cmouse.ScrollWheelValue < pmouse.ScrollWheelValue)
                CallEvent("MOUSE_SCROLL_DOWN", cmouse.ScrollWheelValue - pmouse.ScrollWheelValue);
            if (cmouse.ScrollWheelValue != pmouse.ScrollWheelValue)
                CallEvent("MOUSE_SCROLL", cmouse.ScrollWheelValue - pmouse.ScrollWheelValue);
        }

        void CallEvent(string methodName, object arg)
        {
            object[] args = null;
            if (arg != null)
                args = new object[] { arg };
            receiver.EventCalled(methodName, args);

            MethodInfo method = receiver.GetType().GetMethod(methodName);
            try
            {
                if (method != null)
                {
                    method.Invoke(receiver, args);
                }
            }
            catch (System.Reflection.TargetParameterCountException ex)
            {
                Console.WriteLine("Event " + methodName + " in " + receiver.GetType().ToString() + ", has an invalid number of parameters.");
                string correct = "(Should be " + methodName + "(" + arg.GetType().ToString() + ")";
                Console.WriteLine(correct);
                Console.WriteLine();
            }
        }

        void CallEvent(string methodName, object[] args)
        {
            receiver.EventCalled(methodName, args);
            MethodInfo method = receiver.GetType().GetMethod(methodName);
            try
            {
                if (method != null)
                {
                    method.Invoke(receiver, args);
                }
            }
            catch (System.Reflection.TargetParameterCountException ex)
            {
                Console.WriteLine("Event " + methodName + " in " + receiver.GetType().ToString() + ", has an invalid number of parameters.");
                string correct = "(Should be " + methodName + "(" + args.GetType().ToString() + ")";
                Console.WriteLine(correct);
                Console.WriteLine();
            }
        }
    }
}
