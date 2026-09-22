# Boids

Real-time 3D simulation of flocking behavior based on the [Boids](https://en.wikipedia.org/wiki/Boids) model, implemented in Unity using C# and an MVC architecture.

## Branches
* Master: naive algorithm (checking all the other boids -> O(n²)) on CPU.
* HLSL_naive_algo: naive algorithm on GPU (compute + render), implemented in HLSL.

## Demo

10000 Boids (branch HLSL_naive_algo):

![10000 Boids simulation](https://github.com/ArmandoMarjorie/Boids/blob/master/Docs/gif_animation_004.gif)

150 Boids (branch Master):

![150 Boids simulation outside the cage](https://github.com/ArmandoMarjorie/Boids/blob/master/Docs/gif_animation_006.gif)

![150 Boids simulation inside the cage](https://github.com/ArmandoMarjorie/Boids/blob/master/Docs/gif_animation_007.gif)

All the 3D assets are free in the Unity asset store:
* [3D bird model + animations](https://assetstore.unity.com/packages/3d/characters/animals/quirky-series-free-animals-pack-178235)
* [Sky + clouds](https://assetstore.unity.com/packages/3d/environments/simple-sky-cartoon-assets-42373)

## Features

* Alignment
* Cohesion
* Separation
* Field-of-view based neighborhood detection
* Boundary avoidance (boids stay in a cage)
* Vertical angle limitation
* Configurable simulation parameters
* Smooth agent rotation
* ScriptableObject-based configuration

## Benchmarks

The boid's prefab's animator composent has been desactivated for the following tests. 

### Configurations

* Unity version: 2020.3.48f1
* GPU: NVIDIA GeForce RTX 2060 (6 GB)
* Graphics API: Direct3D11
* CPU: Intel Core i7-10750H CPU @ 2.60GHz
* RAM: 16 Go
* OS: Windows 10, 64 bits

| Implementation | 150 Boids | 1000 Boids | 10000 Boids | 15000 Boids | 30000 Boids
| -------------- | :-------: | :--------: | :---------: | :---------: | :---------:
| **CPU (branch master)** | 80 FPS | 5 FPS | < 2 FPS | < 1 FPS | < 1 FPS
| **GPU (branch HLSL_naive_algo)** | 190 FPS | 170 FPS | 80 FPS | 50 FPS | 20 FPS

## Architecture

![UML Class Diagram](https://github.com/ArmandoMarjorie/Boids/blob/master/Docs/ClassDiagram.png)

CageView is not represented as it is only for debugging/visualization purpose. 

## Future improvements

* Neighborhood detection with an octree
* Obstacle avoidance
