# egp-400-lab1-unity-raytracing

## Advanced Seminar in Realtime Rendering

This repo was a school project where the goal was to get a raytracer working as quickly as possible. In this particular case, that meant using Unity and using the CPU to do all the calculations, forgoing the power of the GPU entirely.

This system works by using a combination of Unity's Physics Raycast system and the Component system. "Materials"/"shaders" are stored as Components on the GameObjects in the scene, and rays are fired from the Camera to find them.

One of the fun challenges of this project was getting it as close to "realtime" as possible without changing the overall architecture of the system (one of the requirements of the assignment), which meant doing some micro-optimizations and doing everything possible to circumvent the C# garbage collector.
