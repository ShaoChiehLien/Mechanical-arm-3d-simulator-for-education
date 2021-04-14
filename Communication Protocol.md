# Software-Control-to-Simulating-Platform Communication Protocol Documentation
### Authors:  ['Po-Hsun Chen', 'Shao-Chieh Lien', 'Shristi Saraff']

## Introduction
The mechanical arm can be controlled by macOS, achieving data transmission
through certain communication protocols. The communication can be realized by WiFi (TCP). The physical layer receives 8 bits of raw data each time, which needs to be verified for accuracy by setting up communication protocols. The communication protocol includes a packet header, packet load and checksum to make sure that the data is accurately transferred.

Our application uses the communication protocols created by [Dobot](https://www.dobot.cc/).
By using our application, you can not only learn to simulate a virtual mechanical arm,
but also control Dobot's real mechanical arms.

## Instruction Packets
The instruction packet for each function in the API can be found [here](http://www.dobot.it/wp-content/uploads/2018/03/Dobot-Communication-Protocol-V1.0.4.pdf).
