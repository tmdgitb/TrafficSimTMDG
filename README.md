# TrafficSimTMDG
## Overview
TrafficSimTMDG is a Traffic Micro-Simulation based on C#.
The driver behaviour is based on 2 types of model:
  1. Car-following model: IDM (https://en.wikipedia.org/wiki/Intelligent_driver_model)
  2. Line-changing model: MOBIL (http://www.mtreiber.de/publications/MOBIL_TRB.pdf)

## Program Hierarchy
In general, the program hierarchy is arranged as follows:
```
- MainForm
-- NodeControl
--- WaySegment
---- Vehicle
```

So in short, when the simulation is running, it calls an update loop in MainForm, which trigger the loop in the NodeControl.
NodeControl then trigger the loop in each WaySegment it stored, all the way to each Vehicle.
