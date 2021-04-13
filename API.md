# Software-Control-to-Simulating-Platform API Documentation
### Language: C# #
### Authors:  ['Po-Hsun Chen', 'Shao-Chieh Lien', 'Shristi Saraff']

## Introduction
API for users to control the robotic arm movement using Wifi communication using Visual Studio Code with .Net framework
<br><br/>
# Defined Structs

## 1. PTPJointParams
### **Description**
Set the Point to point (PTP) parameters of the speed and acceleration of Joint (motor) coordinate axis.
### **Variables**
| Variable        | Data Type    | Default Value       | Description   |
| :-------------: | :----------: | :-----------:       | :-----------: |
| velocity        | float[4]     | {1, 5, 10, 15}      | Rotation velocity of each motor|
| acceleration    | float[4]     | {50, 100, 150, 200} | Rotation acceleration of each motor|


## 2. PTPCoordinateParams
### **Description**
Set the Point to point (PTP) parameters of speed and accerleration of the Cartesian axis.
### **Variables**
| Variable        | Data Type    | Default Value | Description   |
| :-------------: | :----------: | :-----------: | :-----------: |
| xyzVelocity     | float        | 5             | Moving velocity of the tip of suction cup |
| rVelocity       | float        | 5             | Reserved for future use |
| xyzAcceleration | float        | 5             | Moving acceleration of the tip of suction cup|
| rAccleration    | float        | 5             | Reserved for future use|


## 3. JOGCmd
### **Description**
Set the JOG mode parameters for a certain axis to be moved and the coordinate axis to be used.
### **Variables**
| Variable        | Data Type    | Description  |
| :-------------: | :----------: | :-----------:|
| isJoint         | byte         | 1 for Joint (motor) coordinate axis, 0 for Cartesian coordinate axis |
| cmd             | byte         | 1 for motor 1/x axis, 2 for motor 2/y axis, 3 for motor 3/z axis|


## 4. PTPCmd
### **Description**
Set the Point to point (PTP) parameters of starting point and end point to make the simulated arm move.
### **Variables**
| Variable        | Data Type    | Description  |
| :-------------: | :----------: | :-----------:|
| ptpMode         | byte         | 1 for inverse kinemetic, 3 for forward kinemetic|
| x               | float        | First Joint (motor) coordinate axis if ptpMode = 1, First Cartesian coordinate axis if ptpMode = 3|
| y               | float        | Second Joint (motor) coordinate axis if ptpMode = 1, Second Cartesian coordinate axis if ptpMode = 3|
| z               | float        | Third Joint (motor) coordinate axis if ptpMode = 1, Third Cartesian coordinate axis if ptpMode = 3|
| r               | float        | Reserved for future use |


<br><br/>
# Functions

## 1. SetEndEffectorSuctionCup
### **Description**
Enables robot arm to pick up or drop objects by turning the suction cup on or off
### **Parameters**
| Parameter       | Data Type    | Description   |
| :-------------: | :----------: | :-----------: |
|  isSucked       | bool         | Enables or disables suction cup on the robot arm |
### **Return**
None
### **Sample Usage**
```
// Turn on the suction cup
SetEndEffectorSuctionCup(true);

// Turn off the suction cup and release the object
SetEndEffectorSuctionCup(false);
```


## 2. SetJOGCmd
### **Description**
In JOG mode, move a certain coordinate axis by one unit (1 cm or 1 degree)
### **Parameters**
| Parameter       | Data Type    | Description   |
| :-------------: | :----------: | :-----------: |
|  jogCmd         | JOGCmd       | Move a certain coordinate axis by one unit |
### **Return**
None
### **Sample Usage**
```
// Initialize JOGCmd object
JOGCmd jogCmd = new JOGCmd();

// Set up coordinate axis and motor to be used
jogCmd.isJoint = 0;
jogCmd.cmd = 2;

// Function call
SetJOGCmd(jogCmd);
```


## 3. SetPTPJointParams
### **Description**
Set the speed and acceleration of Joint coordinate axis
### **Parameters**
| Parameter       | Data Type      | Description   |
| :-------------: | :----------:   | :-----------: |
|  ptpJointParams | PTPJointParams | Set the speed and acceleration of Joint coordinate axis|
### **Return**
None
### **Sample Usage**
```
// Initialize PTPJointParams object
PTPJointParams ptpJointParams = new PTPJointParams();

// Set up the velocity of forward kinematics
ptpJointParams.velocity[0] = 8F;
ptpJointParams.velocity[2] = 120F;
ptpJointParams.acceleration[1] = 50F;

// Function call
SetPTPJointParams(ptpJointParams);
```


## 4. SetPTPCoordinateParams
### **Description**
Set the speed and acceleration of Cartesian coordinate axis
### **Parameters**
| Parameter            | Data Type           | Description   |
| :-------------:      | :----------:        | :-----------: |
|  ptpCoordinateParams | PTPCoordinateParams | Set the speed and acceleration of Cartesian coordinate axis |
### **Return**
None
### **Sample Usage**
```
// Initialize PTPCoordinateParams object
PTPCoordinateParams ptpCoordinateParams = new PTPCoordinateParams();

// Set up the velocity of inverse kinematics
ptpCoordinateParams.xyzVelocity = 100;
ptpCoordinateParams.rVelocity = 150;

// Function call
SetPTPCoordinateParams(ptpCoordinateParams);
```


## 5. SetPTPCmd
### **Description**
Set the starting point and the end point to make the simulated arm move
### **Parameters**
| Parameter       | Data Type    | Description   |
| :-------------: | :----------: | :-----------: |
|  ptpCmd         | PTPCmd       | Set the starting point and the end point to make the simulated arm move |
### **Return**
None
### **Sample Usage**
```
// Initialize PTPCmd object
PTPCmd ptpCmd = new PTPCmd();

// Move to coordinate （-270, 140, 95） using inverse kinematics
ptpCmd.ptpMode = 1;
ptpCmd.x = -270F;
ptpCmd.y = 140F;
ptpCmd.z = 95F;

// Function call
SetPTPCmd(ptpCmd);
```


## 7. ConnectToServer
### **Description**
Connect to server (Simulated Arm) using port 127.0.0.1 (self loop) at port 6321
### **Parameters**
None
### **Return**
None
### **Sample Usage**
```
// Initializing class object
Program AllFunctions = new Program();

// Connect to server
AllFunctions.ConnectToServer();
```


## 8. Send
### **Description**
Send the machine code to the simulated arm
### **Parameters**
| Parameter       | Data Type    | Description   |
| :-------------: | :----------: | :-----------: |
|  data           | string       | Complete information packet (header + message parameters + message length) to be sent to server |
### **Return**
None
### **Sample Usage**
```
// Initializing class object
Program AllFunctions = new Program();

// Send machine code to server
AllFunctions.Send(hexString);
```