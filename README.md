# Training-Self-Driving-Cars-with-Unity


This project is a simulator for training cars, controlled by [artificial neural networks](https://en.wikipedia.org/wiki/Artificial_neural_network)(ANN), to navigate a maze. Cars improve by means of a [genetic algorithm](https://en.wikipedia.org/wiki/Genetic_algorithm)(GA) that simulates real world natural selection.


The below is an example of a single generation of a sample of 150 cars training. Notes: The lines are the sensors, the purple car is the most fit, and the red are dead cars that can be hidden by a toggleable setting. 


![Driving](https://user-images.githubusercontent.com/43308388/63300753-fb6b8c80-c2a6-11e9-9ba4-02cc2e28900d.gif)


To Navigate the maze the cars make use of 5 "sensors" that each feed (distance of the nearest obstacle / sensor distance) into the NN. The NN then outputs two values cooresponding to Left or Right. The one with a greater value is the action the car takes. The cars have a constant forward velocity and are NN outputs rotate the car to direct it through obstacles.

The population of cars evolve by starting out with entirely random NN's. At each new generation a user determined proportion of the generation will be copies of the most fit car from the preivous generation. Each car then has a user determined chance to have the weights in its neural network [mutated](https://en.wikipedia.org/wiki/Mutation_(genetic_algorithm)). The population of cars are then spawned and run through the course until they either hit an obstacle or survive until the generation time limit.

## Fitness and Boundaries

An integral part of training using a gentic algorithm is derermining a means of assign a [fitness function](https://en.wikipedia.org/wiki/Fitness_function) to the cars that are being trained. This allows for the differentiate of the ability of an individual car. There a countless ways to create a fitness function. Common ones for this sort of problem are checkpoints throughout the course, distance covered, time without crashing, etc.

I chose to determine fitness based upon the distance covered by the cars. With this comes the benefit of not relying on adding checkpoints to the course. However, with this method comes the issue that if cars have the possiblity to go in loops this can create a false positive of cars that can gain infinite fitness if not handled. To account for this I make moving obstacles that follow the cars to eliminate any that turn around or start to loop. 

To accomplish this I created simple boundary objects that move between waypoints at a defined speed.

Note: This method of boundaries ended up being quite similiar to a checkpoint implementation. In retrospect checkpoints may have been more intuitive and simpler to implement.
![boundaries](https://user-images.githubusercontent.com/43308388/63312263-f3bfde00-c2ce-11e9-9ab8-936afbe00e99.png)


## Inspector
To allow for more rapid hyperparameter tuning and testing of new settings I implemented a custom inspector within [Unity](https://unity.com/). 

![Inspector](https://user-images.githubusercontent.com/43308388/63300750-fb6b8c80-c2a6-11e9-883d-19207eb165a9.png)


## Successful Car

Sample of a car succesfully completing the predefined course.
![perfect car](https://user-images.githubusercontent.com/43308388/63300752-fb6b8c80-c2a6-11e9-80ba-2c972b53e9a4.gif)
