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

namespace VoxelCovvEngine
{
    /// <summary>
    /// A camera for drawing and aiming a 3d perspective
    /// </summary>
    public class Camera
    {
        /// <summary>
        /// Where the camera is looking from
        /// </summary>
        public Vector3 position;
        /// <summary>
        /// The yaw and pitch of the camera
        /// </summary>
        public Vector2 rotation;
        /// <summary>
        /// The camera's velocity
        /// </summary>
        Vector3 movement;
        /// <summary>
        /// A reference to the world the camera is viewing
        /// </summary>
        VoxelWorld world;
        /// <summary>
        /// The vertical velocity
        /// </summary>
        float gravity;
        /// <summary>
        /// Whether or not flight is enabled
        /// </summary>
        bool flight = false;
        /// <summary>
        /// The walking movement velocity constant
        /// </summary>
        const float walkSpeed = 0.0625f;
        /// <summary>
        /// The collision box surrounding the camera
        /// </summary>
        CollisionBox cBox = new CollisionBox(new Vector3(-0.3f, -1.25f, -0.3f), new Vector3(0.3f, 0.25f, 0.3f));
        /// <summary>
        /// Whether or not the camera can move
        /// </summary>
        bool mobile = true;
        /// <summary>
        /// Whether or not the camera has moved
        /// </summary>
        public bool moved = false;
        /// <summary>
        /// Camera bob animation
        /// </summary>
        Fader cameraBob = new Fader(16f);
        /// <summary>
        /// Whether or not the camera should capture the mouse
        /// </summary>
        bool mouseBound = true;
        /// <summary>
        /// First or third person view
        /// </summary>
        bool firstPerson = true;
        /// <summary>
        /// The translation from the origin (camera locaton) to draw the view from
        /// </summary>
        Vector3 offset = Vector3.Zero;

        /// <summary>
        /// A reference to the game object
        /// </summary>
        Game game;

        /// <summary>
        /// A new camera
        /// </summary>
        /// <param name="position">The location of the camera in the world</param>
        /// <param name="game">A reference to the game object</param>
        public Camera(Vector3 position, Game game)
        {
            this.position = position;
            this.rotation = Vector2.Zero;
            this.game = game;
        }

        /// <summary>
        /// A new camera object
        /// </summary>
        /// <param name="position">The location of the camera in the world</param>
        /// <param name="state">A reference to the game state running this camera</param>
        /// <param name="mobile">Whether or not this camera can move</param>
        public Camera(Vector3 position, State state, bool mobile)
        {
            this.position = position;
            this.rotation = Vector2.Zero;
            this.game = state.game;
            this.world = state.world;
            this.mobile = mobile;
        }

        /// <summary>
        /// Update the camera
        /// </summary>
        /// <param name="state">The game state running this camera</param>
        public void Update(State state)
        {
            if (mobile)//Can move
            {
                if (!flight)//Cannot fly
                    ApplyMovement(state);//User-controlled movement
                else//Can fly
                    position += movement;//Ignore collisions and move

                if (movement != Vector3.Zero)//Moved
                    cameraBob.Start(true, true);//Bob the camera
                else //Did not move
                    cameraBob.Stop();//Stop bobbing the camera
            }

            moved = (movement != Vector3.Zero);//Moved?

            this.movement = Vector3.Zero;//Reset movement for next loop

            if (mouseBound)//Capture/Lock mouse
            {
                MouseState mouse = Mouse.GetState();//Current mouse input information
                rotation += new Vector2(mouse.X - game.Window.ClientBounds.Width / 2, 
                    (mouse.Y - game.Window.ClientBounds.Height / 2)) * 0.1f;//Rotate camera angle based on mouse movement
                rotation.Y = Math.Max(Math.Min(rotation.Y, 85), -85);//Limit vertical look angle
                
                //Wrap horizontal look angle
                if (rotation.X < 0)
                    rotation.X += 360;
                if (rotation.X >= 360)
                    rotation.X -= 360;

                Mouse.SetPosition(game.Window.ClientBounds.Width / 2, game.Window.ClientBounds.Height / 2);//Capture the mouse
            }

            cameraBob.Fade(true);//Advance bob animation

        }

        void ApplyMovement(State state)
        {
            this.world = state.world;//Assign state world to a variable

            movement.Y += gravity;//Vertical motion
            bool movedZ = false, movedX = false;//Create variables for second movement attempt to allow sliding allong walls
            if (!cBox.CheckCollision(world, position + new Vector3(0, 0, movement.Z))) //No obstructions
            {
                position.Z += movement.Z;//Move along z axis
                movedZ = true;//Z movement has been applied
            }
            if (!cBox.CheckCollision(world, position + new Vector3(movement.X, 0, 0)))//No obstructions
            {
                position.X += movement.X;//Move along x axis
                movedX = true;//X movement has been applied
            }
            if (!cBox.CheckCollision(world, position + new Vector3(0, 0, movement.Z)) && //No obstructions
                !movedZ) //Has not applied z movement
                position.Z += movement.Z;//Second Z movement attempt
            if (!cBox.CheckCollision(world, position + new Vector3(movement.X, 0, 0)) && //No obstructions 
                !movedX) //Has not applied X movement 
                position.X += movement.X;//Second X movement attempt
            if (!cBox.CheckCollision(world, position + new Vector3(0, movement.Y, 0)))
                position.Y += movement.Y;

            if (cBox.CheckCollision(world, position))
                position.Y += 0.25f;//Move up if inside a block
            if (cBox.CheckCollision(world, position + new Vector3(0, gravity, 0)))
            {
                gravity = 0;//Stop falling
            }
            if (!cBox.CheckCollision(world, position + new Vector3(0, -0.04f, 0)))
                gravity -= 0.01f;//Fall

        }

        /// <summary>
        /// The offset of the view position from the origin of rotation
        /// </summary>
        public Vector3 ViewOffset
        {
            get
            {
                Vector3 look = new Vector3(Angle.X, 0, Angle.Z);
                look.Normalize();
                look *= 0.15f;
                return look;
            }
        }

        /// <summary>
        /// Walk
        /// </summary>
        /// <param name="move">Direction to walk</param>
        public void Move(Vector3 move)
        {
            this.movement = (movement + (move * walkSpeed) ) / 2f;//Midpoint (Average)
            movement.Normalize();
            movement *= walkSpeed;//Set distance/speed
        }

        /// <summary>
        /// Toggle perspective
        /// </summary>
        public void KEYBOARD_PRESSED_F5()
        {
            if (firstPerson)
                offset = new Vector3(1f, -0.15f, -1f);
            else
                offset = new Vector3(0, 0, 0);
            firstPerson = !firstPerson;
        }

        /// <summary>
        /// Walk forward
        /// </summary>
        public void KEYBOARD_DOWN_W()
        {
            Vector3 movement = new Vector3(Angle.X, 0, Angle.Z);
            movement.Normalize();
            Move(movement);
        }

        /// <summary>
        /// Walk backward
        /// </summary>
        public void KEYBOARD_DOWN_S()
        {
            Vector3 movement = -new Vector3(Angle.X, 0, Angle.Z);
            movement.Normalize();
            Move(movement);
        }

        /// <summary>
        /// Fly up
        /// </summary>
        public void KEYBOARD_DOWN_Space()
        {
            if (flight) //Can fly
                this.position += new Vector3(0, 1 / 16f, 0);//Move up
        }

        /// <summary>
        /// Jump
        /// </summary>
        public void KEYBOARD_PRESSED_Space()
        {
            if (!flight)
            {
                //Jump
                if (cBox.CheckCollision(world, position + new Vector3(0, -0.04f, 0)))//On ground
                    gravity += 0.15f;//Reverse gravity (not permanently)
            }
        }

        /// <summary>
        /// Fly down
        /// </summary>
        public void KEYBOARD_DOWN_LeftShift()
        {
            if (flight) //Fly down
                this.position -= new Vector3(0, 1 / 16f, 0);//Move down
        }

        /// <summary>
        /// Walk left
        /// </summary>
        public void KEYBOARD_DOWN_A()
        {
            //Move left
            Vector3 movement = new Vector3(Angle.X, 0, Angle.Z);
            movement = Vector3.Transform(movement, Matrix.CreateRotationY(MathHelper.PiOver2));
            movement.Normalize();
            Move(movement);
        }

        /// <summary>
        /// Walk right
        /// </summary>
        public void KEYBOARD_DOWN_D()
        {
            //Move right
            Vector3 movement = new Vector3(Angle.X, 0, Angle.Z);
            movement = Vector3.Transform(movement, Matrix.CreateRotationY(-MathHelper.PiOver2));
            movement.Normalize();
            Move(movement);
        }

        /// <summary>
        /// Unbind mouse
        /// </summary>
        public void KEYBOARD_PRESSED_LeftAlt()
        {
            mouseBound = !mouseBound;
        }

        /// <summary>
        /// Primary action
        /// </summary>
        /// <param name="pos">Mouse location</param>
        public void MOUSE_PRESSED_LEFT(Vector2 pos)
        {
            Vector3 prevCheck = Vector3.Zero, curve = position, check = position;
            int distance = 0;
            Vector3 angle = Angle;//Angle of projection/aim/look

            //Loop until intersect a block
            for (check = position; distance < 100 && world[curve] == null; check += angle / 16)
            {
                check = world.WrapCoordinate(check);//Wrap coordinate at world edges
                prevCheck = check;//Old current coordinate is the new previous coordinate
                distance++;

                curve = (VCHelper.CurvedVector(check, VCHelper.NearestTranslation(position, check, world)));//Apply world curvature
            }

            if (world[curve] != null)
                world[curve] = null;//Delete the block
        }

        /// <summary>
        /// Toggle flight
        /// </summary>
        public void KEYBOARD_PRESSED_F()
        {
            //Toggle flight mode
            flight = !flight;
        }

        /// <summary>
        /// Look direction vector. Forwards.
        /// </summary>
        public Vector3 Angle
        {
            get
            {
                float radius = 1;
                return -(new Vector3(radius * (float)Math.Cos((float)MathHelper.ToRadians(rotation.X + 90)) * (float)Math.Cos((float)MathHelper.ToRadians(rotation.Y)),
                radius * (float)Math.Sin((float)MathHelper.ToRadians(rotation.Y)),
                radius * (float)Math.Sin((float)MathHelper.ToRadians(rotation.X + 90)) * (float)Math.Cos((float)MathHelper.ToRadians(rotation.Y))));
            }
        }

        /// <summary>
        /// Direction vector perpendicular to the look vector. The up direction vector.
        /// </summary>
        public Vector3 UpAngle
        {
            get
            {
                float radius = 1;
                return -(new Vector3(radius * (float)Math.Cos((float)MathHelper.ToRadians(rotation.X + 90)) * (float)Math.Cos((float)MathHelper.ToRadians(rotation.Y + 90)),
                radius * (float)Math.Sin((float)MathHelper.ToRadians(rotation.Y + 90)),
                radius * (float)Math.Sin((float)MathHelper.ToRadians(rotation.X + 90)) * (float)Math.Cos((float)MathHelper.ToRadians(rotation.Y + 90))));
            }
        }

        /// <summary>
        /// Direction vector perpendicular to the look vector. The right direction vector.
        /// </summary>
        public Vector3 RightAngle
        {
            get
            {
                float radius = 1;
                return -(new Vector3(radius * (float)Math.Cos((float)MathHelper.ToRadians(rotation.X)) * (float)Math.Cos((float)MathHelper.ToRadians(rotation.Y)),
                radius * (float)Math.Sin((float)MathHelper.ToRadians(rotation.Y - 90)),
                radius * (float)Math.Sin((float)MathHelper.ToRadians(rotation.X)) * (float)Math.Cos((float)MathHelper.ToRadians(rotation.Y))));
            }
        }

        /// <summary>
        /// The view transformation
        /// </summary>
        public Matrix View
        {
            get
            {
                Vector3 bobbed = position;
                return Matrix.CreateTranslation(-bobbed) * Matrix.CreateRotationY(Rotation.X) * Matrix.CreateTranslation(new Vector3(0, 0, 0.15f)) * Matrix.CreateRotationX(Rotation.Y) * Matrix.CreateTranslation(offset);
            }
        }

        /// <summary>
        /// View transformation without the view offset
        /// </summary>
        public Matrix RawView
        {
            get
            {
                Vector3 bobbed = position;
                return Matrix.CreateLookAt(position, position + Angle, Vector3.Up);
            }
        }

        /// <summary>
        /// Camera bob amount
        /// </summary>
        public float Bob
        {
            get { return (cameraBob.Fade(false) - 0.5f) * 0.15f; }
        }

        /// <summary>
        /// If the camera is looking at an area/object.
        /// </summary>
        /// <param name="objectBounds">The collision box of the object to test for.</param>
        /// <param name="objectPosition">The location of the object to test for in the world.</param>
        /// <param name="transformation">The rotation/transformation of the object to test for.</param>
        /// <returns>True if the camera is looking at the object.</returns>
        public bool IsLookingAt(CollisionBox objectBounds, Vector3 objectPosition, Matrix transformation)
        {
            Vector3 prevCheck = Vector3.Zero, //The previous coordinate tested
                curve = position, //The coordinate to test with the world curvature applied
                check = position;//The coordinate to test
            int distance = 0;//The length of the line of coordinates that have been tested.
            Vector3 angle = Angle;//The amount to increase the test coordinate by
            
            //Until intersects with something
            for (check = position; distance < 100 && world[curve] == null && !objectBounds.Contains(curve, objectPosition, transformation); check += angle / 16)
            {
                check = world.WrapCoordinate(check);//Wrap the coordinate at the world edges
                prevCheck = check;//Old current check is the new previous check
                distance++;

                curve = (VCHelper.CurvedVector(check, VCHelper.NearestTranslation(position, check, world)));//Apply the world curvature to the coordinate
            }
            return (distance < 100);//Did not intersect with anything closer than 100 untits
        }

        /// <summary>
        /// The yaw and pitch in radians
        /// </summary>
        public Vector2 Rotation
        {
            get { return new Vector2(MathHelper.ToRadians(rotation.X), MathHelper.ToRadians(rotation.Y)); }
        }
    }
}
