# Grey Wolf Optimizer Visualization

A simple Windows Forms application that visualizes the **Grey Wolf Optimizer (GWO)** algorithm in action. This project shows how a swarm of "wolves" iteratively approaches the minimum of a 2D Sphere function.

![Grey_Wolf_Optimizer_OfsiadBpbe](https://github.com/user-attachments/assets/7f7b9041-21e9-41a9-9ccf-438d07ffd5a3)


## Features

- Visualizes wolves moving in a 2D space toward the global optimum.
- Highlights the three best wolves (Alpha, Beta, Delta).
- Uses the Sphere function as an objective function.
- Adjustable iteration speed with a numeric input.

## Algorithm Overview

The **Grey Wolf Optimizer (GWO)** is a nature-inspired metaheuristic algorithm based on the hunting behavior and leadership hierarchy of grey wolves in nature. It simulates:

- **Alpha** – the best solution  
- **Beta** – the second-best  
- **Delta** – the third-best  
- **Omega wolves** – the rest, which update their positions based on alpha, beta, and delta  

This application uses GWO to find the minimum of the Sphere function:  
```math 
f(x, y) = x^2 + y^2  
```  
The minimum is at the origin (0, 0).

## How to Run

### Requirements

- .NET Framework (Windows Forms)
- Visual Studio (or any C# compatible IDE)

### Steps

1. Clone this repository

2. Open the solution file in Visual Studio.

3. Build and run the application.

4. Use the numeric input at the bottom of the window to adjust the iteration speed (in milliseconds).

## UI Elements

- **Wolves** – Grey dots  
- **Alpha wolf** – Red  
- **Beta wolf** – Orange  
- **Delta wolf** – Green  
- **Global optimum** – Blue dot at (0, 0)  
- **Speed Control** – Numeric input to slow down or speed up the simulation  
- **Iteration Counter** – Displayed at the top-left

## Customization

You can easily tweak the algorithm by changing constants at the top of the code:

```C# 
const int PopulationSize = 20;  
const int MaxIterations = 100;  
const double RangeMin = -10;  
const double RangeMax = 10;  
```

Or use a different objective function by modifying:

```C#
double ObjectiveFunction(double[] pos)  
{  
    return pos[0] * pos[0] + pos[1] * pos[1];  
}  
```
