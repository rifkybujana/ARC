# ARC
A tool that can make us interact directly with 3D objects in AR via android

# Group 1 (Ar-Cademia)
1. Ararya Pandya Arjana (X MIPA 3)
2. Rifky Bujana Bisri (X MIPA 3)
3. Muhammad Rafly (X MIPA 3)
4. Hafizhuddin Rasyid (XI MIPA 4)
5. Ghefira Airashin (XI MIPA 3)

# Robot description
  The tool made by our group is an amalgamation of the XR function. This robot
can help human activities in various fields, such as education, work,
medicine, etc. In the future this can be of great help to humans like
  visualize the lesson being learned, like an atom. If we can
visualizing atoms while we are studying chemistry, we can understand better about
these atoms. And we can "interact directly" with the object being studied
the.

# The main function
1. Depiction of objects in real
2. Adding digital interversion
3. Interact with 3D objects
4. Provide the required information from the object

# Benefits / Advantages
1. Facilitate the presentation in both presentation and learning
2. Provide real depiction of digital objects
3. Make presentations / lessons more interesting
4. Can interact directly with the object being studied

# Comparison with the current system
1. Our tool combines features from VR (Virtual Reality) and AR (Augmented
Reality) which currently has very little development.
2. By using a cellphone (handphone) as an interface, it makes a much more powerful tool
affordable for the public when compared to other VR gear which is quite priced
expensive.
3. This tool allows us to interact with 3D objects with our hands,
whereas now the majority of VR and AR devices still use the remote

# Procedure
# Hardware:
1. Read the data from the flex sensor on each finger to determine the finger indentation / rotation
2. Read the gyro sensor to determine hand rotation
3. Calculate the results of the flex and gyro sensors to get the motion and
position of the hands and fingers
4. Send the calculated data to the user's smartphone using the bluethoot module
HC05

# Software:
How ARCore Works:
  As your phone moves around the world, ARCore uses a process called odometry and concurrent mapping, or COM, to understand where the phone is relative to the world around it. ARCore detects visually different features in the captured camera image called feature points and uses these points to calculate their change in location. Visual information is combined with inertia measurements from the device's IMU to estimate the camera's pose (position and orientation) relative to the world over time.

  then based on data obtained from hardware we can determine the position and movement of the hand. Then make the object move or do something based on the interaction of the hand
  
# Future Development
1. Refine tool interface
2. Minimizes the electronic module on the glove

# Material
1. Flex sensors 5 pieces (handmade / buy)
2.Gyro sensor (MPU-6050)
3. Long gloves
4. Arduino UNO + Shield
5. 1 meter ribbon cable
6.Jumper cables each 20 (male-female; male-male; female-female)
7. 1 meter AWG cable
8. Blackhousing female iron + pin head
9. Li-Po battery
10. Battery connector to Arduino
11. VR box
12. Bluetooth module
