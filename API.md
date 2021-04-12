# Software-Control-to-Simulating-Platform API Documentation
### Language: C# #
### Authors:  ['Po-Hsun Chen', 'Shao-Chieh Lien', 'Shristi Saraff']

## Introduction
API for users to control the robotic arm movement using Wifi communication
<br><br/>
# Defined Structs

<!--- Add descriptions -->
## 1. PTPJointParams
### **Description**
Point to point (PTP) parameters, set the speed and acceleration of joint(motor) coordinate axis.
### **Variables**
| Variable        | Data Type    | Default Value       | Description   |
| :-------------: | :----------: | :-----------:       | :-----------: |
| velocity        | float[4]     | {1, 5, 10, 15}      | Rotation velocity of each motor|
| acceleration    | float[4]     | {50, 100, 150, 200} | Rotation acceleration of each motor|


<!--- Add descriptions -->
## 2. PTPCoordinateParams
### **Description**
Point to point (PTP) parameters, set the speed and accerleration of the Cartesian axis.
### **Variables**
| Variable        | Data Type    | Default Value | Description   |
| :-------------: | :----------: | :-----------: | :-----------: |
| xyzVelocity     | float        | 5             | Moving velocity of the tip of suction cup |
| rVelocity       | float        | 5             | Reserved for future use |
| xyzAcceleration | float        | 5             | Moving acceleration of the tip of suction cup|
| rAccleration    | float        | 5             | Reserved for future use|


<!--- Add descriptions -->
## 3. JOGCmd
### **Description**
JOG command variable pointers
### **Variables**
| Variable        | Data Type    | Description  |
| :-------------: | :----------: | :-----------:|
| isJoint         | byte         |              |
| cmd             | byte         |              |


<!--- Add descriptions -->
## 4. PTPCmd
### **Description**
Pont to point (PTP) commands, set the starting point and the end point to make the simulated arm move
### **Variables**
| Variable        | Data Type    | Description  |
| :-------------: | :----------: | :-----------:|
| ptpMode         | byte         | 1 for inverse kinemetic, 3 for forward kinemetic|
| x               | float        | First Joint(motor) coordinate axis if ptpMode = 1, First Cartesian(motor) coordinate axis if ptpMode = 3|
| y               | float        | Second Joint(motor) coordinate axis if ptpMode = 1, Second Cartesian(motor) coordinate axis if ptpMode = 3|
| z               | float        | Third Joint(motor) coordinate axis if ptpMode = 1, Third Cartesian(motor) coordinate axis if ptpMode = 3|
| r               | float        | Reserved for future use|


<br><br/>
# Functions

## 1. SetEndEffectorSuctionCup
### **Description**
Enables robot arm to pick up or drop objects by turning the suction cup on or off
### **Parameters**
| Parameter      | Data Type    | Description  |
| :-------------: | :----------: | :-----------: |
|  isSucked | bool | Enables or disables suction cup on the robot arm |
### **Return**
None
### **Sample Usage**
  + // Turn on the suction cup
> *SetEndEffectorSuctionCup(true);*

  + // Turn off the suction cup and release the object
> *SetEndEffectorSuctionCup(false);*


<!--- Improve descriptions and example usage -->
## 2. SetJOGCmd
### **Description**
Executes JOG command
### **Parameters**
| Parameter       | Data Type    | Description   |
| :-------------: | :----------: | :-----------: |
|  jogCmd         | JOGCmd       | JOG command   |
### **Return**
None
### **Sample Usage**
> // Initialize JOGCmd object
> *JOGCmd jogCmd = new JOGCmd();*
> // Set up
> *jogCmd.isJoint = ;*
> *jogCmd.cmd = ;*
> // Function call
> *SetJOGCmd(jogCmd);*


<!--- Add descriptions -->
## 3. SetPTPJointParams
### **Description**

### **Parameters**
| Parameter       | Data Type      | Description   |
| :-------------: | :----------:   | :-----------: |
|  ptpJointParams | PTPJointParams |               |
### **Return**
None
### **Sample Usage**
> // Initialize PTPJointParams object
> *PTPJointParams ptpJointParams = new PTPJointParams();*
> // Set up the velocity of forward kinematics
> *ptpJointParams.velocity[0] = 8F;*
> *ptpJointParams.velocity[2] = 120F;*
> *ptpJointParams.acceleration[1] = 50F;*
> // Function call
> *SetPTPJointParams(ptpJointParams);*


<!--- Add descriptions -->
## 4. SetPTPCoordinateParams
### **Description**

### **Parameters**
| Parameter            | Data Type           | Description   |
| :-------------:      | :----------:        | :-----------: |
|  ptpCoordinateParams | PTPCoordinateParams |               |
### **Return**
None
### **Sample Usage**
> // Initialize PTPCoordinateParams object
> *PTPCoordinateParams ptpCoordinateParams = new PTPCoordinateParams();*
> // Set up the velocity of inverse kinematics
> *ptpCoordinateParams.xyzVelocity = 100;*
> *ptpCoordinateParams.rVelocity = 150;*
> // Function call
> *SetPTPCoordinateParams(ptpCoordinateParams);*


<!--- Add descriptions -->
## 5. SetPTPCmd
### **Description**

### **Parameters**
| Parameter       | Data Type    | Description   |
| :-------------: | :----------: | :-----------: |
|  ptpCmd         | PTPCmd       |               |
### **Return**
None
### **Sample Usage**
> // Initialize PTPCmd object
> *PTPCmd ptpCmd = new PTPCmd();*
> // Move to coordinate（-270, 140, 95） using inverse kinematics
> *ptpCmd.ptpMode = 1;*
> *ptpCmd.x = -270F;*
> *ptpCmd.y = 140F;*
> *ptpCmd.z = 95F;*
> // Function call
> *SetPTPCmd(ptpCmd);*


<!--- Add descriptions and example usage -->
## 6. ToHexString
### **Description**
Converts an array of bytes to a hex string
### **Parameters**
| Parameter       | Data Type    | Description   |
| :-------------: | :----------: | :-----------: |
|  bytes          | byte[]       |               |
### **Return**
| Return Type     | Description  |
| :-------------: | :----------: |
|  string         |              |
### **Sample Usage**


<!--- Add descriptions and example usage -->
## 7. ConnectToServer
### **Description**

### **Parameters**
None
### **Return**
None
### **Sample Usage**


<!--- Add descriptions and example usage -->
## 8. Send
### **Description**

### **Parameters**
| Parameter       | Data Type    | Description   |
| :-------------: | :----------: | :-----------: |
|  data           | string       |               |
### **Return**
None
### **Sample Usage**
