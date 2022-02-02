# ErosionSimulator

To combine my knowledge of morphodynamics and programming, I started this project to learn Unity and C#.
With this project, the aim is to implement the natural physics of erosion, sedimentation and other morphological processes. 

### V0.1
As simulated waterdrops fall on the terrain, the vertex on which the drop falls is eroded along with the cells alongside.
The amount of erosion and sedimentation is dependent of the slope between the current vertex, and the next lowest vertex based on 8-connectivity.
Each drop gets filled with sediment, and looses it when the angle is not sufficient or when it its capacity is full. 
Every drop also has a lifetime, which determines the amount of steps it can move downhill eroding vertices and rising others with the sediment.
The slope that determines the amount of sedimentation represents the velocity with which the water travels.
In real rivers, high flow velocity means more sediment can be transported. 
Every terrain that is created is made pseudo-randomly with Perlin noise.
Every drop that is placed on the terrain is also place pseudo randomly. 
The terrain is represented with snow which appears above a certain altitude on below a maximum steepness.
Grass grows on the terrain below a certain altitude below a maximum steepness as well.
Similarly, lakes form below a certain altitude on very flat terrain. 
Steep terrain is represented as rockwalls. 
All colors in the simulation are added to the mesh through the mesh class in unity, through which every vertex can be given a color. 
Using this option the terrain colors are dependent of heights and slopes. 

Run the .exe file in the zipped ErosionSim folder to run the file. 
The keys for use are:
- ASWD:   move the terrain
- ZX:     rotate the terrain
- Q: reload new terrain
- 1,2,3: iterate 10,000 20,000 or 40,000 drops respectively
- esc: exit simulation

Planned improvements:
- Multi Directional Flow (instead of current Single Directional Flow)
- Different layers of earth, some eroding faster than others
- Enable landslides when eroded is landscape too steep
