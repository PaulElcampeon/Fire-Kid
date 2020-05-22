# Unity 2D Platformer Controller
=======================

<!---%=description%-->

A customizable 2D platformer motor that interacts with Unity's physics engine to do mechanics such as double jumps, wall jumps, and corner grabs. Includes a player controlled prefab that can be dropped into any scene for immediate support.

<!---%=obtain%-->

####Obtain!####
[Releases](https://github.com/cjddmut/Unity-2D-Platformer-Controller/releases)

If you'd like the most up to date version (which is the most cool), then pull the repo or download it [here](https://github.com/cjddmut/Unity-2D-Platformer-Controller/archive/develop.zip) and copy the files in Assets to your project's Assets folder.

<!---%=docrest%-->

## Setup

For immediate player support, drop the PlatformerPlayer2D prefab into the scene. For more complicated interaction, interface with PlatformerMotor2D's members and methods.

## PlatformerMotor2D Inspector Properties

### General ###

**Environment Check Mask** - This tells the motor what layer collisions to consider the environment (to determine if on the ground, wall, or corner).

**Environment Check Distance** - This is how far out the motor will check for the environment.  

### Movement ###

**Ground Speed** - Maximum ground speed.

**Time to Ground Speed** - The time, in seconds, it will take to reach ground speed. This is used to calculate acceleration.

**Ground Stop Distance** - If at full speed, how far will the motor skid to a stop.

**Horizontal Air Speed** - Maximum speed the motor will move horizontally while in the air.

**Time to Air Speed** - The time, in seconds, it will take to reach air speed. This is used to calculate acceleration.

**Air Stop Distance** - If at full air speed, how far will the motor 'skid' to a stop.

**Max Fall Speed** - Maximum fall speed (only y axis when negative).

**Max Fast Fall Speed** - Maximum fall speed when falling fast.

**Fast Fall Gravity Multiplier** - Gravity multiplier when falling fast. A value of 1 means no different, higher values mean faster fall acceleration.

**Preserve Momentum When Landing** - Unity's physics engine will reduce horizontal speed when the motor lands. This means movement speed will have to be accelerated again and can cause a slight pause. Check this one to keep horizontal speed intact.

### Jumping ###

**Base Jump Height** - The height, in Unity units, that the motor will jump to.

**Held Extra Jump Height** - If the motor is informed that the jump is held then this is the additional height the character will jump.

**Allow Double Jump** - If the motor should allow double jump. A double jump is considered when the motor is in the air and a double jump has not occurred. 

**Allow Wall Jump** - If jumping off the wall is allowed.

**Wall Jump Multiplier** - The base jump speed is calculated from Base Jump and Extra Jump Height. The multiplier multiplies the result. Leave at 1 for no change.

### Wall Cling ###

**Allow Wall Cling** - If the motor should cling to the walls (sticking in place).

**Wall Cling Duration** - The time, in seconds, that the motor will stick to walls.

### Wall Slide ###

**Allow Wall Slide** - If the motor should consider any wall sliding calculations. Wall sliding is when the character would slow down while 'sliding' down the wall.

**Wall Slide Speed** - The speed that the character will slide down the wall.

### Corner Grabs ###

**Allow Corner Grab** - If corner grabbing is allowed. 

**Corner Grab Duration** - The time, in seconds, that the motor will stick to corners.

**Corner Jump Multiplier** - The multiplier on a corner jump from the calculated speed.

**Corner Distance Check** - A corner is considered grabbed if the upper corners of the collider do not intersect with the environment but the sides do. The value changes the consideration for box checks dimensions.

### General Wall Interactions ###

**Wall Interaction Threshold** - The input threshold for wall clings, corner grabs, and slides. Could be set to higher to prevent unwanted sticking to walls.

### Dashing ###

**Allow Dashing** - Is dashing allowed?

**Dash Distance** - The distance covered by the dash.

**Dash Duration** - The duration of the dash.

**Dash Cooldown** - How long, in seconds, before the motor will allow dash again?

**Dash Easing Function** - The easing function of the dash. For a dash that movement with a consistent speed pick linear.

### PlatformerMotor2D Members ###

```csharp
float normalizedXMovement
```

Set the x movement direction. This is multiplied by the max speed. -1 is full left, 1 is full right. Higher numbers will result in faster acceleration.

```csharp
MotorState motorState // Readonly

enum MotorState
{
    OnGround,
    InAir,
    FallingFast,
    Sliding,
    OnCorner,
    Clinging,
    Dashing,
    Frozen
}
```

Call this to get state information about the motor. This will be information such as if the object is in the air or on the ground. This can be used to set the appropriate animations.

```csharp
bool facingLeft // Readonly
```

Since the motor needs to know the facing of the object, this information is made available to anyone else who might need it.

```csharp
bool fallFast
```

Set this true to have the motor fall faster. Set to false to fall at normal speeds.

```csharp
Vector2 dashDirection
```

Returns the direction of the current dash. If not dashing then returns Vector2.zero.

```csharp
bool jumpingHeld
```

If jumpingHeld is set to true then the motor will jump further. Set to false if jumping isn't 'held'.

```csharp
bool frozen
```

Setting frozen to true will put the motor in a 'frozen' state. All information will be saved and set once unfrozen (the motor also reduce gravity to 0).

Note: This isn't a way to turn off the motor. To turn off the motor, simply set the script to disabled.

```csharp
Collider2D colliderToUse
```
Set this to use a specific collider for checks instead of grabbing the collider from gameObject.collider2D.

```csharp
delegate void Notification()

Notification onDash
Notification onDashEnd
Notification onJump
```

Attach to these delegates to receive notifications for dash, dash end, and jump events.

### PlatformerMotor2D Methods ###

```csharp
void Jump(float extraSpeed = 0)
```

Call this to have the GameObject try to jump, once called it will be handled in the FixedUpdate tick. The y axis is considered jump.

```csharp
void ForceJump(float extraSpeed = 0)
```

This will force a jump to occur even if the motor doesn't think a jump is valid. This function will not work if the motor is dashing.

```csharp
void EndJump()
```

Call to end a jump. Causes the motor to stop calculated held speed for a jump.

```csharp
void ResetDoubleJump()
```

Resets the state for the a double jump allowing another jump. This doesn't do anything if double jumps aren't allowed.

```csharp
void Dash()
```

Call this to have the GameObject try to dash, once called it will be handled in the FixedUpdate tick. This casues the object to dash along their facing (if left or right for side scrollers).

```csharp
void Dash(Vector2 dir)
```

Send a direction vector to dash allow dashing in a specific direction.

```csharp
void EndDash()
```

Call to end dash immediately.

## PlayerController2D

The PlayerController2D script is a simple script that connects player input to the motor. This is set up as an example and it is encourage to write your own script that interacts with the motor.

## FAQs

**PlatformerMotor2D is messing with values in my rigidbody2D!**
PlatformerMotor2D reduces drag on the rigidbody2D to 0 and handles deceleration on the ground and in the air on its own. The motor will also manipulate the gravity to suit its needs for fast falls and wall interactions.

If your game has moments where it needs to leverage gravity or drag then disable the motor during these moments.

**Can I use PlatformerMotor2D for controlling AI movements?**
Sure can. PlatformerMotor2D doesn't know anything about inputs, it just acts on information passed to it. An AI script can interface with the motor similarly how a player controller script could.

**I let go of the joystick and my GameObject isn't sliding the distance it is supposed to!**
If you're using the supplied PlayerController2D script or one of your own in which you use Input.GetAxis() then there's a built in deceleration in what Input.GetAxis() returns. This can definitely be impacting the distance the GameObject skids to a stop! To see a true skid to stop, set normalizedXMovement to zero.

**Something isn't working right!**
Well, this happens. Please open up an [issue](https://github.com/cjddmut/Unity-2D-Platformer-Controller/issues)!
<!---%title=Unity 2D Platformer Controller%-->
<!---%download=https://github.com/cjddmut/Unity-2D-Platformer-Controller/releases/download/v0.2.0/PC2D_v0.2.0b.unitypackage%-->
<!---%github=https://github.com/cjddmut/Unity-2D-Platformer-Controller%-->
