# Training-Self-Driving-Cars-with-Unity


This project is a simulator for training cars, controlled by [artificial neural networks](https://en.wikipedia.org/wiki/Artificial_neural_network)(NN), to navigate a maze. Cars improve by means of a [genetic algorithm](https://en.wikipedia.org/wiki/Genetic_algorithm)(GA) that simulates real world natural selection.

To Navigate the maze the cars make use of 5 "sensors" that each feed (distance of the nearest obstacle / sensor distance) into the NN. The NN then outputs two values cooresponding to Left or Right. The one with a greater value is the action the car takes. The cars have a constant forward velocity and are NN outputs rotate the car to direct it through obstacles.

The population of cars evolve by starting out with entirely random NN's. At each new generation a user determined proportion of the generation will be copies of the most fit car from the preivous generation. Each car then has a user determined chance to have the weights in its neural network [mutated](https://en.wikipedia.org/wiki/Mutation_(genetic_algorithm)). The population of cars are then spawned and run through the course until they either hit an obstacle or survive until the generation time limit.

The below is an example of a single generation of a sample of 150 cars training. Notes: The lines are the sensors, the purple car is the most fit, and the red are dead cars that can be hidden by a toggleable setting. 


![Driving](https://user-images.githubusercontent.com/43308388/63300753-fb6b8c80-c2a6-11e9-9ba4-02cc2e28900d.gif)

## Boundaries and Fitness

An important 
![boundaries](https://user-images.githubusercontent.com/43308388/63312263-f3bfde00-c2ce-11e9-9ab8-936afbe00e99.png)


## Inspector
Inspector part


![Inspector](https://user-images.githubusercontent.com/43308388/63300750-fb6b8c80-c2a6-11e9-883d-19207eb165a9.png)


## Successful Car
![perfect car](https://user-images.githubusercontent.com/43308388/63300752-fb6b8c80-c2a6-11e9-80ba-2c972b53e9a4.gif)
