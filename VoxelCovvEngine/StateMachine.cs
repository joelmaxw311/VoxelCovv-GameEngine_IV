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
using System.Threading;

namespace VoxelCovvEngine
{
    public class StateMachine : DrawableGameComponent
    {
        public SpriteBatch spriteBatch;
        public ContentManager Content;
        InputListener input;
        List<State> stateStack = new List<State>();
        State nextState;
        
        public StateMachine(Game game)
        : base(game)
        {
            Content = game.Content;
            input = new InputListener(this);
        }

        public override void Initialize()
        {
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (nextState != null)
            {
                CurrentState = nextState;
                CurrentState.Load();
                nextState = null;
            }
            input.Update(gameTime);
            for (int i = 0; i < stateStack.Count; i++)
                stateStack[i].Update();
            base.Update(gameTime);
        }

        protected override void UnloadContent()
        {
            for (int i = 0; i < stateStack.Count; i++)
            {
                stateStack[i].Unload();
            }
            base.UnloadContent();
        }

        public void EventCalled(string eventName, object[] args)
        {
            if (stateStack.Count > 0)
            {
                CurrentState.EventCalled(eventName, args);
                VCHelper.CallEvent(CurrentState, eventName, args);
                VCHelper.CallEvent(Game, eventName, args);
            }
        }

        State CurrentState
        {
            get
            {
                if (stateStack.Count == 0)
                    return null;
                return stateStack[stateStack.Count - 1];
            }
            set
            {
                State current = CurrentState;
                if (current != null)
                    current.Unload();
                stateStack.Add(value);
            }
        }

        public void ChangeState(State state)
        {
            for (int i = 0; i < stateStack.Count; i++)
            {
                stateStack[i].Unload();
            }
            stateStack.Clear();
            nextState = state;
        }

        public void AddState(State state)
        {
            nextState = state;
        }

        public override void Draw(GameTime gameTime)
        {
            for (int i = 0; i < stateStack.Count; i++)
            {
                stateStack[i].Draw(spriteBatch);
            }
            base.Draw(gameTime);
            Thread.Sleep(2);
        }
    }
}
